using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleLightOn : MonoBehaviour
{
    public EventActive eventActive;
    public GameObject Candles;
    public ItemMission mission;
    public AudioSource source;
    private void Update()
    {
        if (eventActive.Active)
        {
            source.Play();
            mission.item_mission = true;
            Candles.SetActive(true);
            this.enabled = false;
        }
    }
}
