using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Testing
{
	/// <summary>
	/// Класс вопросов с вводом правильного ответа в текстовое поле
	/// </summary>
	public class EnterAnswerQuestionForm : QuestionForm<AnswerOfText>
	{
		private InputField inputAnswer;
		private string enteredAnswer = "";

		private AnswerOfText currentAnswer;

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
			currentAnswer = Controls.Values.Single(a => a.IsCorrect);
			SetInfo();
			base.ControlsLoaded();
		}
		
		private void SetInfo()
		{
			Title.text = Question.Name;

			inputAnswer = Instantiate(TestControlsSettings.Default().InputFieldObject).GetComponent<InputField>();
			inputAnswer.transform.SetParent(AnswerArea.transform);
			inputAnswer.transform.localScale = Vector3.one;
			inputAnswer.transform.localPosition = Vector3.ProjectOnPlane(inputAnswer.transform.localPosition, Vector3.forward);
			inputAnswer.transform.localRotation = Quaternion.identity;
			inputAnswer.onValueChanged.AddListener(text => { enteredAnswer = text; });
		}

		public override bool CheckIfRight()
		{
			IsAnswerRight = true;

			float parsedEnteredAnswer;
			float parsedRightAnswer;

			var dotProvider = new NumberFormatInfo
			{
				CurrencyDecimalSeparator = "."
			};

			//проверяем, является ли введенным ответом число
			if ((Single.TryParse(enteredAnswer, out parsedEnteredAnswer) || Single.TryParse(enteredAnswer,
					 NumberStyles.AllowDecimalPoint, dotProvider, out parsedEnteredAnswer))
				&& (Single.TryParse(currentAnswer.Name, out parsedRightAnswer) || Single.TryParse(currentAnswer.Name,
						NumberStyles.AllowDecimalPoint, dotProvider, out parsedRightAnswer)))
			{
				if (Mathf.Abs(parsedEnteredAnswer - parsedRightAnswer) > Mathf.Epsilon)
					IsAnswerRight = false;
			}
			else
			{
				if (!String.Equals(enteredAnswer, currentAnswer.Name, StringComparison.CurrentCultureIgnoreCase))
					IsAnswerRight = false;
			}

			return IsAnswerRight;
		}

		public override PassedQuestion SetInLog()
		{
			var passedQuestion = new PassedQuestion(Question.QuestionId, enteredAnswer, IsAnswerRight);
			return passedQuestion;
		}
	}

}