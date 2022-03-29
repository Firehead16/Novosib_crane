using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Testing
{
	/// <summary>
	/// Класс вопросов с выбором только одного варианта ответа.
	/// </summary>
	 [RequireComponent(typeof(ToggleGroup))]
	public class RadioBoxQuestionForm : QuestionForm<AnswerOfText>
	{
		private List<Toggle> toggleAnswers = new List<Toggle>();
		private ToggleGroup thisAnswerGroup;

		private List<AnswerOfText> answers;
		private int questionVar;


		public override void Load()
		{
			thisAnswerGroup = GetComponent<ToggleGroup>();
			base.Load();
		}

		protected override IReadOnlyCollection<AnswerOfText> GetChildControls()
		{
			return  TestControl.GetAnswersText(QuestionId);
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

			//Узнаем, сколько вариантов у вопроса, нужно будет в дальнейшем для отображения разного количества вариантов ответа
			questionVar = answers.Count; 

			for (int i = 0; i < questionVar; i++)
			{
				var answer = Instantiate(TestControlsSettings.Default().RadioBoxObject).GetComponent<Toggle>();
				answer.transform.SetParent(AnswerArea.transform);
				answer.transform.localScale = Vector3.one;
				answer.transform.localPosition = Vector3.ProjectOnPlane(answer.transform.localPosition, Vector3.forward);
				answer.transform.localRotation = Quaternion.identity;
				answer.group = thisAnswerGroup;
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