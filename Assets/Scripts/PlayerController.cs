using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movePower = 6f;
    public string playerPosition = "";
    public GameObject bomb;
    public GameObject bomb_hold_side;
    public GameObject bomb_walk_side;
    public GameObject gun_up;
    public GameObject gun_side;
    public GameObject gun_down;
    public GameObject gun_hold_side;
    public GameObject gun_walk_side;
    private Rigidbody2D rb;
    private Animator anim;
    private int direction = 1;
    private bool alive = true;
    private bool holdingBomb = false;
    private bool holdingGun = false;
    private PlayerSide currentSide = PlayerSide.Down;
    private Dictionary<PlayerSide, Vector2> bomb_Position = new Dictionary<PlayerSide, Vector2>(){
        {PlayerSide.Up, new Vector2(0, 0.13f)},
        {PlayerSide.Side, new Vector2(-0.45f, -0.41f)},
        {PlayerSide.Down, new Vector2(0, -0.51f)}
    };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Run();
        // Restart();
        if (alive)
        {
            Hurt();
            Attack();

        }
        else
        {
            anim.SetBool("isMove", false);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            holdingGun = false;
            holdingBomb = true;
            anim.SetInteger("Status", 1);
            bomb.SetActive(holdingBomb);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            holdingGun = true;
            holdingBomb = false;
            anim.SetInteger("Status", 2);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            holdingGun = false;
            holdingBomb = false;
            anim.SetInteger("Status", 0);
        }
    }

    void Run()
    {
        Vector2 moveDir = new Vector2(Input.GetAxisRaw("Horizontal" + playerPosition),
                                      Input.GetAxisRaw("Vertical" + playerPosition)).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, 0.5f, LayerMask.GetMask("Wall"));

        if (hit.collider == null)
        {
            bool isMoving = moveDir.x != 0 || moveDir.y != 0;
            anim.SetBool("isMove", isMoving);

            if (isMoving) // 只有在移動時才更新方向
            {
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
                    currentSide = PlayerSide.Side;
                }
                else if (moveDir.y > 0)
                {
                    anim.SetBool("isUp", true);
                    anim.SetBool("isSide", false);
                    currentSide = PlayerSide.Up;
                }
                else if (moveDir.y < 0)
                {
                    anim.SetBool("isUp", false);
                    anim.SetBool("isSide", false);
                    currentSide = PlayerSide.Down;
                }
            }
            SetBombPosition(isMoving);
            SetGun(isMoving);

            float threshold = 0.01f;
            float moveX = Mathf.Abs(moveDir.x) < threshold ? 0 : moveDir.x;
            float moveY = Mathf.Abs(moveDir.y) < threshold ? 0 : moveDir.y;
            anim.SetFloat("MoveX", moveX);
            anim.SetFloat("MoveY", moveY);

            rb.linearVelocity = moveDir * movePower;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isMove", false);
        }
    }
    void SetGun(bool isMoving)
    {
        if (!holdingGun)
        {
            gun_hold_side.SetActive(false);
            gun_walk_side.SetActive(false);
            gun_up.SetActive(false);
            gun_side.SetActive(false);
            gun_down.SetActive(false);
        }
        else
        {
            if (currentSide == PlayerSide.Up)
            {
                gun_up.SetActive(true);
                gun_side.SetActive(false);
                gun_down.SetActive(false);
            }
            else if (currentSide == PlayerSide.Down)
            {
                gun_up.SetActive(false);
                gun_side.SetActive(false);
                gun_down.SetActive(true);
            }
            else
            {
                gun_walk_side.SetActive(true);
                gun_up.SetActive(false);
                gun_side.SetActive(true);
                gun_down.SetActive(false);
            }
            if (!isMoving)
            {
                if (currentSide == PlayerSide.Side)
                {
                    gun_walk_side.SetActive(false);
                    gun_hold_side.SetActive(true);
                }
                else
                {
                    gun_walk_side.SetActive(false);
                    gun_hold_side.SetActive(false);
                }
            }
        }
    }

    void SetBombPosition(bool isMoving)
    {
        if (!holdingBomb)
        {
            bomb_hold_side.SetActive(false);
            bomb_walk_side.SetActive(false);
            bomb.SetActive(false);
        }
        else
        {
            if (isMoving)
            {
                bomb_hold_side.SetActive(false);
                if (currentSide == PlayerSide.Side)
                {
                    bomb_walk_side.SetActive(true);
                }
                else
                {
                    bomb_walk_side.SetActive(false);
                }
            }
            else
            {
                bomb_walk_side.SetActive(false);
                if (currentSide == PlayerSide.Side)
                {
                    bomb_hold_side.SetActive(true);
                }
                else
                {
                    bomb_hold_side.SetActive(false);
                }
            }
            if (currentSide == PlayerSide.Up)
            {
                bomb.GetComponent<Renderer>().sortingOrder = -1;
            }
            else
            {
                bomb.GetComponent<Renderer>().sortingOrder = 1;
            }
            bomb.transform.localPosition = bomb_Position[currentSide];
        }
    }

    void _Run()
    {

        // anim.SetBool("isMove", false);
        Vector2 moveDir = new Vector2(Input.GetAxisRaw("Horizontal" + playerPosition), Input.GetAxisRaw("Vertical" + playerPosition)).normalized;

        // Make sure there no obstacle
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, 0.5f, LayerMask.GetMask("Wall"));

        if (hit.collider == null)
        {
            if (moveDir.x != 0 || moveDir.y != 0)
            {
                anim.SetBool("isMove", true);
            }
            else
            {

                anim.SetBool("isMove", false);
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
            float threshold = 0.1f;
            float moveX = Mathf.Abs(moveDir.x) < threshold ? 0 : moveDir.x;
            float moveY = Mathf.Abs(moveDir.y) < threshold ? 0 : moveDir.y;
            anim.SetFloat("MoveX", moveX);
            anim.SetFloat("MoveY", moveY);
            rb.linearVelocity = moveDir * movePower;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isMove", false);
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
public enum PlayerSide
{
    Up,
    Side,
    Down
}