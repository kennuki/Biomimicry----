using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend_Move : MonoBehaviour
{
    private Vector3 target;
    public Friend_NextTarget nextTarget;
    public FacePoint_Player facePoint;
    private Transform nowPoint,nextPoint;
    private float player_move_dis;
    private void Update()
    {
        nowPoint = AllArea.Instance.friend_point;
        if (nextTarget.target != null)
            nextPoint = nextTarget.target;
        player_move_dis = facePoint.dis_has_moved;
        move();
    }
    private void move()
    {

        if (nextTarget.target != null&&nowPoint!=null)
        {
            Vector3 dir = (nextPoint.position - nowPoint.position).normalized;
            Vector3 target_pos = nowPoint.position + dir * player_move_dis * 1.1f;
            transform.position = Vector3.Lerp(transform.position, target_pos, 0.2f);
        }
    }
}
