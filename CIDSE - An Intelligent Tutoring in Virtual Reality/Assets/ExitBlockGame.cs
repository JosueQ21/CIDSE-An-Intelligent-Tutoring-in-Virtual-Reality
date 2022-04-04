using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ExitBlockGame : XRBaseInteractable
{
    public InputActionReference reference = null;
    public GameObject player = null;
    public GameObject text = null;
    private float distance;
    public float accDistance = 2;


    protected override void Awake()
    {
        reference.action.started += ToClass;
        text.SetActive(false);
    }

    protected override void OnDestroy()
    {
        reference.action.started -= ToClass;
    }

    private void Update()
    {
        distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if (distance < accDistance)
        {
            text.SetActive(true);
        }
        else
        {
            text.SetActive(false);
        }
    }

    private void ToClass(InputAction.CallbackContext context)
    {
        if (distance < accDistance)
        {
            SceneManager.LoadScene(2);
        }
    }
}
