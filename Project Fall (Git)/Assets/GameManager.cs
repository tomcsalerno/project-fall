using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    InputHandler inputHandler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inputHandler.GetRestartInput()) 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
