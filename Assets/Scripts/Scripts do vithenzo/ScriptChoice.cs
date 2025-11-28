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

    IEnumerator Start()
    {

        // Espera 1 frame até o NPCSpawner atualizar GameStats.currentNPCIsImpostor
        yield return null;

        npcEhImpostor = GameStats.currentNPCIsImpostor;

        if (npcEhImpostor)
            Debug.Log("NPC É IMPOSTOR (ROOM SCENE)!");
        else
            Debug.Log("NPC Inocente (ROOM SCENE).");

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
        if (linhaAtual >= dialogueDataAtual.falas.Count) return;

        var fala = dialogueDataAtual.falas[linhaAtual];

        // troca aqui!
        string texto = AplicarFiltroImpostor(fala.texto);

        nomeText.text = fala.nomePersonagem;
        dialogueText.text = "";

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(texto));
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

        // preparar radio para próxima ida ao Game
        GameStats.mostrarRadio = true;
        GameStats.shouldGoToRoomAfterDialog = true; // manter fluxo caso queira novos encontros
        VerificarFimDeJogo();

        SceneManager.LoadScene("Game");
    }

    public void EscolherDeixarIr()
    {
        PararDialogo();
        OcultarBotoesFinais();

        painelDialogo.SetActive(true);

        if (GameStats.currentNPCIsImpostor)
        {
            GameStats.totalInocentesMortos++; // impostor que ficou pode matar inocentes: contamos como vítimas
            GameStats.relatorioUltimaNoite = $"{GameStats.totalInocentesMortos} pessoas foram mortas naquela noite.";
        }
        else
        {
            GameStats.relatorioUltimaNoite = "Nenhuma tragédia ocorreu ainda essa noite.";
        }

        GameStats.mostrarRadio = true;
        GameStats.shouldGoToRoomAfterDialog = true;
        VerificarFimDeJogo();

        SceneManager.LoadScene("Game");
    }

    private void VerificarFimDeJogo()
    {
        // Vitória: 10 impostores mortos
        if (GameStats.totalImpostoresMortos >= 10)
        {
            Debug.Log("[Game] Jogador venceu! 10 impostores mortos.");
            SceneManager.LoadScene("VictoryScene"); // crie essa cena
            return;
        }

        // Derrota: 5 inocentes mortos
        if (GameStats.totalInocentesMortos >= 5)
        {
            Debug.Log("[Game] Jogador perdeu! 5 inocentes mortos.");
            SceneManager.LoadScene("GameOverScene"); // crie essa cena
            return;
        }
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

