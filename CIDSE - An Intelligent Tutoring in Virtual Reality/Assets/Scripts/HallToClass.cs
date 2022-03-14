using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HallToClass : XRBaseInteractable
{
    protected override void Awake()
    {
        base.Awake();
        onHoverEntered.AddListener(StartPress);
    }

    protected override void OnDestroy()
    {
        onHoverEntered.RemoveListener(StartPress);
    }

    private void StartPress(XRBaseInteractor interactor)
    {
        SceneManager.LoadScene(2);
    }
}
