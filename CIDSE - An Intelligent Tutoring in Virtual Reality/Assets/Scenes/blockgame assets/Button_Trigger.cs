using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Button_Trigger : MonoBehaviour
{
    private Animator _buttonAnimator;
    public GameObject TutorialWall;
    private Animator _tutorialWall;
    // Start is called before the first frame update
    void Start()
    {
        _buttonAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _buttonAnimator.SetTrigger("ButtonPressed");
        StartCoroutine(waiter());
    }

    private IEnumerator waiter()
    {
        yield return new WaitForSeconds(2);
        _tutorialWall = TutorialWall.GetComponent<Animator>();
        _tutorialWall.SetTrigger("wallToDrop");
    }
}
