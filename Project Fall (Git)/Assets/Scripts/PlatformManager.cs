using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformManager : MonoBehaviour
{
    public GameManager gameManager;
    public InputHandler inputHandler;
    [SerializeField] Platform[] platforms;

    public GameObject player;

    bool gameStarted = false;

    int pIndex = 0;
    int maxIndex;

    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        if (inputHandler == null)
            inputHandler = GameObject.FindGameObjectWithTag("Input Handler").GetComponent<InputHandler>();

        gameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted)
        {
            if (inputHandler.GetSwitchInput())
            {
                FillPlatformArray();

                GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().gravityScale = 1;

                gameStarted = true;
            }
        }

        // Checks if player initiated to switch platforms
        if (gameStarted)
        {
            GetPlatformSwitch();

            WinCollisionCheck();

            if (inputHandler.GetRestartInput())
            {
                gameManager.RestartRequest();
            }

            if (player.transform.position.y < platforms[maxIndex].transform.position.y - 3)
                gameManager.RestartRequest();
        }
    }

    void GetPlatformSwitch()
    {
        if (inputHandler.GetSwitchInput() && platforms[pIndex].CollisionCheck() && pIndex < maxIndex && !platforms[pIndex + 1].TriggerCheck())
        {
            platforms[pIndex].SetCurrentPlatform(false);

            pIndex++;

            platforms[pIndex].SetCurrentPlatform(true);
        }
    }

    void FillPlatformArray()
    {
        // Finds all platform game objects and puts them into an array
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Platform");

        platforms = new Platform[gos.Length];
        for (int i = 0; i < gos.Length; i++)
        {
            platforms[i] = gos[i].GetComponent<Platform>();
        }

        maxIndex = platforms.Length - 1;

        for (int i = 0; i < maxIndex; i++)
        {
            if (platforms[i].transform.position.y < platforms[i + 1].transform.position.y)
            {
                Platform temp = platforms[i];
                platforms[i] = platforms[i + 1];
                platforms[i + 1] = temp;

                i = -1;
            }
        }

        foreach (Platform p in platforms)
        {
            p.OnGameStart();
        }

        // Sets the first platform to current platform
        platforms[pIndex].SetCurrentPlatform(true);
        // Sets last platform as winning platform
        platforms[maxIndex].SetWinPlatform();
    }

    public void WinCollisionCheck()
    {
        if (platforms[maxIndex].CollisionCheck())
        {
            platforms[pIndex].SetCurrentPlatform(false);
            gameManager.GameOver();
        }
    }
}
