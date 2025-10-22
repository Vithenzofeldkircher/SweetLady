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

        dialogueText.text = "";
        StartCoroutine(TypeLine(dialogueData.falas[currentLine]));
    }

    void Update()
    {

        if (!canAdvance)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                TrocarCena();
            }
            return; // Sai do Update para não processar o restante.
        }


        // avanço do texto com Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                // Pula digitaçã mostra texto completo
                StopAllCoroutines();
                dialogueText.text = dialogueData.falas[currentLine];
                isTyping = false;
            }
            else
            {
                // Tenta ir para a próxima fala
                currentLine++;
                if (currentLine < dialogueData.falas.Count)
                {
                    // Inicia a próxima linha
                    StartCoroutine(TypeLine(dialogueData.falas[currentLine]));
                }
                else
                {

                    dialogueText.text = "";
                    canAdvance = false; // AQUI é onde a troca de cena é habilitada.
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
}