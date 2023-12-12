using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier2 : MonoBehaviour
{
    public AudioSource source;
    [SerializeField]
    SoldierAttack soldierAttack;
    [SerializeField]
    SoldierStop soldierStop;
    public Animator anim;
    public Transform target;
    public Transform Soldier;
    public float Anim_Speed = 1;
    public float speed = 1;
    private float OriginSpeed;
    public float orbitSpeed = 5f;
    private Vector3 PosCache;
    private Transform player;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
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
            Soldier.transform.RotateAround(target.position, Vector3.up, speed*orbitSpeed * Time.deltaTime*FloorControll.WorldSpeed);
        }
        Vector3 moveDirection = PosCache - Soldier.transform.position;
        if (moveDirection != Vector3.zero)
        {
            if(speed == 0)
            {
            }
            else
            {
                Soldier.transform.LookAt(Soldier.transform.position - moveDirection);
                PosCache = Soldier.transform.position;
            }

        }
        anim.SetFloat("Anim_Speed", Anim_Speed* FloorControll.WorldSpeed);
    }
    public void walkVoice()
    {
        source.Play();
    }
    public void onSpeedChange(object sender, StatuEventArgs e)
    {
        if (e.Recover)
        {
            speed = OriginSpeed;
            Debug.Log("Recover");
        }

        else
            speed = e.Speed;
    }
}
