using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier2 : MonoBehaviour
{
    [SerializeField]
    SoldierAttack soldierAttack;
    [SerializeField]
    SoldierStop soldierStop;
    public Animator anim;
    public Transform target;
    public Transform Soldier;
    public float speed = 1;
    private float OriginSpeed;
    public float orbitSpeed = 5f;
    private Vector3 PosCache;
    private Transform player;
    private void Awake()
    {
        OriginSpeed = speed;
        player = GameObject.Find("Character").transform;
        soldierAttack.speedchange += onSpeedChange;
        soldierStop.speedchange += onSpeedChange;
        anim.SetInteger("Walk", 1); 
        PosCache = Soldier.transform.position;
    }
    void Update()
    {
        if (target != null)
        {
            Soldier.transform.RotateAround(target.position, Vector3.up, speed*orbitSpeed * Time.deltaTime);
        }
        Vector3 moveDirection = PosCache - Soldier.transform.position;
        if (moveDirection != Vector3.zero)
        {
            if(speed == 0)
            {
                Soldier.transform.LookAt(player);
            }
            else
            {
                Soldier.transform.LookAt(Soldier.transform.position - moveDirection);
                PosCache = Soldier.transform.position;
            }

        }

    }
    public void onSpeedChange(object sender, StatuEventArgs e)
    {
        if (e.Recover)
            speed= OriginSpeed;
        else
            speed = e.Speed;
    }
}
