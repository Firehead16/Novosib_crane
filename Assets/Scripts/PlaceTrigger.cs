using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

public class PlaceTrigger : MonoBehaviour
{
    public QuestStepSettings quest;

    public Transform cargoQuest = null;
    
    [SerializeField]
    private List<GameObject> triggerVisitors = null;

    private Coroutine catcherCoroutine = null;

    public float BulkProgress;

    [SerializeField]
    private List<PlaceTriggerElement> triggerElements = null;

    public bool ContainsCargo = false;

    private float angleDiff;

    private void Update()
    {
        if (!QuestStorage.Instance.SafeIsUnityNull() && QuestStorage.Instance.QuestType != Quest.Bulk &&
            !quest.SafeIsUnityNull() && (!triggerElements.Any() || triggerElements.All(t => t.IsTrigger)) &&
            triggerVisitors.Contains(quest.CargoTarget.CargoItself.gameObject))
        {
            angleDiff = Mathf.Round(Mathf.Abs(quest.CargoTarget.CargoItself.rotation.eulerAngles.y - transform.rotation.y));
            if (angleDiff >= 180) angleDiff -= 180;

            ContainsCargo = true;
            if (quest.CargoTarget.gameObject.GetComponent<Cargo>().IsGrabbed &&
                quest.CargoTarget.CargoItself.GetComponent<Rigidbody>().velocity.y < 0)
            {
                if (angleDiff < 5 || angleDiff > 175)
                {
                    CanvasControl.Instance.OffAngleDiff();
                    if (QuestStorage.Instance.LiftingDevice == LiftingDevice.Spreader)
                    {
                        if (CraneControl.Instance.inputActionAsset.FindAction("SpreaderRelease").ReadValue<float>() > 0)
                            quest.CargoTarget.gameObject.GetComponent<Cargo>().ReleaseInCargo();
                    }
                    else quest.CargoTarget.gameObject.GetComponent<Cargo>().ReleaseInCargo();
                }
                else CanvasControl.Instance.SetAngleDiff(angleDiff);
            }
            else quest.InTrigger = true;
        }
        else ContainsCargo = false;

        if (!QuestStorage.Instance.SafeIsUnityNull() && QuestStorage.Instance.QuestType == Quest.Bulk)
        {
            quest.InTrigger = GetComponent<FluidCounter>().finishedParticles.Count >= QuestControl.Instance.ParticlesToComplete;
            BulkProgress = GetComponent<FluidCounter>().finishedParticles.Count / (float)QuestControl.Instance.ParticlesToComplete * 100;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggerVisitors.Contains(other.gameObject)) triggerVisitors.Add(other.gameObject);
    }
    
    private void OnTriggerExit(Collider other) => triggerVisitors.Remove(other.gameObject);
}
