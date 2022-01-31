using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    public Vector3 SpawnLocation;

    public GameObject loadingScreen;
    public GameObject asset;
    
    public Slider rotate;
    public Slider zoom;
    public Camera cam;

    public Toggle doorControl;

    //Made by Josue
    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "mainmenu")
        {
            asset.transform.eulerAngles = new Vector3(0, rotate.value * 360, 0);
            cam.orthographicSize = zoom.value;
        }
    }

    //Josue Stuff
    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    public void LoadGame(int nextScene)
    {
        loadingScreen.gameObject.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()));
        scenesLoading.Add(SceneManager.LoadSceneAsync(nextScene));

        StartCoroutine(GetSceneLoadProgress());
    }

    public IEnumerator GetSceneLoadProgress()
    {
        for(int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[1].isDone)
            {
                yield return null;
            }
        }

        loadingScreen.gameObject.SetActive(false);
    }
}
