using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierStop : MonoBehaviour
{
    private FloorControll floorControll;
    private Animator anim;
    private Collider cd;
    public FloorControll.FloorColor floorColor;
    public event System.EventHandler<StatuEventArgs> speedchange;
    private void Start()
    {
        floorControll = FloorControll.Instance;
        anim = GetComponent<Animator>();
        cd = GetComponent<Collider>();
    }
    private void Stop()
    {
        speedchange(this, new StatuEventArgs(0,false));
    }
    bool Change = false;
    private void Update()
    {
        if(floorControll.floor == floorColor)
        {
            Change = true;
            anim.enabled = false;
            if (cd != null)
                cd.enabled = false;
            Stop();
        }
        else if(Change)
        {
            anim.enabled = true;
            if (cd != null)
                cd.enabled = true;
            speedchange(this, new StatuEventArgs(0, true));
            Change = false;
        }
    }

}
