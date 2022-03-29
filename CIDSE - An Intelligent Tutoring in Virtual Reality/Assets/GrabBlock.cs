using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabBlock : MonoBehaviour
{
    private bool duplicate = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "ifBlock" || other.gameObject.tag == "elseBlock") && duplicate == false)
        {
            duplicate = true;
            GameObject BlockDupe = Instantiate(other.gameObject);
            BlockDupe.transform.position = other.transform.position;
            BlockDupe.gameObject.AddComponent<XRGrabInteractable>();
            BlockDupe.gameObject.AddComponent<Rigidbody>();
            BlockDupe.gameObject.tag = "Untagged";
            StartCoroutine(waiter());
        }
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(4);
        duplicate = false;
    }
}
