using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Settings
{
	[CreateAssetMenu(fileName = "RebindAssetsSettings", menuName = "Settings/RebindAssetsSettings")]
	public class RebindAssetsSettings : SubSettings<RebindAssetsSettings>
	{
		[SerializeField] private string customInputAssetFolder = @"/CustomInputData/";

		public string CustomInputAssetFolder => customInputAssetFolder;
		[SerializeField] private List<InputActionAsset> assets = null;
		public List<InputActionAsset> Assets
		{
			get
			{
				var result = new List<InputActionAsset>();
				foreach (var asset in assets)
				{
					result.Add(GlobalLinkStorage.Application.GetControl<IInputBindingControl>().ReadInputAsset(asset));
				}
				return result;
			}
		}
	}
}