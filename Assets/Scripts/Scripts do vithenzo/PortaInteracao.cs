using UnityEngine;
using UnityEngine.SceneManagement;

public class PortaInteracao : MonoBehaviour
{
    [Header("Configurações")]
    public string cenaDestino = "RoomScene";   // Nome da cena para onde vai
    public float distanciaInteracao = 2f;      // Distância máxima para interagir

    private Transform player;

    void Start()
    {
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");

        if (objPlayer != null)
        {
            player = objPlayer.transform;
        }
        else
        {
            Debug.LogError("Nenhum objeto com Tag 'Player' foi encontrado na cena!");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distancia = Vector3.Distance(player.position, transform.position);

        if (distancia <= distanciaInteracao)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Carregando cena: " + cenaDestino);
                SceneManager.LoadScene(cenaDestino);
            }
        }
    }
}
