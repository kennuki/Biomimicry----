using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public DeadPanel deadPanel;
    public PanicRed_PP panic;
    void Start()
    {
        Player = GameObject.Find("Character").transform;
        startTime = Time.time;
        startPos = transform.position;
        height = height + (target.position.y - transform.position.y);
        float dis = Vector3.Distance(target.position, transform.position);
        duration = dis / speed;
        foreach(Renderer render in render)
        {
            render.material.EnableKeyword("_EMISSION");
        }
        StartCoroutine(playerDead(0.2f));
    }

    void Update()
    {

    }
    private IEnumerator dollswoop()
    {
        float progress = (Time.time - startTime) / duration;
        while (progress <= 1f)
        {
            Vector3 endPos = target.position + target.rotation * offset;

            float parabolicHeight = height * 4 * progress * (1 - progress);

            Vector3 currentPos = Vector3.Lerp(startPos, endPos, progress);
            currentPos.y = currentPos.y + parabolicHeight;

            transform.position = currentPos;
            yield return null;
        }
    }
    private IEnumerator playerDead(float time)
    {
        Vector3 AngleDifference = new Vector3(0, transform.eulerAngles.y - Player.eulerAngles.y, 0);
        Quaternion TargetRotation = Player.rotation * Quaternion.Euler(0, DeadAngle.y + AngleDifference.y, DeadAngle.z);
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            Player.rotation = Quaternion.Lerp(Player.rotation, TargetRotation, 0.5f);
            yield return null;
        }
        Player.rotation = TargetRotation;
        StartCoroutine(dollswoop());
        yield return new WaitForSeconds(0.2f);
        panic.State = 0;
    }
}
