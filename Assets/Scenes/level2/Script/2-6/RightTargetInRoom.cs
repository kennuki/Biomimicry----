using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightTargetInRoom : MonoBehaviour
{
    //public bool inRoom = false;
    public GameObject[] target;
    private void OnTriggerStay(Collider other)
    {

    }
    private bool inRoom(Collider other)
    {
        foreach(GameObject gameObject in target)
        {
            if (other.gameObject.name != gameObject.name)
            {
                return false;
            }
        }
        return true;
    }
}
