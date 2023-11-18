using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    private Animator anim;
    public Transform PointA, PointB;
    private Vector3 PosA, PosB;
    public float speed = 2f;
    public float speedRate = 1;
    private bool Change = true;
    private Vector3 patrolDirection;
    private float TurnTime = 0.5f;
    float OriginRotate_Y;
    private Vector3 TargetPos()
    {
        if (Change)
            return PosA;
        return PosB;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("Walk", 1);
        OriginRotate_Y = transform.eulerAngles.y;
        PosA = PointA.position;
        PosB = PointB.position;
    }
    void Update()
    {
        TargetPos();
        Move();
    }

    int state = 0;
    void Move()
    {
        patrolDirection = (transform.position - TargetPos()).normalized;
        //patrolDirection = transform.rotation * patrolDirection;
        transform.Translate(patrolDirection * -speed * speedRate * 0.05f, Space.World);
        float Distance = Vector3.Distance(transform.position, TargetPos());
        if (Distance < 1)
        {
            if(state == 0)
            {
                OriginRotate_Y += 180;
                anim.SetInteger("Walk", 0);
                StartCoroutine(Turn());
                state = 1;
            }


            if(state == 2)
            {
                anim.SetInteger("Walk", 1);
                state = 0;
                Change = !Change;
            }

        }

    }
    private IEnumerator Turn()
    {
        for (float i = 0; i < TurnTime; i += Time.deltaTime)
        {
            transform.Rotate(0, 180 * (Time.deltaTime/TurnTime), 0);
            yield return null;
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,OriginRotate_Y, transform.eulerAngles.z);
        state = 2;
    }
    

}
