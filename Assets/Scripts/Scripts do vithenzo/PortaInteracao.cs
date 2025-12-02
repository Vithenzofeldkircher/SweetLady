using UnityEngine;
using UnityEngine.SceneManagement;

public class PortaInteracao : MonoBehaviour
{
    public string cenaDestino = "RoomScene";
    public float distanciaInteracao = 2f;

    Transform player;

    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
            player = obj.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distancia = Vector3.Distance(player.position, transform.position);

        if (distancia <= distanciaInteracao && Input.GetKeyDown(KeyCode.E))
        {
            VictorySceneAudio audio = Object.FindFirstObjectByType<VictorySceneAudio>();
            if (audio != null)
            {
                audio.IrParaRoomScene();
                return;
            }

            SceneManager.LoadScene(cenaDestino);
        }
    }
}
