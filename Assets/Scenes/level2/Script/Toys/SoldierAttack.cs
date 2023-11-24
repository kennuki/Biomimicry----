using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoldierAttack : MonoBehaviour
{
    private Animator anim;
    private GameObject player;
    private Collider cd;
    private void Start()
    {
        player = GameObject.Find("Character");
        anim = GetComponent<Animator>();
        cd = GetComponent<Collider>();
    }
    private void Attack()
    {
        anim.SetInteger("Attack", 1);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Character")
        {
            Attack();
            cd.enabled = false;
        }
    }
}
