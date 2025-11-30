using UnityEngine;

public class Interactable : MonoBehaviour
{
    [TextArea] // Para o texto ficar mais fácil de editar no Inspector
    public string interactionMessage = "Você interagiu com o objeto!";

    // Esse método é chamado quando o player clica no objeto
    public virtual void Interact()
    {
        Debug.Log(interactionMessage);
    }
}