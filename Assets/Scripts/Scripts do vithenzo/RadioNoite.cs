using UnityEngine;
using TMPro;
using System.Collections;

public class RadioNoite : MonoBehaviour
{
    public TMP_Text textoRadio;
    public TMP_Text nomeRadio;
    public float delayEntreLetras = 0.03f;
    public GameDialogManager manager;

    void Start()
    {
        nomeRadio.text = "Rádio da Base";
        StartCoroutine(Escrever(GameStats.relatorioUltimaNoite));
    }

    IEnumerator Escrever(string linha)
    {
        textoRadio.text = "";

        foreach (char c in linha)
        {
            textoRadio.text += c;
            yield return new WaitForSeconds(delayEntreLetras);
        }
    }

    public void TocarMensagem()
    {
        StartCoroutine(RadioFlow());
    }

    IEnumerator RadioFlow()
    {
        // toca a mensagem da noite usando o texto já salvo
        yield return StartCoroutine(Escrever(GameStats.relatorioUltimaNoite));

        yield return new WaitForSeconds(2f);

        // desliga o evento do rádio
        GameStats.mostrarRadio = false;

        // volta para o diálogo normal
        manager.MostrarDialogoNormal();
    }
}
