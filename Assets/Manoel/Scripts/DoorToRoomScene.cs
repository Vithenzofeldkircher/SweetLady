using UnityEngine;

public class DoorToRoomScene : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            VictorySceneAudio v = Object.FindFirstObjectByType<VictorySceneAudio>();
            if (v != null) v.IrParaRoomScene();
        }
    }
}
