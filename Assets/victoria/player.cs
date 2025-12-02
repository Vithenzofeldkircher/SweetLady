using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public class Player : MonoBehaviour
{
    [Header("Velocidades")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float speed;

    Rigidbody2D rb;
    Animator anim;
    Vector3 originalScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    void Start()
    {
        // Segurança: congelar rotação Z para não rodar com física
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Movimento
        Vector3 moveDir = new Vector3(h, v, 0).normalized;
        transform.Translate(moveDir * speed * Time.deltaTime);

        // Envia valores para o Animator
        anim.SetFloat("Horizontal", h);
        anim.SetFloat("Vertical", v);

        // Idle quando não está movendo
        anim.SetBool("IsMoving", moveDir.magnitude > 0.1f);
    }
}
