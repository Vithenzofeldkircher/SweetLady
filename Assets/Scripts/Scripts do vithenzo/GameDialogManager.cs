using UnityEngine;

public class GameDialogManager : MonoBehaviour
{
    public DialogoSistema velhinha;
    public RadioInteract radioInteract;  // Script para interação com radio (E)
    public PortaInteracao portaInteract;  // Script para interação com porta (E)
    public DialogueData velhinhaDialogueData;
    public DialogueData radioIntroData;


    void Start()
    {
        // Começa SEM interação com rádio e porta
        //radioInteract.gameObject.SetActive(false);
        //portaInteract.gameObject.SetActive(false);

        // Configura diálogo da velhinha
        // Se já tocou o diálogo inicial antes, NÃO executar de novo
        if (GameStats.dialogoInicialTocado)
        {
            // Apenas ativa rádio e porta se necessário
            radioInteract.gameObject.SetActive(true);
            portaInteract.gameObject.SetActive(false); // depende da lógica
            return;
        }

        // Primeira vez entrando na cena → toca diálogo inicial
        GameStats.dialogoInicialTocado = true;

        velhinha.mudarCenaAoTerminar = false;
        velhinha.onDialogoAcabar = AtivarRadio;

        velhinha.IniciarDialogo();
    }

    void AtivarRadio()
    {
        // Ativa o objeto do rádio para que o jogador possa interagir
        radioInteract.gameObject.SetActive(true);

        // Quando usar o rádio pela primeira vez → toca diálogo inicial
      //  radioInteract.dialogoInicial = radioIntroData;
        radioInteract.dialogoSistema.onDialogoAcabar = AtivarPorta;
    }

    void AtivarPorta()
    {
        // Ativa a porta após o rádio
        portaInteract.gameObject.SetActive(true);
    }
}
