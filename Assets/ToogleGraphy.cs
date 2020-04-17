using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToogleGraphy : MonoBehaviour
{
    public InputActionReference ToogleAction;

    // Start is called before the first frame update
    void Start()
    {
        ToogleAction.action.performed += Toogle;
    }

    private void Toogle(InputAction.CallbackContext obj)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
