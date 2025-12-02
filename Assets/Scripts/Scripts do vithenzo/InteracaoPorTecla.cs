using UnityEngine;
using System;

public class InteracaoPorTecla : MonoBehaviour
{
    public float distancia = 2f;
    public KeyCode tecla = KeyCode.E;
    public Transform player;
    public Action onInteract;

    [Header("UI")]
    public GameObject iconeInteragir;

    void Start()
    {
        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null) player = obj.transform;
        }

        if (iconeInteragir != null)
            iconeInteragir.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float d = Vector3.Distance(transform.position, player.position);

        bool podeInteragir = d <= distancia;

        if (iconeInteragir != null)
            iconeInteragir.SetActive(podeInteragir);

        if (podeInteragir && Input.GetKeyDown(tecla))
        {
            onInteract?.Invoke();
        }
    }
}
