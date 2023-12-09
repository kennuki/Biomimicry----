using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePoint_Player : MonoBehaviour
{
    public Transform nextPoint;
    public float dis_has_moved;
    private Transform nowPoint;
    private Transform player;
    private void Start()
    {
        player = GameObject.Find("Character").transform;
    }
    private void Update()
    {
        if(player == null)
        {
            player = GameObject.Find("Character").transform;
        }
        if (AllArea.Instance.player_point != null)
        {
            nowPoint = AllArea.Instance.player_point;
            CalculateNextPoint();
            CalculateDis();
        }


    }
    private void CalculateNextPoint()
    {
        List<Transform> Nearpoint = nowPoint.GetComponent<NearArea>().nearPoint;
        float min_dis=100;
        float dis; 
        foreach(Transform near in Nearpoint) 
        {
            dis = Vector3.Distance(player.position, near.position);
            if (dis < min_dis)
            {
                nextPoint = near;
                min_dis = dis;
            }
        }
    }
    private void CalculateDis()
    {
        dis_has_moved = Distance2D(nextPoint.position,nowPoint.position)-Distance2D(nextPoint.position, player.position);
        //Debug.Log(dis_has_moved);
    }
    public float Distance2D(Vector3 a, Vector3 b)
    {
        a.y = 0;
        b.y = 0;
        return Vector3.Distance(a, b);
    }
}
