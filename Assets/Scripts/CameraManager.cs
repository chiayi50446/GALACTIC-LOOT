using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Image fadePanel;
    private float fadeDuration = 1.5f;
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    private PlayerController player1Controller;
    private PlayerController player2Controller;
    [SerializeField] private GameObject alertnessLevelPanel;

    private Vector3[][] nextCameraPosition = {
        new Vector3[]{ //Level1
            new Vector3(100f, 0f, -10f),
            new Vector3(0f, 0f, -10f),
            new Vector3(9.5f, 57.5f, -10f),
            new Vector3(56.5f, 53.5f, -10f)
            },
        new Vector3[]{ //Level2
            new Vector3(0f, 20f, -10f),
            new Vector3(30f, 0f, -10f),
            new Vector3(30f, 20f, -10f),
            new Vector3(0f, 40f, -10f)
            }
    };

    public Vector3[][] nextPlayer1Position = {
        new Vector3[]{ //Level1
            new Vector3(98f,-2f,0),
            new Vector3(8.5f,-5f,0),
            new Vector3(19f,52f,0),
            new Vector3(56f,48f,0),
            },
        new Vector3[]{ //Level2
            new Vector3(9f, 16f, 0),
            new Vector3(18.5f, -4f, 0),
            new Vector3(24f, 15f, 0),
            new Vector3(-1f, 35f, 0)
            }
    };

    public Vector3[][] nextPlayer2Position = {
        new Vector3[]{ //Level1
            new Vector3(102f,-2f,0),
            new Vector3(10.5f,-5f,0),
            new Vector3(21f,52f,0),
            new Vector3(58f,48f,0),
            },
        new Vector3[]{ //Level2
            new Vector3(11f, 16f, 0),
            new Vector3(20.5f, -4f, 0),
            new Vector3(36f, 15f, 0),
            new Vector3(1f, 35f, 0)
            }
    };

    public Vector3[][] exitPoint = {
        new Vector3[]{ //Level1
            new Vector3(50f, 3.5f, 0),
            new Vector3(100f, 3.5f, 0),
            new Vector3(-7f, 3.5f, 0),
            new Vector3(14.5f, 61.5f, 0)
            },
        new Vector3[]{ //Level2
            new Vector3(0.4f, 6f, 0),
            new Vector3(2.9f, 26.4f, 0),
            new Vector3(35.4f, 6.3f, 0),
            new Vector3(29.4f, 26.4f, 0)
            }
    };
    private int nowMapIndex;
    private int nowRoomIndex;
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
        play1MoveY = Input.GetAxisRaw("Vertical1");
        play2MoveY = Input.GetAxisRaw("Vertical2");
        if (player1Enter)
        {
            if (play1MoveY < 0 && player1Controller.getFreeze() && !player2Enter)
            {
                player1.transform.position = exitPoint[nowMapIndex - 1][nowRoomIndex - 1];
                player1Controller.CancelFreeze();
                player1Enter = false;
                AudioManager.Instance.playEnterDoorSound();
            }
        }

        if (player2Enter)
        {
            if (play2MoveY < 0 && player2Controller.getFreeze() && !player1Enter)
            {
                player2.transform.position = exitPoint[nowMapIndex - 1][nowRoomIndex - 1];
                player2Controller.CancelFreeze();
                player2Enter = false;
                AudioManager.Instance.playEnterDoorSound();
            }
        }
    }

    public void enterDoor(int mapIndex, int roomIndex, Collider2D other)
    {
        nowMapIndex = mapIndex;
        nowRoomIndex = roomIndex;
        AudioManager.Instance.playEnterDoorSound();
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
            StartCoroutine(moveCamera(mapIndex, roomIndex));
        }
    }

    private IEnumerator moveCamera(int mapIndex, int roomIndex)
    {
        PlayBossRoomBGM(mapIndex, roomIndex);
        AudioManager.Instance.playMoveCameraSound();

        yield return StartCoroutine(Fade(1));
        mainCamera.transform.position = nextCameraPosition[mapIndex - 1][roomIndex - 1];
        yield return StartCoroutine(Fade(0));

        if (mapIndex == 1 && roomIndex == 2)
        {
            EventManager.Instance.TriggerDisplayGuide(alertnessLevelPanel);
        }
        player1Enter = false;
        player2Enter = false;
        player1Controller.CancelFreeze();
        player2Controller.CancelFreeze();

        if (roomIndex == nextCameraPosition[mapIndex - 1].Count())
        {
            EventManager.Instance.TriggerActiveBoss();
            EventManager.Instance.TriggerActiveBossHealthUI();
            EventManager.Instance.TriggerActivePlayerHealthUI();
        }
    }

    private void PlayBossRoomBGM(int mapIndex, int roomIndex)
    {
        if (roomIndex == nextCameraPosition[mapIndex - 1].Count())
        {
            if (mapIndex == 1)
            {
                AudioManager.Instance.PlayLevel1BossMusic();
            }
            else
            {
                AudioManager.Instance.PlayLevel2BossMusic();
            }
        }
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
