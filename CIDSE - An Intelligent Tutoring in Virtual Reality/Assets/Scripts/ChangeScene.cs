using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class ChangeScene : XRBaseInteractable
{

    protected override void Awake()
    {
        base.Awake();
        onHoverEntered.AddListener(StartPress);
    }

    private void OnDestroy()
    {
        onHoverEntered.RemoveListener(StartPress);
    }

    private void StartPress(XRBaseInteractor interactor)
    {
        GameManager.Instance.SpawnLocation.Set(-1, -1, 1);
        GameManager.Instance.LoadGame(1);
    }
    
}
