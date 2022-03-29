using System.Collections;
using Sirenix.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

public class SmallPortalRand : MonoBehaviour
{
    [SerializeField] private Transform RotatablePart;
    [SerializeField] private float speed;
    private bool turn;

    IEnumerator RandMove()
    {
        yield return new WaitForSeconds(Random.Range(1, 10));
        while (true)
        {
            for (float i = 0; i < 3; i += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward * (turn ? speed : -speed), i);
                yield return new WaitForEndOfFrame();
                // Debug.Log(i);
            }
            yield return new WaitForSeconds(10 - Random.Range(1, 3));
            if (!RotatablePart.SafeIsUnityNull())
            {
                Quaternion RotPart = RotatablePart.rotation;
                //Debug.Log(RotPart);
                for (float i = 0; i < 3; i += Time.deltaTime)
                {
                    if (turn) RotatablePart.rotation = Quaternion.Lerp(RotPart, RotPart * Quaternion.Euler(0, 90, 0), i / 3);
                    else RotatablePart.rotation = Quaternion.Lerp(RotPart, RotPart * Quaternion.Euler(0, -90, 0), i / 3);
                
                    yield return new WaitForEndOfFrame();
                    // Debug.Log(i);
                }
                yield return new WaitForSeconds(10 - Random.Range(1, 3));
            }
            turn = !turn;
            yield return null;
        }
    }
    
    private void Start()
    {
        turn = Random.value > 0.5f;
        StartCoroutine(RandMove());
    }
}
