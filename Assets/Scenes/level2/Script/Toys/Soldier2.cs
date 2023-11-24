using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier2 : MonoBehaviour
{
    [SerializeField]
    SoldierAttack soldierAttack;
    private Animator anim;
    public Transform target;
    public Transform Soldier;
    public float speed = 1;
    public float orbitSpeed = 5f;
    private Vector3 PosCache;
    private void Start()
    {
        soldierAttack.speedchange += onSpeedChange;
        anim = GetComponent<Animator>();
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
            Soldier.transform.LookAt(Soldier.transform.position - moveDirection);
            PosCache = Soldier.transform.position;
        }

    }
    public void onSpeedChange(object sender, StatuEventArgs e)
    {
        speed = e.Speed;
    }
}
