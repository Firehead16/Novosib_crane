using System;
using UnityEngine;

namespace Core.Testing
{
	[Serializable]
	public class Complication
	{
		[SerializeField]
		private int complicationId;
		public int ComplicationId { get => complicationId; set => complicationId = value; }

		[SerializeField]
		private string name;
		public string Name { get => name; set => name = value; }
	}
}