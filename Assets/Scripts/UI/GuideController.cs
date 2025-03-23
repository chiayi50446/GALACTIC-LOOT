using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GuideController : MonoBehaviour
{
    [SerializeField] private GameObject alertnessLevelPanel;

    private bool isDisplayGuide;
    private GameObject nowDisplay;
    private GameObject closeBtn;
    private List<string> displayedList;

    void OnEnable()
    {
        EventManager.Instance.DisplayGuide += openGuide;
    }

    void OnDisable()
    {
        EventManager.Instance.DisplayGuide -= openGuide;
    }

    void Start()
    {
        displayedList = new List<string>();
        // if (alertnessLevelPanel != null)
        // {
        //     openGuide(alertnessLevelPanel);
        // }
    }

    void Update()
    {
        if (isDisplayGuide && closeBtn.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                closeGuide();
            }
        }
    }


    private void openGuide(GameObject displayGuide)
    {
        if (displayedList.Contains(displayGuide.name))
        {
            return;
        }
        displayedList.Add(displayGuide.name);
        nowDisplay = displayGuide;
        isDisplayGuide = true;
        Time.timeScale = 0;

        nowDisplay.SetActive(true);
        closeBtn = nowDisplay.transform.Find("CloseHint").gameObject;
        // closeBtn.SetActive(false);
        // StartCoroutine(Helper.Delay_RealTime(() => { closeBtn.SetActive(true); }, 1f));
    }

    private void closeGuide()
    {
        Time.timeScale = 1;
        isDisplayGuide = false;
        nowDisplay.SetActive(false);
    }
}
