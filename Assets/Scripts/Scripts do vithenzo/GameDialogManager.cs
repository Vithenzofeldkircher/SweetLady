using UnityEngine;

public class GameDialogManager : MonoBehaviour
{
    public DialogoSistema velhinha;
    public RadioInteract radioInteract;
    public PortaInteract portaInteract;
    public DialogoSistema dialogueSistema;

    void Start()
    {
        dialogueSistema.IniciarDialogo();

        // Começa SEM interação com radio e porta
        radioInteract.gameObject.SetActive(false);
        portaInteract.gameObject.SetActive(false);

        // Inicia diálogo da velhinha
        velhinha.mudaCenaAoTerminar = false;
        velhinha.onDialogoAcabar = MostrarInteracaoRadio;

        velhinha.IniciarDialogo();
    }

    void MostrarInteracaoRadio()
    {
        radioInteract.gameObject.SetActive(true);
    }

    public void MostrarInteracaoPorta()
    {
        portaInteract.gameObject.SetActive(true);
    }
}
