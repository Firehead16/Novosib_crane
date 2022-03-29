using System.Collections.Generic;
using Core.Settings;
using UnityEngine;

namespace Core.Gameplay.Questing
{
    [CreateAssetMenu(fileName = "QuestsStorage", menuName = "Settings/QuestsStorage")]
    public class QuestsStorage : SubSettings<QuestsStorage>
    {
        [SerializeField] private List<QuestSettings> quests;
        public List<QuestSettings> Quests => quests;
    }
}