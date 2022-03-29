using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Testing
{
	/// <summary>
	/// Класс вопросов с выбором более чем одного варианта ответа
	/// </summary>
	public class ToggleAnswerQuestionForm : QuestionForm<AnswerOfText>
	{
		private List<Toggle> toggleAnswers = new List<Toggle>();
		private List<AnswerOfText> answers = new List<AnswerOfText>();
		
		protected bool IsAnswerEntered;


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
			SetInfo();
			base.ControlsLoaded();
		}


		private void SetInfo()
		{
			Title.text = Question.Name;

			foreach (var answer in answers)
			{
				var item = Instantiate(TestControlsSettings.Default().ToggleObject).GetComponent<Toggle>();
				item.transform.SetParent(AnswerArea.transform);
				item.transform.localScale = Vector3.one;
				item.transform.localPosition = Vector3.ProjectOnPlane(item.transform.localPosition, Vector3.forward);
				item.transform.localRotation = Quaternion.identity;

				if (item.GetComponentInChildren<Text>())
				{
					item.GetComponentInChildren<Text>().text = answer.Name;
				}

				if (item.GetComponentInChildren<TMP_Text>())
				{
					item.GetComponentInChildren<TMP_Text>().text = answer.Name;
				}

				toggleAnswers.Add(item);
			}
		}

		public override bool CheckIfRight()
		{
			IsAnswerRight = true;
			for (int i = 0; i < answers.Count; i++)
			{
				if (toggleAnswers[i].isOn != answers[i].IsCorrect) IsAnswerRight = false;
			}
			return IsAnswerRight;
		}

		public void HighlightRightAnswer()
		{
			IsAnswerRight = true;
			IsAnswerEntered = true;
			for (int i = 0; i < answers.Count; i++)
			{
				if (toggleAnswers[i].isOn && answers[i].IsCorrect) toggleAnswers[i].GetComponentInChildren<Text>().color = Color.green;

				else if (toggleAnswers[i].isOn && (answers[i].IsCorrect == false))
				{
					toggleAnswers[i].GetComponentInChildren<Text>().color = Color.red;
					IsAnswerRight = false;
				}
				else
				{
					IsAnswerEntered = false;
				}
			}
		}

		public override PassedQuestion SetInLog()
		{
			var passedQuestion = new PassedQuestion()
			{
				QuestionId = Question.QuestionId,
				Answer = ""
			};

			for (int i = 0; i < toggleAnswers.Count; i++)
			{
				if (toggleAnswers[i].isOn)
				{
					passedQuestion.Answer += $"{answers[i].Name}, ";
				}
			}

			if (passedQuestion.Answer.Length >= 2)
				passedQuestion.Answer = passedQuestion.Answer.Substring(0, passedQuestion.Answer.Length - 2);

			passedQuestion.IsRight = IsAnswerRight;
			return passedQuestion;
		}
	}
}