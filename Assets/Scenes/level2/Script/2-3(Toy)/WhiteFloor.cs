using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteFloor : MonoBehaviour
{
    public int State = 0;
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.name);
        if (other.name == "Run")
        {
            if (State == 0)
                FloorControll.WorldSpeed = 1.5f;
            if (State == 1)
                FloorControll.WorldSpeed = 0;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Run")
        {
            if (State == 0)
                FloorControll.WorldSpeed = 1;
            if (State == 1)
                FloorControll.WorldSpeed = 1;
        }
    }
}
