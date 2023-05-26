using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Base : MonoBehaviour
{
    // Start is called before the first frame update
    TestInputAction inputAction;

    void Awake()
    {
        inputAction = new TestInputAction();
    }

    void OnEnable()
    {
        inputAction.TestPlayer.Enable();
        inputAction.TestPlayer.Action1.performed += DoAction1;
        inputAction.TestPlayer.Action2.performed += DoAction2;
        inputAction.TestPlayer.Action3.performed += DoAction3;
        inputAction.TestPlayer.Action4.performed += DoAction4;
        inputAction.TestPlayer.Action5.performed += DoAction5;
    }

    void OnDisable()
    {

        inputAction.TestPlayer.Action5.performed -= DoAction5;
        inputAction.TestPlayer.Action4.performed -= DoAction4;
        inputAction.TestPlayer.Action3.performed -= DoAction3;
        inputAction.TestPlayer.Action2.performed -= DoAction2;
        inputAction.TestPlayer.Action1.performed -= DoAction1;
        inputAction.TestPlayer.Disable();
    }

    protected virtual void DoAction1(InputAction.CallbackContext _)
    {
        
    }

    protected virtual void DoAction2(InputAction.CallbackContext _)
    {

    }

    protected virtual void DoAction3(InputAction.CallbackContext _)
    {

    }

    protected virtual void DoAction4(InputAction.CallbackContext _)
    {

    }

    protected virtual void DoAction5(InputAction.CallbackContext _)
    {

    }

    // Update is called once per frame

}
