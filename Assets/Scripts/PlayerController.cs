using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    //movement variables
    [Range(3, 10)]
    public float speed = 5.0f;
    public float jumpForce = 7.0f;
    
    //component refs
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    //gorundcheck variables
    [Range(0.01f, 0.1f)]
    public float groundCheckRadius = 0.02f;
    public LayerMask isGroundLayer;
    public bool isGrounded = false;

    private Transform groundCheck;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (jumpForce < 0) jumpForce = 7;

        //groundcheck initaliztion
        GameObject newGameObject = new GameObject();
        newGameObject.transform.SetParent(transform);
        newGameObject.transform.localPosition = Vector3.zero;
        newGameObject.name = "GroundCheck";
        groundCheck = newGameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIsGrounded();

        float hInput = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector3(hInput * speed, rb.linearVelocity.y, 0);

        if (Input.GetButtonDown ("Jump") && isGrounded)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        //sprite flip
        if (hInput != 0) sr.flipX = (hInput < 0);
        //if (hInput > 0 && sr.flipX || hInput < 0 && !sr.flipX) sr.flipX = !sr.flipX;

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("speed", Mathf.Abs ( hInput));
    }

    void CheckIsGrounded()
    {
        if (!isGrounded)
        {
            if (rb.linearVelocity.y <= 0) isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius,isGroundLayer);
        }
       else isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

    }
}
