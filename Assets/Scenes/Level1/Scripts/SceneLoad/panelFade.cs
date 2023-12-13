using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panelFade : MonoBehaviour
{
    public GameObject panel;
    public Image[] image;
    private bool targetActive = false;
    private void Update()
    {
        if (panel.activeSelf == true && targetActive == false)
        {
            foreach (Image image in image)
            {
                image.color = new Color(1, 1, 1, 0);
            }
            StartCoroutine(Fadein());
        }
        else if (panel.activeSelf == false&&targetActive == true)
        {
            StopAllCoroutines();
            foreach(Image image in image)
            {
                image.color = new Color(1, 1, 1, 0);
            }
            targetActive = false;
        }
    }
    private IEnumerator Fadein()
    {
        yield return null;
        targetActive = true;
        for(float i = 0; i <= 2; i+=Time.deltaTime)
        {
            foreach (Image image in image)
            {
                image.color = new Color(1, 1, 1, i/2);
            }
            yield return null;
        }

    }
}
