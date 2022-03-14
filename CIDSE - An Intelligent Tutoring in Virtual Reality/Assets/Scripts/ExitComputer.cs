using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class ExitComputer : XRBaseInteractable
{
    public InputActionReference reference = null;
    public GameObject screen = null;

    protected override void Awake()
    {
        base.Awake();
        reference.action.started += SelectGame;
    }

    private void OnDestroy()
    {
        reference.action.started -= SelectGame;
    }

    private void SelectGame(InputAction.CallbackContext context)
    {
        if (screen.activeSelf)
        {
            screen.SetActive(false);
        }
    }
}
