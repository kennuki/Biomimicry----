using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointSerial : MonoBehaviour
{
   

    private int PrevioudSavePointIndex = 0;
    public static int CurrentSavePointIndex = 0;

    private void Start()
    {
        
       
    }
    private void Update()
    {
        Debug.Log(CurrentSavePointIndex);
        if(CurrentSavePointIndex> PrevioudSavePointIndex)
        {
            PrevioudSavePointIndex = CurrentSavePointIndex;
        }
    }

    
}
