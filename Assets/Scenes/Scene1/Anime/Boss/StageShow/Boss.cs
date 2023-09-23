using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    AudioSource BossAudio;
    public PanicRed_PP panic;
    public Transform target; 
    private NavMeshAgent navMeshAgent;
    private Renderer[] renderers;
    private bool isInvisible = false;
    private float invisibleDuration = 3.0f;
    private float invisibleStartTime = -7.0f;
    private float invisibleCD = 13;

    private float Distance; 

    private void Start()
    {
        BossAudio = GetComponent<AudioSource>();
        target = GameObject.Find("Character").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        renderers = GetComponentsInChildren<Renderer>();       
    }

    private void Update()
    {
        Distance = Vector3.Distance(target.position, transform.position);
        Debug.Log(Distance);
        if (target == null)
        {
            target = GameObject.Find("Character").transform;

        }
        else
        {
            navMeshAgent.SetDestination(target.position);
        }

        // 檢查是否需要施放隱形技能
        if (isInvisible == false && Time.time - invisibleStartTime >= invisibleCD)
        {
            SetAlphaForRenderers(0,0);
            SetAlphaForRenderers(1,0f);
            ActivateInvisibility();
        }
        else if(isInvisible == true&& Time.time- invisibleStartTime >= invisibleDuration)
        {
            SetAlphaForRenderers(0,1);
            SetAlphaForRenderers(1,0.3f);
            isInvisible = false;
        }
    }

    public void ActivateInvisibility()
    {
        // 啟動隱形技能
        isInvisible = true;
        invisibleStartTime = Time.time;
        BossAudio.PlayOneShot(BossAudio.clip);
        panic.State = 0;
    }

    private void SetAlphaForRenderers(int index,float alpha)
    {
        foreach (Renderer renderer in renderers)
        {
            foreach (Material material in renderer.materials)
            {
                Color color = material.color;
                color.a = alpha;
                material.color = color;
            }
        }
    }


}
