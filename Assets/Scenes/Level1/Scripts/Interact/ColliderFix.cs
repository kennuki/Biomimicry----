using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderFix : MonoBehaviour
{
    public float raycastDistance = 0.1f; 

    private Vector3 previousPosition;
    private Vector3 currentPosition;
    private bool isColliding = false;

    private void Start()
    {
        previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        currentPosition = transform.position;

        RaycastHit hit;
        if (Physics.Raycast(previousPosition, (currentPosition - previousPosition).normalized, out hit, raycastDistance))
        {
            if(hit.transform.gameObject.name != this.gameObject.name)
            {
                if(hit.transform.gameObject.tag == "Obstacle")
                {
                    isColliding = true;
                }
                else if (hit.transform.gameObject.tag == "Glass")
                {
                    isColliding = true;
                }
            }
            else
            {
                if (isColliding && transform.parent == null)
                {
                    transform.position = previousPosition - (currentPosition - previousPosition).normalized * 0.1f;
                }
                isColliding = false;
            }
        }
        else
        {
            if (isColliding&&transform.parent==null)
            {
                transform.position = previousPosition - (currentPosition - previousPosition).normalized * 0.1f;
            }
            isColliding = false;
        }

        previousPosition = transform.position;
    }
}
