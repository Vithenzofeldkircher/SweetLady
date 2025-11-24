using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptScene : MonoBehaviour
{
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
