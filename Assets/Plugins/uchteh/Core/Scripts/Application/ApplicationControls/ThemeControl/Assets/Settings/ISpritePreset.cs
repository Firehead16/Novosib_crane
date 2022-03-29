using UnityEngine;

namespace Core.Ui
{
	public interface ISpritePreset
	{
		string Name { get; set; }
		Sprite TargetImage { get; }
	}
}