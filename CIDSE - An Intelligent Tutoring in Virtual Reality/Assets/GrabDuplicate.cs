using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabDuplicate : MonoBehaviour
{
    public GameObject rootObj;
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
        if (other.gameObject.tag == "Plyaer")
        {
            GameObject duplicate = Instantiate(rootObj);
            duplicate.transform.position = duplicate.transform.position + new Vector3(1, 1, 1);
        }
    }
}
