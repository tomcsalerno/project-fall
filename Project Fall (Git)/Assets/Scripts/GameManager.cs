using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float restartDelay = .75f;

    public void GameOver()
    {
        Time.timeScale = 0.5f;
        Invoke("Restart", restartDelay);
    }

    public void RestartRequest()
    {
        Restart();
    }

    void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
