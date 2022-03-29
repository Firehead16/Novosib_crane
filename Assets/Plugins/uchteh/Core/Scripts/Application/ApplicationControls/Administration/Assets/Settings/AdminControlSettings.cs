using Core.Settings;
using UnityEngine;


namespace Core.Global.Administration
{
	[CreateAssetMenu(fileName = "TestControlsSettings", menuName = "Settings/TestControlsSettings")]
	public class AdminControlSettings : SubSettings<AdminControlSettings>
	{
		[Header(header: "Префабы для администраторской части")]
		public GameObject UserPrefabFiller;
	}

}