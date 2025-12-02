using UnityEngine;
using System;
public class RadioInteract : MonoBehaviour
{
    public DialogoSistema dialogoSistema;  
    public DialogueData dialogoInicial;   
    public RadioNoite radioNoite;          

    private bool playerPerto = false;

    public Action onRadioAcabar;

    void Update()
    {
        if (playerPerto && Input.GetKeyDown(KeyCode.E))
        {
            if (!GameStats.radioDialogoInicialTocado)
            {

                dialogoSistema.dialogueData = dialogoInicial;
                dialogoSistema.IniciarDialogo();
                GameStats.radioDialogoInicialTocado = true;
            }
            else if (GameStats.radioRelatorioAtivo)
            {

                StartCoroutine(radioNoite.RadioFlow());
                GameStats.radioRelatorioAtivo = false;
            }
            else
            {
                Debug.Log("Nada para tocar no rádio agora.");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerPerto = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerPerto = false;
    }
}
