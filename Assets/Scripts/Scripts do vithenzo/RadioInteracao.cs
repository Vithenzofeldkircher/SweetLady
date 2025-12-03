using UnityEngine;

public class RadioInteracao : MonoBehaviour
{
    public RadioNoite radioNoite;
    public DialogoSistema dialogoSistema;
    public DialogueData dialogoInicial;
    public float distanciaInteracao = 2f;

    Transform player;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        if (dist <= distanciaInteracao && Input.GetKeyDown(KeyCode.E))
        {
            // Impede diálogo duplo
            // if (dialogoSistema.DialogoAtivo) return;

            if (!GameStats.radioDialogoInicialTocado)
            {
                // Primeiro diálogo do rádio
                dialogoSistema.dialogueData = dialogoInicial;
                dialogoSistema.IniciarDialogo();

                GameStats.radioDialogoInicialTocado = true;
            }
            else
            {
                // Segunda interação ? fluxo do Rádio da Noite
                radioNoite.gameObject.SetActive(true);
                StartCoroutine(radioNoite.RadioFlow());
            }
        }
    }
}
