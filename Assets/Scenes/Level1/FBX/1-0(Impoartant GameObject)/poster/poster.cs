using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class poster : MonoBehaviour
{
    private EventActive eventActive;
    public RectTransform rect;
    private Vector3 ImageMaxSize;
    private int ifOpen = 0;
    void Start()
    {
        eventActive = GetComponent<EventActive>();
        ImageMaxSize = rect.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (eventActive.Active == true && ifOpen == 0)
            StartCoroutine(Open());
        if (Input.GetKeyDown(KeyCode.F) && ifOpen == 1)
        {
            StopAllCoroutines();
            StartCoroutine(Close());
        }
    }

    
    public IEnumerator Open()
    {
        ifOpen = 1;
        rect.gameObject.SetActive(true);
        rect.localScale = new Vector3(0, 0, 1);
        for(float i =0;i<0.5f;i+= Time.deltaTime)
        {
            rect.localScale = new Vector3(ImageMaxSize.x * i * 2, ImageMaxSize.y * i * 2, 1);
            yield return null;
        }
        rect.localScale = ImageMaxSize;
    }
    public IEnumerator Close()
    {
        ifOpen = 2;
        rect.gameObject.SetActive(true);
        for (float i = 0.5f; i > 0; i -= Time.deltaTime)
        {
            rect.localScale = new Vector3(ImageMaxSize.x * i * 2, ImageMaxSize.y * i * 2, 1);
            yield return null;
        }
        rect.gameObject.SetActive(false);
        eventActive.Active = false;
        ifOpen = 0;
    }
}
