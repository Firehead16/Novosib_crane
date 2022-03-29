using System.Collections.Generic;
using Core.Settings;
using UnityEngine;

namespace Core.Global
{
    [CreateAssetMenu(fileName = "ApplicationManagerSettings", menuName = "Settings/ApplicationManagerSettings")]
	public class ApplicationManagerSettings : SubSettings<ApplicationManagerSettings>,IControlsList<IApplicationControl>
    {   
        
        /// <summary>
        /// Список контроллеров при загрузке приложения
        /// </summary>
       [SerializeField] private List<IApplicationControl> applicationControls = null;

        public List<IApplicationControl> Controls => applicationControls;
    }
}

