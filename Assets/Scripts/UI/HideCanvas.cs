using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HideCanvas : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.f12Key.IsPressed())
        {
            canvas.enabled = !canvas.enabled;
        }
    }
}
