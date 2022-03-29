using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Ui;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Testing
{
	[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
	public class QuestionChangePanel : Panel, ITestPanel
	{
		[SerializeField]
		private TMP_InputField nameQuestionField = null;

		[SerializeField]
		private Dropdown questionTypeDropdown = null;

		[SerializeField]
		private Text warningText = null;

		[SerializeField]
		private Button addButton = null;

		[SerializeField]
		private Button saveButton = null;

		[SerializeField]
		private Button closeButton = null;

		[SerializeField]
		private string standartText = "";

		[SerializeField]
		private Sprite standartSprite = null;

		#region Вопросы

		#region Checkbox

		[SerializeField]
		private Transform checkBoxContent = null;
		private void CreateTextAnswer(AnswerOfText textAnswer, bool isNewAnswer)
		{
			var go = Instantiate(TestControlsSettings.Default().TextLinePrefab, checkBoxContent);

			if (!isNewAnswer)
			{
				go.transform.GetChild(0).GetComponent<InputField>().text = textAnswer.Name;
				go.transform.GetChild(0).GetComponent<InputField>().onValueChanged.AddListener(answerName =>
				{
					if (answers.Single(a => a == textAnswer) is AnswerOfText answer)
					{
						answer.Name = answerName;
					}

				});

				go.GetComponentInChildren<Toggle>().isOn = textAnswer.IsCorrect;
				go.GetComponentInChildren<Toggle>().onValueChanged.AddListener(isOn =>
				{
					if (answers.Single(a => a == textAnswer) is AnswerOfText answer)
					{
						answer.IsCorrect = isOn;
					}
				});
				go.GetComponentInChildren<Button>().onClick.AddListener(() =>
				{
					answers.Remove(textAnswer);
					Destroy(go);
				});
			}
			else
			{
				go.transform.GetChild(0).GetComponent<InputField>().text = standartText;
				go.transform.GetChild(0).GetComponent<InputField>().onValueChanged.AddListener(answerName =>
				{
					if (answers.Single(a => a == textAnswer) is AnswerOfText answer)
					{
						answer.Name = answerName;
					}

				});
				go.GetComponentInChildren<Toggle>().interactable = true;
				go.GetComponentInChildren<Toggle>().isOn = textAnswer.IsCorrect;
				go.GetComponentInChildren<Toggle>().onValueChanged.AddListener(isOn =>
				{
					if (answers.Single(a => a == textAnswer) is AnswerOfText answer)
					{
						answer.IsCorrect = isOn;
					}
				});
				go.GetComponentInChildren<Button>().interactable = true;
				go.GetComponentInChildren<Button>().onClick.AddListener(() =>
				{
					answers.Remove(textAnswer);
					Destroy(go);
				});
			}

			abstractListObject.Add(go);
			answers.Add(textAnswer);

			Rebuild();
		}

		#endregion

		#region Radiobox

		[SerializeField]
		private Transform radioBoxContent = null;

		private void CreateRadioAnswer(AnswerOfText textAnswer, bool isNewAnswer)
		{
			var go = Instantiate(TestControlsSettings.Default().RadioBoxPrefab, radioBoxContent);
			if (!isNewAnswer)
			{
				go.transform.GetChild(0).GetComponent<InputField>().text = textAnswer.Name;
				go.transform.GetChild(0).GetComponent<InputField>().onValueChanged.AddListener(answerName =>
				{
					if (answers.Single(a => a == textAnswer) is AnswerOfText answer)
					{
						answer.Name = answerName;
					}

				});
				go.GetComponentInChildren<Toggle>().isOn = textAnswer.IsCorrect;
				go.GetComponentInChildren<Toggle>().group = radioBoxContent.GetComponent<ToggleGroup>();
				go.GetComponentInChildren<Toggle>().onValueChanged.AddListener(isOn =>
				{
					if (answers.Single(a => a == textAnswer) is AnswerOfText answer)
					{
						answer.IsCorrect = isOn;
					}
				});
				go.GetComponentInChildren<Button>().onClick.AddListener(() =>
				{
					answers.Remove(textAnswer);
					Destroy(go);
				});
			}
			else
			{
				go.transform.GetChild(0).GetComponent<InputField>().text = standartText;
				go.transform.GetChild(0).GetComponent<InputField>().onValueChanged.AddListener(answerName =>
				{
					if (answers.Single(a => a == textAnswer) is AnswerOfText answer)
					{
						answer.Name = answerName;
					}

				});
				go.GetComponentInChildren<Toggle>().isOn = textAnswer.IsCorrect;
				go.GetComponentInChildren<Toggle>().group = radioBoxContent.GetComponent<ToggleGroup>();
				go.GetComponentInChildren<Toggle>().onValueChanged.AddListener(isOn =>
				{
					if (answers.Single(a => a == textAnswer) is AnswerOfText answer)
					{
						answer.IsCorrect = isOn;
					}
				});
				go.GetComponentInChildren<Button>().onClick.AddListener(() =>
				{
					answers.Remove(textAnswer);
					Destroy(go);
				});
			}

			abstractListObject.Add(go);
			answers.Add(textAnswer);

			Rebuild();
		}

		#endregion

		#region Image

		[SerializeField]
		private Transform imageContent = null;
		
		private void CreateImageAnswer(AnswerImage imageAnswer, bool isNewAnswer)
		{
			var go = Instantiate(TestControlsSettings.Default().ImagePrefab, imageContent);
			if (!isNewAnswer)
			{
				go.transform.GetChild(0).GetComponent<Image>().sprite = TestControl.FillAnswerImageSprite(imageAnswer.ImageData);
				go.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
				{
					string filename;
					var data = GetAnswerImage(out filename);
					AnswerImage answer = answers.Single(a => a == imageAnswer) as AnswerImage;
					if (answer != null)
					{
						answer.ImageName = filename;
						answer.ImageData = data;
					}
					go.transform.GetChild(0).GetComponent<Image>().sprite = TestControl.FillAnswerImageSprite(data);
				});
				go.GetComponentInChildren<Toggle>().isOn = imageAnswer.IsCorrect;
				go.GetComponentInChildren<Toggle>().onValueChanged.AddListener(isOn =>
				{
					AnswerImage answer = answers.Single(a => a == imageAnswer) as AnswerImage;
					if (answer != null)
					{
						answer.IsCorrect = isOn;
					}
				});
				go.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
				{
					answers.Remove(imageAnswer);
					Destroy(go);
				});
			}
			else
			{
				go.transform.GetChild(0).GetComponent<Image>().sprite = standartSprite;
				go.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
				{
					string filename;
					var data = GetAnswerImage(out filename);
					AnswerImage answer = answers.Single(a => a == imageAnswer) as AnswerImage;
					if (answer != null)
					{
						answer.ImageName = filename;
						answer.ImageData = data;
					}
					go.transform.GetChild(0).GetComponent<Image>().sprite = TestControl.FillAnswerImageSprite(data);
				});
				go.GetComponentInChildren<Toggle>().isOn = imageAnswer.IsCorrect;
				go.GetComponentInChildren<Toggle>().onValueChanged.AddListener(isOn =>
				{
					AnswerImage answer = answers.Single(a => a == imageAnswer) as AnswerImage;
					if (answer != null)
					{
						answer.IsCorrect = isOn;
					}
				});
				go.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
				{
					answers.Remove(imageAnswer);
					Destroy(go);
				});
			}

			abstractListObject.Add(go);
			answers.Add(imageAnswer);

			Rebuild();
		}

		private byte[] GetAnswerImage(out string shortFileName)
		{

			var extensions = new[] {
			new ExtensionFilter("Image Files", "png", "jpg", "jpeg" )
		};
			var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);

			shortFileName = path[0].Substring(path[0].LastIndexOf('\\') + 1);
			byte[] imageData;

			using (FileStream fs = new FileStream(path[0], FileMode.Open))
			{
				imageData = new byte[fs.Length];
				fs.Read(imageData, 0, imageData.Length);
			}

			return imageData;
		}

		#endregion

		#region OrderList

		[SerializeField]
		private Transform orderContent = null;
		
		private void CreateOrderAnswer(AnswerOrderList answerOrderList, bool isNewAnswer)
		{
			var go = Instantiate(TestControlsSettings.Default().OrderPrefab, orderContent);
			if (!isNewAnswer)
			{
				go.transform.GetChild(0).GetComponent<InputField>().text = answerOrderList.StringValue;
				go.transform.GetChild(0).GetComponent<InputField>().onValueChanged.AddListener(text =>
				{
					AnswerOrderList answer = answers.Single(a => a == answerOrderList) as AnswerOrderList;
					if (answer != null)
					{
						answer.StringValue = text;
					}

				});
				go.transform.GetChild(1).GetComponent<InputField>().text = answerOrderList.StringOrder + "";
				go.transform.GetChild(1).GetComponent<InputField>().onValueChanged.AddListener(order =>
				{
					AnswerOrderList answer = answers.Single(a => a == answerOrderList) as AnswerOrderList;
					if (answer != null)
					{
						answer.StringOrder = int.Parse(order);
					}

				});
				go.GetComponentInChildren<Button>().onClick.AddListener(() =>
				{
					answers.Remove(answerOrderList);
					Destroy(go);
				});
			}
			else
			{
				go.transform.GetChild(0).GetComponent<Image>().sprite = standartSprite;
				go.transform.GetChild(0).GetComponent<InputField>().onValueChanged.AddListener(text =>
				{
					AnswerOrderList answer = answers.Single(a => a == answerOrderList) as AnswerOrderList;
					if (answer != null)
					{
						answer.StringValue = text;
					}

				});
				go.transform.GetChild(1).GetComponent<InputField>().text = "";
				go.transform.GetChild(1).GetComponent<InputField>().onValueChanged.AddListener(order =>
				{
					AnswerOrderList answer = answers.Single(a => a == answerOrderList) as AnswerOrderList;
					if (answer != null)
					{
						answer.StringOrder = int.Parse(order);
					}

				});
				go.GetComponentInChildren<Button>().onClick.AddListener(() =>
				{
					answers.Remove(answerOrderList);
					Destroy(go);
				});
			}
			abstractListObject.Add(go);
			answers.Add(answerOrderList);

			Rebuild();
		}

		#endregion

		#region Drag&Drop

		[SerializeField]
		private Transform drugAndDropContent = null;
		
		private void CreateDrugAndDropAnswer(AnswerDragNDrop answerDragNDrop, bool isNewAnswer)
		{
			var go = Instantiate(TestControlsSettings.Default().DrugAndDropPrefab, drugAndDropContent);
			if (!isNewAnswer)
			{
				var image = go.transform.GetComponent<Image>();
				var button = go.transform.GetComponent<Button>();
				button.GetComponentInChildren<Text>().text = answerDragNDrop.TextInfo;
				image.color = answerDragNDrop.IsAnswerText ? Color.green : Color.white;

				button.onClick.AddListener((() =>
				{
					answerDragNDrop.IsAnswerText = !answerDragNDrop.IsAnswerText;
					image.color = answerDragNDrop.IsAnswerText ? Color.green : Color.white;
				}));
			}
			else
			{
				var image = go.transform.GetComponent<Image>();
				var button = go.transform.GetComponent<Button>();
				button.GetComponentInChildren<Text>().text = answerDragNDrop.TextInfo;
				image.color = answerDragNDrop.IsAnswerText ? Color.green : Color.white;

				button.onClick.AddListener((() =>
				{
					answerDragNDrop.IsAnswerText = !answerDragNDrop.IsAnswerText;
					image.color = answerDragNDrop.IsAnswerText ? Color.green : Color.white;
				}));
			}

			abstractListObject.Add(go);
			answers.Add(answerDragNDrop);

			Rebuild();
		}

		#endregion

		#region KeyPhrase

		private void CreateKeyPhrase(AnswerOfText textAnswer, bool isNewAnswer)
		{
			if (checkBoxContent.childCount != 0)
			{
				return;
			}

			var go = Instantiate(TestControlsSettings.Default().TextLinePrefab, checkBoxContent);
			if (!isNewAnswer)
			{
				go.transform.GetChild(0).GetComponent<InputField>().text = textAnswer.Name;
				go.transform.GetChild(0).GetComponent<InputField>().onValueChanged.AddListener(answerName =>
				{
					if (answers.Single(a => a == textAnswer) is AnswerOfText answer)
					{
						answer.Name = answerName;
					}

				});
				go.GetComponentInChildren<Toggle>().interactable = false;
				go.GetComponentInChildren<Button>().onClick.AddListener(() =>
				{
					answers.Remove(textAnswer);
					Destroy(go);
				});
			}
			else
			{
				go.transform.GetChild(0).GetComponent<InputField>().text = standartText;
				go.transform.GetChild(0).GetComponent<InputField>().onValueChanged.AddListener(answerName =>
				{
					//по правкам нужно запретить ввод прописными буквами, только строчными. Переводим буквы в нижний регистр:
					var lowerString = go.transform.GetChild(0).GetComponent<InputField>().text.ToLower();

					go.transform.GetChild(0).GetComponent<InputField>().text = lowerString;

					if (answers.Single(a => a == textAnswer) is AnswerOfText answer)
					{
						answer.Name = lowerString;
					}

				});
				go.GetComponentInChildren<Toggle>().interactable = false;
				go.GetComponentInChildren<Button>().interactable = true;
				go.GetComponentInChildren<Button>().onClick.AddListener(() =>
				{
					answers.Remove(textAnswer);
					Destroy(go);
				});
			}

			abstractListObject.Add(go);
			answers.Add(textAnswer);

			Rebuild();
		}

		#endregion

		#endregion

	


		private List<Question.Type> types;
		private Question curQuestion;
		private bool isNew;

		[ShowInInspector]
		private List<Answer> answers = new List<Answer>();
		private List<GameObject> abstractListObject = new List<GameObject>();


		public override void Initialize()
		{
			base.Initialize();

			nameQuestionField.onValueChanged.AddListener(ChangeName);
			questionTypeDropdown.onValueChanged.AddListener(ChangeTypeQuestion);

			addButton.onClick.AddListener(AddAnswer);
			saveButton.onClick.AddListener(Save);
			closeButton.onClick.AddListener(Hide);
		}

		public void Show(Question question, bool isNewCount)
		{

			curQuestion = question;
			isNew = isNewCount;
			nameQuestionField.text = curQuestion.Name;

			FillType();

			if (isNew)
			{
				FillNewQuestion();
			}
			else
			{
				FillEditQuestion();
			}

			base.Show();
		}

		public override void Hide()
		{
			nameQuestionField.text = "";
			warningText.text = "";

			ClearTable();
			
			base.Hide();
		}

		/// <summary>
		/// Заполнить типы вопроса
		/// </summary>
		private void FillType()
		{
			questionTypeDropdown.ClearOptions();

			List<string> buttonNames = new List<string>();

			types = Enum.GetValues(typeof(Question.Type)).Cast<Question.Type>().ToList();

			foreach (var type in types)
			{
				switch (type)
				{
					case Question.Type.Image:
						{
							buttonNames.Add("Вопрос с выбором правильного изображения.");
							break;
						}
					case Question.Type.CheckBox:
						{
							buttonNames.Add("Вопрос с выбором нескольких вариантов ответа.");
							break;
						}
					case Question.Type.RadioBox:
						{
							buttonNames.Add("Вопрос с выбором одного правильного ответа.");
							break;
						}
					case Question.Type.KeyPhrase:
						{
							buttonNames.Add("Вопрос с вводом текста.");
							break;
						}
					case Question.Type.SpaceFill:
						{
							buttonNames.Add("Вопрос с заполнением пропусков в тексте.");
							break;
						}
					case Question.Type.OrderList:
						{
							buttonNames.Add("Вопрос с восстановлением последовательности ответов в правильном порядке.");
							break;
						}
					default:
						buttonNames.Add(type.ToString());
						break;
				}
			}

			questionTypeDropdown.AddOptions(buttonNames);

			if (isNew)
			{
				questionTypeDropdown.value = 0;
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate(questionTypeDropdown.GetComponent<RectTransform>());

		}

		/// <summary>
		/// Заполнить таблицу с новым вопросом
		/// </summary>
		private void FillNewQuestion()
		{
			ClearTable();

			switch (curQuestion.TypeQuestion)
			{
				case Question.Type.Image:
					DisableAllContentAccept(imageContent);
					DestroyAllChild(imageContent);
					break;
				case Question.Type.CheckBox:
					DisableAllContentAccept(checkBoxContent);
					DestroyAllChild(checkBoxContent);
					break;
				case Question.Type.RadioBox:
					DisableAllContentAccept(radioBoxContent);
					DestroyAllChild(radioBoxContent);
					break;
				case Question.Type.KeyPhrase:
					DisableAllContentAccept(checkBoxContent);
					DestroyAllChild(checkBoxContent);
					break;
				case Question.Type.SpaceFill:
					DisableAllContentAccept(drugAndDropContent);
					DestroyAllChild(drugAndDropContent);
					break;
				case Question.Type.OrderList:
					DisableAllContentAccept(orderContent);
					DestroyAllChild(orderContent);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponentInParent<RectTransform>());
			Rebuild();
		}

		/// <summary>
		/// Заполнить таблицу с сушествующем вопросом
		/// </summary>
		private void FillEditQuestion()
		{
			ClearTable();

			questionTypeDropdown.value = types.FindIndex(t => t == curQuestion.TypeQuestion);

			switch (curQuestion.TypeQuestion)
			{
				case Question.Type.CheckBox:
					{
						DisableAllContentAccept(checkBoxContent);
						DestroyAllChild(checkBoxContent);

						List<AnswerOfText> tempList = TestControl.GetAnswersText(curQuestion.QuestionId);
						foreach (var currentAnswer in tempList)
						{
							CreateTextAnswer(currentAnswer, false);
						}
						break;
					}
				case Question.Type.Image:
					{
						DisableAllContentAccept(imageContent);
						DestroyAllChild(imageContent);

						List<AnswerImage> tempList = TestControl.GetImageList(curQuestion.QuestionId);
						foreach (var currentAnswer in tempList)
						{
							CreateImageAnswer(currentAnswer, false);
						}
						break;
					}
				case Question.Type.KeyPhrase:
					{
						DisableAllContentAccept(checkBoxContent);
						DestroyAllChild(checkBoxContent);

						List<AnswerOfText> tempList = TestControl.GetAnswersText(curQuestion.QuestionId);
						foreach (var currentAnswer in tempList)
						{
							CreateKeyPhrase(currentAnswer, false);
						}
						break;
					}
				case Question.Type.OrderList:
					{
						DisableAllContentAccept(orderContent);
						DestroyAllChild(orderContent);

						List<AnswerOrderList> tempList = TestControl.GetOrderList(curQuestion.QuestionId);
						foreach (var currentAnswer in tempList)
						{
							CreateOrderAnswer(currentAnswer, false);
						}
						break;
					}
				case Question.Type.RadioBox:
					{
						DisableAllContentAccept(radioBoxContent);
						DestroyAllChild(radioBoxContent);

						List<AnswerOfText> tempList = TestControl.GetAnswersText(curQuestion.QuestionId);
						foreach (var currentAnswer in tempList)
						{
							CreateRadioAnswer(currentAnswer, false);
						}
						break;
					}
				case Question.Type.SpaceFill:
					{
						DisableAllContentAccept(drugAndDropContent);
						DestroyAllChild(drugAndDropContent);

						List<AnswerDragNDrop> tempList = TestControl.GetDragNDropList(curQuestion.QuestionId);
						foreach (var currentAnswer in tempList)
						{
							CreateDrugAndDropAnswer(currentAnswer, false);
						}
						break;
					}
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponentInParent<RectTransform>());
			Rebuild();
		}

		/// <summary>
		/// Выключить все панели кроме одной
		/// </summary>
		/// <param name="thisContent"></param>
		private void DisableAllContentAccept(Transform thisContent)
		{

			checkBoxContent.gameObject.SetActive(false);
			drugAndDropContent.gameObject.SetActive(false);
			orderContent.gameObject.SetActive(false);
			imageContent.gameObject.SetActive(false);
			radioBoxContent.gameObject.SetActive(false);

			thisContent.gameObject.SetActive(true);
		}

		/// <summary>
		/// Удалить все дочерние объекты 
		/// </summary>
		/// <param name="currentTransform"></param>
		private void DestroyAllChild(Transform currentTransform)
		{
			foreach (Transform child in currentTransform.transform)
			{
				Destroy(child.gameObject);
			}
		}

		/// <summary>
		/// Удалить все дочерние объекты
		/// </summary>
		private void DestroyAllChild()
		{
			DestroyAllChild(checkBoxContent);
			DestroyAllChild(orderContent);
			DestroyAllChild(radioBoxContent);
			DestroyAllChild(drugAndDropContent);
		}

		/// <summary>
		/// Перезагрузить таблицу
		/// </summary>
		private void Rebuild()
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(checkBoxContent.GetComponent<RectTransform>());
			LayoutRebuilder.ForceRebuildLayoutImmediate(imageContent.GetComponent<RectTransform>());
			LayoutRebuilder.ForceRebuildLayoutImmediate(orderContent.GetComponent<RectTransform>());
			LayoutRebuilder.ForceRebuildLayoutImmediate(drugAndDropContent.GetComponent<RectTransform>());
			LayoutRebuilder.ForceRebuildLayoutImmediate(radioBoxContent.GetComponent<RectTransform>());
		}

		/// <summary>
		/// Очистить таблицу
		/// </summary>
		private void ClearTable()
		{
			DestroyAllChild();

			if (abstractListObject.Any())
			{
				foreach (var cellObject in abstractListObject)
				{
					Destroy(cellObject);
				}
			}
			abstractListObject.Clear();
			answers.Clear();
		}


		/// <summary>
		/// Сохранить вопрос
		/// </summary>
		private void Save()
		{
			if (IsValid())
			{

				if (isNew)
				{
					int idQuestion = TestControl.AddQuestion(curQuestion);

					foreach (var answer in answers)
					{
						answer.QuestionId = idQuestion;
						answer.AddInDatabase();
					}
				}
				else
				{
					Question question = new Question()
					{
						TestId = curQuestion.TestId,
						Name = curQuestion.Name,
						TypeQuestion = curQuestion.TypeQuestion
					};

					TestControl.DeleteQuestion(curQuestion);
					int idQuestion = TestControl.AddQuestion(question);

					foreach (var answer in answers)
					{
						answer.QuestionId = idQuestion;
						answer.AddInDatabase();
					}
				}

				Hide();
			}
		}

		/// <summary>
		/// Изменить название вопроса
		/// </summary>
		/// <param name="nameQuestion"></param>
		private void ChangeName(string nameQuestion)
		{
			curQuestion.Name = nameQuestion;
		}

		/// <summary>
		/// Изменить тип вопроса
		/// </summary>
		/// <param name="type"></param>
		private void ChangeTypeQuestion(int type)
		{
			curQuestion.TypeQuestion = (Question.Type)type;
			FillNewQuestion();
		}

		/// <summary>
		/// Добавить новый ответ
		/// </summary>
		private void AddAnswer()
		{
			switch (curQuestion.TypeQuestion)
			{
				case Question.Type.CheckBox:
					{
						var preparedAnswText = new AnswerOfText()
						{
							IsCorrect = false,
							IsPhraseAnswer = false,
							Name = "",
						};
						CreateTextAnswer(preparedAnswText, true);
						break;
					}
				case Question.Type.Image:
					{
						var preparedImage = new AnswerImage()
						{
							IsCorrect = false,
						};
						CreateImageAnswer(preparedImage, true);
						break;
					}
				case Question.Type.KeyPhrase:
					{
						var preparedAnswText = new AnswerOfText()
						{
							IsCorrect = true,
							IsPhraseAnswer = true,
							Name = "",
						};
						CreateKeyPhrase(preparedAnswText, true);
						break;
					}
				case Question.Type.OrderList:
					{
						var preparedAnswer = new AnswerOrderList()
						{
							StringOrder = 0,
							StringValue = "",
						};
						CreateOrderAnswer(preparedAnswer, true);
						break;
					}
				case Question.Type.RadioBox:
					{
						var preparedAnswText = new AnswerOfText()
						{
							IsCorrect = false,
							Name = "",
							IsPhraseAnswer = false
						};
						CreateRadioAnswer(preparedAnswText, true);
						break;
					}
				case Question.Type.SpaceFill:
					{
						ClearTable();
						List<string> paragraphs = curQuestion.Name.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

						for (int index = 0; index < paragraphs.Count; index++)
						{
							List<string> listWord = paragraphs[index].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();

							for (int i = 0; i < listWord.Count; i++)
							{
								bool paragraph = i == listWord.Count - 1;

								if (index == paragraphs.Count - 1)
								{
									paragraph = false;
								}

								var preparedAnswer = new AnswerDragNDrop()
								{
									TextIndex = i,
									TextInfo = listWord[i],
									IsAnswerText = false,
									Paragraph = paragraph,
								};
								CreateDrugAndDropAnswer(preparedAnswer, true);
							}


						}

						break;
					}
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponentInParent<RectTransform>());
		}

		/// <summary>
		/// Проверить правильность заполнения формы
		/// </summary>
		/// <returns></returns>
		private bool IsValid()
		{
			bool valid = true;

			warningText.text = "";

			if (string.IsNullOrEmpty(curQuestion.Name))
			{
				warningText.text = "Не указано название вопроса \n";
				valid = false;
			}

			if (!answers.Any())
			{
				warningText.text += "Необходим хотя бы один вариант ответа \n";
				valid = false;
			}

			foreach (var answer in answers)
			{
				if (string.IsNullOrEmpty(answer.GetName()))
				{
					warningText.text += "Не у всех ответов указаны названия";
					valid = false;
				}
			}


			// у ответа с порядковым номером он должен быть указан и не должен уже существовать
			if (curQuestion.TypeQuestion == Question.Type.OrderList)
			{
				foreach (var answer in answers)
				{
					if (answer is AnswerOrderList tempAnswer)
					{
						List<AnswerOrderList> answersWithOrders = answers.OfType<AnswerOrderList>().Where(a => a.StringOrder == tempAnswer.StringOrder).ToList();

						if (answersWithOrders.Count > 1)
						{
							warningText.text += "Указаны ответы с одинаковым порядковым номером";
							valid = false;
							break;
						}
					}
				}
			}

			return valid;
		}
	}
}
