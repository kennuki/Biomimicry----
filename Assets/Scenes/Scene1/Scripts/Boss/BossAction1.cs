using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAction1 : MonoBehaviour
{
    RandomTeleport teleport;
    Boss boss;
    AudioSource BossAudio;
    public PanicRed_PP panic;
    public Animator anim_body;
    private void Start()
    {
        boss = this.GetComponent<Boss>();
        teleport = GetComponent<RandomTeleport>();
        BossAudio = GetComponent<AudioSource>();
        StartCoroutine(Orei());
    }
    public IEnumerator Orei()
    {
        yield return new WaitForSeconds(5f);
        anim_body.enabled = true;
        anim_body.SetBool("Dance", true);
        anim_body.SetInteger("Idle", 1);
        yield return new WaitForSeconds(32f);
        Skip.SkipDrama = false;
        BossAudio.PlayOneShot(BossAudio.clip);
        anim_body.SetBool("Dance", false);
        anim_body.SetInteger("Idle", 0);    
        panic.State = 0;
        boss.enabled = true;
        teleport.enabled = true;
        Character.AllProhibit = false;
        Character.ActionProhibit = false;
        Character.MoveOnly = true;
    }
}
