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
            NextPage();
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PrevPage();
        }
        if (Input.GetKeyDown(KeyCode.Return) && currentPanelIndex == 2)
        {
            OK();
        }
    }

    public void NextPage()
    {
        if (currentPanelIndex < 2)
        {
            panel[currentPanelIndex].SetActive(false);
            currentPanelIndex++;
            panel[currentPanelIndex].SetActive(true);
        }
    }
    public void PrevPage()
    {
        if (currentPanelIndex > 0)
        {
            panel[currentPanelIndex].SetActive(false);
            currentPanelIndex--;
            panel[currentPanelIndex].SetActive(true);
        }
    }
    public void OK()
    {
        panel[currentPanelIndex].SetActive(false);
        currentPanelIndex = 0;
        panel[currentPanelIndex].SetActive(true);
        nextStep.SetActive(true);
        gameObject.SetActive(false);
    }
}
