using UnityEngine;
public class PushableObjectController : MonoBehaviour
{
    [SerializeField] private LayerMask stopLayer;
    [SerializeField] private GameObject goalObject;
    [SerializeField] private GameObject goalTreasure;
    private float slideSpeed;
    private Vector3 moveDirection;
    private bool isSliding = false;
    private bool isTrigger1 = false;
    private bool isTrigger2 = false;
    private Rigidbody2D rb;
    private bool isGoal;

    void Start()
    {
        slideSpeed = 5f;
        stopLayer = LayerMask.GetMask("PushWall");
        rb = GetComponent<Rigidbody2D>();
        isGoal = false;
    }
    private void Update()
    {
        if (isGoal) return;

        if (isTrigger1)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameObject player = GameObject.Find("Player1");
                if (player != null)
                {
                    Vector3 direction = GetPushDirection(player);
                    if (direction != Vector3.zero)
                    {
                        Push(direction);
                    }
                }
            }
        }

        if (isTrigger2)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                GameObject player = GameObject.Find("Player2");
                if (player != null)
                {
                    Vector3 direction = GetPushDirection(player);
                    if (direction != Vector3.zero)
                    {
                        Push(direction);
                    }
                }
            }
        }

        if (isSliding)
        {
            rb.linearVelocity = moveDirection * slideSpeed;
        }
    }

    private Vector3 GetPushDirection(GameObject player)
    {
        PlayerSide nowPlayerSide = player.GetComponent<PlayerController>().GetPlayerSide();
        Vector3 diff = transform.position - player.transform.position;

        if (nowPlayerSide == PlayerSide.Side)
        {
            return diff.x > 0 ? Vector3.right : Vector3.left;
        }
        else
        {
            return (nowPlayerSide == PlayerSide.Up) ? Vector3.up : Vector3.down;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == goalObject)
        {
            isGoal = true;
            if (goalTreasure != null && !goalTreasure.activeSelf)
            {
                goalTreasure.SetActive(true);
            }
        }

        if (other.tag == "Player")
        {
            if (other.gameObject.name == "Player1")
                isTrigger1 = true;
            else
                isTrigger2 = true;

        }

        if (((1 << other.gameObject.layer) & stopLayer) != 0)
        {
            isSliding = false;
            rb.linearVelocity = Vector2.zero;
            AudioManager.Instance.playStopSound();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (other.gameObject.name == "Player1")
                isTrigger1 = false;
            else
                isTrigger2 = false;
        }
    }

    public void Push(Vector3 direction)
    {
        if (!isSliding)
        {
            moveDirection = direction.normalized;
            isSliding = true;
            AudioManager.Instance.playPushSound();
        }
    }
}

