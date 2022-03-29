using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Testing
{
	[Serializable]
	public class Test
	{
		[SerializeField]
		private int testId;
		public int TestId { get => testId; set => testId = value; }

		[SerializeField]
		private string name;
		public string Name { get => name; set => name = value; }

		[SerializeField]
		private string description;
		public string Description { get => description; set => description = value; }

		[SerializeField]
		private Complication complication;
		public Complication Complication { get => complication; set => complication = value; }

		[SerializeField]
		private int complicationId;
		public int ComplicationId { get => complicationId; set => complicationId = value; }

		[SerializeField]
		private List<Question> questions  = new List<Question>();
		public List<Question> Questions { get => questions; set => questions = value; }

	}
}