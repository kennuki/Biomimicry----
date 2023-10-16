using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dead : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Boss")
        {
            StartCoroutine(PlayerDead());
        }
    }
    public GameObject DeadPanel;
    public IEnumerator PlayerDead()
    {
        Time.timeScale = 0;
        DeadPanel.SetActive(true);
        yield break;
    }
}
