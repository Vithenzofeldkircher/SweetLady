using TMPro;
using UnityEngine;
using System.Collections;

public class ScriptChoice : MonoBehaviour
{
    [Header("Referências")]
    public DialogueData[] dialogues;
    public TMP_Text dialogueText;
    public TMP_Text nomeText;
    public float typingSpeed = 0.02f;

    [Header("UI")]
    public GameObject botaoPerguntar;
    public GameObject botaoEnvenenar;
    public GameObject botaoDeixarIr;

    [Header("Configuração")]
    public int maxPerguntas = 3;

    private int perguntasFeitas = 0;
    private DialogueData dialogueDataAtual;
    private int linhaAtual = 0;
    private bool isTyping = false;
    private bool terminouDialogo = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        botaoEnvenenar.SetActive(false);
        botaoDeixarIr.SetActive(false);
        botaoPerguntar.SetActive(true);

        dialogueText.text = "";
        nomeText.text = "";
    }

    public void Perguntar()
    {
        if (isTyping || (dialogueDataAtual != null && !terminouDialogo))
            return; // impede clicar enquanto o diálogo atual ainda não acabou

        if (perguntasFeitas >= maxPerguntas) return;

        // Seleciona o diálogo correspondente
        dialogueDataAtual = dialogues[perguntasFeitas];
        perguntasFeitas++;
        linhaAtual = 0;
        terminouDialogo = false;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        MostrarFalaAtual();

        if (perguntasFeitas >= maxPerguntas)
            MostrarOpcoesFinais();
    }

    void MostrarOpcoesFinais()
    {
        botaoPerguntar.SetActive(false);
        botaoEnvenenar.SetActive(true);
        botaoDeixarIr.SetActive(true);
    }

    void Update()
    {
        if (dialogueDataAtual == null || terminouDialogo) return;

        if (Input.GetKeyDown(KeyCode.Return))
            AvancarFala();
    }

    public void AvancarFala()
    {
        if (dialogueDataAtual == null) return;

        if (isTyping)
        {
            // Termina a digitação imediatamente
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            dialogueText.text = dialogueDataAtual.falas[linhaAtual].texto;
            isTyping = false;
            return;
        }

        // Vai pra próxima fala
        linhaAtual++;

        if (linhaAtual < dialogueDataAtual.falas.Count)
        {
            MostrarFalaAtual();
        }
        else
        {
            // Terminou o diálogo atual
            terminouDialogo = true;
            dialogueText.text = "";
            nomeText.text = "";
        }
    }

    void MostrarFalaAtual()
    {
        if (dialogueDataAtual == null || linhaAtual >= dialogueDataAtual.falas.Count)
            return;

        var fala = dialogueDataAtual.falas[linhaAtual];
        nomeText.text = fala.nomePersonagem;
        dialogueText.text = "";

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(fala.texto));
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
}


