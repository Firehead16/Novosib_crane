using System;
using UnityEngine;

namespace Core.Testing
{
	[Serializable]
	public class AnswerDragNDrop : Answer
	{
		[SerializeField]
		private string textInfo;
		public string TextInfo { get => textInfo; set => textInfo = value; }

		[SerializeField]
		private bool isAnswerText;
		public bool IsAnswerText { get => isAnswerText; set => isAnswerText = value; }
		
		[SerializeField]
		private int textIndex;
		public int TextIndex { get => textIndex; set => textIndex = value; }
		
		[SerializeField]
		private bool paragraph;
		public bool Paragraph { get => paragraph; set => paragraph = value; }

		public override void AddInDatabase()
		{
			TestControl.AddAnswerDragNDrop(this);			
		}

		public override void EditInDatabase()
		{
			TestControl.EditAnswerDragNDrop(this);
		}

		public override string GetName()
		{
			return TextInfo;
		}
	}
}