using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text resultText = null;

    [SerializeField] private Button menu = null;
    [SerializeField] private Button reset = null;

    // Start is called before the first frame update
    void Start()
    {
        menu.onClick.AddListener(() => backToMenu());
        reset.onClick.AddListener(() => resetQuest());
        
        for (int i = 0; i < QuestStorage.Instance.Mistakes.Count; i++)
        {
            resultText.text += QuestStorage.Instance.Mistakes[i] + '\n';
        }
        
        if (!Directory.Exists(Application.dataPath + "/QuestLogs"))
        {
            Directory.CreateDirectory(Application.dataPath + "/QuestLogs");

            string AppPath = Path.GetDirectoryName(Application.dataPath);
            string name = QuestStorage.Instance.UserName.Replace(' ', '-');
            name = name.Remove(name.Length - 1);
            
            File.WriteAllText(AppPath + "/QuestLogs" + "/" + name + '-' + DateTime.Now.ToString("dd-MM-yyyy-HH-mm") + ".ini", resultText.text);
            Debug.Log(AppPath + "/QuestLogs" + "/" + QuestStorage.Instance.UserName.Replace(' ', '-') + '-' + DateTime.Now.ToString("dd-MM-yyyy-HH-mm") + ".ini");
        }
        else
        {
            string AppPath = Path.GetDirectoryName(Application.dataPath);
            string name = QuestStorage.Instance.UserName.Replace(' ', '-');
            name = name.Remove(name.Length - 1);
            
            File.WriteAllText(AppPath + "/QuestLogs" + "/" + name + '-' + DateTime.Now.ToString("dd-MM-yyyy-HH-mm") + ".ini", resultText.text);
            Debug.Log(AppPath + "/QuestLogs" + "/" + QuestStorage.Instance.UserName.Replace(' ', '-') + '-' + DateTime.Now.ToString("dd-MM-yyyy-HH-mm") + ".ini");
        }
    }

    void backToMenu() => SceneManager.LoadScene(QuestStorage.Instance.MainScene);

    void resetQuest()
    {
        QuestStorage.Instance.Mistakes = new List<string>();
        SceneManager.LoadScene(QuestStorage.Instance.QuestScene);
    }
}
