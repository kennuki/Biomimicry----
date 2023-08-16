using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointTrigger : MonoBehaviour
{
    private Collider Cd;
    public int SavePoint_Index;
    private void Start()
    {
        Cd = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Character")
        {
            if(SavePoint_Index> SavePointSerial.CurrentSavePointIndex)
            {
                SavePointSerial.CurrentSavePointIndex = SavePoint_Index;
            }
            Cd.enabled = false;
        }
    }
}
