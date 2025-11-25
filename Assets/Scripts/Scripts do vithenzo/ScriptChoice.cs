using TMPro;
using UnityEngine;
using System.Collections;

public class ScriptChoice : MonoBehaviour
{
    [Header("falas finais")]
    public int falaEnvenenarNormal;
    public int falaEnvenenarImpostor;
    public int falaDeixarIrNormal;
    public int falaDeixarIrImpostor;

    [Header("Referências")]
    public DialogueData[] dialogues;
    public TMP_Text dialogueText;
    public TMP_Text nomeText;
    public float typingSpeed = 0.02f;

    [Header("UI")]
    public GameObject botaoPerguntar;
    public GameObject botaoEnvenenar;
    public GameObject botaoDeixarIr;
    public GameObject painelDialogo; // painel do diálogo

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
        npcEhImpostor = (Random.Range(0,20)==0);

        botaoEnvenenar.SetActive(false);
        botaoDeixarIr.SetActive(false);
        botaoPerguntar.SetActive(true);

        painelDialogo.SetActive(true); // painel começa visível
        dialogueText.text = "";
        nomeText.text = "";

        IniciarDialogoAutomatico();
    }

    void IniciarDialogoAutomatico()
    {
        if (dialogues.Length > 0)
        {
            dialogueDataAtual = dialogues[0];
            perguntasFeitas = 1;
            linhaAtual = 0;
            terminouDialogo = false;

            painelDialogo.SetActive(true); // garante que o painel aparevca
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            MostrarFalaAtual();
        }
    }

    public void Perguntar()
    {
        if (isTyping || (dialogueDataAtual != null && !terminouDialogo))
            return; // impede clicar enquanto o diálogo atual ainda não acabou

        if (perguntasFeitas >= maxPerguntas) return;

        dialogueDataAtual = dialogues[perguntasFeitas];
        perguntasFeitas++;
        linhaAtual = 0;
        terminouDialogo = false;

        painelDialogo.SetActive(true); //mostra o painel ao iniciar novo diálogo

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

        // Vai pra proxima fala
        linhaAtual++;

        if (linhaAtual < dialogueDataAtual.falas.Count)
        {
            MostrarFalaAtual();
        }
        else
        {
            // Terminou o dialogo atual
            terminouDialogo = true;
            dialogueText.text = "";
            nomeText.text = "";

            painelDialogo.SetActive(false); //esconde o painel quando termina
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

    public void EscolherEnvenenar()
    {
        painelDialogo.SetActive(true);
        terminouDialogo = false;

        // impostor envenenado → nenhuma morte ocorre
        if (npcEhImpostor)
        {
            GameStats.relatorioUltimaNoite =
                "Nenhuma morte ocorreu esta noite. O impostor foi neutralizado antes de atacar.";

            EscreverResultadoFinal(GameStats.relatorioUltimaNoite);
        }
        else
        {
            // inocente envenenado → 1 morte
            GameStats.mortesTotais++;

            GameStats.relatorioUltimaNoite =
                $"{GameStats.mortesTotais} humanos foram mortos esta noite.";

            EscreverResultadoFinal(GameStats.relatorioUltimaNoite);
        }

        StartCoroutine(VoltarParaCenaGame());
    }

    public void EscolherDeixarIr()
    {
        painelDialogo.SetActive(true);
        terminouDialogo = false;

        // impostor livre → 1 morte brutal
        if (npcEhImpostor)
        {
            GameStats.mortesTotais++;

            GameStats.relatorioUltimaNoite =
                $"{GameStats.mortesTotais} corpos foram encontrados mutilados naquela noite.";

            EscreverResultadoFinal(GameStats.relatorioUltimaNoite);
        }
        else
        {
            // inocente livre → nenhuma morte
            GameStats.relatorioUltimaNoite =
                "Nada foi reportado esta noite.";

            EscreverResultadoFinal(GameStats.relatorioUltimaNoite);
        }

        StartCoroutine(VoltarParaCenaGame());
    }


    void EscreverResultadoFinal(string msg)
    {
        dialogueText.text = "";
        nomeText.text = "Relatório da Noite";

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(msg));
    }

    IEnumerator VoltarParaCenaGame()
    {
        yield return new WaitForSeconds(2.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

}