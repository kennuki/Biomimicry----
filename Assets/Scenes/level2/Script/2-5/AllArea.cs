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
    private void Awake()
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
        ItemArea.Clear();

        update_AreaFull(player_point);
        update_AreaFull(friend_point);
        update_AreaFull(box1_point);
        update_AreaFull(box2_point);
    }
    public void update_AreaFull(Transform target)
    {
        if (target != null)
        {
            ItemArea.Add(target);
        }
    }
   
}
