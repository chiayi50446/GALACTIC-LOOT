using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] panel;
    [SerializeField]
    private GameObject nextStep;
    private int currentPanelIndex = 0;
    void Start()
    {
        currentPanelIndex = 0;
        panel[currentPanelIndex].SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentPanelIndex < 2)
            {
                panel[currentPanelIndex].SetActive(false);
                currentPanelIndex++;
                panel[currentPanelIndex].SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentPanelIndex > 0)
            {
                panel[currentPanelIndex].SetActive(false);
                currentPanelIndex--;
                panel[currentPanelIndex].SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Return) && currentPanelIndex == 2)
        {
            nextStep.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
