using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActionAsset = null;
    [SerializeField] private float test;
    private InputAction testAction;

    void Start()
    {
        testAction = inputActionAsset.FindAction("New action");
        testAction.Enable();
    }

    void Update()
    {
        test = testAction.ReadValue<float>();
        Debug.Log(test);
    }
 }
