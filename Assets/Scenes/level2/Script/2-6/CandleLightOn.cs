using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleLightOn : MonoBehaviour
{
    public EventActive eventActive;
    public GameObject Candles;
    private void Update()
    {
        if (eventActive.Active)
        {
            Candles.SetActive(true);
            this.enabled = false;
        }
    }
}
