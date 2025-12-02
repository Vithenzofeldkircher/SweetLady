using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ScriptChoice : MonoBehaviour
{
    [Header("Referências")]
    public DialogueData[] dialogues;
    public TMP_Text dialogueText;
    public TMP_Text nomeText;
    public float typingSpeed = 0.02f;

    [Header("UI")]
    public GameObject botaoPerguntar;   // GameObject do botão (com componente Button)
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

    // referênca ao componente Button para manipular via código
    private Button btnPerguntar;

    IEnumerator Start()
    {
        // proteção contra referências faltando
        if (botaoPerguntar == null) Debug.LogWarning("[ScriptChoice] botaoPerguntar não atribuído!");
        if (dialogueText == null) Debug.LogWarning("[ScriptChoice] dialogueText não atribuído!");
        if (nomeText == null) Debug.LogWarning("[ScriptChoice] nomeText não atribuído!");
        if (painelDialogo == null) Debug.LogWarning("[ScriptChoice] painelDialogo não atribuído!");

        // pega componente Button (se existir)
        if (botaoPerguntar != null)
            btnPerguntar = botaoPerguntar.GetComponent<Button>();

        // Se o botão existir, remove listeners antigos (se houver) e adiciona por código
        if (btnPerguntar != null)
        {
            btnPerguntar.onClick.RemoveAllListeners();
            btnPerguntar.onClick.AddListener(() => {
                Debug.Log("[ScriptChoice] Clique no botão Perguntar detectado.");
                Perguntar();
            });
            btnPerguntar.interactable = false; // inicialmente desligado até terminar a fala
        }
        else
        {
            Debug.LogWarning("[ScriptChoice] componente Button não encontrado no objeto botaoPerguntar.");
        }

        // Espera 1 frame até o NPCSpawner atualizar GameStats.currentNPCIsImpostor
        yield return null;

        npcEhImpostor = GameStats.currentNPCIsImpostor;

        if (npcEhImpostor)
            Debug.Log("NPC É IMPOSTOR (ROOM SCENE)!");
        else
            Debug.Log("NPC Inocente (ROOM SCENE).");

        if (botaoEnvenenar != null) botaoEnvenenar.SetActive(false);
        if (botaoDeixarIr != null) botaoDeixarIr.SetActive(false);
        if (botaoPerguntar != null) botaoPerguntar.SetActive(true);

        if (painelDialogo != null) painelDialogo.SetActive(true);
        if (dialogueText != null) dialogueText.text = "";
        if (nomeText != null) nomeText.text = "";

        IniciarDialogoAutomatico();
    }

    void IniciarDialogoAutomatico()
    {
        if (dialogues == null || dialogues.Length == 0)
        {
            Debug.LogWarning("[ScriptChoice] dialogues vazio!");
            return;
        }

        dialogueDataAtual = dialogues[0];
        perguntasFeitas = 1;
        linhaAtual = 0;
        terminouDialogo = false;

        MostrarFalaAtual();
    }

    public void Perguntar()
    {
        Debug.Log($"[ScriptChoice] Perguntar() chamado. terminouDialogo={terminouDialogo}, perguntasFeitas={perguntasFeitas}, max={maxPerguntas}");

        if (isTyping)
        {
            Debug.Log("[ScriptChoice] Ainda digitando, ignorando Perguntar()");
            return;
        }

        if (!terminouDialogo)
        {
            Debug.Log("[ScriptChoice] diálogo ainda não terminado (terminouDialogo=false)");
            return;
        }

        if (perguntasFeitas >= maxPerguntas)
        {
            Debug.Log("[ScriptChoice] já atingiu maxPerguntas");
            return;
        }

        // troca o dialogueData e reinicia
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
        if (botaoPerguntar != null) botaoPerguntar.SetActive(false);
        if (botaoEnvenenar != null) botaoEnvenenar.SetActive(true);
        if (botaoDeixarIr != null) botaoDeixarIr.SetActive(true);
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
            // pára a corrotina atual e mostra a linha completa
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            dialogueText.text = dialogueDataAtual.falas[linhaAtual].texto;
            isTyping = false;

            // se a fala exibida for a última do diálogo, marcar terminouDialogo
            if (linhaAtual == dialogueDataAtual.falas.Count - 1)
            {
                terminouDialogo = true;
                if (btnPerguntar != null) btnPerguntar.interactable = true;
            }
            return;
        }

        linhaAtual++;

        if (linhaAtual < dialogueDataAtual.falas.Count)
        {
            MostrarFalaAtual();
        }
        else
        {
            // quando o diálogo termina (todas as linhas), permitimos perguntar
            terminouDialogo = true;
            if (dialogueText != null) dialogueText.text = "";
            if (nomeText != null) nomeText.text = "";
            if (btnPerguntar != null) btnPerguntar.interactable = true;

            Debug.Log("[ScriptChoice] Diálogo terminou - terminouDialogo = true");
        }
    }

    string AplicarFiltroImpostor(string texto)
    {
        // só aplica na RoomScene
        if (SceneManager.GetActiveScene().name != "RoomScene")
            return texto;

        // só aplica se o NPC for impostor
        if (!npcEhImpostor)
            return texto;

        return texto.Replace("t", "T");
    }

    void MostrarFalaAtual()
    {
        if (dialogueDataAtual == null) return;
        if (linhaAtual >= dialogueDataAtual.falas.Count) return;

        var fala = dialogueDataAtual.falas[linhaAtual];
        string texto = AplicarFiltroImpostor(fala.texto);

        if (nomeText != null) nomeText.text = fala.nomePersonagem;
        if (dialogueText != null) dialogueText.text = "";

        // sempre que começar uma fala nova, não está terminado
        terminouDialogo = false;
        if (btnPerguntar != null) btnPerguntar.interactable = false;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(texto));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        if (dialogueText != null) dialogueText.text = "";

        foreach (char c in line)
        {
            if (dialogueText != null) dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        // se esta fala é a última do dialogueDataAtual, marcamos que terminou o diálogo
        if (dialogueDataAtual != null && linhaAtual == dialogueDataAtual.falas.Count - 1)
        {
            terminouDialogo = true;
            Debug.Log("[ScriptChoice] última fala exibida -> terminouDialogo = true");
            if (btnPerguntar != null) btnPerguntar.interactable = true;
        }
    }

    public void EscolherEnvenenar()
    {
        PararDialogo();
        OcultarBotoesFinais();

        if (painelDialogo != null) painelDialogo.SetActive(true);

        if (GameStats.currentNPCIsImpostor)
        {
            GameStats.totalImpostoresMortos++;
            GameStats.relatorioUltimaNoite = $"{GameStats.totalImpostoresMortos} impostores foram encontrados mortos esta noite.";
        }
        else
        {
            GameStats.totalInocentesMortos++;
            GameStats.relatorioUltimaNoite = $"{GameStats.totalInocentesMortos} humanos foram mortos esta noite.";
        }

        GameStats.mostrarRadio = true;
        GameStats.shouldGoToRoomAfterDialog = true;
        VerificarFimDeJogo();

        SceneManager.LoadScene("Victoria");
        Time.timeScale = 1.0f;
    }

    public void EscolherDeixarIr()
    {
        PararDialogo();
        OcultarBotoesFinais();

        if (painelDialogo != null) painelDialogo.SetActive(true);

        if (GameStats.currentNPCIsImpostor)
        {
            GameStats.totalInocentesMortos++;
            GameStats.relatorioUltimaNoite = $"{GameStats.totalInocentesMortos} pessoas foram mortas naquela noite.";
        }
        else
        {
            GameStats.relatorioUltimaNoite = "Nenhuma tragédia ocorreu ainda essa noite.";
        }

        GameStats.mostrarRadio = true;
        GameStats.shouldGoToRoomAfterDialog = true;
        VerificarFimDeJogo();

        SceneManager.LoadScene("Victoria");
        Time.timeScale = 1.0f;
    }

    private void VerificarFimDeJogo()
    {
        if (GameStats.totalImpostoresMortos >= 5)
        {
            Debug.Log("[Game] Jogador venceu! 5 impostores mortos.");
            SceneManager.LoadScene("VictoryScene");
            return;
        }

        if (GameStats.totalInocentesMortos >= 5)
        {
            Debug.Log("[Game] Jogador perdeu! 5 inocentes mortos.");
            SceneManager.LoadScene("GameOverScene");
            return;
        }
    }

    void OcultarBotoesFinais()
    {
        if (botaoPerguntar != null) botaoPerguntar.SetActive(false);
        if (botaoEnvenenar != null) botaoEnvenenar.SetActive(false);
        if (botaoDeixarIr != null) botaoDeixarIr.SetActive(false);
    }

    public void PararDialogo()
    {
        StopAllCoroutines();
        isTyping = false;
        this.enabled = false;
    }
}
