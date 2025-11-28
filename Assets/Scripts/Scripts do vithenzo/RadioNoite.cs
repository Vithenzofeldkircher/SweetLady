using TMPro;
using UnityEngine;
using System.Collections;

public class RadioNoite : MonoBehaviour
{
    public TMP_Text textoRadio;
    public TMP_Text nomeRadio;
    public float delayEntreLetras = 0.03f;

    public GameDialogManager manager;

    void Awake()
    {
        nomeRadio.text = "Rádio da Base";
    }

    public void TocarMensagem()
    {
        StartCoroutine(RadioFlow());
    }

    IEnumerator RadioFlow()
    {
        textoRadio.text = "";

        yield return StartCoroutine(Escrever(GameStats.relatorioUltimaNoite));

        yield return new WaitForSeconds(1.5f);

        GameStats.mostrarRadio = false;
        Debug.Log("[RadioNoite] Radio terminou. Relatório: " + GameStats.relatorioUltimaNoite);
        manager.MostrarDialogoNormal();
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
}
