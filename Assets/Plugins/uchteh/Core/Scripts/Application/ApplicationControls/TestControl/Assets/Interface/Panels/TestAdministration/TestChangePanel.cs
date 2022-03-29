using System;
using System.Collections.Generic;
using System.Linq;
using Core.Extensions;
using UnityEngine;
using UnityEngine.UI;
using Core.Ui;
using TMPro;

namespace Core.Testing
{
	[RequireComponent(typeof(CanvasGroupHideAndShowBehavior))]
	public class TestChangePanel : Panel, ITestPanel
	{
		public event Action<Test> OnDeleteTest;
		public event Action<Question, bool> OnChangeQuestionButtonClick;
		public event Action<Question> OnDeleteQuestionButtonClick;
		public event Action<Test> OnSaveButtonClick;

		[SerializeField]
		private Table table = null;

		[SerializeField]
		private TMP_InputField testName = null, testDescription = null;

		[SerializeField]
		private Dropdown complicationDropdown = null;

		[SerializeField]
		private Text warningText = null;

		[SerializeField]
		private Button addQuestionButton = null, editQuestionButton = null, deleteQuestionButton = null;

		[SerializeField]
		private Button saveButton = null;

		[SerializeField]
		private Button closeButton = null;

		private Test curTest;
		private bool isNew;

		private List<Complication> complications;

		public override void Initialize()
		{
			base.Initialize();

			testName.onValueChanged.AddListener(ChangeTestName);
			testDescription.onValueChanged.AddListener(ChangeDescription);
			complicationDropdown.onValueChanged.AddListener(ChangeComplication);

			addQuestionButton.onClick.AddListener(AddQuestion);
			editQuestionButton.onClick.AddListener(EditQuestion);
			deleteQuestionButton.onClick.AddListener(DeleteQuestion);
			saveButton.onClick.AddListener(Save);
			closeButton.onClick.AddListener(Hide);
		}

		public void Show(Test test, bool isNewCount)
		{
			isNew = isNewCount;
			curTest = test;

			FillComplications();
			FillFields();
			FillTable();

			base.Show();
		}

		public override void Hide()
		{
			if (isNew)
			{
				OnDeleteTest?.Invoke(curTest);
			}
			base.Hide();
		}

