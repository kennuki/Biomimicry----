using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAction1 : MonoBehaviour
{
    RandomTeleport teleport;
    Boss boss;
    public AudioSource BossAudio;
    public AudioClip music;
    public PanicRed_PP panic;
    public Animator anim_body;
    private void Start()
    {
        boss = this.GetComponent<Boss>();
        teleport = GetComponent<RandomTeleport>();
        StartCoroutine(Orei());
    }
    public IEnumerator Orei()
    {
        yield return new WaitForSeconds(2f);
        BossAudio.PlayOneShot(music);
        yield return new WaitForSeconds(3f);
        anim_body.enabled = true;

        anim_body.SetBool("Dance", true);
        anim_body.SetInteger("Idle", 1);
        yield return new WaitForSeconds(29.5f);
        BossAudio.Stop();
        Skip.SkipDrama = false;
        anim_body.SetBool("Dance", false);
        anim_body.SetInteger("Idle", 0);    
        panic.State = 0;
        boss.enabled = true;
        teleport.enabled = true;
        Character.AllProhibit = false;
        Character.ActionProhibit = false;
        Character.MoveOnly = false;
    }
}
