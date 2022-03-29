using System.Collections.Generic;
using System.Linq;
using Core.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Testing
{
	/// <summary>
	/// Единственное, чем отличается данный класс от QuestionRadio - наличие картинки (Вопросы типа: "Что изображено на картинке под номером 3" )
	/// </summary>
	public class ImageRadioBoxQuestionForm : QuestionForm<AnswerOfText>
	{
		private List<Toggle> toggleAnswers = new List<Toggle>();
		private ToggleGroup answerGroup;

		private List<AnswerOfText> answers;

		private struct GetFromDbImage
		{
			public byte[] ImageData;
		}

		/// <summary>
		/// Изображение, которое мы получаем
		/// </summary>
		private GetFromDbImage tempImage;

		/// <summary>
		/// Изображение, которое потом связывается с объектом на сцене в инспекторе
		/// </summary>
		public Image questionImage;

		public override void Load()
		{
			answerGroup = GetComponent<ToggleGroup>();
			base.Load();
		}

		protected override IReadOnlyCollection<AnswerOfText> GetChildControls()
		{
			return TestControl.GetAnswersText(QuestionId);
		}

		protected override string GetControlKey(AnswerOfText loadedControl)
		{
			return loadedControl.AnswerId.ToString();
		}

		protected override void ControlsLoaded()
		{
			answers = Controls.Values.ToList();
			tempImage = new GetFromDbImage
			{
				ImageData = TestControl.GetQuestionImage(QuestionId)
			};

			GetImage();
			SetInfo();
			base.ControlsLoaded();
		}
		
		private void GetImage()
		{
			Texture2D fillTexture = new Texture2D(1, 1);
			fillTexture.LoadImage(tempImage.ImageData);
			questionImage.GetComponent<RectTransform>().SetWidth(fillTexture.width);
			questionImage.GetComponent<RectTransform>().SetHeight(fillTexture.height);
			questionImage.sprite = Sprite.Create(fillTexture, new Rect(0, 0, fillTexture.width, fillTexture.height), new Vector2());
		}

		private void SetInfo()
		{
			Title.text = Question.Name;
			
			for (int i = 0; i < answers.Count; i++)
			{
				var answer = Instantiate(TestControlsSettings.Default().RadioBoxObject).GetComponent<Toggle>();
				answer.transform.SetParent(AnswerArea.transform);
				answer.transform.localScale = Vector3.one;
				answer.transform.localPosition = Vector3.ProjectOnPlane(answer.transform.localPosition, Vector3.forward);
				answer.transform.localRotation = Quaternion.identity;
				answer.group = answerGroup;
				answer.GetComponentInChildren<Text>().text = answers[i].Name;
				toggleAnswers.Add(answer);
			}
		}

		public override bool CheckIfRight()
		{
			IsAnswerRight = true;
			for (int i = 0; i < toggleAnswers.Count; i++)
			{
				if (toggleAnswers[i].isOn != answers[i].IsCorrect) IsAnswerRight = false;
			}
			return IsAnswerRight;
		}

		public override PassedQuestion SetInLog()
		{
			PassedQuestion question = new PassedQuestion(Question.QuestionId, "");

			for (int i = 0; i < toggleAnswers.Count; i++)
			{
				if (toggleAnswers[i].isOn)
				{
					question.Answer += $"{answers[i].Name}";
				}
			}
			question.IsRight = IsAnswerRight;
			return question;
		}
	}
}
