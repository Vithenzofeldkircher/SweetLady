using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DialogoSistema : MonoBehaviour
{

    [Header("Refer�ncias")]
    public DialogueData dialogueData;     // arraste seu DialogueData pra ca
    public TMP_Text dialogueText;         //  o TextMeshPro da UI
    public float typingSpeed = 0.001f;     // velocidade da digita��o aqiu

    int currentLine = 0;
    bool isTyping = false;
    bool canAdvance = true;

    void Start()
    {
        if (dialogueData == null || dialogueData.falas.Count == 0)
        {
            Debug.LogError("DialogueData n�o configurado ou vazio");
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
            return; // Sai do Update para n�o processar o restante.
        }


        // avan�o do texto com Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                // Pula digita�� mostra texto completo
                StopAllCoroutines();
                dialogueText.text = dialogueData.falas[currentLine];
                isTyping = false;
            }
            else
            {
                // Tenta ir para a pr�xima fala
                currentLine++;
                if (currentLine < dialogueData.falas.Count)
                {
                    // Inicia a pr�xima linha
                    StartCoroutine(TypeLine(dialogueData.falas[currentLine]));
                }
                else
                {

                    dialogueText.text = "";
                    canAdvance = false; // AQUI � onde a troca de cena � habilitada.
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

    [Header("Transi��o")]
    public string nextSceneName = "NomeDaProximaCena";

    void TrocarCena()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}