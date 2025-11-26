using UnityEngine;

public class GameDialogManager : MonoBehaviour
{
    public DialogoSistema dialogueSistema;
    public RadioNoite radioNoite;

    void Start()
    {
        if (GameStats.mostrarRadio)
        {
            MostrarRadio();
        }
        else
        {
            MostrarDialogoNormal();
        }
    }

    public void MostrarRadio()
    {
        dialogueSistema.gameObject.SetActive(false);
        radioNoite.gameObject.SetActive(true);
        radioNoite.TocarMensagem();
    }

    public void MostrarDialogoNormal()
    {
        radioNoite.gameObject.SetActive(false);
        dialogueSistema.gameObject.SetActive(true);
        dialogueSistema.IniciarDialogo();
    }
}


