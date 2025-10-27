using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class DialogoSistema : MonoBehaviour
{

    [Header("Referências")]
    public DialogueData dialogueData;     // arraste seu DialogueData pra ca
    public TMP_Text dialogueText;         //  o TextMeshPro da UI
    public float typingSpeed = 0.001f;     // velocidade da digitação aqiu
    public TMP_Text nomeText;              // Cv pro nome do personagem 

    int currentLine = 0;
    bool isTyping = false;
    bool canAdvance = true;

    
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

        if (!canAdvance)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                TrocarCena();
            }
            return; // Sai do Update prea não fazer o restante.
        }


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
                currentLine++;
                if (currentLine < dialogueData.falas.Count)
                {
                    MostrarFalaAtual();
                }
                else
                {
                    dialogueText.text = "";
                    nomeText.text = "";
                    canAdvance = false;
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

        // Pega a fala atual
        var falaAtual = dialogueData.falas[currentLine];

        // atualiza o nome e limpa o texto
        nomeText.text = falaAtual.nomePersonagem;
        dialogueText.text = "";

        // vomeca a digitação da fala
        StartCoroutine(TypeLine(falaAtual.texto));
    }


}