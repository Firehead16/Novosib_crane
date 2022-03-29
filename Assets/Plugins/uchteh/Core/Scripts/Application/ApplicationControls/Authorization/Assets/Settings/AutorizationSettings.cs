using UnityEngine;

namespace Core.Settings
{
	[CreateAssetMenu(fileName = "AutorizationSettings", menuName = "Settings/AutorizationSettings")]
    public class AutorizationSettings : SubSettings<AutorizationSettings>
    {
	    public int MinLoginChar = 3;
        public int MaxLoginChar = 14;

        public int MinPasswordChar = 4;
        public int MaxPasswordChar = 20;
    }
}