using TMPro;
using UnityEngine;
using System.Collections;

public class ScriptChoice : MonoBehaviour
{
    [Header("Referências")]
    public DialogueData[] dialogues; // 3 DialogueData diferentes
    public TMP_Text dialogueText;
    public TMP_Text nomeText;
    public float typingSpeed = 0.001f;

    [Header("UI")]
    public GameObject botaoPerguntar;
    public GameObject botaoEnvenenar;
    public GameObject botaoDeixarIr;

    [Header("Limite de Perguntas")]
    public int maxPerguntas = 3;
    private int perguntasFeitas = 0;

    private DialogueData dialogueData;
    private int currentLine = 0;
    private bool isTyping = false;

    void Start()
    {
        botaoEnvenenar.SetActive(false);
        botaoDeixarIr.SetActive(false);
    }

    public void Perguntar()
    {
        if (perguntasFeitas >= maxPerguntas)
            return;

        dialogueData = dialogues[perguntasFeitas];
        perguntasFeitas++;
        currentLine = 0;
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
        if (dialogueData == null) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = dialogueData.falas[currentLine].texto;
                isTyping = false;
            }
            else
            {
                if (currentLine < dialogueData.falas.Count - 1)
                {
                    currentLine++;
                    MostrarFalaAtual();
                }
                else
                {
                    dialogueText.text = "";
                    nomeText.text = "";
                }
            }
        }
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

    void MostrarFalaAtual()
    {
        if (dialogueData == null) return;
        if (currentLine < 0 || currentLine >= dialogueData.falas.Count) return;

        var falaAtual = dialogueData.falas[currentLine];
        nomeText.text = falaAtual.nomePersonagem;
        dialogueText.text = "";

        StartCoroutine(TypeLine(falaAtual.texto));
    }

}

