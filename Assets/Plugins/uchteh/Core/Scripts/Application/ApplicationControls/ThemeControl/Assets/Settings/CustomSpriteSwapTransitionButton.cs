using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Ui
{
	[Serializable]
	public class CustomSpriteSwapTransitionButton : ISpritePreset
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

		[SerializeField] private SpriteState hpd_Images = default ;
		public SpriteState HpdImages => hpd_Images;
	}
}