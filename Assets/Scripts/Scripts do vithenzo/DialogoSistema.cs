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
    public float typingSpeed = 0.03f;

    [Header("Configurações")]
    public bool mudaCenaAoTerminar = false;
    public string nextSceneName = "";

    private int currentLine = 0;
    private bool isTyping = false;
    private bool dialogoAtivo = false;
    private bool aguardandoTrocaCena = false;

    void Start()
    {
        // Se quiser iniciar automaticamente, mantenha:
        if (dialogueData != null)
            IniciarDialogo();
    }

    void Update()
    {
        // Se está aguardando tecla F para trocar de cena
        if (aguardandoTrocaCena)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                TrocarCena();
            }
            return;
        }

        if (!dialogoAtivo)
            return;

        // Avançar fala com Enter
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
                    EncerrarDialogo();
                }
            }
        }
    }

    public void IniciarDialogo()
    {
        if (dialogueData == null || dialogueData.falas.Count == 0)
        {
            Debug.LogWarning("[DialogoSistema] DialogueData vazio ao iniciar!");
            return;
        }

        currentLine = 0;
        dialogoAtivo = true;
        aguardandoTrocaCena = false;
        gameObject.SetActive(true);

        MostrarFalaAtual(); // mostra imediatamente
    }

    private void MostrarFalaAtual()
    {
        if (dialogueData == null || currentLine < 0 || currentLine >= dialogueData.falas.Count)
            return;

        var falaAtual = dialogueData.falas[currentLine];
        nomeText.text = falaAtual.nomePersonagem;
        StopAllCoroutines();
        StartCoroutine(TypeLine(falaAtual.texto));
    }

    private IEnumerator TypeLine(string line)
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

    private void EncerrarDialogo()
    {
        Debug.Log("[DialogoSistema] Fim do diálogo.");
        dialogoAtivo = false;

        if (mudaCenaAoTerminar)
        {
            aguardandoTrocaCena = true;
            dialogueText.text = "<i>Pressione F para continuar...</i>";
            nomeText.text = "";
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void TrocarCena()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Debug.Log($"[DialogoSistema] Trocando para cena: {nextSceneName}");
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("[DialogoSistema] Nome da próxima cena não definido!");
        }
    }
}
