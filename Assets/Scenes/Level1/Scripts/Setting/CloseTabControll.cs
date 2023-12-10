using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CloseTabControll : MonoBehaviour
{
    public GameObject panel;
    public CameraRotate CameraRotate;
    private bool isPanelOpen = false;
    public Button closeButton;
    Canvas canvas;
    private void Start()
    {
        canvas = this.GetComponent<Canvas>();
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
        if (panel.activeSelf)
        {
            canvas.sortingOrder = 10;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && panel != null)
        {
            isPanelOpen = !isPanelOpen;
            panel.SetActive(isPanelOpen);
            if (isPanelOpen)
            {
                Time.timeScale = 0.01f;
                CameraRotate.cameratotate = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else 
            {
                canvas.sortingOrder = -5;
                Time.timeScale = 1;
                CameraRotate.cameratotate = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public void ClosePanel()
    {
        if (panel != null)
        {
            Time.timeScale = 1;
            CameraRotate.cameratotate = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isPanelOpen = false;
            panel.SetActive(false);
            canvas.sortingOrder = -5;
        }
    }
}
