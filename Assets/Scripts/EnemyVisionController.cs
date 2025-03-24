using System.Collections.Generic;
using UnityEngine;

public class EnemyVisionController : MonoBehaviour
{
    [SerializeField] private float viewDistance = 4f;
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private int segments = 10;
    [SerializeField] private Direction direction = Direction.down;
    [SerializeField] private EnemyType type = EnemyType.stationary;
    private Mesh mesh;
    private bool isHittingPlayer1;
    private bool isHittingPlayer2;
    private Dictionary<Direction, float> directionRotate = new Dictionary<Direction, float>
    {
        {Direction.up, 270},
        {Direction.left, 180},
        {Direction.down, 90},
        {Direction.right, 0},
    };

    private Dictionary<float, float> viewDistanceData = new Dictionary<float, float>
    {
        {0f, 4f},
        {1f, 4f},
        {2f, 5f},
        {3f, 5f},
    };

    void Awake()
    {
        EventManager.Instance.UpdateVision += UpdateViewDistance;
    }

    void OnDisable()
    {
        EventManager.Instance.UpdateVision -= UpdateViewDistance;
    }
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        isHittingPlayer1 = false;
        isHittingPlayer2 = false;
        UpdateViewDistance();
        // UpdateMesh();
    }

    void FixedUpdate()
    {
        UpdateMesh();
    }

    public void SetDirection(Direction newDirection)
    {
        direction = newDirection;
    }

    private void UpdateMesh()
    {
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        Vector3 localOrigin = Vector3.zero;
        Vector3 worldOrigin = transform.position;
        vertices[0] = localOrigin;
        float rotate = directionRotate[direction];

        bool hitPlayer1 = false;
        bool hitPlayer2 = false;

        // caculate vertices
        for (int i = 0; i <= segments; i++)
        {
            float angle = (-viewAngle / 2 + (viewAngle * i / segments) - rotate) * Mathf.Deg2Rad;
            Vector3 directionPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            Vector3 endPoint = directionPos * viewDistance;

            // Check if vision blocks by wall and object or not
            RaycastHit2D hitWall = Physics2D.Raycast(worldOrigin, directionPos, viewDistance, LayerMask.GetMask("Wall", "Object"));
            if (hitWall.collider != null)
            {
                endPoint = (Vector3)hitWall.point - worldOrigin;
            }
            Debug.DrawRay(worldOrigin, endPoint, Color.yellow);
            if ((direction == Direction.right)) endPoint.x *= -1;
            vertices[i + 1] = new Vector3(endPoint.x, endPoint.y, 0.01f); //確保渲染時z軸固定 避免顯示層級錯誤

            // Check if hit players or not
            RaycastHit2D hitPlayer = Physics2D.Raycast(worldOrigin, directionPos, viewDistance, LayerMask.GetMask("Player", "Wall", "Object"));
            if (hitPlayer.collider != null)
            {
                if (hitPlayer.collider.name == "Player1")
                {
                    hitPlayer1 = true;
                }
                if (hitPlayer.collider.name == "Player2")
                {
                    hitPlayer2 = true;
                    Debug.DrawRay(worldOrigin, endPoint, Color.yellow);
                    Debug.Log(hitWall.point);
                }
            }
        }

        if (hitPlayer1 && !isHittingPlayer1)
        {
            isHittingPlayer1 = true;
            GameState.Instance.SetLastAlertPlayerType(1);
            GameState.Instance.SetLastAlertEnemyType(type);
            UpdateAlertnessLevel();
            // Debug.Log("Hit player1");
        }
        if (!hitPlayer1 && isHittingPlayer1)
        {
            isHittingPlayer1 = false;
            // Debug.Log("Hit leave player1");
        }
        if (hitPlayer2 && !isHittingPlayer2)
        {
            isHittingPlayer2 = true;
            GameState.Instance.SetLastAlertPlayerType(2);
            GameState.Instance.SetLastAlertEnemyType(type);
            UpdateAlertnessLevel();
            // Debug.Log("Hit player2");
        }
        if (!hitPlayer2 && isHittingPlayer2)
        {
            isHittingPlayer2 = false;
            // Debug.Log("Hit leave player2");
        }

        // draw triangles
        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 2;
            triangles[i * 3 + 2] = i + 1;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    private void UpdateAlertnessLevel()
    {
        float newAlertnessLevel = GameState.Instance.GetAlertnessLevel();
        GameState.Instance.SetAlertnessLevel(newAlertnessLevel + 1);
        EventManager.Instance.TriggerUpdateAlertnessLevel();
    }

    private void UpdateViewDistance()
    {
        float newAlertnessLevel = GameState.Instance.GetAlertnessLevel();
        viewDistance = viewDistanceData[newAlertnessLevel];
    }
}
