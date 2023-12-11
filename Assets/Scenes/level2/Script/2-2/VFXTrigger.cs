using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXTrigger : EventTriggerFunction
{
    public AudioSource source;
    public ParticleSystem whiff;
    private Collider cd;
    private void Start()
    {
        cd = GetComponent<Collider>();
    }
    public override void Enter()
    {
        if (Trigger)
        {
            whiff.Play();
            source.Play();
            Trigger = false;
            cd.enabled = false;
            StartCoroutine(Restart());
        }
    }
    private IEnumerator Restart()
    {
        yield return new WaitForSeconds(5);
        cd.enabled = true;
    }
}
