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

    private Button btnPerguntar;

    IEnumerator Start()
    {
        if (botaoPerguntar != null)
            btnPerguntar = botaoPerguntar.GetComponent<Button>();

        if (btnPerguntar != null)
        {
            btnPerguntar.onClick.RemoveAllListeners();
            btnPerguntar.onClick.AddListener(() => {
                Perguntar();
            });
            btnPerguntar.interactable = false;
        }

        yield return null;

        npcEhImpostor = GameStats.currentNPCIsImpostor;

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
        dialogueDataAtual = dialogues[0];
        perguntasFeitas = 1;
        linhaAtual = 0;
        terminouDialogo = false;
        MostrarFalaAtual();
    }

    public void Perguntar()
    {
        if (isTyping || !terminouDialogo) return;
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
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            dialogueText.text = dialogueDataAtual.falas[linhaAtual].texto;
            isTyping = false;

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
            terminouDialogo = true;

            if (dialogueText != null) dialogueText.text = "";
            if (nomeText != null) nomeText.text = "";
            if (btnPerguntar != null) btnPerguntar.interactable = true;
        }
    }

    string AplicarFiltroImpostor(string texto)
    {
        if (SceneManager.GetActiveScene().name != "RoomScene") return texto;
        if (!npcEhImpostor) return texto;

        return texto.Replace("t", "T");
    }

    void MostrarFalaAtual()
    {
        if (linhaAtual >= dialogueDataAtual.falas.Count) return;

        var fala = dialogueDataAtual.falas[linhaAtual];
        string texto = AplicarFiltroImpostor(fala.texto);

        if (nomeText != null) nomeText.text = fala.nomePersonagem;
        if (dialogueText != null) dialogueText.text = "";

        terminouDialogo = false;
        if (btnPerguntar != null) btnPerguntar.interactable = false;

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

        if (linhaAtual == dialogueDataAtual.falas.Count - 1)
        {
            terminouDialogo = true;
            if (btnPerguntar != null) btnPerguntar.interactable = true;
        }
    }

    // -------------------------------
    //        ESCOLHAS FINAIS
    // -------------------------------

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

        GameStats.mostrarRadio = true;

        // 🔊 CHAMAR RÁDIO ANTES DE MUDAR CENA
        TocarRadio();

        SceneManager.LoadScene("Victoria");
        Time.timeScale = 1f;
    }

    public void EscolherDeixarIr()
    {
        PararDialogo();
        OcultarBotoesFinais();
        painelDialogo.SetActive(true);

        if (GameStats.currentNPCIsImpostor)
        {
            GameStats.totalInocentesMortos++;
            GameStats.relatorioUltimaNoite = $"{GameStats.totalInocentesMortos} pessoas foram mortas naquela noite.";
        }
        else
        {
            GameStats.relatorioUltimaNoite = "Nenhuma tragédia ocorreu essa noite.";
        }

        GameStats.mostrarRadio = true;

        // 🔊 CHAMAR RÁDIO ANTES DE MUDAR CENA
        TocarRadio();

        SceneManager.LoadScene("Victoria");
        Time.timeScale = 1f;
    }

    // -------------------------------
    //          FUNÇÃO DO RÁDIO  
    // -------------------------------

    void TocarRadio()
    {
        // 🔔 Ajuste para o nome da **sua** classe de rádio
        RadioNoite radio = FindObjectOfType<RadioNoite>();

        if (radio != null)
        {
            radio.gameObject.SetActive(true);
            radio.StartCoroutine(radio.RadioFlow());  // reproduz a notícia
        }
    }

    // -------------------------------

    private void VerificarFimDeJogo()
    {
        if (GameStats.totalImpostoresMortos >= 1)
        {
            SceneManager.LoadScene("vitoria");
            return;
        }

        if (GameStats.totalInocentesMortos >= 1)
        {
            SceneManager.LoadScene("derrota");
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
