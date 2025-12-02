using UnityEngine;

public class GameDialogManager : MonoBehaviour
{
    public DialogoSistema velhinha;
    public RadioInteract radioInteract;  // Script para interação com radio (E)
    public PortaInteracao portaInteract;  // Script para interação com porta (E)

    void Start()
    {
        // Começa SEM interação com rádio e porta
        radioInteract.gameObject.SetActive(false);
        portaInteract.gameObject.SetActive(false);

        // Configura diálogo da velhinha
        velhinha.mudarCenaAoTerminar = false; // NÃO muda de cena automaticamente
        velhinha.onDialogoAcabar = AtivarRadio;

        // Inicia diálogo da velhinha
        velhinha.IniciarDialogo();
    }

    void AtivarRadio()
    {
        // Ativa o objeto do rádio para que o jogador possa interagir
        radioInteract.gameObject.SetActive(true);

        // Passa referência para que, ao terminar o rádio, a porta seja ativada
        radioInteract.onRadioAcabar = AtivarPorta;
    }

    void AtivarPorta()
    {
        // Ativa a porta após o rádio
        portaInteract.gameObject.SetActive(true);
    }
}
