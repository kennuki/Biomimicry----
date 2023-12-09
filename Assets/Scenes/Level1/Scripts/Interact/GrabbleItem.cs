using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbleItem : MonoBehaviour
{
    public Vector3 Offset;
    public Vector3 Angle;
    private void OnCollisionEnter(Collision other)
    {
        if (GrabItem.ThrowItem == false && other.gameObject.layer != 7)
        {
            transform.SetParent(null);
        }
    }
    public bool put = false;
}
