using System;
using UnityEngine;

namespace Core.Testing
{
	[Serializable]
	public class PassedQuestion
	{
		[SerializeField]
		private int logId;
		public int LogId {get => logId; set => logId = value; }

		[SerializeField]
		private int questionId;
		public int QuestionId {get => questionId; set => questionId = value; }

		[SerializeField]
		private string name;
		public string Name  {get => name; set => name = value; }

		[SerializeField]
		private string answer;
		public string Answer{get => answer; set => answer = value; } 

		[SerializeField]
		private bool isRight;
		public bool IsRight{get => isRight; set => isRight = value; } 

		public PassedQuestion() { }

		public PassedQuestion(int questionId, string answer)
		{
			QuestionId = questionId;
			Answer = answer;
		}

		public PassedQuestion(int questionId, string answer, bool isRight) : this(questionId, answer)
		{
			IsRight = isRight;
		}
	}
}