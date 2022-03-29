using System;
using UnityEngine;

namespace Core.Testing
{
	[Serializable]
	public abstract class Answer : IQuestionAnswer
	{
		[SerializeField]
		private int answerId;
		public int AnswerId { get => answerId; set => answerId = value; }

		[SerializeField]
		private int questionId;
		public int QuestionId { get => questionId; set => questionId = value; }

		public abstract void AddInDatabase();

		public abstract void EditInDatabase();

		public abstract string GetName();
		
		public bool IsNewInstanse { get; set; }
		
		public bool IsLoaded { get; set; }
		
		public void Load() { }

		public void Unload() { }
	}

}