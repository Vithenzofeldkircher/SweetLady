using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class RadioController : MonoBehaviour
{
    public static Action OnRadioOn;
    public static Action OnRadioOff;

    public AudioSource murmurSource;
    public string[] legendas;
    public TextMeshProUGUI subtitleText;
    public float tempoLegenda = 4f;
    public float delayEntreMensagens = 3f;
    public Button botaoRadio;

    bool jaTocou;
    bool ativo;

    void Start()
    {
        if (botaoRadio != null)
            botaoRadio.onClick.AddListener(AcionarRadio);

        if (murmurSource != null && murmurSource.clip != null)
        {
            murmurSource.loop = true;
            murmurSource.volume = 1f;
            murmurSource.Play();
        }
    }

    void AcionarRadio()
    {
        ativo = !ativo;

        if (ativo) OnRadioOn?.Invoke();
        else OnRadioOff?.Invoke();

        if (!jaTocou && ativo)
        {
            jaTocou = true;
            StartCoroutine(MostrarMensagens());
        }
    }

    IEnumerator MostrarMensagens()
    {
        for (int i = 0; i < legendas.Length; i++)
        {
            if (murmurSource != null)
                murmurSource.volume = 0.25f;

            subtitleText.text = legendas[i];
            yield return new WaitForSeconds(tempoLegenda);

            subtitleText.text = "";

            if (murmurSource != null)
                murmurSource.volume = 1f;

            yield return new WaitForSeconds(delayEntreMensagens);
        }

        ativo = false;
        OnRadioOff?.Invoke();
    }
}