		/// <summary>
		/// Заполнить сложности
		/// </summary>
		private void FillComplications()
		{
			complications = TestControl.GetComplications();
			complicationDropdown.ClearOptions();

			if (complications.Count != 0)
			{
				List<string> buttonNames = new List<string>();
				foreach (var complication in complications)
				{
					buttonNames.Add(complication.Name);
				}
				complicationDropdown.AddOptions(buttonNames);
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate(complicationDropdown.GetComponent<RectTransform>());
		}

		/// <summary>
		/// Заполнить поля значениями
		/// </summary>
		private void FillFields()
		{
			testName.text = curTest.Name;
			testDescription.text = curTest.Description;

			if (curTest.Complication == null)
			{
				ChangeComplication(0);
			}
			else
			{
				ChangeComplication(complications.FindIndex(c => c.ComplicationId == curTest.Complication.ComplicationId));
			}
		}

		/// <summary>
		/// Заполнить таблицу вопросами
		/// </summary>
		private void FillTable()
		{
			table.Clear();

			float cellSize = 100;

			if (curTest.Questions.Any())
			{
				foreach (var question in curTest.Questions)
				{
					string allAnswerText = "";
					string rightAnswerText = "";

					Button countCell = UiBuilder.CreateButton(table.transform.GetChild(0), Vector3.one, question.Name);
					countCell.GetComponentInChildren<RectTransform>().SetHeight(cellSize);

					Button allAnswerCell = null;
					Button rightAnswerCell = null;

					switch (question.TypeQuestion)
					{
						case Question.Type.CheckBox:
							{
								List<AnswerOfText> answersOfText = TestControl.GetAnswersText(question.QuestionId);

								foreach (var answerOfText in answersOfText)
								{
									allAnswerText += "• " + answerOfText.Name + "\n";
									if (answerOfText.IsCorrect)
									{
										rightAnswerText += "• " + answerOfText.Name + "\n";
									}
								}

								allAnswerCell = UiBuilder.CreateButton(table.transform.GetChild(1), Vector3.one, allAnswerText);
								allAnswerCell.GetComponentInChildren<RectTransform>().SetHeight(cellSize);

								rightAnswerCell = UiBuilder.CreateButton(table.transform.GetChild(2), Vector3.one, rightAnswerText);
								rightAnswerCell.GetComponentInChildren<RectTransform>().SetHeight(cellSize);

								break;
							}
						case Question.Type.RadioBox:
							{
								List<AnswerOfText> tempList = TestControl.GetAnswersText(question.QuestionId);

								foreach (var answerOfText in tempList)
								{
									allAnswerText += "• " + answerOfText.Name + "\n";
									if (answerOfText.IsCorrect)
									{
										rightAnswerText += "• " + answerOfText.Name + "\n";
									}
								}

								allAnswerCell = UiBuilder.CreateButton(table.transform.GetChild(1), Vector3.one, allAnswerText);
								allAnswerCell.GetComponentInChildren<RectTransform>().SetHeight(cellSize);

								rightAnswerCell = UiBuilder.CreateButton(table.transform.GetChild(2), Vector3.one, rightAnswerText);
								rightAnswerCell.GetComponentInChildren<RectTransform>().SetHeight(cellSize);
								break;
							}
						case Question.Type.Image:
							{
								allAnswerCell = UiBuilder.CreateButton(table.transform.GetChild(1), Vector3.one, allAnswerText);
								allAnswerCell.GetComponentInChildren<RectTransform>().SetHeight(cellSize);

								rightAnswerCell = UiBuilder.CreateButton(table.transform.GetChild(2), Vector3.one, rightAnswerText);
								rightAnswerCell.GetComponentInChildren<RectTransform>().SetHeight(cellSize);

								var allAnswerGrid = allAnswerCell.gameObject.AddComponent<GridLayoutGroup>();
								allAnswerGrid.spacing = new Vector2(10, 10);

								var rightAnswerGrid = rightAnswerCell.gameObject.AddComponent<GridLayoutGroup>();
								rightAnswerGrid.spacing = new Vector2(10, 10);

								List<AnswerImage> answerImages = TestControl.GetImageList(question.QuestionId);
								foreach (var answerImage in answerImages)
								{
									if (answerImage.IsCorrect)
									{
										Image rightAnswerImage = UiBuilder.CreateImage(rightAnswerGrid.transform, Vector3.one);
										TestControl.FillImage(rightAnswerImage, answerImage.ImageData);
									}

									Image allAnswerImage = UiBuilder.CreateImage(allAnswerGrid.transform, Vector3.one);
									TestControl.FillImage(allAnswerImage, answerImage.ImageData);
								}
								break;
							}
						case Question.Type.KeyPhrase:
							{
								List<AnswerOfText> answersOfText = TestControl.GetAnswersText(question.QuestionId);

								allAnswerText = "______";
								foreach (var answerOfText in answersOfText)
								{
									if (answerOfText.IsCorrect)
									{
										rightAnswerText += "• " + answerOfText.Name + "\n";
									}
								}

								allAnswerCell = UiBuilder.CreateButton(table.transform.GetChild(1), Vector3.one, allAnswerText);
								allAnswerCell.GetComponentInChildren<RectTransform>().SetHeight(cellSize);

								rightAnswerCell = UiBuilder.CreateButton(table.transform.GetChild(2), Vector3.one, rightAnswerText);
								rightAnswerCell.GetComponentInChildren<RectTransform>().SetHeight(cellSize);
								break;
							}
						case Question.Type.OrderList:
							{
								List<AnswerOrderList> answerOrderLists = TestControl.GetOrderList(question.QuestionId);
								answerOrderLists = TestControl.ShuffleOrderList(answerOrderLists);

								foreach (var answerOrderList in answerOrderLists)
								{
									allAnswerText += "• " + answerOrderList.StringValue + "\n";
								}

								answerOrderLists = answerOrderLists.OrderBy(w => w.StringOrder).ToList();
								foreach (var answerOfText in answerOrderLists)
								{
									rightAnswerText += "• " + answerOfText.StringValue + "\n";
								}

								allAnswerCell = UiBuilder.CreateButton(table.transform.GetChild(1), Vector3.one, allAnswerText);
								allAnswerCell.GetComponentInChildren<RectTransform>().SetHeight(cellSize);

								rightAnswerCell = UiBuilder.CreateButton(table.transform.GetChild(2), Vector3.one, rightAnswerText);
								rightAnswerCell.GetComponentInChildren<RectTransform>().SetHeight(cellSize);
								break;
							}
						case Question.Type.SpaceFill:
							{
								List<AnswerDragNDrop> answerDragNDrops = TestControl.GetDragNDropList(question.QuestionId);

								foreach (var answerDragNDrop in answerDragNDrops)
								{
									if (!answerDragNDrop.IsAnswerText)
									{
										allAnswerText += answerDragNDrop.TextInfo + " ";
										rightAnswerText += answerDragNDrop.TextInfo + " ";
									}
									else
									{
										allAnswerText += "______ ";
										rightAnswerText += "<b>" + answerDragNDrop.TextInfo + "</b>" + " ";
									}
								}

								allAnswerCell = UiBuilder.CreateButton(table.transform.GetChild(1), Vector3.one, allAnswerText);
								allAnswerCell.GetComponentInChildren<RectTransform>().SetHeight(cellSize);

								rightAnswerCell = UiBuilder.CreateButton(table.transform.GetChild(2), Vector3.one, rightAnswerText);
								rightAnswerCell.GetComponentInChildren<RectTransform>().SetHeight(cellSize);
								break;
							}
					}

					TableLine line = new TableLine(table, question, new List<Button>()
					{
						countCell,
						allAnswerCell,
						rightAnswerCell
					});
					table.AddLine(line);
				}
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate(table.GetComponent<RectTransform>());
		}
		
		/// <summary>
		/// Сохранить тест
		/// </summary>
		private void Save()
		{
			if (IsValid())
			{
				OnSaveButtonClick?.Invoke(curTest);
				isNew = false;
				Hide();
			}
		}

		/// <summary>
		/// Добавить вопрос для теста
		/// </summary>
		private void AddQuestion()
		{
			Question question = new Question()
			{
				TestId = curTest.TestId
			};

			OnChangeQuestionButtonClick?.Invoke(question, true);
		}

		/// <summary>
		/// Редактировать вопрос для теста
		/// </summary>
		private void EditQuestion()
		{
			if (table.CurLine != null)
			{
				OnChangeQuestionButtonClick?.Invoke(table.CurLine.TableObject, false);
			}
		}

		/// <summary>
		/// Удалить вопрос для теста
		/// </summary>
		private void DeleteQuestion()
		{
			if (table.CurLine != null)
			{
				OnDeleteQuestionButtonClick?.Invoke(table.CurLine.TableObject);
				FillTable();
			}
		}

		/// <summary>
		/// Изменить имя теста
		/// </summary>
		/// <param name="changeTestName"></param>
		private void ChangeTestName(string changeTestName)
		{
			curTest.Name = changeTestName;
		}

		/// <summary>
		/// Изменить описание теста
		/// </summary>
		/// <param name="changeTestDescription"></param>
		private void ChangeDescription(string changeTestDescription)
		{
			curTest.Description = changeTestDescription;
		}

		/// <summary>
		/// Изменить сложность теста
		/// </summary>
		/// <param name="id"></param>
		private void ChangeComplication(int id)
		{
			complicationDropdown.value = id;
			curTest.Complication = complications[id];
		}

		/// <summary>
		/// Проверить правильность заполнения
		/// </summary>
		/// <returns></returns>
		private bool IsValid()
		{
			bool result = true;
			warningText.text = "";

			if (string.IsNullOrEmpty(curTest.Name))
			{
				warningText.text = "Необходимо ввести название теста";
				result = false;
			}

			if (curTest.Complication == null)
			{
				warningText.text = "Не выбран уровень сложности";
				result = false;
			}

			return result;
		}
	}
}

