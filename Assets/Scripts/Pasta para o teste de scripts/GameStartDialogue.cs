using UnityEngine;

public class GameStartDialogue : MonoBehaviour
{
    public static GameStartDialogue instance;

    public DialogoSistema dialogoSistema;
    public DialogueData dialogoInicial;
    public GameObject radioObj;

    void Start()
    {
        // Rádio começa desativado
        radioObj.SetActive(false);

        if (!GameStats.dialogoInicialTocado)
        {
            GameStats.dialogoInicialTocado = true;

            dialogoSistema.onDialogoAcabar = AtivarRadio;
            dialogoSistema.dialogueData = dialogoInicial;
            dialogoSistema.IniciarDialogo();
        }
        else
        {
            AtivarRadio();
        }
    }

    void AtivarRadio()
    {
        radioObj.SetActive(true);
    }
}
