using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
    public GameObject painelDialogo;

    [Header("Configuração")]
    public int maxPerguntas = 3;

    private int perguntasFeitas = 0;
    private DialogueData dialogueDataAtual;
    private int linhaAtual = 0;
    private bool isTyping = false;
    private bool terminouDialogo = false;
    private Coroutine typingCoroutine;
    private bool npcEhImpostor;

    void Start()
    {
        npcEhImpostor = (Random.Range(0, 20) == 0);

        if (npcEhImpostor)
            Debug.Log("⚠️ NPC GERADO COMO IMPOSTOR!");
        else
            Debug.Log("NPC inocente.");

        botaoEnvenenar.SetActive(false);
        botaoDeixarIr.SetActive(false);
        botaoPerguntar.SetActive(true);

        painelDialogo.SetActive(true);
        dialogueText.text = "";
        nomeText.text = "";

        IniciarDialogoAutomatico();
    }

    void IniciarDialogoAutomatico()
    {
        dialogueDataAtual = dialogues[0];
        perguntasFeitas = 1;
        linhaAtual = 0;
        terminouDialogo = false;

        MostrarFalaAtual();
    }

    public void Perguntar()
    {
        if (isTyping) return;
        if (!terminouDialogo) return;
        if (perguntasFeitas >= maxPerguntas) return;

        dialogueDataAtual = dialogues[perguntasFeitas];
        perguntasFeitas++;
        linhaAtual = 0;
        terminouDialogo = false;

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
        if (dialogueDataAtual == null) return;

        if (Input.GetKeyDown(KeyCode.Return))
            AvancarFala();
    }

    public void AvancarFala()
    {
        if (dialogueDataAtual == null) return;

        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = dialogueDataAtual.falas[linhaAtual].texto;
            isTyping = false;
            return;
        }

        linhaAtual++;

        if (linhaAtual < dialogueDataAtual.falas.Count)
        {
            MostrarFalaAtual();
        }
        else
        {
            terminouDialogo = true;
            dialogueText.text = "";
            nomeText.text = "";
        }
    }

    void MostrarFalaAtual()
    {
        if (linhaAtual >= dialogueDataAtual.falas.Count) return;

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

    public void EscolherEnvenenar()
    {
        PararDialogo();
        OcultarBotoesFinais();

        painelDialogo.SetActive(true);

        if (npcEhImpostor)
        {
            GameStats.relatorioUltimaNoite =
                "Nenhuma morte ocorreu esta noite. O impostor foi neutralizado antes de atacar.";
        }
        else
        {
            GameStats.totalMortes++;
            GameStats.relatorioUltimaNoite =
                $"{GameStats.totalMortes} humanos foram mortos esta noite.";
        }

        GameStats.mostrarRadio = true;

        SceneManager.LoadScene("Game");
    }

    public void EscolherDeixarIr()
    {
        PararDialogo();
        OcultarBotoesFinais();

        painelDialogo.SetActive(true);

        if (npcEhImpostor)
        {
            GameStats.totalMortes++;
            GameStats.relatorioUltimaNoite =
                $"{GameStats.totalMortes} corpos foram encontrados mutilados naquela noite.";
        }
        else
        {
            GameStats.relatorioUltimaNoite = "Nada foi reportado esta noite.";
        }

        GameStats.mostrarRadio = true;

        SceneManager.LoadScene("Game");
    }

    void OcultarBotoesFinais()
    {
        botaoPerguntar.SetActive(false);
        botaoEnvenenar.SetActive(false);
        botaoDeixarIr.SetActive(false);
    }


    public void PararDialogo()
    {
        StopAllCoroutines();
        isTyping = false;
        this.enabled = false;
    }
}

