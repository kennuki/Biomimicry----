using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePoint_Player : MonoBehaviour
{
    public Transform nextPoint;
    private Transform nowPoint;
    private Transform player;
    private void Start()
    {
        player = GameObject.Find("Character").transform;
    }
    private void Update()
    {
        if (AllArea.Instance.player_point != null)
        {
            nowPoint = AllArea.Instance.player_point;
            CalculateNextPoint();
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
}
