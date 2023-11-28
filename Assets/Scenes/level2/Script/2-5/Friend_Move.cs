using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend_Move : MonoBehaviour
{
    public Animator anim;
    public Friend_NextTarget nextTarget;
    public FacePoint_Player facePoint;
    private Transform nowPoint,nextPoint;
    private float player_move_dis;
    private float counter = 0;
    private bool move_to_center = false;
    private Vector3 posValue;
    public Vector3 PosValue
    {
        get { return posValue; }
        set
        {
            if (posValue != value)
            {
                posValue = value;
                move_to_center = true;
            }
        }
    }
    private void Start()
    {
        anim.SetInteger("Walk", 1);
    }
    private void Update()
    {
        nowPoint = AllArea.Instance.friend_point;;
        if (nextTarget.target != null)
            nextPoint = nextTarget.target;
        player_move_dis = facePoint.dis_has_moved;
        PosValue = nowPoint.position;
        if (!move_to_center)
        {
            move();
        }
        else
        {
            MoveToCenter();
        }
    }
    private void move()
    {

        if (nextTarget.target != null&&nowPoint!=null)
        {
            Vector3 dir = (nextPoint.position - nowPoint.position).normalized;
            Vector3 target_pos = nowPoint.position + dir * player_move_dis * 1f;
            transform.position = Vector3.Lerp(transform.position, target_pos, 0.05f);
        }
    }
    private void MoveToCenter()
    {
        counter += Time.deltaTime;
        if (nextTarget.target != null && nowPoint != null)
        {
            transform.position = Vector3.Lerp(transform.position, nowPoint.position, 0.02f);
        }
        if (counter > 0.3f)
        {
            move_to_center = false;
            counter = 0;
        }
    }
    
}
