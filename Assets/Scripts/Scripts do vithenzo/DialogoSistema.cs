using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DialogoSistema : MonoBehaviour
{
    [Header("Referências")]
    public DialogueData dialogueData;
    public TMP_Text dialogueText;
    public TMP_Text nomeText;
    public float typingSpeed = 0.02f; // ligeiramente mais alto evita race do TMP

    int currentLine = 0;
    bool isTyping = false;
    bool canAdvance = true;
    Coroutine typingCoroutine; // referência única da corrotina

    void Start()
    {
        if (dialogueData == null || dialogueData.falas.Count == 0)
        {
            Debug.LogError("DialogueData não configurado ou vazio");
            return;
        }

        MostrarFalaAtual();
    }

    void Update()
    {
        //Bloqueia entrada enquanto digita (como no ScriptChoice)
        if (isTyping) return;

        if (!canAdvance)
        {
            if (Input.GetKeyDown(KeyCode.F))
                TrocarCena();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            currentLine++;
            if (currentLine < dialogueData.falas.Count)
            {
                MostrarFalaAtual();
            }
            else
            {
                dialogueText.text = "";
                nomeText.text = "";
                dialogueData = null; // evita reaproveitar dados antigos
                canAdvance = false;
            }
        }
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        yield return null; // dá 1 frame pro TMP inicializar direito (evita mistura visual)

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    [Header("Transição")]
    public string nextSceneName = "NomeDaProximaCena";

    void TrocarCena()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    void MostrarFalaAtual()
    {
        if (currentLine < 0 || currentLine >= dialogueData.falas.Count)
            return;

        var falaAtual = dialogueData.falas[currentLine];

        nomeText.text = falaAtual.nomePersonagem;
        dialogueText.text = "";

        // Garante que só 1 coroutine por vez
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        typingCoroutine = StartCoroutine(TypeLine(falaAtual.texto));
    }
}

