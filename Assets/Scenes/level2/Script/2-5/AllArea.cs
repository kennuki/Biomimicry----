using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllArea : MonoBehaviour
{
    public static AllArea Instance;
    public Transform player_point;
    public Transform friend_point;
    public Transform box1_point;
    public Transform box2_point;
    public List<Transform> ItemArea;
    public enum ItemType
    {
        player,friend,box1,box2
    }
    private void Start()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        Instance = this;
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        FullArea();
    }


    private void FullArea()
    {
        update_AreaFull(player_point);
        update_AreaFull(friend_point);
        update_AreaFull(box1_point);
        update_AreaFull(box1_point);
    }
    public void update_AreaFull(Transform target)
    {
        if (target != null)
        {
            ItemArea.Add(target);
            foreach(Transform Area in ItemArea)
            {
                if (Area.name == target.name)
                    ItemArea.Remove(target);
            }
        }
    }
    public Transform GetPoint(ItemType item)
    {
        switch (item)
        {
            case ItemType.player:
                return player_point;
            case ItemType.friend:
                return friend_point;
            case ItemType.box1:
                return box1_point;
            case ItemType.box2:
                return box2_point;
            default:
                return null;
        }
    }
    public static float Distance2D(Vector3 a,Vector3 b)
    {
        a.y = 0;
        b.y = 0;
        return Vector3.Distance(a, b);
    }
}
