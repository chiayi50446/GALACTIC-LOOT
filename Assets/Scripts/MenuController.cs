using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject startButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (startButton != null)
        {
            startButton.GetComponent<Button>().Select();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
