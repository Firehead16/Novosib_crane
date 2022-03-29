using System;
using UnityEngine;

namespace Core.Testing
{
	[Serializable]
	public class AnswerOrderList : Answer
	{
		[SerializeField]
		private string stringValue;
		public string StringValue { get => stringValue; set => stringValue = value; }

		[SerializeField]
		private int stringOrder;
		public int StringOrder { get => stringOrder; set => stringOrder = value; }

		public override void AddInDatabase()
		{
			TestControl.AddAnswerOrderList(this);
		}

		public override void EditInDatabase()
		{
			TestControl.EditAnswerOrderList(this);
		}

		public override string GetName()
		{
			return StringValue;
		}


	}
}