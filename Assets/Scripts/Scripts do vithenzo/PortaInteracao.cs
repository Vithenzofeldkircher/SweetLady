using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PortaInteracao : MonoBehaviour
{
    public string cenaDestino = "RoomScene";
    public float distanciaInteracao = 2f;
    private Transform player;

    void Start()
    {
        // Acha o player automaticamente
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
            player = obj.transform;
    }

    void Update()
    {
        if (player == null) return;

        // Checa a distância
        float distancia = Vector3.Distance(player.position, transform.position);

        if (distancia <= distanciaInteracao && Input.GetKeyDown(KeyCode.E))
        {
            // Vai para a cena
            SceneManager.LoadScene(cenaDestino);
        }
    }
}
