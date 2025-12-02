using UnityEngine;
using UnityEngine.SceneManagement;

public class PortaInteract : MonoBehaviour
{
    public string cenaDestino = "RoomScene";
    public InteracaoPorTecla interacao;

    void Start()
    {
        interacao.onInteract = () =>
        {
            SceneManager.LoadScene(cenaDestino);
        };
    }
}
