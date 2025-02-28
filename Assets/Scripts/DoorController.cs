using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Image fadePanel;
    private float fadeDuration = 1.5f;
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    [SerializeField] private int mapIndex;
    [SerializeField] private int roomIndex;
    private PlayerController player1Controller;
    private PlayerController player2Controller;
    private Vector3[][] nextCameraPosition = {
        new Vector3[]{ //Map1
            new Vector3(9.5f, 57.5f, -10f),
            new Vector3(56.5f, 53.5f, -10f)
            },
        new Vector3[]{ //Map2
            new Vector3(0f, 20f, -10f)
            }
    };
    public Vector3[][] nextPlayer1Position = {
        new Vector3[]{ //Map1
            new Vector3(0,0,0),
            new Vector3(0,0,0),
            },
        new Vector3[]{ //Map2
            new Vector3(10f, 15f, 0)
            }
    };
    public Vector3[][] nextPlayer2Position = {
        new Vector3[]{ //Map1
            new Vector3(0,0,0),
            new Vector3(0,0,0),
            },
        new Vector3[]{ //Map2
            new Vector3(8.5f, 15f, 0)
            }
    };

    public Vector3[][] exitPoint = {
        new Vector3[]{ //Map1
            new Vector3(9.5f, 57.5f, -10f),
            new Vector3(56.5f, 53.5f, -10f)
            },
        new Vector3[]{ //Map2
            new Vector3(0.45f, 6f, 0)
            }
    };
    private bool player1Enter;
    private bool player2Enter;
    private float play1MoveY;
    private float play2MoveY;

    void Start()
    {
        player1Enter = false;
        player2Enter = false;
        player1Controller = player1.GetComponent<PlayerController>();
        player2Controller = player2.GetComponent<PlayerController>();
        fadePanel.color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        play1MoveY = Input.GetAxisRaw("Vertical");
        play2MoveY = Input.GetAxisRaw("Vertical2");
        if (player1Enter)
        {
            if (play1MoveY < 0 && player1Controller.getFreeze())
            {
                player1.transform.position = exitPoint[mapIndex - 1][roomIndex - 1];
                player1Controller.CancelFreeze();
                player1Enter = false;
            }
        }

        if (player2Enter)
        {
            if (play2MoveY < 0 && player2Controller.getFreeze())
            {
                player2.transform.position = exitPoint[mapIndex - 1][roomIndex - 1];
                player2Controller.CancelFreeze();
                player2Enter = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player1")
        {
            player1.transform.position = nextPlayer1Position[mapIndex - 1][roomIndex - 1];
            player1Controller.SetFreeze();
            player1Enter = true;
        }
        if (other.name == "Player2")
        {
            player2.transform.position = nextPlayer2Position[mapIndex - 1][roomIndex - 1];
            player2Controller.SetFreeze();
            player2Enter = true;
        }

        if (player1Enter && player2Enter)
        {
            StartCoroutine(moveCamera());
        }
    }

    private IEnumerator moveCamera()
    {
        yield return StartCoroutine(Fade(1));
        mainCamera.transform.position = nextCameraPosition[mapIndex - 1][roomIndex - 1];
        yield return StartCoroutine(Fade(0));
        player1Controller.CancelFreeze();
        player2Controller.CancelFreeze();
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float elapsedTime = 0;
        Color color = fadePanel.color;
        float startAlpha = color.a;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            fadePanel.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        fadePanel.color = color;
    }
}
