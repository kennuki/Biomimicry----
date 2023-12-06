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
        if(CurrentSavePointIndex> PrevioudSavePointIndex)
        {
            PrevioudSavePointIndex = CurrentSavePointIndex;
        }
        Debug.Log(CurrentSavePointIndex);
    }

    
}
