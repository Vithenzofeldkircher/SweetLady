using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptScene : MonoBehaviour
{
    public void trocaDeCenaStart()
    {
        SceneManager.LoadScene("Start");
        Time.timeScale = 1.0f;
    }

    public void trocaDeCenaGame()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1.0f;
    }

    public void trocaDeCenaCredits()
    {
        SceneManager.LoadScene("Creditos");
        Time.timeScale = 1.0f;
    }

    public void Saida()
    {
        Application.Quit();
    }
}
