using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LeaveGame : MonoBehaviour
{
    public InputActionReference reference = null;

    // Start is called before the first frame update
    void Awake()
    {
        reference.action.started += buttonPress;
    }

    private void OnDestroy()
    {
        reference.action.started -= buttonPress;
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    private void buttonPress(InputAction.CallbackContext context)
    {
        //Load back to classroon
        GameManager.Instance.LoadGame(2);
    }
}
