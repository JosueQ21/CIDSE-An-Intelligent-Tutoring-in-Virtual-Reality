using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Teacher : XRBaseInteractable
{
    public InputActionReference reference = null;
    public GameObject player = null;
    public VideoPlayer videoPlayer;
    public GameObject text = null;
    

    private float distance;
    public float accDistance = 2;

    protected override void Awake()
    {
        reference.action.started += SelectPlay;
    }

    private void OnDestroy()
    {
        reference.action.started -= SelectPlay;
    }

    private void Update()
    {
        distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if(distance < accDistance)
        {
            text.SetActive(true);
            //text.transform.LookAt(player.transform);
        }
        else
        {
            text.SetActive(false);
        }
    }

    private void SelectPlay(InputAction.CallbackContext context)
    {
        distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if(distance < accDistance)
        {
            videoPlayer.Play();
        }
    }
}
