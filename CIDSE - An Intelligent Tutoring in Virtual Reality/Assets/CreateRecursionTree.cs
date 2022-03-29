using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CreateRecursionTree : MonoBehaviour
{
    public List<string> tree = new List<string>();
    public bool trigger = false;
    public string col = "";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuildTreePress()
    {
        trigger = true;
    }

    public void BuildTreeRelease()
    {
        trigger = false;
    }
    
    public void OnCollisionEnter(Collision other)
    {
        
        if ((other.gameObject.tag == "ifBlock" || other.gameObject.tag == "elseBlock") && trigger == true)
        {
            col = other.gameObject.tag;
            if (other.gameObject.tag == "ifBlock")
            {
                tree.Add("if");
                other.transform.parent = transform;
            }
            if (other.gameObject.tag == "elseBlock")
            {
                tree.Add("else");
                other.transform.parent = transform;
            }
        }
    }

}

