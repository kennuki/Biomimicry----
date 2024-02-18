using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPoint : MonoBehaviour
{
    public AllArea.ItemType itemType;
    private Transform NowPoint;
    private List<Transform> NearPoint;
    private Transform item;
    private void Start()
    {
        item = this.transform;
        if (itemType == AllArea.ItemType.player)
            item = GameObject.Find("Character").transform;
        Get_Min_Point();
    }
    private void Update()
    {
        if (itemType == AllArea.ItemType.player)
        {
            if (item == null)
            {
                item = GameObject.Find("Character").transform;
                Get_Min_Point();
            }

        }
        GetNearPoint();
        Calculate_NowPoint();
        Output_NowPoint();
    }
    private void Get_Min_Point()
    {
        float min_dis=1000;
        float dis;
        foreach (Transform point in PointInfo.Instance.AllPoint)
        {
            dis = Vector3.Distance(point.position, item.position);
            if (dis < min_dis)
            {
                NowPoint = point;
                min_dis = dis;
            }
        }
    }
    private void Output_NowPoint()
    {
        switch (itemType)
        {
            case AllArea.ItemType.player:
                AllArea.Instance.player_point = NowPoint;
                break;
            case AllArea.ItemType.friend:
                AllArea.Instance.friend_point = NowPoint;
                break;
            case AllArea.ItemType.box1:
                AllArea.Instance.box1_point = NowPoint;
                break;
            default:
                break;
        }
    }
    private void GetNearPoint()
    {
        if (NowPoint!=null)
            NearPoint = NowPoint.GetComponent<NearArea>().nearPoint;
    }
    private void Calculate_NowPoint()
    {
        float dis;
        foreach(Transform near in NearPoint)    
        {
            dis = Distance2D(item.position, near.position);
            if (dis < Distance2D(near.position, NowPoint.position) / 2f)
            {
                if (itemType != AllArea.ItemType.friend)
                    NowPoint = near;
                if (itemType == AllArea.ItemType.player)
                    AllArea.Instance.player_pos_change = true;
            }
            
        }
        if (itemType == AllArea.ItemType.friend && AllArea.Instance.player_pos_change)
        {
            NowPoint = AllArea.Instance.friend_next_point;
            AllArea.Instance.player_pos_change = false;
        }
       
    }
    public float Distance2D(Vector3 a, Vector3 b)
    {
        a.y = 0;
        b.y = 0;
        return Vector3.Distance(a, b);
    }
}
