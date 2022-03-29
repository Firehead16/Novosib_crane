using System;
using UnityEngine;

namespace Core.Testing
{
	[Serializable]
	public class AnswerOfText : Answer
	{
		[SerializeField]
		private string name;
		public string Name { get => name; set => name = value; }
		
		[SerializeField]
		private bool isPhraseAnswer;
		public bool IsPhraseAnswer { get => isPhraseAnswer; set => isPhraseAnswer = value; }
		
		[SerializeField]
		private bool isCorrect;
		public bool IsCorrect { get => isCorrect; set => isCorrect = value; }


		public override void AddInDatabase()
		{
			TestControl.AddAnswerText(this);
		}

		public override void EditInDatabase()
		{
			TestControl.EditAnswerText(this);
		}

		public override string GetName()
		{
			return Name;
		}
	}
}