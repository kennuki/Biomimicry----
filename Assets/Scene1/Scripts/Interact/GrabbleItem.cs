using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbleItem : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (GrabItem.ThrowItem == false && other.gameObject.layer != 7)
        {
            transform.SetParent(null);
        }
    }
}
