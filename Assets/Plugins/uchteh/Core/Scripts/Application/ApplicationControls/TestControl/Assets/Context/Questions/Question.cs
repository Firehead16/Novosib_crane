using System;
using UnityEngine;

namespace Core.Testing
{
	[Serializable]
	public class Question
	{
		public enum Type
		{
			Image = 0,
			CheckBox = 1,
			RadioBox = 2,
			KeyPhrase = 3,
			SpaceFill = 4,
			OrderList = 5,
		}

		[SerializeField]
		private int questionId;
		public int QuestionId { get => questionId; set => questionId = value; }

		[SerializeField]
		private string name;
		public string Name { get => name; set => name = value; }

		[SerializeField]
		private string advice;
		public string Advice { get => advice; set => advice = value; }

		[SerializeField]
		private Type typeQuestion;
		public Type TypeQuestion { get => typeQuestion; set => typeQuestion = value; }

		[SerializeField]
		private int testId;
		public int TestId { get => testId; set => testId = value; }
	}
}