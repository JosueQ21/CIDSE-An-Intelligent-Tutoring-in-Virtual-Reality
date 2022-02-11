using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smoothTurn : MonoBehaviour
{
    public Rigidbody player;
    public Transform rotator;
    public float speed;

    void Update()
    {
        var joystickAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

        if(joystickAxis.x >= .8f)
        {
            player.transform.RotateAround(rotator.position, rotator.up, speed * .1f);

        }
        if (joystickAxis.x <= -.8f)
        {
            player.transform.RotateAround(rotator.position, rotator.up, speed * -.1f);

        }
    }
}
