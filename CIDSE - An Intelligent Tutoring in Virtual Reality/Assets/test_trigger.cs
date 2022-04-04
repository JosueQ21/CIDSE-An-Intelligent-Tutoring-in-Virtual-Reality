using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_trigger : MonoBehaviour
{
    private Animator _buttonAnimator;
    public  GameObject x;
    // Start is called before the first frame update
    void Start()
    {
        _buttonAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _buttonAnimator.SetTrigger("ButtonPressed");
        GameObject spark = Instantiate(x.gameObject, new Vector3(3.68199992f, 0.819000006f, 4.59600019f), Quaternion.Euler(0, 0, 90));

        

    }
}
