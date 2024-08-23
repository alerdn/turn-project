using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void ReloadLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
}