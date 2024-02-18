using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookUp : MonoBehaviour
{
    private Vector3 lastTopPosition;
    void Update()
    {
        Vector3 currentTopPosition = transform.position + Vector3.up * 0.04f;
        if (lastTopPosition != Vector3.zero)
        {
            Vector3 direction = (lastTopPosition-transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,0.15f);
        }
        lastTopPosition = currentTopPosition;
    }
}
