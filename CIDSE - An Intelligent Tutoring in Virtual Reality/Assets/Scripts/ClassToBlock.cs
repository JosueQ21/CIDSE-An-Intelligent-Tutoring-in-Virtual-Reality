using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class ClassToBlock : XRBaseInteractable
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
            SceneManager.LoadScene(5);
        }
    }
    /*
    protected override void Awake()
    {
        base.Awake();
        onHoverEntered.AddListener(selectGame);
        onHoverExited.AddListener(exitGame);
    }

    private void OnDestroy()
    {
        onHoverEntered.RemoveListener(selectGame);
        onHoverExited.RemoveListener(exitGame);
    }

    private void selectGame(XRBaseInteractor interactor)
    {
        //Computer can be turned on when hand is in.
        reference.action.started += buttonPress;
    }

    private void exitGame(XRBaseInteractor interactor)
    {
        //Make it not possible to turn on computer when hand is not in
        reference.action.started -= buttonPress;
    }

    private void buttonPress(InputAction.CallbackContext context)
    {
        GameManager.Instance.LoadGame(5);
    }
    */
}
