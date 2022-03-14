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
    public GameObject player = null;
    public GameObject text = null;
    private float distance;
    public float accDistance = 2;


    protected override void Awake()
    {
        reference.action.started += TurnOn;
        //screen.SetActive(false);
    }

    protected override void OnDestroy()
    {
        reference.action.started -= TurnOn;
    }

    private void Update()
    {
        distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if (distance < accDistance)
        {
            text.SetActive(true);
            //text.transform.LookAt(player.transform);
        }
        else
        {
            text.SetActive(false);
        }
    }

    private void TurnOn(InputAction.CallbackContext context)
    {
        distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if (distance < accDistance)
        {
            screen.SetActive(true);
        }
    }
    //Previous way of doing it
    /*protected override void Awake()
    {
        base.Awake();
        screen.SetActive(false);
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
    */
}
