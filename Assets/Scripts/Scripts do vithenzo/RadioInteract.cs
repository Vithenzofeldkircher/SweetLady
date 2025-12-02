using UnityEngine;
using System;
public class RadioInteract : MonoBehaviour
{
    public DialogoSistema dialogoSistema;  // componente para diálogos normais (DialogueData)
    public DialogueData dialogoInicial;    // diálogo inicial do rádio
    public RadioNoite radioNoite;           // componente para o relatório após decisão

    private bool playerPerto = false;

    public Action onRadioAcabar;

    void Update()
    {
        if (playerPerto && Input.GetKeyDown(KeyCode.E))
        {
            if (!GameStats.radioDialogoInicialTocado)
            {
                // Toca diálogo inicial
                dialogoSistema.dialogueData = dialogoInicial;
                dialogoSistema.IniciarDialogo();
                GameStats.radioDialogoInicialTocado = true;
            }
            else if (GameStats.radioRelatorioAtivo)
            {
                // Toca o relatório da noite
                StartCoroutine(radioNoite.RadioFlow());
                GameStats.radioRelatorioAtivo = false; // desativa pra não repetir
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
