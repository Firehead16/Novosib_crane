using System.Collections.Generic;
using Core.Settings;
using UnityEngine;

namespace Core.Gameplay
{
    [CreateAssetMenu(fileName = "GameplayManagerSettings", menuName = "Settings/GameplayManagerSettings")]
    public class GameplayManagerSettings : SubSettings<GameplayManagerSettings>,IInitializebleList<IGameplayControl>
    {
     [SerializeField]   private List<IGameplayControl> gamePlayControls = null;
        public List<IGameplayControl> Controls { get =>gamePlayControls; }   
    }
}