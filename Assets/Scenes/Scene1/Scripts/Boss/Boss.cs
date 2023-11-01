using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class Boss : MonoBehaviour
{
    public Animator anim;
    AudioSource BossAudio;
    private PanicRed_PP panic;
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
        panic = GameObject.Find("PostProcessing").transform.Find("Effect").transform.Find("Panic").GetComponent<PanicRed_PP>();
        Dead_Camera = this.transform.Find("Cm_Dead").GetComponent<CinemachineVirtualCamera>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = false;
        renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            //Debug.Log(renderer.name);
        }
        navMeshAgent.enabled = true;
    }

    private void Update()
    {

        if(panic == null)
        {
            panic = GameObject.Find("PostProcessing").transform.Find("Effect").transform.Find("Panic").GetComponent<PanicRed_PP>();
        }
        //Debug.Log(Distance);
        if (target == null)
        {
            target = GameObject.Find("Character").transform;

        }
        else
        {
            navMeshAgent.SetDestination(target.position);
        }
        Distance = Vector3.Distance(target.position, transform.position);

        if (navMeshAgent.velocity.magnitude > 0.1f)
        {
            if (Random.Range(0, 10) < 2)
            {
                anim.SetInteger("Walk", 2);
            }
            else
            {
                anim.SetInteger("Walk", 1);
            }
        }
        else
        {
            anim.SetInteger("Walk", 0);
        }

        // 檢查是否需要施放隱形技能
        if (isInvisible == false && Time.time - invisibleStartTime >= invisibleCD)
        {
            //Debug.Log("0");
            //SetAlphaForRenderers(0,0);
           // SetAlphaForRenderers(1,0f);
            //ActivateInvisibility();
        }
        else if(isInvisible == true&& Time.time- invisibleStartTime >= invisibleDuration)
        {
            //SetAlphaForRenderers(0,1);
           // SetAlphaForRenderers(1,0.3f);
            //isInvisible = false;
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
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Character")
        {
            StartCoroutine(PlayerDead());
        }
    }


    public static bool Dead = false;
    public GameObject DeadPanel;
    public Transform Hand;
    public IEnumerator PlayerDead()
    {
        int layerIndex = LayerMask.NameToLayer("Character");
        Dead = true;
        Character.ActionProhibit = true;
        Character.AllProhibit = true;
        CameraRotate.cameratotate = false;
        navMeshAgent.enabled = false;
        anim.SetInteger("Dead", 1);
        target.GetComponent<CharacterController>().enabled = false;
        StartCoroutine(ddd());
        StartCoroutine(DropHead());
        Camera.main.cullingMask |= 1 << layerIndex;
        yield return new WaitForSeconds(10);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CameraRotate.cameratotate = false;
        DeadPanel.SetActive(true);
        target.SetParent(null);
        Dead_Camera.Priority = 0;
        Camera.main.cullingMask &= ~(1 << layerIndex);
        Time.timeScale = 0;
        yield break;
    }

    public CinemachineVirtualCamera Dead_Camera;
    private IEnumerator DropHead()
    {

        Dead_Camera.Priority = 11;

        yield return new WaitForSeconds(10f);

    }
    private IEnumerator ddd()
    {
        while (true)
        {
            target.position = Hand.position;
            yield return null;
        }

    }
}
