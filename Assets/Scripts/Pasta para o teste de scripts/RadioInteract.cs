using UnityEngine;

public class RadioInteract : MonoBehaviour
{
    public float distanciaInteracao = 2f;
    public Transform player;

    public DialogoSistema dialogoSistema;
    public DialogueData dialogoInicialRadio;

    public RadioNoite radioNoite;
    public GameObject portaObj;

    void Start()
    {
        portaObj.SetActive(false);
    }

    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) > distanciaInteracao)
            return;

        if (!Input.GetKeyDown(KeyCode.E))
            return;

        // Se um diálogo já está acontecendo, não iniciar outro
        //if (GameDialogManager.DialogoAtivo) return;

        // PRIMEIRA INTERAÇÃO DO RÁDIO
        if (!GameStats.radioDialogoInicialTocado)
        {
            GameStats.radioDialogoInicialTocado = true;

            dialogoSistema.dialogueData = dialogoInicialRadio;
            dialogoSistema.onDialogoAcabar = AtivarPorta;
            dialogoSistema.IniciarDialogo();
            return;
        }

        // INTERAÇÕES APÓS ESCOLHA NA ROOMSCENE
        if (GameStats.mostrarRadio)
        {
            StartCoroutine(radioNoite.RadioFlow());
        }
    }

    void AtivarPorta()
    {
        portaObj.SetActive(true);
    }
}
