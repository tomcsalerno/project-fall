using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public InputHandler inputHandler;
    [SerializeField] GameObject[] platforms;
    [SerializeField] Platform[] platformComponents;

    int pIndex = 0;
    int maxIndex;

    // Start is called before the first frame update
    void Start()
    {
        if (inputHandler == null)
            inputHandler = GameObject.FindGameObjectWithTag("Input Handler").GetComponent<InputHandler>();

        FillPlatformArrays();
        SetFirstPlatform();
        SetLastPlatform();
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if player initiated to switch platforms
        GetPlatformSwitch();
    }

    void GetPlatformSwitch()
    {
        if (inputHandler.GetSwitchInput() && platformComponents[pIndex].CollisionCheck() && pIndex < maxIndex)
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

    void SetFirstPlatform()
    {
        // Sets the first platform to current platform
        platformComponents[pIndex].SetCurrentPlatform(true);
    }

    void SetLastPlatform()
    {
        // Sets last platform as winning platform
        platformComponents[maxIndex].SetWinPlatform();
    }

    public bool WinCollisionCheck()
    {
        if (platformComponents[maxIndex].CollisionCheck()) 
        {
            platformComponents[pIndex].SetCurrentPlatform(false);
            return true;
        }

        return false;
    }
}
