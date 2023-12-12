using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DollSwoop : MonoBehaviour
{
    public Transform target; 
    public float height = 2f;
    public float speed = 10f;
    private float duration;
    public Vector3 offset = Vector3.zero;
    private float startTime;
    private Vector3 startPos;
    public Renderer[] render;
    private Transform Player;
    public Vector3 DeadAngle = new Vector3(0, 90, 0);
    public GameObject deadPanel;
    public PanicRed_PP panic, panic2;
    public ScreenDistort screenDistort;
    public HingeJoint joint;
    public AudioSource source;
    public AudioSource source_loop;
    public AudioSource source_door;
    public AudioClip[] clip;
    public Image black;
    void Start()
    {
        Player = GameObject.Find("Character").transform;

        StartCoroutine(DeadSymptom());
    }

    void Update()
    {

    }
    private IEnumerator dollswoop()
    {
        startTime = Time.time;
        startPos = transform.position;
        height = height + (target.position.y - transform.position.y);
        float dis = Vector3.Distance(target.position, transform.position);
        duration = dis / speed;
        foreach (Renderer render in render)
        {
            render.material.EnableKeyword("_EMISSION");
        }
        float progress=0;
        while (progress <= 1f)
        {
            progress = (Time.time - startTime) / duration;
            Vector3 endPos = target.position + target.rotation * offset;

            float parabolicHeight = height * 4 * progress * (1 - progress);

            Vector3 currentPos = Vector3.Lerp(startPos, endPos, progress);
            currentPos.y = currentPos.y + parabolicHeight;

            transform.position = currentPos;
            yield return null;
        }
        black.color = new Color(0, 0, 0, 1);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        deadPanel.SetActive(true);
        yield return null;
        yield break;
    }
    private IEnumerator playerDead(float time)
    {
        transform.LookAt(Player.position);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
        Vector3 AngleDifference = new Vector3(0, transform.eulerAngles.y - Player.eulerAngles.y, 0);
        Quaternion TargetRotation = Player.rotation * Quaternion.Euler(0, DeadAngle.y + AngleDifference.y, DeadAngle.z);
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            Player.rotation = Quaternion.Lerp(Player.rotation, TargetRotation, 0.2f);
            yield return null;
        }
        Player.rotation = TargetRotation;
        source.PlayOneShot(clip[0],1);
        panic.quick = false;
        panic.State = 0;
        panic.speed = 1;
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(dollswoop());
        //source.PlayOneShot(clip[1],0.1f);
    }
    private IEnumerator DeadSymptom()
    {
        Character.AllProhibit = true;
        Character.ActionProhibit = true;
        Character.MoveOnly = true;
        source.PlayOneShot(clip[1],0.6f);
        StartCoroutine(screenDistort.TransitionPostProcessing());
        StartCoroutine(screenDistort.DistortPingPong());
        StartCoroutine(PlayerSpeed());
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(ChangeAudioPropertiesOverTime(2f, 0.8f, 1.55f));
        source_door.Stop();
        source_door.PlayOneShot(clip[2],0.08f);
        JointSpring jointSpring = joint.spring;
        jointSpring.spring = 1.5f;
        joint.spring = jointSpring;
        yield return new WaitForSeconds(2.5f);
        joint.useLimits = true;
        source_door.Stop();
        source_door.PlayOneShot(clip[3],0.35f);
        yield return new WaitForSeconds(1f);
        
        StartCoroutine(playerDead(0.2f));

    }
    private IEnumerator PlayerSpeed()
    {
        while (true)
        {
            Player.gameObject.GetComponent<Character>().Origin_speed = 0.4f;
            yield return null;
        }
    }
    private IEnumerator ChangeAudioPropertiesOverTime(float changeDuration,float targetVolume,float targetPitch)
    {
        float initialVolume = source_loop.volume;
        float initialPitch = source_loop.pitch;
        float timer = 0f;
        source_loop.Play();
        while (timer < changeDuration)
        {
            float progress = timer / changeDuration;

            // 線性插值改變音量和 pitch
            source_loop.volume = Mathf.Lerp(initialVolume, targetVolume, progress);
            source_loop.pitch = Mathf.Lerp(initialPitch, targetPitch, progress);

            timer += Time.deltaTime;
            yield return null;
        }

        // 確保最終設置為目標值
        source_loop.volume = targetVolume;
        source_loop.pitch = targetPitch;
    }
}
