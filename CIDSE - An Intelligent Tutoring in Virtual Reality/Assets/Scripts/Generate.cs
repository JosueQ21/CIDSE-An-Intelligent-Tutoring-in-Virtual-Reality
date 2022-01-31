using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Generate : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public GameObject[] startPrefabs;
    public GameObject[] exitPrefabs;
    public GameObject[] blockPrefabs;
    public GameObject[] doorPrefabs;

    public bool useBoxColliders;
    public bool useLightsForDebugging;
    public bool restoreLightsAfterDebugging;

    public KeyCode reloadKey = KeyCode.Backspace;
    public KeyCode toggleMapKey = KeyCode.M;

    [Range(2,200)]public int mainLength = 20;
    [Range(0, 100)] public int branchLength = 10;
    [Range(0, 50)] public int numBranchLength = 20;
    [Range(0, 200)] public int doorPercent = 50;
    [Range(0, 1f)] public float constructionDely;

    public List<Tile> generatedTiles = new List<Tile>();

    GameObject goCamera, goPlayer;
    List<Connector> availableConnectors = new List<Connector>();
    Color startLightColor = Color.white;
    Transform tileFrom, tileTo, tileRoot;
    Transform container;
    int attempts;
    int maxAttempts = 50;

    // Start is called before the first frame update
    void Start()
    {
        goCamera = GameObject.Find("PreCamera");
        goPlayer = GameObject.FindWithTag("Player");
        StartCoroutine(MazeBuild());
    }

    void Update()
    {
        if (Input.GetKeyDown(reloadKey))
        {
            SceneManager.LoadScene("Game2");
        }
        if(Input.GetKeyDown(toggleMapKey))
        {
            goCamera.SetActive(!goCamera.activeInHierarchy);
            goPlayer.SetActive(!goPlayer.activeInHierarchy);
        }
    }


    IEnumerator MazeBuild()
    {
        goCamera.SetActive(true);
        goPlayer.SetActive(false);
        GameObject goContainer = new GameObject("Main Path");
        container = goContainer.transform;
        container.SetParent(transform);
        tileRoot = CreateStartTile();
        DebugRoomLighting(tileRoot, Color.blue);
        tileTo = tileRoot;
        while(generatedTiles.Count < mainLength)
        {
            yield return new WaitForSeconds(constructionDely);
            tileFrom = tileTo;
            if(generatedTiles.Count == mainLength - 1)
            {
                tileTo = CreateExitTile();
                DebugRoomLighting(tileTo, Color.magenta);
            }
            else
            {
                tileTo = CreateTile();
                DebugRoomLighting(tileTo, Color.yellow);
            }

            ConnectTiles();
            CollisionCheck();
        }

        foreach(Connector connector in container.GetComponentsInChildren<Connector>())
        {
            if (!connector.isConnected)
            {
                if(!availableConnectors.Contains(connector))
                {
                    availableConnectors.Add(connector);
                }
            }
        }

        for(int b = 0; b < numBranchLength; b++)
        {
            if(availableConnectors.Count > 0)
            {
                goContainer = new GameObject("Branch" + (b + 1));
                container = goContainer.transform;
                container.SetParent(transform);
                int availIndex = Random.Range(0, availableConnectors.Count);
                tileRoot = availableConnectors[availIndex].transform.parent.parent;
                availableConnectors.RemoveAt(availIndex);
                tileTo = tileRoot;
                for (int i = 0; i < branchLength - 1; i++)
                {
                    yield return new WaitForSeconds(constructionDely);
                    tileFrom = tileTo;
                    tileTo = CreateTile();
                    DebugRoomLighting(tileTo, Color.green);
                    ConnectTiles();
                    CollisionCheck();
                    if(attempts >= maxAttempts){
                        break;
                    }
                }
            }
            else { break; }
        }
        LightRestoration();
        CleanupBoxs();
        BlockedPassages();
        goCamera.SetActive(false);
        goPlayer.SetActive(true);
    }

    void BlockedPassages()
    {
        foreach(Connector connector in transform.GetComponentsInChildren<Connector>())
        {
            if (!connector.isConnected)
            {
                Vector3 pos = connector.transform.position;
                int wallIndex = Random.Range(0, blockPrefabs.Length);
                GameObject goWall = Instantiate(blockPrefabs[wallIndex], pos, connector.transform.rotation, connector.transform) as GameObject;
                goWall.name = blockPrefabs[wallIndex].name;
            }
        }
    }

    void CollisionCheck()
    {
        BoxCollider box = tileTo.GetComponent<BoxCollider>();
        if(box == null)
        {
            box = tileTo.gameObject.AddComponent<BoxCollider>();
            box.isTrigger = true;
        }
        Vector3 offset = (tileTo.right * box.center.x) + (tileTo.up * box.center.y) + (tileTo.forward * box.center.z);
        Vector3 halfExtents = box.bounds.extents;
        List<Collider> hits = Physics.OverlapBox(tileTo.position + offset, halfExtents, Quaternion.identity, LayerMask.GetMask("Tile")).ToList();
        if(hits.Count > 0)
        {
            if(hits.Exists(x => x.transform != tileFrom && x.transform != tileTo))
            {
                attempts++;
                int toIndex = generatedTiles.FindIndex(x => x.tile == tileTo);
                if(generatedTiles[toIndex].connector != null)
                {
                    generatedTiles[toIndex].connector.isConnected = false;
                }
                generatedTiles.RemoveAt(toIndex);
                DestroyImmediate(tileTo.gameObject);
                if(attempts >= maxAttempts)
                {
                    int fromIndex = generatedTiles.FindIndex(x => x.tile == tileFrom);
                    Tile myTileFrom = generatedTiles[fromIndex];
                    if(tileFrom != tileRoot)
                    {
                        if(myTileFrom.connector != null)
                        {
                            myTileFrom.connector.isConnected = false;
                        }
                        availableConnectors.RemoveAll(x => x.transform.parent.parent == tileFrom);
                        generatedTiles.RemoveAt(fromIndex);
                        DestroyImmediate(tileFrom.gameObject);
                        if (myTileFrom.origin != tileRoot)
                        {
                            tileFrom = myTileFrom.origin;
                        }
                        else if (container.name.Contains("Main"))
                        {
                            if(myTileFrom.origin != null)
                            {
                                tileRoot = myTileFrom.origin;
                                tileFrom = tileRoot;
                            }
                        }
                        else if (availableConnectors.Count > 0)
                        {
                            int availableIndex = Random.Range(0, availableConnectors.Count);
                            tileRoot = availableConnectors[availableIndex].transform.parent.parent;
                            availableConnectors.RemoveAt(availableIndex);
                            tileFrom = tileRoot;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (container.name.Contains("Main"))
                    {
                        if(myTileFrom.origin != null)
                        {
                            tileRoot = myTileFrom.origin;
                            tileFrom = tileRoot;
                        }
                    }
                    else if(availableConnectors.Count > 0)
                    {
                        int availableIndex = Random.Range(0, availableConnectors.Count);
                        tileRoot = availableConnectors[availableIndex].transform.parent.parent;
                        availableConnectors.RemoveAt(availableIndex);
                        tileFrom = tileRoot;
                    }
                    else
                    {
                        return;
                    }
                }

                if (tileFrom != null)
                {
                    if (generatedTiles.Count == mainLength - 1)
                    {
                        tileTo = CreateExitTile();
                        DebugRoomLighting(tileTo, Color.magenta);
                    }
                    else
                    {
                        tileTo = CreateTile();
                        Color retryColor = container.name.Contains("Branch") ? Color.green : Color.yellow;
                        DebugRoomLighting(tileTo, retryColor * 2f);
                    }
                    ConnectTiles();
                    CollisionCheck();
                }

            }
            else { attempts = 0; }
        }

    }

    void LightRestoration()
    {
        if(useLightsForDebugging && restoreLightsAfterDebugging && Application.isEditor)
        {
            Light[] lights = transform.GetComponentsInChildren<Light>();
            foreach(Light light in lights)
            {
                light.color = startLightColor;
            }
        }
    }

    void CleanupBoxs()
    {
        if (!useBoxColliders)
        {
            foreach(Tile myTile in generatedTiles)
            {
                BoxCollider box = myTile.tile.GetComponent<BoxCollider>();
                if(box != null) { Destroy(box); }
            }
        }
    }
    void DebugRoomLighting(Transform tile, Color lightColor)
    {
        if(useLightsForDebugging && Application.isEditor)
        {
            Light[] lights = tile.GetComponentsInChildren<Light>();
            if(lights.Length > 0)
            {
                if(startLightColor == Color.white)
                {
                    startLightColor = lights[0].color;
                }
                foreach(Light light in lights)
                {
                    light.color = lightColor;
                }
            }
        }
    }
    void ConnectTiles()
    {
        Transform connectFrom = GetRandomconnector(tileFrom);
        if (connectFrom == null) { return; }
        Transform connectTo = GetRandomconnector(tileTo);
        if (connectFrom == null) { return; }
        connectTo.SetParent(connectFrom);
        tileTo.SetParent(connectTo);
        connectTo.localPosition = Vector3.zero;
        connectTo.localRotation = Quaternion.identity;
        connectTo.Rotate(0, 180f, 0);
        tileTo.SetParent(container);
        connectTo.SetParent(tileTo.Find("Connectors"));
        generatedTiles.Last().connector = connectFrom.GetComponent<Connector>();
    }

    Transform GetRandomconnector(Transform tile)
    {
        if (tile == null) { return null; }
        List<Connector> connectorList = tile.GetComponentsInChildren<Connector>().ToList().FindAll(x => x.isConnected == false); ;
        if(connectorList.Count > 0)
        {
            int connectorIndex = Random.Range(0, connectorList.Count);
            connectorList[connectorIndex].isConnected = true;
            if(tile == tileFrom)
            {
                BoxCollider box = tile.GetComponent<BoxCollider>();
                if(box == null)
                {
                    box = tile.gameObject.AddComponent<BoxCollider>();
                    box.isTrigger = true;
                }
            }
            return connectorList[connectorIndex].transform;
        }
        return null;
    }


    Transform CreateTile()
    {
        int index = Random.Range(0, tilePrefabs.Length);
        GameObject goTile = Instantiate(tilePrefabs[index], Vector3.zero, Quaternion.identity, container) as GameObject;
        goTile.name = tilePrefabs[index].name;
        Transform origin = generatedTiles[generatedTiles.FindIndex(x => x.tile == tileFrom)].tile;
        generatedTiles.Add(new Tile(goTile.transform, origin));
        return goTile.transform;
    }

    Transform CreateExitTile()
    {
        int index = Random.Range(0, exitPrefabs.Length);
        GameObject goTile = Instantiate(exitPrefabs[index], Vector3.zero, Quaternion.identity, container) as GameObject;
        goTile.name = "Exit Room";
        Transform origin = generatedTiles[generatedTiles.FindIndex(x => x.tile == tileFrom)].tile;
        generatedTiles.Add(new Tile(goTile.transform, origin));
        return goTile.transform;
    }

    Transform CreateStartTile()
    {
        int index = Random.Range(0, startPrefabs.Length);
        GameObject goTile = Instantiate(startPrefabs[index], Vector3.zero, Quaternion.identity, container) as GameObject;
        goTile.name = "Start Room";
        float yRot = Random.Range(0, 4) * 90f;
        goTile.transform.Rotate(0, yRot, 0);
        goPlayer.transform.LookAt(goTile.GetComponentInChildren<Connector>().transform);
        generatedTiles.Add(new Tile(goTile.transform, null));
        return goTile.transform;
    }
}
