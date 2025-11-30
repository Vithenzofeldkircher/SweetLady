using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [Header("Velocidades")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;

    Rigidbody2D rb;
    Animator animator;
    Vector3 originalScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
    }

    void Start()
    {
        // Segurança: congelar rotação Z para não rodar com física
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // Movimento usando linearVelocity (novo)
        rb.linearVelocity = new Vector2(horizontal * speed, vertical * speed);

        // Animação
        if (animator != null)
            animator.SetFloat("Speed", rb.linearVelocity.sqrMagnitude);

        // Flip do sprite
        if (horizontal > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (horizontal < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }
}
