using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonCreator : MonoBehaviour
{
    public Button sampleButton;
    public string[] option; 
    public DialogAsset[] dialogAssets;
    public int padding = 180;
    public Dialog dialog;
    public DialogManager manager;
    void Start()
    {
    }
    public void CreateButtonList()
    {
        RemoveObjectsWithSpecificName();
        buttons = new Button[option.Length];
        for(int i = 0; i < option.Length; i++)
        {

            if (i == option.Length - 1)
            {
                Button newButton = CreateButton(option[i], i);
                newButton.gameObject.SetActive(true);
                newButton.onClick.AddListener(dialog.Exit);
                newButton.onClick.AddListener(RemoveObjectsWithSpecificName);
                buttons[i] = newButton;
            }
            else
            {
                Button newButton = CreateButton(option[i], i);
                DialogClickFun newFun = CreateFun(dialogAssets[i], newButton.gameObject);
                newButton.gameObject.SetActive(true);
                newButton.onClick.AddListener(newFun.OnClick);
                newButton.onClick.AddListener(RemoveObjectsWithSpecificName);
                buttons[i] = newButton;
            }
        }
        setButton();
    }
    Button CreateButton(string buttonText, int index)
    {
        Button newButton = Instantiate(sampleButton, this.transform);

        RectTransform rectTransform = newButton.GetComponent<RectTransform>();
        RectTransform rectTransform_sample = sampleButton.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = rectTransform_sample.anchoredPosition - new Vector2(0, index*padding);
        TextMeshProUGUI textMesh = newButton.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = buttonText;
        float text_width = textMesh.preferredWidth;
        Vector2 target_size = new Vector2(rectTransform_sample.sizeDelta.x + text_width, rectTransform_sample.sizeDelta.y);
        rectTransform.sizeDelta = target_size;

        return newButton;
    }
    DialogClickFun CreateFun(DialogAsset dialogInfo,GameObject target)
    {
        DialogClickFun newClickFun = target.AddComponent<DialogClickFun>();
        newClickFun.dialogInfo = dialogInfo;
        newClickFun.dialog = dialog;
        newClickFun.manager = manager; 
        return newClickFun;
    }
    void RemoveObjectsWithSpecificName()
    {
        Debug.Log("remove");
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);

            if (child.name == "Button_sample(Clone)")
            {
                Destroy(child.gameObject);
            }
        }
        buttons = new Button[0];
    }


    public Button[] buttons; // 將所有按鈕指派到這個陣列
    private int currentIndex = 0;
    void Update()
    {
        if (buttons.Length > 0)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                SelectButton(currentIndex - 1);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                SelectButton(currentIndex + 1);
            }
        }

    }
    void setButton()
    {
        if (buttons.Length > 0)
        {
            EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);
        }
    }
    void SelectButton(int newIndex)
    {
        newIndex = Mathf.Clamp(newIndex, 0, buttons.Length - 1);

        buttons[currentIndex].OnDeselect(null);

        currentIndex = newIndex;

        EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);
    }
}
