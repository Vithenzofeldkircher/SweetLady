using UnityEngine;

public class GameDialogManager : MonoBehaviour
{
    public DialogoSistema velhinha;
    public DialogoSistema dialogoSistema; // Gerencia o diálogo inicial global

    public RadioInteracao radioInteract;
    public PortaInteracao portaInteract;

    public DialogueData velhinhaDialogueData;
    public DialogueData dialogoInicial;
    public DialogueData radioIntroData;

    void Start()
    {

        // Diálogo inicial só toca 1 vez no jogo inteiro
        if (!GameStats.dialogoInicialTocado)
        {
            GameStats.dialogoInicialTocado = true;

            dialogoSistema.dialogueData = dialogoInicial;
            dialogoSistema.onDialogoAcabar = AtivarRadio;
            dialogoSistema.IniciarDialogo();
        }
        else
        {
            // Se já tocou antes, inicia direto a velhinha
            velhinha.dialogueData = velhinhaDialogueData;
            velhinha.onDialogoAcabar = AtivarRadio;
            velhinha.IniciarDialogo();
        }
    }

    void AtivarRadio()
    {
        radioInteract.gameObject.SetActive(true);

        // Define o diálogo inicial do rádio
        radioInteract.dialogoInicial = radioIntroData;

        // Após o diálogo do rádio → libera a porta
        if (radioInteract.dialogoSistema != null)
            radioInteract.dialogoSistema.onDialogoAcabar = AtivarPorta;
    }

    void AtivarPorta()
    {
        portaInteract.gameObject.SetActive(true);
    }
}
