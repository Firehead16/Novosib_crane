using System.Collections.Generic;
using System.Linq;
using Core.Ui;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Testing
{
	public struct SpaceFillData
	{
		public string Text;
		public bool IsAnswerText;
		public bool IsEndOfParagraph;
	}

	public struct TextLine
	{
		public List<SpaceFillData> LineData;
	}

	public class DragnDropTextQuestionForm : QuestionForm<AnswerDragNDrop>
	{
		/// <summary>
		/// Контейнер для дроп-зон текстов ответа
		/// </summary>
		[SerializeField]
		protected Transform VariantsContainer;

		/// <summary>
		/// Контейнер для бэкграунда текстов ответа
		/// </summary>
		[SerializeField]
		protected Transform BackgroundVariantsContainer;

		/// <summary>
		/// Лист с ответами
		/// </summary>
		private List<AnswerDragNDrop> answers = new List<AnswerDragNDrop>();


		private List<SpaceFillData> stringContainer = new List<SpaceFillData>();
		private List<TextLine> textLines = new List<TextLine>();
		private int deltaString;
		private List<Image> lines = new List<Image>();

		/// <summary>
		/// Тексты для Drop зон
		/// </summary>
		private List<Text> backgroundText = new List<Text>();

		/// <summary>
		/// Drop zones
		/// </summary>
		private List<Text> placeholders = new List<Text>();

		/// <summary>
		/// Drag Items
		/// </summary>
		private List<DragTextHandeler> movableAnswers = new List<DragTextHandeler>();
		private List<string> answerValues = new List<string>();


		protected override IReadOnlyCollection<AnswerDragNDrop> GetChildControls()
		{
			return TestControl.GetDragNDropList(QuestionId);
		}

		protected override string GetControlKey(AnswerDragNDrop loadedControl)
		{
			return loadedControl.AnswerId.ToString();
		}

		protected override void ControlsLoaded()
		{
			answers = Controls.Values.ToList();
			SetInfo();
			base.ControlsLoaded();
		}

		private void SetInfo()
		{
			Title.text = "Заполните пробелы";

			foreach (var answer in answers)
			{
				ParseToList(answer.TextInfo, answer.IsAnswerText, answer.Paragraph);
			}

			deltaString = GetSpaceLength();
			PrepareLines(stringContainer);

			// Создаем строки
			for (var i = 0; i < textLines.Count; i++)
			{
				var lineContainer = Instantiate(TestControlsSettings.Default().LineContainer).GetComponent<Image>();
				lineContainer.transform.SetParent(AnswerArea);
				lineContainer.transform.localScale = Vector3.one;
				lineContainer.transform.localPosition = Vector3.ProjectOnPlane(lineContainer.transform.localPosition, Vector3.forward);
				lineContainer.transform.localRotation = Quaternion.identity;
				lines.Add(lineContainer);
			}


			int currentPlaceholderNumber = 0;

			// Создаем текст и зоны
			for (int i = 0; i < textLines.Count; i++)
			{
				foreach (var word in textLines[i].LineData) //по идее щас работаем с конкретным словом в строке
				{
					Text someOutput;

					if (!word.IsAnswerText) // если слово не ответ, про просто выводим его
					{
						someOutput = Instantiate(TestControlsSettings.Default().TextObject).GetComponent<Text>();
						someOutput.transform.SetParent(lines[i].transform);
						someOutput.transform.localScale = Vector3.one;
						someOutput.transform.localPosition = Vector3.ProjectOnPlane(someOutput.transform.localPosition, Vector3.forward);
						someOutput.transform.localRotation = Quaternion.identity;

						someOutput.text = word.Text;
					}
					else //если слово ответ, то выводим его внизу со свойством Drag, а на его месте выводим прочерки со свойством Drop
					{

						// Drop Zone
						someOutput = Instantiate(TestControlsSettings.Default().PlaceholderText).GetComponent<Text>();//создаем место для пробелов
						someOutput.transform.SetParent(lines[i].transform);
						someOutput.transform.localScale = Vector3.one;
						someOutput.transform.localPosition = Vector3.ProjectOnPlane(someOutput.transform.localPosition, Vector3.forward);
						someOutput.transform.localRotation = Quaternion.identity;
						someOutput.text = "";

						for (int j = 0; j < deltaString; j++)
						{
							someOutput.text += "_"; //заполняем место пропусками
						}

						placeholders.Add(someOutput);
						placeholders[currentPlaceholderNumber].GetComponent<DropTextHandeler>().Placeholdernumber = currentPlaceholderNumber;

						// Drag Item
						var answer = Instantiate(TestControlsSettings.Default().DragTextHandeler).GetComponent<DragTextHandeler>();
						answer.transform.localRotation = Quaternion.identity;

						movableAnswers.Add(answer);
						movableAnswers[currentPlaceholderNumber].Number = currentPlaceholderNumber;
						answerValues.Add(word.Text);

						//answer.restrictToDropZone = true;
						currentPlaceholderNumber++;
					}
				}
			}

			RandomizeAnswerOrder();
		}

		public override bool CheckIfRight()
		{
			bool result = true;
			foreach (var placeholder in placeholders)
			{
				if (placeholder.GetComponent<DropTextHandeler>().IsPlaceholderFilled())
				{
					if (!placeholder.GetComponent<DropTextHandeler>().CheckPlaceholder())
					{
						result = false;
					}
				}
				else
				{
					result = false;
				}
			}

			IsAnswerRight = result;
			return result;
		}

		public override PassedQuestion SetInLog()
		{
			PassedQuestion passedQuestion = new PassedQuestion
			{
				QuestionId = Question.QuestionId,
				Answer = ""
			};

			foreach (var placeholder in placeholders)
			{
				if (placeholder.GetComponent<DropTextHandeler>().GetAnswer() != -1)
					passedQuestion.Answer += $"{placeholder.GetComponent<DropTextHandeler>().GetAnswer() + 1}, ";
			}

			if (passedQuestion.Answer.Length >= 2)
				passedQuestion.Answer = passedQuestion.Answer.Substring(0, passedQuestion.Answer.Length - 2);

			passedQuestion.IsRight = IsAnswerRight;
			return passedQuestion;

		}

		private void ParseToList(string tempAnswer, bool isQuestion, bool paragraph)
		{
			List<string> parsedString = tempAnswer.Split(' ').ToList();
			foreach (var smallString in parsedString)
			{
				stringContainer.Add(new SpaceFillData()
				{
					Text = smallString,
					IsAnswerText = isQuestion,
					IsEndOfParagraph = paragraph
				});
			}
		}

		/// <summary>
		/// Получить самую длинную строку с ответом
		/// </summary>
		/// <returns></returns>
		private int GetSpaceLength()
		{
			string longest = "";

			foreach (var someString in stringContainer)
			{
				if (someString.IsAnswerText && someString.Text.Length > longest.Length)
				{
					longest = someString.Text;
				}
			}
			return longest.Length * 2;
		}

		/// <summary>
		/// Разбитие списка слов на строки
		/// </summary>
		/// <param name="pTextContainer">Список слов</param>
		private void PrepareLines(List<SpaceFillData> pTextContainer)
		{
			var lineLength = 0;
			TextLine line;
			line.LineData = new List<SpaceFillData>();

			foreach (var word in pTextContainer)
			{
				int deltaLength;

				deltaLength = word.IsAnswerText ? word.Text.Length : deltaString;

				if (lineLength + deltaLength < 65)
				{
					line.LineData.Add(word);
					lineLength += deltaLength;

					if (word.IsEndOfParagraph)
					{
						textLines.Add(line);//заканчиваем работу со строкой

						line.LineData = new List<SpaceFillData>();//создаем новую строку
						lineLength = 0; //строка новая - ее длина равна 0
					}
				}
				else
				{
					textLines.Add(line);//kзаканчиваем работу со строкой

					line.LineData = new List<SpaceFillData>();//создаем новую строку
					lineLength = 0; //строка новая - ее длина равна 0

					//слово все еще нужно занести
					if ((lineLength + deltaLength) < 65) line.LineData.Add(word);
					lineLength += deltaLength;
				}


			}
			textLines.Add(line);
		}

		private void RandomizeAnswerOrder()
		{
			var randomNumbers = new List<int>();

			foreach (var movableAnswer in movableAnswers)
			{
				var temp = Random.Range(0, movableAnswers.Count);

				while (randomNumbers.Exists(member => member == temp))
				{
					temp = Random.Range(0, movableAnswers.Count);
				}

				randomNumbers.Add(temp);
			}

			var resultList = new List<DropTextHandeler>();

			for (int i = 0; i < movableAnswers.Count; i++)
			{
				resultList.Add(Instantiate(TestControlsSettings.Default().PlaceholderText).GetComponent<DropTextHandeler>());

				movableAnswers[randomNumbers[i]].Number = randomNumbers[i];

				resultList[i].transform.SetParent(VariantsContainer);
				resultList[i].transform.localPosition = Vector3.ProjectOnPlane(resultList[i].transform.localPosition, Vector3.forward);
				resultList[i].transform.localRotation = Quaternion.identity;
				resultList[i].transform.localScale = Vector3.one;

				movableAnswers[randomNumbers[i]].SetParent(resultList[i].GetComponent<RectTransform>());
				movableAnswers[randomNumbers[i]].transform.localPosition = Vector3.zero;

				if (answerValues[randomNumbers[i]].Length < 4)
				{
					answerValues[randomNumbers[i]].Insert(0, " ");
					answerValues[randomNumbers[i]] += " ";
				}

				resultList[i].GetComponent<Text>().text = answerValues[randomNumbers[i]];
				resultList[i].GetComponent<Text>().color = new Color(0.6f, 0.5f, 0.6f);

				movableAnswers[randomNumbers[i]].GetComponent<Text>().text = answerValues[randomNumbers[i]];

				// Добавляем в бэкграунд текста ответов
				backgroundText.Add(Instantiate(TestControlsSettings.Default().TextObject).GetComponent<Text>());
				backgroundText[i].transform.SetParent(BackgroundVariantsContainer);
				backgroundText[i].transform.position = resultList[i].transform.position;
				backgroundText[i].transform.localPosition = Vector3.ProjectOnPlane(backgroundText[i].transform.localPosition, Vector3.forward);
				backgroundText[i].transform.localScale = Vector3.one;
				backgroundText[i].text = answerValues[randomNumbers[i]];
				backgroundText[i].color = new Color(0.6f, 0.5f, 0.6f);
				backgroundText[i].fontSize = 28;
			}
		}

	}

}

