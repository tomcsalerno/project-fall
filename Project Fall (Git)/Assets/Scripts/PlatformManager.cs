using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformManager : MonoBehaviour
{
    public InputHandler inputHandler;
    [SerializeField] GameObject[] platforms;
    [SerializeField] Platform[] platformComponents;

    bool gameStarted = false;

    int pIndex = 0;
    int maxIndex;

    // Start is called before the first frame update
    void Start()
    {
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
                FillPlatformArrays();
                SetFirstLastPlatforms();

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
                RestartScene();
            }
        }
    }

    void GetPlatformSwitch()
    {
        if (inputHandler.GetSwitchInput() && platformComponents[pIndex].CollisionCheck() && pIndex < maxIndex && !platformComponents[pIndex + 1].TriggerCheck())
        {
            platformComponents[pIndex].SetCurrentPlatform(false);

            pIndex++;

            platformComponents[pIndex].SetCurrentPlatform(true);
        }
    }

    void FillPlatformArrays()
    {
        // Finds all platform gmae objects and puts them into an array
        platforms = GameObject.FindGameObjectsWithTag("Platform");

        maxIndex = platforms.Length - 1;

        for (int i = 0; i < maxIndex; i++)
        {
            if (platforms[i].transform.position.y < platforms[i + 1].transform.position.y)
            {
                GameObject temp = platforms[i];
                platforms[i] = platforms[i + 1];
                platforms[i + 1] = temp;

                i = -1;
            }
        }

        platformComponents = new Platform[platforms.Length];

        for (int i = 0; i < platforms.Length; i++)
        {
            platformComponents[i] = platforms[i].GetComponent<Platform>();
        }
    }

    void SetFirstLastPlatforms()
    {
        // Sets the first platform to current platform
        platformComponents[pIndex].SetCurrentPlatform(true);
        // Sets last platform as winning platform
        platformComponents[maxIndex].SetWinPlatform();
    }

    public void WinCollisionCheck()
    {
        if (platformComponents[maxIndex].CollisionCheck())
        {
            platformComponents[pIndex].SetCurrentPlatform(false);
            StartCoroutine(PlayerHasWon());
        }
    }

    IEnumerator PlayerHasWon()
    {
        Time.timeScale = 0.5f;

        yield return new WaitForSeconds(.75f);

        RestartScene();
    }

    void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
