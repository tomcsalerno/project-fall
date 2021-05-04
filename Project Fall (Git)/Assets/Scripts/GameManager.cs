using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    InputHandler inputHandler;
    
    [SerializeField]
    PlatformManager platformManager;

    // Start is called before the first frame update
    void Start()
    {
        if (inputHandler == null)
			inputHandler = GameObject.FindGameObjectWithTag("Input Handler").GetComponent<InputHandler>();

        if (platformManager == null)
			platformManager = GameObject.FindGameObjectWithTag("Platform Manager").GetComponent<PlatformManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputHandler.GetRestartInput()) 
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (platformManager.WinCollisionCheck())
            Time.timeScale = 0.5f;
    }
}
