using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Testing
{
	[Serializable]
	public class PassedTest : Test
	{
		[SerializeField]
		private int logId;
		public int LogId { get => logId; set => logId = value; }

		[SerializeField]
		private DateTime date;
		public DateTime Date { get => date; set => date = value; }

		[SerializeField]
		private string log;
		public string Log { get => log; set => log = value; }

		[SerializeField]
		private int grade;
		public int Grade { get => grade; set => grade = value; }

		[SerializeField]
		private List<PassedQuestion> passedQuestions = new List<PassedQuestion>();
		public List<PassedQuestion> PassedQuestions { get => passedQuestions; set => passedQuestions = value; }

		[SerializeField]
		private int personId;
		public int PersonId { get => personId; set => personId = value; }
	}
}