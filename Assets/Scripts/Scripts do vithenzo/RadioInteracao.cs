using UnityEngine;

public class RadioInteracao : MonoBehaviour
{
    public RadioNoite radioNoite;
    public DialogoSistema dialogManager;  // Seu sistema de diálogo
    public GameDialogManager dialogoInicial;    // Seu Dialogue Data para primeiro diálogo
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
            if (!GameStats.jaFaleiComNPC)
            {
                // Toca diálogo inicial
                dialogManager.IniciarDialogo(dialogoInicial);
            }
            else
            {
                // Toca o rádio normalmente, depois do diálogo com NPC
                radioNoite.gameObject.SetActive(true);
                StartCoroutine(radioNoite.RadioFlow());
            }
        }
    }
}
