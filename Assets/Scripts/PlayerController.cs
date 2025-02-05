using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movePower = 6f;
    public string playerPosition = "";
    private Rigidbody2D rb;
    private Animator anim;
    private int direction = 1;
    private bool alive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Restart();
        if (alive)
        {
            Hurt();
            Attack();
            Run();

        }
        else
        {
            anim.SetBool("isMove", false);
        }
    }

    void Run()
    {

        anim.SetBool("isMove", false);
        Vector2 moveDir = new Vector2(Input.GetAxisRaw("Horizontal" + playerPosition), Input.GetAxisRaw("Vertical" + playerPosition)).normalized;
        anim.SetFloat("MoveX", moveDir.x);
        anim.SetFloat("MoveY", moveDir.y);
        // Make sure there no obstacle
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, 0.5f, LayerMask.GetMask("Wall"));

        if (hit.collider == null)
        {
            if (moveDir.x != 0 || moveDir.y != 0)
            {
                anim.SetBool("isMove", true);
            }
            //Deal with direction
            if (moveDir.x != 0)
            {
                if (moveDir.x < 0 && direction != 1)
                {
                    direction = 1;
                    Flip();
                }
                else if (moveDir.x > 0 && direction != -1)
                {
                    direction = -1;
                    Flip();
                }
                anim.SetBool("isSide", true);
                anim.SetBool("isUp", false);


            }
            else if (moveDir.y > 0)
            {
                anim.SetBool("isUp", true);
                anim.SetBool("isSide", false);
            }
            else if (moveDir.y < 0)
            {
                anim.SetBool("isUp", false);
                anim.SetBool("isSide", false);

            }
            rb.linearVelocity = moveDir * movePower;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1; // 反轉 X 軸
        transform.localScale = newScale;
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetTrigger("attack");
        }
    }
    void Hurt()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.SetTrigger("hurt");
            if (direction == 1)
                rb.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
            else
                rb.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
        }
    }
}