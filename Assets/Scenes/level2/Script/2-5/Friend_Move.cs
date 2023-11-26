using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend_Move : MonoBehaviour
{
    private Vector3 target;
    private Friend_NextTarget nextTarget;
    private void Update()
    {
        move();
    }
    private void move()
    {
        if (nextTarget != null)
        {
            target = nextTarget.target.position;
            transform.position = new Vector3(target.x, transform.position.y, target.z);
        }
    }
}
