using System.Collections.Generic;
using UnityEngine;

public class EnemyVisionController : MonoBehaviour
{
    [SerializeField] private float viewDistance = 4f;
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private int segments = 10;
    [SerializeField] private string direction = "down";
    private Mesh mesh;
    private Dictionary<string, float> directionRotate = new Dictionary<string, float>
    {
        {"up", 270},
        {"left", 180},
        {"down", 90},
        {"right", 0},
    };

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        // UpdateMesh();
    }

    void FixedUpdate()
    {
        UpdateMesh();
    }

    public void SetDirection(string newDirection)
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
            if ((direction == "right")) endPoint.x *= -1;
            vertices[i + 1] = new Vector3(endPoint.x, endPoint.y, 0.01f); //確保渲染時z軸固定 避免顯示層級錯誤

            // Check if hit players or not
            RaycastHit2D hitPlayer = Physics2D.Raycast(worldOrigin, directionPos, viewDistance, LayerMask.GetMask("Player"));
            if (hitPlayer.collider != null)
            {
                Debug.Log("Hit: " + hitPlayer.collider.name);
            }
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
}
