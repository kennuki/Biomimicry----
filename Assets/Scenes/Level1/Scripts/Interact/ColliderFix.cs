using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderFix : MonoBehaviour
{
    private Vector3 Origin_Pos;
    public float raycastDistance = 0.1f;
    public float reverse_rate = 0.1f;
    private Vector3 previousPosition;
    private Vector3 currentPosition;
    private bool isColliding = false;

    private void Start()
    {
        Origin_Pos = transform.position;
        previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (transform.position.y < Origin_Pos.y - 100)
        {
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = Origin_Pos;
        }
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
                    transform.position = previousPosition - (currentPosition - previousPosition).normalized * reverse_rate;
                }
                isColliding = false;
            }
        }
        else
        {
            if (isColliding&&transform.parent==null)
            {
                transform.position = previousPosition - (currentPosition - previousPosition).normalized * reverse_rate;
            }
            isColliding = false;
        }

        previousPosition = transform.position;
    }
}
