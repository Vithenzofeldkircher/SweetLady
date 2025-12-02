using UnityEngine;

public class RadioInteracao : MonoBehaviour
{
    public RadioNoite radioNoite;
    public DialogoSistema dialogManager; 
    public GameDialogManager dialogoInicial;    
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

                dialogManager.IniciarDialogo(dialogoInicial);
            }
            else
            {

                radioNoite.gameObject.SetActive(true);
                StartCoroutine(radioNoite.RadioFlow());
            }
        }
    }
}
