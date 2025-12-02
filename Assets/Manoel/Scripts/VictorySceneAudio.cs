using UnityEngine;
using UnityEngine.SceneManagement;  


public class VictorySceneAudio : MonoBehaviour
{
    public AudioClip[] musicasSuspense;
    public AudioClip estatica;
    public AudioClip batidas;

    AudioSource musicaSource;
    AudioSource estaticaSource;
    AudioSource batidasSource;

    void Start()
    {
        musicaSource = gameObject.AddComponent<AudioSource>();
        estaticaSource = gameObject.AddComponent<AudioSource>();
        batidasSource = gameObject.AddComponent<AudioSource>();

        int index = Random.Range(0, musicasSuspense.Length);
        musicaSource.clip = musicasSuspense[index];
        musicaSource.loop = true;
        musicaSource.Play();
    }

    public void RadioLigou()
    {
        musicaSource.Pause();
        estaticaSource.clip = estatica;
        estaticaSource.loop = true;
        estaticaSource.Play();
        batidasSource.Stop();
    }

    public void RadioDesligou()
    {
        estaticaSource.Stop();
        batidasSource.clip = batidas;
        batidasSource.loop = true;
        batidasSource.Play();
        musicaSource.Pause();
    }

    public void IrParaRoomScene()
    {
        estaticaSource.Stop();
        batidasSource.Stop();
        musicaSource.Stop();
        SceneManager.LoadScene("RoomScene");
    }
}
