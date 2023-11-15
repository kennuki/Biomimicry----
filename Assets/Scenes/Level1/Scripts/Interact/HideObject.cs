using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObject : MonoBehaviour
{
    public Transform target;
    public GameObject[] HideObjects;
    public float hideDistance = 30f;
    private bool currentActiveState = true;

    void Update()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            bool newActiveState = (distanceToTarget <= hideDistance);
            if (newActiveState != currentActiveState)
            {
                SetObjectActive(newActiveState);
                currentActiveState = newActiveState;
            }
        }
    }

    void SetObjectActive(bool isActive)
    {
        foreach(GameObject gameObject in HideObjects)
        {
            gameObject.SetActive(isActive);
        }
    }
}
