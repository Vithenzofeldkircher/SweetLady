using UnityEngine;

public class RadioInteracao : MonoBehaviour
{
    public RadioNoite radioNoite;
    public DialogoSistema dialogManager; 
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
            // Se um diálogo está rolando, não deixa iniciar outro
            //if (dialogManager.DialogoAtivo) return;

            if (!GameStats.radioDialogoInicialTocado)
            {
                // Primeiro diálogo
                dialogManager.dialogueData = dialogoInicial;
                dialogManager.IniciarDialogo();

                GameStats.radioDialogoInicialTocado = true;
            }
            else
            {
                // Rádio Noite
                radioNoite.gameObject.SetActive(true);
                StartCoroutine(radioNoite.RadioFlow());
            }
        }
    }
}
