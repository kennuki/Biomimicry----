using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CloseTabControll : MonoBehaviour
{
    public GameObject panel; 

    private bool isPanelOpen = false;
    public Button closeButton;
    private void Start()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClosePanel);
        }
        if (panel != null)
        {
            panel.SetActive(isPanelOpen);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && panel != null)
        {
            isPanelOpen = !isPanelOpen;
            panel.SetActive(isPanelOpen);
        }
    }

    public void ClosePanel()
    {
        if (panel != null)
        {
            isPanelOpen = false;
            panel.SetActive(false);
        }
    }
}
