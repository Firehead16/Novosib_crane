using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Ui
{
	[Serializable]
	public class CustomImage : ISpritePreset
	{
		[SerializeField, HideLabel, HorizontalGroup("row")] private string name;

		public string Name
		{
			get => name;
			set => name = value;
		}

		[SerializeField, HideLabel, PreviewField(50, ObjectFieldAlignment.Left), HorizontalGroup("row")]
		private Sprite targetImage = null;

		public Sprite TargetImage => targetImage;
	}
}