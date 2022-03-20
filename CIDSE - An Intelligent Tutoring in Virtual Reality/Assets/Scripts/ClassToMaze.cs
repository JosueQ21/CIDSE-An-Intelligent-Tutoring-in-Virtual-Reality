using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class ClassToMaze : XRBaseInteractable
{
    public InputActionReference reference = null;
    public GameObject screen = null;
    public GameObject player = null;
    private float distance;
    public float accDistance = 2;

    protected override void Awake()
    {
        base.Awake();
        reference.action.started += SelectGame;
    }

    private void OnDestroy()
    {
        reference.action.started -= SelectGame;
    }

    private void Update()
    {
        distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
    }

    private void SelectGame(InputAction.CallbackContext context)
    {
        if (distance < accDistance)
        {
            SceneManager.LoadScene(4);
        }
    }
}
