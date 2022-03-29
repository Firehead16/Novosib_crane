using System;
using UnityEngine;

namespace Core.Testing
{
	[Serializable]
	public class AnswerImage : Answer
	{
		[SerializeField]
		private string imageName;
		public string ImageName { get => imageName; set => imageName = value; }
		
		[SerializeField]
		private bool isCorrect;
		public bool IsCorrect { get => isCorrect; set => isCorrect = value; }

		[SerializeField]
		private byte[] imageData;
		public byte[] ImageData { get => imageData; set => imageData = value; }

		public override void AddInDatabase()
		{
			TestControl.AddAnswerImage(this);			
		}

		public override void EditInDatabase()
		{
			TestControl.EditAnswerImage(this);
		}

		public override string GetName()
		{
			return ImageName;
		}
	}
}