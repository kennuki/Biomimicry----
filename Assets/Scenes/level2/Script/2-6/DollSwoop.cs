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
    void Start()
    {
        startTime = Time.time;
        startPos = transform.position;
        height = height + (target.position.y - transform.position.y);
        float dis = Vector3.Distance(target.position, transform.position);
        duration = dis / speed;
        foreach(Renderer render in render)
        {
            render.material.EnableKeyword("_EMISSION");
        }
        if (target == null)
        {
            Debug.LogError("Please assign a target object in the inspector.");
        }
    }

    void Update()
    {
        if (target != null)
        {
            float progress = (Time.time - startTime) / duration;

            if (progress <= 1f)
            {
                Vector3 endPos = target.position+ target.rotation*offset;

                float parabolicHeight = height *4 * progress * (1 - progress);

                Vector3 currentPos = Vector3.Lerp(startPos, endPos, progress);
                currentPos.y = currentPos.y+parabolicHeight;

                transform.position = currentPos;
            }
            else
            {
                Debug.Log("Movement completed!");
            }
        }
    }
}
