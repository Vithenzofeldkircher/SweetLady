using UnityEngine;

public class RadioInteract : MonoBehaviour
{
    public RadioNoite radio;
    public InteracaoPorTecla interacao;

    void Start()
    {
        interacao.onInteract = () =>
        {
            radio.gameObject.SetActive(true);
            radio.TocarMensagem();
        };

        radio.gameObject.SetActive(false);
    }
}
