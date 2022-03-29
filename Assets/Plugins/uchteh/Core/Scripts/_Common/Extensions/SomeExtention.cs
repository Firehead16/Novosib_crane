using Core.Gameplay.Modul;
using Core.Gameplay.Questing;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;



namespace Core.Extensions
{
	public static class SomeExtention
    {
        public class Triangle
        {

            public Triangle(float sideX, float sideY, float sideZ)
            {
                side = new Vector3(sideX, sideY, sideZ);
                angles = CalculateAngles(Side);
            }

            public Triangle(Vector3 cornerXposition, Vector3 cornerYposition, Vector3 cornerZposition)
            {
                side = new Vector3(
                    (cornerZposition - cornerYposition).magnitude,
                    (cornerXposition - cornerZposition).magnitude,
                    (cornerXposition - cornerYposition).magnitude);
                angles = CalculateAngles(Side);
            }

            private Vector3 side;
            private Vector3 angles;

            public Vector3 Side => side;

            public Vector3 Angles => angles;

            /// <summary>
            /// Вычисляет углы треугольника, противолежащие сторонам
            /// </summary>
            /// <!--
            ///                    _
            ///                   /y\ <=== angle 
            ///                  /   \
            ///                 /     \ X
            ///     side  ==>Z /       \ 
            ///               /         \
            ///              /x_________z\
            ///                    Y       
            ///     X* X = Z *Z + Y * Y - 2 * Mathf.Cos(x) * Z * Y 
            /// -->
            /// <param name="side">vector3, в котором лежат 3 стороны треугольника</param>
            /// <returns>vector3, в котором лежат 3 угла треугольника, противолежащие соответственно 3 сторонам из side</returns>
            public static Vector3 CalculateAngles(Vector3 side)
            {
                Vector3 angle = new Vector3
                {
                    x = (Mathf.Acos((side.y * side.y + side.z * side.z - side.x * side.x) /
                                    (2 * side.y * side.z))) * Mathf.Rad2Deg,
                    y = (Mathf.Acos((side.x * side.x + side.z * side.z - side.y * side.y) /
                                    (2 * side.x * side.z))) * Mathf.Rad2Deg,
                    z = (Mathf.Acos((side.x * side.x + side.y * side.y - side.z * side.z) /
                                    (2 * side.x * side.y))) * Mathf.Rad2Deg
                };

                return angle;
            }
        }
        
        /// <summary>
        /// Добавление квеста модулю. Создает рядом с квестом папку Quests (если не создана) и создает в ней файл квеста .asset
        /// </summary>
        /// <typeparam name="TModul"></typeparam>
        /// <typeparam name="TStatus"></typeparam>
        /// <param name="currentModulScriptPath"></param>
        public static void NewQuest<TModul, TStatus>(string currentModulScriptPath)
            where TModul : Modul<TModul, TStatus>
        {
            string directoryName;
            string questsDirectoryName;
            try
            {
                directoryName = Path.GetDirectoryName(currentModulScriptPath);
                questsDirectoryName = Path.GetDirectoryName(Application.dataPath) + "\\" + directoryName + @"\Quests";

            }
            catch (Exception e)
            {
                Debug.LogError("Квест не создан. Выберете префаб для создания квеста " + e);
                throw;
            }
            if (!Directory.Exists(questsDirectoryName))
            {
                Directory.CreateDirectory(questsDirectoryName);
            }
            var newQuest = ScriptableObject.CreateInstance<Quest>();
#if UNITY_EDITOR
            AssetDatabase.CreateAsset(newQuest, directoryName + @"\Quests" + @"\NewQuest.asset");
#endif
            newQuest.QuestData = (QuestData)Activator.CreateInstance(typeof(QuestData<,>).MakeGenericType(typeof(TModul), typeof(TStatus)));

        }

        /// <summary>
        /// Добавление инициализатора модулю. Создает рядом с квестом папку Initializers (если не создана) и создает в ней файл инициализатора .asset
        /// </summary>
        /// <typeparam name="TModul"></typeparam>
        /// <typeparam name="TStatus"></typeparam>
        /// <typeparam name="TFuncs"></typeparam>
        /// <typeparam name="TOrgans"></typeparam>
        public static void NewInitializer<TModul, TStatus, TFuncs, TOrgans>(string currentModulScriptPath)
            where TModul : Modul<TModul, TStatus>
        {
            string directoryName;
            string initializersDirectoryName;
            try
            {
                directoryName = Path.GetDirectoryName(currentModulScriptPath);
                initializersDirectoryName = Path.GetDirectoryName(Application.dataPath) + "\\" + directoryName + @"\Initializers";

            }
            catch (Exception e)
            {
                Debug.LogError("Инициализатор не создан. Выберете префаб для создания инициализатора " + e);
                return;

            }

            if (!Directory.Exists(initializersDirectoryName))
            {
                Directory.CreateDirectory(initializersDirectoryName);
            }
            var initializer = ScriptableObject.CreateInstance<Initializer>();
#if UNITY_EDITOR
            AssetDatabase.CreateAsset(initializer, directoryName + @"\Initializers" + @"\NewInitializer.asset");
#endif
            initializer.InitializerData = (InitializerData)Activator.CreateInstance(typeof(InitializerData<,,,>).MakeGenericType(typeof(TModul), typeof(TStatus), typeof(TFuncs), typeof(TOrgans)));
        }

        /// <summary>
        /// Установить объект в седло (в пространстве, в иерархии, установить Saddle как id) todo уточнить описание
        /// </summary>
        /// <param name="_"></param>
        /// <param name="saddle"></param>
        public static void SetSaddle(this IHasSaddle _, ISaddle saddle)
        {
            if (saddle != null)
                SetSaddle(_.GameObject.transform, saddle.GameObject.transform);
        }

        /// <summary>
        /// Установить родителя
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="parent"></param>
        public static void SetSaddle(this Transform transform, Transform parent)
        {
            transform.SetParent(parent);
            transform.CopyPositionAndRotation(parent);
        }

        public static float[,] GetAllHeights(this TerrainData td)
        {
            return td.GetHeights(0, 0, td.heightmapResolution, td.heightmapResolution);
        }

        #if UNITY_EDITOR

        #region Меню террейнов

        [MenuItem("Tools/TerrainData/SetTerrainLayers")]
        public static void SetTerrainLayers()
        {
            Debug.Log("First object " + Selection.gameObjects[0], Selection.gameObjects[0]);
            var layer = Selection.gameObjects[0].GetComponent<Terrain>().terrainData.terrainLayers;
            for (var index = 1; index < Selection.gameObjects.Length; index++)
            {
                var gameObject = Selection.gameObjects[index];
                gameObject.GetComponent<Terrain>().terrainData.terrainLayers = layer;
            }
        }

        #endregion
        
        #endif
    }
       
}