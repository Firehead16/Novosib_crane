using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailStop : MonoBehaviour
{
    private Coroutine onlyCoroutine = null;
    void Update()
    {
        if ((transform.position.z == GetComponent<MovablePart>().minLimit ||
             transform.position.z == GetComponent<MovablePart>().maxLimit) &&
            Mathf.Abs(GetComponent<MovablePart>().velocityValue) > 0.5f &&
            onlyCoroutine == null)
        {
            onlyCoroutine = StartCoroutine(RailStopMistake());
        }
    }

    IEnumerator RailStopMistake()
    {
        QuestStorage.Instance.AddMistake("¬ъезд в тупиковый упор на большой скорости");
        Debug.Log("¬ъезд в тупиковый упор на большой скорости");
        yield return new WaitForSeconds(30);
        onlyCoroutine = null;
    }
}
