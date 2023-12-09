using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightTargetInRoom : MonoBehaviour
{
    public bool inRoom = false;
    public GameObject target;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == target.name)
        {
            inRoom = true;
            if (other.gameObject.GetComponent<ItemMission>() != null)
            {
                Debug.Log("fef");
                inRoom = other.gameObject.GetComponent<ItemMission>().item_mission;
            }

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == target.name)
            inRoom = false;
    }
}
