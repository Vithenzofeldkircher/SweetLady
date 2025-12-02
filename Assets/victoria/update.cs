using UnityEngine;


[RequireComponent(typeof(Animator))]    
public class UpdateController : MonoBehaviour
{
    public float speed = 5f;       // Velocidade do movimento
    public Animator anim;          // Referência ao Animator

    void Start()
    {
            anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
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
