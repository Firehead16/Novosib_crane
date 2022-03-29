using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EPOOutline;
using Obi;
using Sirenix.Utilities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class QuestControl : MonoBehaviour
{
    public static QuestControl Instance;
    [SerializeField] private QuestStepSettings questStep = null;
    [SerializeField] public Cargo cargoTarget = null;
    [SerializeField] public ObiEmitter bulkCargo = null;
    [SerializeField] public PlaceTrigger placeTrigger = null;
    [SerializeField] private List<QuestStepSettings> allQuests = new List<QuestStepSettings>();
    [SerializeField] private QuestStepSettings currentQuest = null;
    public int ParticlesToComplete = 400;
    [SerializeField] private Recorder recorder = null;

    [SerializeField] private List<QuestChooseParameters> questParametersList = new List<QuestChooseParameters>();

    private bool isComplited;

    private Coroutine exitCoroutine = null;

    public static string Name;

    #region Время

    public static float StartTime;

    private bool timerFail = false;
    
    public string questTitle;
    public string placeTitle;

    public static string GetTime() => DiffMin() + ":" + DiffSec();

    #endregion
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Name = QuestStorage.Instance.UserName;
    }

    void Start()
    {
        //отсчёт времени от старта сцены
        StartTime = Time.time;

        //туман
        if (!QuestStorage.Instance.SafeIsUnityNull() && QuestStorage.Instance.fog > 0)
        {
            RenderSettings.fog = true;
            RenderSettings.fogDensity = QuestStorage.Instance.fog * 0.4f;
        }
        else{
            RenderSettings.fog = false;
        }
        
        if (!QuestStorage.Instance.SafeIsUnityNull())
        {
            ChangeQuestParameters();
            UpdateQuestInfo();
        }
        
        SetQuestName();

        //Если включена запись - стартуем запись
        if (QuestStorage.Instance.RecordThis)
        {
            recorder.StartRecording();
        }

        if (!QuestStorage.Instance.SafeIsUnityNull() && QuestStorage.Instance.Mistakes.Count > 0)
        {
            QuestStorage.Instance.Mistakes = new List<string>();
        }
    }

    void SetQuestName()
    {
        switch (QuestStorage.Instance.QuestType)
        {
            case Quest.Pipes:
                questTitle = "Погрузка труб ";
                break;
            case Quest.Containers:
                switch (QuestStorage.Instance.CargoType)
                {
                    case CargoType.GenCargo:
                        questTitle = "Перемещение ген. грузов ";
                        break;
                    case CargoType.Container20Ft:
                        questTitle = "Перемещение 20FT контейнеров ";
                        break;
                    case CargoType.Container40Ft:
                        questTitle = "Перемещение 40FT контейнеров ";
                        break;
                }
                break;
            case Quest.Auto:
                questTitle = "Перемещение автотехники ";
                break;
            case Quest.Bulk:
                questTitle = "Перемещение сыпучих грузов ";
                break;
        }

        switch (QuestStorage.Instance.QuestCargoPlace)
        {
            case QuestCargoPlace.RailwayPlatform:
                placeTitle = "на Ж/Д платформы";
                break;
            case QuestCargoPlace.Auto:
                placeTitle = "в автотранспорт";
                break;
            case QuestCargoPlace.Pontoon:
                placeTitle = "на плашкоут";
                break;
            case QuestCargoPlace.SecondVessel:
                placeTitle = "на другое судно";
                break;
            case QuestCargoPlace.AnotherContainer:
                placeTitle = "на другие контейнеры";
                break;
            case QuestCargoPlace.HoldToPontoon:
                placeTitle = "из трюма на плашкоут";
                break;
            case QuestCargoPlace.PontoonToHold:
                placeTitle = "с плашкоута в трюм";
                break;
        }
    }

    void Update()
    {
        #region Засчитывание груза и переход к след. заданию 
        if (!QuestStorage.Instance.SafeIsUnityNull() && !currentQuest.SafeIsUnityNull() && (!cargoTarget.SafeIsUnityNull() &&
            cargoTarget.IsGrabbed == false || currentQuest.numParticles > 0) && currentQuest.InTrigger) CompleteQuest();
        #endregion
        
        if(Keyboard.current.f10Key.isPressed) CompleteQuest();
        
        //ветер
        if (!QuestStorage.Instance.SafeIsUnityNull() && QuestStorage.Instance.wind > 0 &&
            CraneControl.Instance.CurrentGrabControl.WindParameter != QuestStorage.Instance.wind)
        {
            Debug.Log("QC did wind");
            CraneControl.Instance.CurrentGrabControl.WindParameter = QuestStorage.Instance.wind;
        }
        
        //включаем плейстриггер только когда груз уже захвачен
        if (!currentQuest.PlaceTrigger.GetComponent<MeshRenderer>().enabled &&
            (QuestStorage.Instance.QuestType == Quest.Bulk && CraneControl.Instance.CurrentGrabControl.GreiferClosed ||
             QuestStorage.Instance.QuestType != Quest.Bulk && currentQuest.CargoTarget.IsGrabbed))
        {
            currentQuest.PlaceTrigger.GetComponent<MeshRenderer>().enabled = true;
        }
        else if (QuestStorage.Instance.QuestType == Quest.Bulk &&
                 !CraneControl.Instance.CurrentGrabControl.GreiferClosed)
            currentQuest.PlaceTrigger.GetComponent<MeshRenderer>().enabled = false;

        if (QuestStorage.Instance.questMode == QuestMode.Exam && QuestStorage.Instance.Timer > 0 &&
            QuestStorage.Instance.Timer * 60 < (Time.time - StartTime) && timerFail == false)
        {
            timerFail = true;
            QuestStorage.Instance.AddMistake("Задание не пройдено за заданное время");
            //Если включена запись - останавливаем запись
            if (QuestStorage.Instance.RecordThis && exitCoroutine == null)
            {
                exitCoroutine = StartCoroutine(StopRecord(QuestStorage.Instance.ResultScene));
            }
            else
            {
                isComplited = true;
                // загружаем сцену с результатами
                SceneManager.LoadScene(QuestStorage.Instance.ResultScene);
            }
        }
    }

    private void ChangeQuestParameters()
    {
        QuestStepSettings lastQuestStep;
        switch (QuestStorage.Instance.QuestType)
        {
            case Quest.Pipes:

                HideQuestParametersExceptOne((int)Quest.Pipes);

                for (int cargoCount = 0; cargoCount < questParametersList[(int)Quest.Pipes].Cargo.Count; cargoCount++)
                {
                    lastQuestStep = Instantiate(questStep, Vector3.zero, quaternion.identity);
                    lastQuestStep.transform.SetParent(this.transform);
                    allQuests.Add(lastQuestStep);
                    allQuests.Last().CargoTarget = questParametersList[(int)Quest.Pipes].Cargo[cargoCount].GetComponent<Cargo>();
                    allQuests.Last().CargoTarget.gameObject.SetActive(true);

                    questParametersList[(int)Quest.Pipes].Cargo[cargoCount].GetComponent<Cargo>().IsTraverse =
                        QuestStorage.Instance.LiftingDevice == LiftingDevice.Traverse;

                    SetPipesTriggers(cargoCount, (int)QuestStorage.Instance.QuestCargoPlace);
                    //Проверить притягивание, возможно выключить
                    //allQuests.Last().CargoTarget.snapPlace = allQuests.Last().PlaceTrigger.transform;
                }
                break;
            case Quest.Containers:
                
                HideQuestParametersExceptOne((int)Quest.Containers);

                int count = QuestStorage.Instance.CargoType == CargoType.Container40Ft ? 2 : 4;
                for (int cargoCount = 0; cargoCount < count; cargoCount++) // < questParametersList[(int)Quest.Containers].Cargo.Count
                {
                    lastQuestStep = Instantiate(questStep, Vector3.zero, quaternion.identity);
                    lastQuestStep.transform.SetParent(this.transform);
                    allQuests.Add(lastQuestStep);

                    switch (QuestStorage.Instance.CargoType)
                    {
                        case CargoType.GenCargo: //4 ген. груза
                            allQuests.Last().CargoTarget = questParametersList[(int)Quest.Containers].GenCargo[cargoCount].GetComponent<Cargo>();
                            allQuests.Last().PlaceTrigger = questParametersList[(int) Quest.Containers]
                                .GenPlaceTriggers[cargoCount].GetComponent<PlaceTrigger>();
                            allQuests.Last().CargoTarget.gameObject.SetActive(true);
                            break;
                        
                        case CargoType.Container20Ft: //4 контейнера 20ft
                            allQuests.Last().CargoTarget = questParametersList[(int)Quest.Containers].Cargo20[cargoCount].GetComponent<Cargo>();
                            allQuests.Last().PlaceTrigger = questParametersList[(int) Quest.Containers]
                                .PlaceTriggers20[cargoCount + 4 * (int)QuestStorage.Instance.QuestCargoPlace].GetComponent<PlaceTrigger>();
                            allQuests.Last().CargoTarget.gameObject.SetActive(true);
                            allQuests.Last().CargoTarget.snapPlace = allQuests.Last().PlaceTrigger.transform;
                            break;
                        
                        case CargoType.Container40Ft: //2 контейнера 40ft
                            allQuests.Last().CargoTarget = questParametersList[(int)Quest.Containers].Cargo[cargoCount].GetComponent<Cargo>();
                            allQuests.Last().PlaceTrigger = questParametersList[(int) Quest.Containers]
                                .PlaceTriggers[cargoCount + 2 * (int)QuestStorage.Instance.QuestCargoPlace].GetComponent<PlaceTrigger>();
                            allQuests.Last().CargoTarget.gameObject.SetActive(true);
                            allQuests.Last().CargoTarget.snapPlace = allQuests.Last().PlaceTrigger.transform;
                            break;
                    }
                }
                break;
            case Quest.Auto:
                HideQuestParametersExceptOne((int)Quest.Auto);
                for (int cargoCount = 0; cargoCount < 4; cargoCount++)
                {
                    lastQuestStep = Instantiate(questStep, Vector3.zero, quaternion.identity);
                    lastQuestStep.transform.SetParent(this.transform);
                    allQuests.Add(lastQuestStep);

                    switch (QuestStorage.Instance.QuestCargoPlace)
                    {
                        case QuestCargoPlace.HoldToPontoon:
                            allQuests.Last().CargoTarget = questParametersList[(int) Quest.Auto].Cargo[cargoCount]
                                .GetComponent<Cargo>();
                            allQuests.Last().PlaceTrigger = questParametersList[(int) Quest.Auto]
                                .PlaceTriggers[cargoCount].GetComponent<PlaceTrigger>();
                            allQuests.Last().CargoTarget.gameObject.SetActive(true);
                            break;
                        case QuestCargoPlace.PontoonToHold: //использую 20 как второй вариант квеста
                            allQuests.Last().CargoTarget = questParametersList[(int) Quest.Auto].Cargo20[cargoCount]
                                .GetComponent<Cargo>();
                            allQuests.Last().PlaceTrigger = questParametersList[(int) Quest.Auto]
                                .PlaceTriggers20[cargoCount].GetComponent<PlaceTrigger>();
                            allQuests.Last().CargoTarget.gameObject.SetActive(true);
                            break;
                    }
                }

                break;
            case Quest.Bulk:
                HideQuestParametersExceptOne((int)Quest.Bulk);
                lastQuestStep = Instantiate(questStep, Vector3.zero, quaternion.identity);
                lastQuestStep.transform.SetParent(this.transform);
                lastQuestStep.numParticles = ParticlesToComplete;
                allQuests.Add(lastQuestStep);

                switch (QuestStorage.Instance.QuestCargoPlace)
                {
                    case QuestCargoPlace.PontoonToHold: //С плашкоута в трюм
                        allQuests.Last().BulkCargo =
                            questParametersList[(int) Quest.Bulk].Cargo[1].GetComponent<ObiEmitter>();
                        Debug.Log("bulk " + allQuests.Last().BulkCargo);
                        allQuests.Last().PlaceTrigger = questParametersList[(int) Quest.Bulk].PlaceTriggers[0]
                            .GetComponent<PlaceTrigger>();
                        questParametersList[(int) Quest.Bulk].Cargo[1].transform.parent.gameObject.SetActive(true); //1 - плашкоут
                        questParametersList[(int) Quest.Bulk].AdditionalObjects[0].gameObject.SetActive(false); // отключаем второе дно в трюме
                        break;
                    case QuestCargoPlace.HoldToPontoon: //Из трюма на плашкоут
                        allQuests.Last().BulkCargo =
                            questParametersList[(int) Quest.Bulk].Cargo[0].GetComponent<ObiEmitter>();
                        allQuests.Last().PlaceTrigger = questParametersList[(int) Quest.Bulk].PlaceTriggers[1]
                            .GetComponent<PlaceTrigger>();
                        questParametersList[(int) Quest.Bulk].Cargo[0].transform.parent.gameObject.SetActive(true); //0 - трюм
                        questParametersList[(int) Quest.Bulk].AdditionalObjects[0].gameObject.SetActive(true);
                        break;
                    case QuestCargoPlace.RailwayPlatform: //Из трюма в вагон
                        allQuests.Last().BulkCargo =
                            questParametersList[(int) Quest.Bulk].Cargo[0].GetComponent<ObiEmitter>();
                        allQuests.Last().PlaceTrigger = questParametersList[(int) Quest.Bulk].PlaceTriggers[2]
                            .GetComponent<PlaceTrigger>();
                        questParametersList[(int) Quest.Bulk].Cargo[0].transform.parent.gameObject.SetActive(true);
                        questParametersList[(int) Quest.Bulk].AdditionalObjects[0].gameObject.SetActive(true);
                        break;
                    case QuestCargoPlace.SecondVessel: //Из трюма одного судна в трюм другого
                        allQuests.Last().BulkCargo =
                            questParametersList[(int) Quest.Bulk].Cargo[0].GetComponent<ObiEmitter>();
                        allQuests.Last().PlaceTrigger = questParametersList[(int) Quest.Bulk].PlaceTriggers[3]
                            .GetComponent<PlaceTrigger>();
                        questParametersList[(int) Quest.Bulk].Cargo[0].transform.parent.gameObject.SetActive(true);
                        questParametersList[(int) Quest.Bulk].AdditionalObjects[0].gameObject.SetActive(true);
                        break;
                }
                break;
        }
    }

    void SetPipesTriggers(int cargoCount, int parameter)
    {
        if (cargoCount == 0)
        {
            allQuests.Last().PlaceTrigger = questParametersList[(int)Quest.Pipes].PlaceTriggers[parameter]
                .GetComponent<PlaceTrigger>();
        }
        else
        {
            GameObject placeTriggerDuplicate;
            placeTriggerDuplicate = Instantiate(questParametersList[(int)Quest.Pipes].PlaceTriggers[parameter].gameObject,
                questParametersList[(int)Quest.Pipes].PlaceTriggers[parameter].position,
                questParametersList[(int)Quest.Pipes].PlaceTriggers[parameter].rotation);
            placeTriggerDuplicate.transform.parent = questParametersList[(int)Quest.Pipes].PlaceTriggers[parameter].parent;
            placeTriggerDuplicate.transform.localScale = questParametersList[(int)Quest.Pipes].PlaceTriggers[parameter].localScale;
            placeTriggerDuplicate.gameObject.SetActive(false);
            allQuests.Last().PlaceTrigger = placeTriggerDuplicate.GetComponent<PlaceTrigger>();
        }
    }

    // Обновить информацию о задании
    private void UpdateQuestInfo()
    {
        if (allQuests.Any())
        {
            currentQuest = allQuests[0];
            cargoTarget = currentQuest.CargoTarget;
            placeTrigger = currentQuest.PlaceTrigger;
            bulkCargo = currentQuest.BulkCargo;

            if (!cargoTarget.SafeIsUnityNull())
            {
                cargoTarget.IsAvaliable = true;
                cargoTarget.GetComponent<Outlinable>().enabled = true; //подкрашиваем нужный груз
                placeTrigger.GetComponent<PlaceTrigger>().cargoQuest = cargoTarget.GetComponent<Cargo>().CargoItself;
            }

            placeTrigger.gameObject.SetActive(true);
            placeTrigger.GetComponent<MeshRenderer>().enabled = false;
            placeTrigger.GetComponent<PlaceTrigger>().quest = currentQuest;
        }
        else CompleteQuest();
    }

    // Завершение задания после остановки записи
    private IEnumerator StopRecord(int scene)
    {
        isComplited = true;
        // Останавливаем запись
        recorder.StopRecording();
        while (recorder.GetComponent<RecorderMmpeg>()._isRecording)
        {
            yield return new WaitForEndOfFrame();
        }
        exitCoroutine = null;
        // загружаем сцену с результатами
        SceneManager.LoadScene(scene);
    }


    // Окончание задания
    private void CompleteQuest()
    {
        if (allQuests.Count > 0) //завершение шага задания, переход к следующему
        {
            //currentQuest.PlaceTrigger.gameObject.SetActive(false);
            currentQuest.PlaceTrigger.GetComponent<MeshRenderer>().enabled = false;
            currentQuest.PlaceTrigger.GetComponent<PlaceTrigger>().enabled = false;
            if(QuestStorage.Instance.QuestType != Quest.Bulk) currentQuest.CargoTarget.GetComponent<Cargo>().SetSolverActive(false);
            allQuests.RemoveAt(0);
            UpdateQuestInfo();
        }
        else
        {
            if (isComplited == false)
            {
                //Если включена запись - останавливаем запись
                if (QuestStorage.Instance.RecordThis && exitCoroutine == null)
                {
                    // Записываем в результат название задания
                    CompleteQuestLog();
                    exitCoroutine = StartCoroutine(StopRecord(QuestStorage.Instance.ResultScene));
                }
                else
                {
                    isComplited = true;
                    // Записываем в результат название задания
                    CompleteQuestLog();
                    // загружаем сцену с результатами
                    SceneManager.LoadScene(QuestStorage.Instance.ResultScene);
                }
            }
        }
    }

    public void ResetOrExit(int scene)
    {
        if(exitCoroutine == null){
            if (QuestStorage.Instance.RecordThis)
            {
                exitCoroutine = StartCoroutine(StopRecord(scene));
            }
            else{
                SceneManager.LoadScene(scene);
            }
        }
        
    }

    void CompleteQuestLog()
    {
        Debug.Log("Completed quest");
        
        QuestStorage.Instance.AddMistake("Задание " + '"' + questTitle + placeTitle + '"' + " завершено за ");
    }

    void HideQuestParametersExceptOne(int index)
    {
        //Выключаем доп. объекты, участвующие в других заданиях
        foreach (var questParams in questParametersList.Except(new List<QuestChooseParameters>
            {questParametersList[index]}))
        {
            questParams.gameObject.SetActive(false);
            foreach (var addObj in questParams.AdditionalObjects)
            {
                addObj.gameObject.SetActive(false);
            }
        }
        
        //Включаем нужные доп. объекты, если они выключены
        if(!questParametersList[index].isActiveAndEnabled) questParametersList[index].gameObject.SetActive(true);
        foreach (var addObj in questParametersList[index].AdditionalObjects)
        {
            if(!addObj.gameObject.activeSelf) addObj.gameObject.SetActive(true);
        }
    }

    #region Функции подсчёта времени

    // Посчитать минуты
    public static string DiffMin()
    {
        return ((int) ((Time.time - StartTime) / 60f)).ToString();
    }

    // Посчитать секунды
    public static string DiffSec()
    {
        if ((int)((Time.time - StartTime) % 60f) < 10)
        {
            return "0" + (int)((Time.time - StartTime) % 60f);
        }
        else return ((int)((Time.time - StartTime) % 60f)).ToString();
    }

    #endregion
}
