using UnityEngine;

public class RadioInteracao : MonoBehaviour
{
    public RadioNoite radioNoite;
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

        if (dist <= distanciaInteracao)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                radioNoite.gameObject.SetActive(true);
                radioNoite.TocarMensagem();
            }
        }
    }
}
