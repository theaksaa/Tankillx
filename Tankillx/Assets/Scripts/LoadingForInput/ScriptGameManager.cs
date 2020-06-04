using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct Level
{
    public GameObject level;
    public float X, Y, Z;
}

public class ScriptGameManager : MonoBehaviour
{
    public GameObject loadingInput;
    public GameObject Score;
    public GameObject tankRedInputCheck;
    public GameObject tankBlueInputCheck;

    public GameObject MazeGenerator;

    public GameObject TankRed;
    public GameObject TankBlue;

    public GameObject RedScore;
    public GameObject BlueScore;

    public GameObject[] Pickups;

    public GameObject PauseMenu;

    private int count = 0;
    private int pickupTimer = -1;
    private int pickupTimerCount = -1;
    private bool isGameStarted = false;
    private bool spawnNewLevel = false;

    public bool gamePaused = false;

    private GameObject maze;
    private GameObject[] tanks = new GameObject[2];

    private bool spawningLevel = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        CheckInput();
        if (pickupTimer != -1 && pickupTimerCount++ >= pickupTimer)
        {
            Debug.Log("pickup");
            pickupTimer = Random.Range(1000, 2000);
            pickupTimerCount = 0;
            Instantiate(Pickups[Random.Range(0, Pickups.Length - 1)], maze.GetComponent<MazeGenerator>().RandomSpawn(), Quaternion.identity);
        }

        if (spawningLevel) return;
        if (isGameStarted)
        {
            if ((tanks[0] == null) || (tanks[1] == null))
            {
                if (count == 0)
                {
                    if (tanks[0] != null)
                    {
                        tanks[0].GetComponent<TankMovement>().enabledMovement = false;
                    }
                    if (tanks[1] != null)
                    {
                        tanks[1].GetComponent<TankMovement>().enabledMovement = false;
                    }
                    count++;
                }
                else if (count++ >= 150)
                {
                    Debug.Log("death");
                    maze.GetComponent<MazeGenerator>().DeleteMaze();
                    SpawnLevel();
                    count = 0;
                }
            }
        }
        else
        {
            if (tankRedInputCheck.activeSelf && tankBlueInputCheck.activeSelf)
            {
                if (count++ >= 100)
                {
                    isGameStarted = true;
                    SpawnLevel();
                    count = 0;
                }
            }

            if (Input.GetKey("v"))
            {
                tankRedInputCheck.SetActive(true);
            }

            if (Input.GetKey("m"))
            {
                tankBlueInputCheck.SetActive(true);

            }
        }
    }

    private void SpawnTanks()
    {
        for (int i = 0; i < 2; i++) if (tanks[i] != null) Destroy(tanks[i]);

        tanks[0] = Instantiate(TankRed, maze.GetComponent<MazeGenerator>().RandomSpawn(), Quaternion.identity);
        tanks[1] = Instantiate(TankBlue, maze.GetComponent<MazeGenerator>().RandomSpawn(), Quaternion.identity);
    }

    // Update is called once per frame
    private void SpawnLevel()
    {
        pickupTimer = Random.Range(1000, 2000);
        pickupTimerCount = 0;
        spawningLevel = true;
        RemoveAllWithTag("Bullet");
        RemoveAllWithTag("Pickup");

        MazeGenerator.GetComponent<MazeGenerator>().mazeRows = Random.Range(3, 7);
        MazeGenerator.GetComponent<MazeGenerator>().mazeColumns = Random.Range(3, 13);
        MazeGenerator.GetComponent<MazeGenerator>().RandomPath = Random.Range(5, 25);

        maze = Instantiate(MazeGenerator, new Vector3(0, 0, 0), Quaternion.identity);
        Invoke("SpawnTanks", 0f);

        loadingInput.SetActive(false);
        Score.SetActive(true);

        //TankRed.GetComponent<TankMovement>().enabledMovement = true;
        //TankBlue.GetComponent<TankMovement>().enabledMovement = true;

        isGameStarted = true;
        spawningLevel = false;
    }

    private void RemoveAllWithTag(string tag)
    {
        foreach (GameObject gameobject in GameObject.FindGameObjectsWithTag(tag)) Destroy(gameobject);
    }

    private void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        gamePaused = false;
        PauseMenu.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        gamePaused = true;
        PauseMenu.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
