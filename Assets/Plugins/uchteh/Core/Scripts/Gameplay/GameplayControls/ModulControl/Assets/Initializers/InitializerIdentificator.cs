using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Gameplay.Modul
{
    [Serializable]
    public class InitializerIdentificator 
    {
        [SerializeField, OnValueChanged("ChangeInitializer")]
        private Initializer initializer =null;
        public Initializer Initializer => initializer;

     

        [BoxGroup("Гнездо на сцене:")]
        [SerializeField, LabelText("Идентификатор модуля"), HideLabel, HideIf("hideSaddle"),OnValueChanged("SearchSaddleId")]
        private string saddleId;
        public string SaddleId => saddleId;


        [SerializeField,BoxGroup("SearchResults:", false), DisplayAsString(false), HideLabel, ShowIf("searchActive")]
        // ReSharper disable once NotAccessedField.Local
        private string searchedValues;

      
        #region Работа в инспектора

        /// <summary>
        /// Состояние отображения Id седла
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private bool hideSaddle;

        /// <summary>
        /// Поиск седла активен
        /// </summary>
        private bool searchActive;


        /// <summary>
        /// Сменить квест
        /// </summary>
        private void ChangeInitializer()
        {
            hideSaddle = Initializer == null;
            saddleId = "";
        }


        /// <summary>
        /// Поиск седла
        /// </summary>
        private void SearchSaddleId()
        {
            if (!searchActive)
            {
                searchActive = true;
            }
            searchedValues = "";
            var searchedSaddlesId = new List<string>();
            if (Initializer)
            {
                var saddleType = Initializer.GetModulSaddleTypeId();
                Regex regex = new Regex(@"(\/)");
                int slashCount = regex.Matches(SaddleId).Count;
                if (slashCount == 0)
                {
                    var saddleOnScene = ModulControl.GetSaddleIdsOnScene(saddleType); // s1id, s2id
                    searchedSaddlesId = saddleOnScene;
                    searchedSaddlesId.Add("Free");
                }
                else if (slashCount > 0)
                {
                    var childSaddlesInPotentialModuls =
                        (ModulControl.GetChildSaddlesInPotentialModuls(saddleType)); // 2=> s1id/s3id
                    foreach (var searchedSaddleId in ModulControl.GetSaddleIdsOnScene(saddleType))
                    {
                        foreach (var childSaddleIdInPotentialModul in childSaddlesInPotentialModuls)
                        {
                            searchedSaddlesId.Add(searchedSaddleId + "/" + childSaddleIdInPotentialModul);
                        }
                    }
                    var childSaddlesIdInFreeModuls =
                        (ModulControl.GetChildSaddlesIdInFreeModuls(saddleType)); // 3=> free/s3id
                    foreach (var childSaddleIdInFreeModul in childSaddlesIdInFreeModuls)
                    {
                        searchedSaddlesId.Add("Free/" + childSaddleIdInFreeModul);
                    }
                }

                var findSaddleId = searchedSaddlesId.Where(x => x.ToLower().Contains(SaddleId.ToLower())).ToList();
                foreach (var find in findSaddleId)
                {
                    searchedValues += find + "\n";
                }

                if (findSaddleId.Count == 0 && slashCount > 0 && SaddleId.Last().Equals('/'))
                {
                    saddleId = SaddleId.Substring(0,
                        SaddleId.Length - 1); //Если попытались найти по слешу дочерние но их нет
                }
                else if (findSaddleId.Count == 1)
                {
                    searchActive = false;

                    saddleId = findSaddleId[0];
                    GUI.FocusControl(null);
                }
            }
        }
        #endregion
    }
}