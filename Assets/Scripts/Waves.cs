using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Waves : MonoBehaviour
{
    private Coroutine waveCoroutine;

    private bool canBeStopped = false;

    private float wavesParameter;

    IEnumerator MakeWave()
    {
        while (true)
        {
            canBeStopped = false;
            for (float i = 0; i < 1; i += Time.deltaTime)
            {
                transform.rotation = Quaternion.Slerp(quaternion.identity, Quaternion.Euler(0, 0, 3f * wavesParameter), i);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            for (float j = 0; j < 1; j += Time.deltaTime)
            {
                transform.rotation = Quaternion.Slerp(Quaternion.Euler(0, 0, 3f * wavesParameter), Quaternion.Euler(0, 0, -3f * wavesParameter), j);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            for (float k = 0; k <= 1; k += Time.deltaTime)
            {
                transform.rotation = Quaternion.Slerp(Quaternion.Euler(0, 0, -3f * wavesParameter), Quaternion.Euler(0, 0, 0), k);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            canBeStopped = true;
            yield return new WaitForSeconds(3 + Random.value);
            yield return null;
        }
        //windCoroutine = null;
    }
    
    void Start()
    {
        if (!QuestStorage.Instance.SafeIsUnityNull() && QuestStorage.Instance.waves > 0)
        {
            wavesParameter = QuestStorage.Instance.waves;
            waveCoroutine = StartCoroutine(MakeWave());
        }
    }
}
