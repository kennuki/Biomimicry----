using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightTargetInRoom : MonoBehaviour
{
    public bool inRoom = false;
    public GameObject target;
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == target.name)
        {
            inRoom = true;
        }
    }
}
