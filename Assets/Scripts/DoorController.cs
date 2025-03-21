using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private int mapIndex;
    [SerializeField] private int roomIndex;
    [SerializeField] private CameraManager cameraManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        cameraManager.enterDoor(mapIndex, roomIndex, other);
    }
}
