using System;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDataPersistent
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
    public GameObject flash;
    public GameObject bomb_explosion;
    public GameObject bulletPrefab; // bullet Prefab
    private Rigidbody2D rb;
    private Animator anim;
    private int direction = 1;
    private bool alive = true;
    private bool holdingBomb = false;
    private bool holdingGun = false;
    public float attackRate = 0.5f;
    private float nextAttackTime = 0f;
    private PlayerSide currentSide = PlayerSide.Down;
    private CharacterType playerType;
    private Dictionary<PlayerSide, Vector2> bomb_Position = new Dictionary<PlayerSide, Vector2>(){
        {PlayerSide.Up, new Vector2(0, 0.13f)},
        {PlayerSide.Side, new Vector2(-0.45f, -0.41f)},
        {PlayerSide.Down, new Vector2(0, -0.51f)}
    };
    private Dictionary<PlayerSide, Vector2> flash_Position = new Dictionary<PlayerSide, Vector2>(){
        {PlayerSide.Up, new Vector2(0, 0.6f)},
        {PlayerSide.Side, new Vector2(-0.9f, -0.5f)},
        {PlayerSide.Down, new Vector2(0, -1.25f)}
    };
    private Dictionary<PlayerSide, Vector2> flash_Position_rifle = new Dictionary<PlayerSide, Vector2>(){
        {PlayerSide.Up, new Vector2(0, 1.5f)},
        {PlayerSide.Side, new Vector2(-1.8f, -0.43f)},
        {PlayerSide.Down, new Vector2(0, -2f)}
    };
    private Dictionary<PlayerSide, int> flash_Rotation = new Dictionary<PlayerSide, int>(){
        {PlayerSide.Up, -90},
        {PlayerSide.Side, 0},
        {PlayerSide.Down, 90}
    };
    private Dictionary<PlayerSide, int> flash_Order = new Dictionary<PlayerSide, int>(){
        {PlayerSide.Up, -2},
        {PlayerSide.Side, 3},
        {PlayerSide.Down, 2}
    };
    private Dictionary<PlayerSide, Vector3> bomb_explosion_Position = new Dictionary<PlayerSide, Vector3>(){
        {PlayerSide.Up, new Vector3(0, 3f, 0)},
        {PlayerSide.Side, new Vector3(3f, -0.5f, 0)},
        {PlayerSide.Down, new Vector3(0, -3f, 0)}
    };

    private bool isFreeze = false;
    void Awake()
    {
        playerType = GameState.Instance.GetPlayerType(Int32.Parse(playerPosition));
    }
    void Start()
    {
        EventManager.Instance.UpdateUserTakenItem += UpdateTakenItem;
        EventManager.Instance.ActivePlayerHealth += OpenHealthUI;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GetComponent<HealthSystem>().SetHealth(GameState.charactersData[playerType].Health);
    }

    void OnDestroy()
    {
        EventManager.Instance.UpdateUserTakenItem -= UpdateTakenItem;
        EventManager.Instance.ActivePlayerHealth -= OpenHealthUI;
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
        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     holdingGun = true;
        //     holdingBomb = false;
        //     anim.SetInteger("Status", 2);
        // }
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

        // RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, 0.1f, LayerMask.GetMask("Wall"));

        bool isMoving = moveDir.x != 0 || moveDir.y != 0;
        SetBombPosition(isMoving);
        SetGun(isMoving);
        if (/*hit.collider == null &&*/ !isFreeze)
        {
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

    void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1; // 反轉 X 軸
        transform.localScale = newScale;

        transform.Find("HealthBarCanvas").localScale *= -1;
    }
    void Attack()
    {
        if (Input.GetAxisRaw("Fire" + playerPosition) > 0 && Time.time >= nextAttackTime)
        {
            if (holdingBomb)
            {
                anim.SetTrigger("IsThrow");
                anim.SetInteger("Status", 0);
                SetBombExplosionPosition();
                StartCoroutine(Helper.Delay(ThrowBomb, 1f));
            }
            else if (holdingGun)
            {
                flash.transform.localPosition = flash_Position_rifle[currentSide];
                flash.transform.localRotation = Quaternion.Euler(0, 0, flash_Rotation[currentSide]);
                flash.GetComponent<Renderer>().sortingOrder = flash_Order[currentSide];
                flash.SetActive(true);
                flash.GetComponent<PunchController>().isUsingGun = true;

                GameObject bullet = Instantiate(bulletPrefab, flash.transform.position, flash.transform.rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

                switch (currentSide)
                {
                    case PlayerSide.Up:
                        rb.linearVelocity = new Vector2(0, 10);
                        break;
                    case PlayerSide.Down:
                        rb.linearVelocity = new Vector2(0, -10);
                        break;
                    case PlayerSide.Side:
                        rb.linearVelocity = new Vector2(10, 0) * (-direction);
                        break;
                    default:
                        break;
                }
                StartCoroutine(Helper.Delay(() => { flash.SetActive(false); }, 0.2f));
            }
            else
            {
                flash.transform.localPosition = flash_Position[currentSide];
                flash.transform.localRotation = Quaternion.Euler(0, 0, flash_Rotation[currentSide]);
                flash.GetComponent<Renderer>().sortingOrder = flash_Order[currentSide];
                flash.SetActive(true);
                flash.GetComponent<PunchController>().isUsingGun = false;
                flash.GetComponent<PunchController>().damage = GameState.charactersData[playerType].Attack;
                StartCoroutine(Helper.Delay(() => { flash.SetActive(false); }, 0.2f));
            }
            nextAttackTime = Time.time + attackRate;
        }
    }

    void ThrowBomb()
    {
        bomb.GetComponent<BombController>().IsThrowing = false;
        holdingBomb = false;
        bomb_explosion.SetActive(true);
        StartCoroutine(Helper.Delay(() =>
        {
            bomb_explosion.SetActive(false);
            EventManager.Instance.TriggerRemoveInventory(Int32.Parse(playerPosition));
        }, 1.5f));
    }

    void SetBombExplosionPosition()
    {
        var positionChange = new Vector3(bomb_explosion_Position[currentSide].x * direction * (-1), bomb_explosion_Position[currentSide].y, 0);
        var endPosition = transform.position + positionChange;
        bomb.GetComponent<BombController>().startPoint = bomb.transform.position;
        bomb.GetComponent<BombController>().targetPoint = endPosition;
        bomb.GetComponent<BombController>().controlPoint = bomb.transform.position + new Vector3(0, 2f, 0);
        StartCoroutine(Helper.Delay(() => { bomb.GetComponent<BombController>().IsThrowing = true; }, 0.4f));
        bomb_explosion.transform.position = endPosition;
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
    void UpdateTakenItem(string itemName, int pNum)
    {
        if (pNum.ToString().Equals(playerPosition))
        {
            if (itemName == "Rifle")
            {
                holdingGun = true;
                holdingBomb = false;
                anim.SetInteger("Status", 2);
            }
            if (itemName == "Bomb")
            {
                holdingGun = false;
                holdingBomb = true;
                anim.SetInteger("Status", 1);
                bomb.SetActive(holdingBomb);
            }
        }
    }

    public void SetFreeze()
    {
        Debug.Log(this.name + "Freeze");
        isFreeze = true;
        anim.SetBool("isUp", false);
        anim.SetBool("isSide", false);
        currentSide = PlayerSide.Down;
    }

    public bool getFreeze()
    {
        return isFreeze;
    }

    public void CancelFreeze()
    {
        Debug.Log(this.name + "cancel Freeze");
        isFreeze = false;
    }

    void OpenHealthUI()
    {
        var healthBarCanvas = transform.Find("HealthBarCanvas").gameObject;
        healthBarCanvas.SetActive(true);
        healthBarCanvas.GetComponent<BarController>().SetMaxAndInitValue(GameState.charactersData[playerType].Health);
    }

    public void LoadData(GameState data)
    {
        GetComponent<Animator>().runtimeAnimatorController = GameState.animatorController[playerType];
    }

    public void SaveData()
    {
        throw new System.NotImplementedException();
    }
}
public enum PlayerSide
{
    Up,
    Side,
    Down
}