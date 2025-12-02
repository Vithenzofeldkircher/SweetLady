using UnityEngine;
using UnityEngine.SceneManagement;

public class PortaInteract : MonoBehaviour
{
    public float distanciaInteracao = 2f;
    public Transform player;

    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) > distanciaInteracao)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("RoomScene");
        }
    }
}
