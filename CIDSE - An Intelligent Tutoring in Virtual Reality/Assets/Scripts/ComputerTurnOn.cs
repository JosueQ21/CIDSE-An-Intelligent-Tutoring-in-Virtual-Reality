using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ComputerTurnOn : XRBaseInteractable
{
    public GameObject screen = null;
    public InputActionReference reference = null;
    protected override void Awake()
    {
        base.Awake();
        onHoverEntered.AddListener(turnOn);
        onHoverExited.AddListener(exitComputer);
    }

    private void OnDestroy()
    {
        onHoverEntered.RemoveListener(turnOn);
        onHoverExited.RemoveListener(exitComputer);
    }

    private void turnOn(XRBaseInteractor interactor)
    {
        //Computer can be turned on when hand is in.
        reference.action.started += buttonPress;
    }

    private void exitComputer(XRBaseInteractor interactor)
    {
        //Make it not possible to turn on computer when hand is not in
        reference.action.started -= buttonPress;
    }

    private void buttonPress(InputAction.CallbackContext context)
    {
        //Turn on the computer screen
        screen.SetActive(true);
    }

}
