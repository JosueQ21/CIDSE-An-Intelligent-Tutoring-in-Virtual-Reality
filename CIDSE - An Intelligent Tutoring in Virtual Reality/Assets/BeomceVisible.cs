using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeomceVisible : MonoBehaviour
{
    public GameObject Object1;
    public GameObject Object2;
    private void OnEnable()
    {
        Button_Trigger.onButtonPressed += SetGameScene;
    }

    private void SetGameScene()
    {
        StartCoroutine(waiter());        
    }

    private IEnumerator waiter()
    {
        yield return new WaitForSeconds(2);
        Object1.SetActive(false);
        Object2.SetActive(true);
    }
}
