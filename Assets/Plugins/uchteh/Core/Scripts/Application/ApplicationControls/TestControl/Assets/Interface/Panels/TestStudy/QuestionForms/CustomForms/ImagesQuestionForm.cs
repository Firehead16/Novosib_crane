using System.Collections.Generic;
using System.Linq;
using Core.Extensions;
using Core.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Testing
{

	/// <summary>
	/// Вспомогательный класс для корректного нажатия на изображения.
	/// </summary>
	public class ButtonData
	{
		public int ButtonId;
		public Button ImageAnswer;
		public Image SelectedImage;

		public bool IsPressed { get; private set; }

		public ButtonData(int buttonId, Button imageAnswer, Image selectedImage)
		{
			ButtonId = buttonId;
			ImageAnswer = imageAnswer;
			SelectedImage = selectedImage;
		}

		/// <summary>
		/// Вызывается при нажатии на кнопку
		/// </summary>
		public void ChangeState()
		{
			IsPressed = !IsPressed;
		}
	}

	/// <summary>
	/// Класс вопросов, связанных с выбором правильной картинки. Связан с классом AnswerImage в папке Answer
	/// </summary>
	public class ImagesQuestionForm : QuestionForm<AnswerImage>
	{
		private List<ButtonData> imageAnswers = new List<ButtonData>();
		private List<AnswerImage> answers;
		private int questionVar;


		protected override IReadOnlyCollection<AnswerImage> GetChildControls()
		{
			return TestControl.GetImageList(QuestionId);
		}

		protected override string GetControlKey(AnswerImage loadedControl)
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
			Title.text = Question.Name;

			questionVar = answers.Count;
			for (int i = 0; i < questionVar; i++)
			{
				var someButton = UiBuilder.CreateButton(AnswerArea.transform, Vector3.one, "");
				someButton.transform.localPosition = Vector3.ProjectOnPlane(someButton.transform.localPosition, Vector3.forward);
				someButton.transform.localRotation = Quaternion.identity;

				var selectedImage = UiBuilder.CreateImage(someButton.transform, Vector3.one);
				selectedImage.transform.localPosition = Vector3.ProjectOnPlane(someButton.transform.localPosition, Vector3.forward);
				selectedImage.GetComponent<RectTransform>().SetWidth(300);
				selectedImage.GetComponent<RectTransform>().SetHeight(300);
				selectedImage.sprite = null;
				selectedImage.color = new Color(255,255,255,0.2f);

				TestControl.FillImage(someButton.GetComponent<Button>().image, answers[i].ImageData);

				if (someButton.GetComponentInChildren<Text>())
				{
					someButton.GetComponentInChildren<Text>().text = "";
				}

				if (someButton.GetComponentInChildren<TMP_Text>())
				{
					someButton.GetComponentInChildren<TMP_Text>().text = "";
				}

				someButton.name = answers[i].ImageName;
				imageAnswers.Add(new ButtonData(i, someButton.GetComponent<Button>(), selectedImage));
			}

			foreach (var button in imageAnswers)
			{
				button.ImageAnswer.onClick.AddListener(() => ButtonClick(button.ButtonId));
			}
		}
		
		public override bool CheckIfRight()
		{
			for (int i = 0; i < questionVar; i++)
			{
				if (imageAnswers[i].IsPressed != answers[i].IsCorrect)
					return false;
			}
			return true;
		}

		public override PassedQuestion SetInLog()
		{
			var passedQuestion = new PassedQuestion
			{
				QuestionId = Question.QuestionId,
				Answer = ""
			};

			for (int i = 0; i < imageAnswers.Count; i++)
			{
				if (imageAnswers[i].IsPressed)
				{
					passedQuestion.Answer += $"{answers[i].ImageName}, ";
				}
			}
			if (passedQuestion.Answer.Length >= 2)
				passedQuestion.Answer = passedQuestion.Answer.Substring(0, passedQuestion.Answer.Length - 2);
			passedQuestion.IsRight = CheckIfRight();
			return passedQuestion;
		}

		private void ButtonClick(int pressedButtonId)
		{
			foreach (var someButton in imageAnswers)
			{
				if (someButton.ButtonId == pressedButtonId)
				{
					someButton.ChangeState();
					someButton.SelectedImage.color = someButton.IsPressed ? new Color(0.423f, 0.423f, 0.423f, 0.2f) : new Color(255, 255, 255, 0.2f);
				}
			}
		}
	}

}