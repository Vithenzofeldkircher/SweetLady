
using UnityEngine;
using UnityEngine.SceneManagement;


public class PortaInteracao : MonoBehaviour
{
    public string cenaDestino = "RoomScene"; // cena que será carregada
    public float distanciaInteracao = 2f; // distância max para interagir


    private Transform player;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) <= distanciaInteracao)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(cenaDestino);
            }
        }
    }
}