using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    public float GetRotationInput() 
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public bool GetSwitchInput() 
    {
        return Input.GetButtonDown("Jump");
    }

    public bool GetRestartInput()
    {
        return Input.GetKeyDown(KeyCode.R);
    }
}
