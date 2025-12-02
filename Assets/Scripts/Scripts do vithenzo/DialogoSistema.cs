using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class DialogoSistema : MonoBehaviour
{
    public Action onDialogoAcabar;

    [Header("Referências")]
    public DialogueData dialogueData;
    public TMP_Text dialogueText;
    public TMP_Text nomeText;
    public float typingSpeed = 0.03f;

    [Header("Configurações")]
    public bool mudarCenaAoTerminar = false;
    public string proximaCena = "";

    private int currentLine = 0;
    private bool isTyping = false;
    public bool dialogoAtivo = false;
    private bool aguardandoTeclaTroca = false;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        // Aguarda F para trocar de cena (se ativado)
        if (aguardandoTeclaTroca)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                TrocarCena();
            }
            return;
        }

        if (!dialogoAtivo) return;

        // Avançar com Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = dialogueData.falas[currentLine].texto;
                isTyping = false;
                return;
            }

            currentLine++;

            if (currentLine < dialogueData.falas.Count)
                MostrarFalaAtual();
            else
                EncerrarDialogo();
        }
    }

    // Chamar este método para iniciar o diálogo
    public void IniciarDialogo(GameDialogManager dialogoInicial)
    {
        IniciarDialogo(null);

        if (dialogueData == null || dialogueData.falas.Count == 0)
        {
            Debug.LogWarning("[DialogoSistema] DialogueData vazio.");
            return;
        }

        currentLine = 0;
        dialogoAtivo = true;
        aguardandoTeclaTroca = false;

        gameObject.SetActive(true);
        MostrarFalaAtual();
    }

    private void MostrarFalaAtual()
    {
        if (currentLine < 0 || currentLine >= dialogueData.falas.Count)
            return;

        var fala = dialogueData.falas[currentLine];

        nomeText.text = fala.nomePersonagem;

        // Filtro caso o NPC seja impostor (apenas na RoomScene)
        bool impostor = GameStats.currentNPCIsImpostor;
        string textoProcessado = TextProcessor.ProcessForImpostor(impostor, fala.texto);

        StopAllCoroutines();
        StartCoroutine(TypeLine(textoProcessado));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void EncerrarDialogo()
    {
        dialogoAtivo = false;

        Debug.Log("[DialogoSistema] Fim do diálogo.");

        // Se configurar mudança de cena
        if (mudarCenaAoTerminar)
        {
            aguardandoTeclaTroca = true;
            dialogueText.text = "<i>Pressione F para continuar...</i>";
            nomeText.text = "";
        }
        else
        {
            gameObject.SetActive(false);
        }

        onDialogoAcabar?.Invoke(); // dispara evento para o rádio
    }

    private void TrocarCena()
    {
        if (!string.IsNullOrEmpty(proximaCena))
        {
            Debug.Log($"Trocando cena para: {proximaCena}");
            SceneManager.LoadScene(proximaCena);
        }
        else
        {
            Debug.LogWarning("[DialogoSistema] próximaCena não definida!");
        }
    }

    internal void IniciarDialogo()
    {
        IniciarDialogo(null);
    }

    public static class TextProcessor
    {
        public static string ProcessForImpostor(bool isImpostor, string input)
        {
            if (SceneManager.GetActiveScene().name != "RoomScene") return input;
            if (!isImpostor || string.IsNullOrEmpty(input)) return input;

            return input.Replace('t', 'T');
        }
    }
}

