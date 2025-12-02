using UnityEngine;

public class RadioInteract : MonoBehaviour
{
    public float distanciaInteracao = 2f;
    public Transform player;

    public DialogoSistema dialogoSistema;
    public DialogueData dialogoInicialRadio;

    public RadioNoite radioNoite;

    void Start()
    {
        
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
            dialogoSistema.IniciarDialogo();

            return;
        }



        if (GameStats.mostrarRadio)
        {
            // Chame o método que inicia a Coroutine internamente
            radioNoite.IniciarRadioFlow();
            return; // Certifique-se de sair para evitar interações duplicadas
        }
    }
}
