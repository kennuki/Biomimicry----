﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    [System.Serializable]
    public class TelePortPoint
    {
        public int PointCount;
        public Transform[] Point;
        public int[] MaxTeleportTimes;
    }
    public TelePortPoint telePort;

    public Animator anim;
    AudioSource BossAudio;
    private PanicRed_PP panic;
    private PanicRed_PP panic2;
    private Transform target; 
    private NavMeshAgent navMeshAgent;
    private Renderer[] renderers;

    private float DistanceLast;
    private float DistancePlayer;
    private void Start()
    {
        BossAudio = GetComponent<AudioSource>();
        target = GameObject.Find("Character").transform;
        flashlight = target.transform.Find("Head").transform.Find("FlashLight").GetComponent<Light>();
        panic = GameObject.Find("PostProcessing").transform.Find("Effect").transform.Find("Panic").GetComponent<PanicRed_PP>();
        panic2 = GameObject.Find("PostProcessing").transform.Find("Effect").transform.Find("Panic2").GetComponent<PanicRed_PP>();
        Dead_Camera = this.transform.Find("Cm_Dead").GetComponent<CinemachineVirtualCamera>();
        Cm1 = GameObject.Find("CameraGroup").transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        cinemachineBrain = Camera.main.transform.GetComponent<CinemachineBrain>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = false;
        renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            //Debug.Log(renderer.name);
        }
        navMeshAgent.enabled = true;
    }



    int State = 0;/*State 0 = Can see target and not over limited distance
                    State 1 = Cannot see target. On the way to LastPosition
                    State 2 = Cannot see target. Reached LastPosition and start wandering in random Teleport Position for 3 second.
                    State 3 = Search valid teleport point.
                    State 4 = After State 3.
                    State 5 = Teleport duration.
                   */
    private void Update()
    {
        WalkAnim();
        if(panic == null)
        {
            panic = GameObject.Find("PostProcessing").transform.Find("Effect").transform.Find("Panic").GetComponent<PanicRed_PP>();
        }
     

        Vector3 direction = target.position - transform.position;
        direction.Normalize();
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if(hit.transform.gameObject.name == "Character")
            {
                LastPos = hit.point;
                See = true;
                NoSeeTime = 0;
            }
            else
            {
                See = false;
            }
        }

        DistanceLast = Vector3.Distance(LastPos, transform.position);
        DistancePlayer = Vector3.Distance(target.position, transform.position);

        if(State == 0)
        {
            if (navMeshAgent.enabled == true)
            {
                navMeshAgent.SetDestination(LastPos);
                ChoosePath();
            }
        }
        if(State == 1)
        {

            if (navMeshAgent.path == null || navMeshAgent.path.corners.Length == 0)
            {
                RandomIndex = Random.Range(0, telePort.PointCount);
                State = 2;
            }
            else if (DistanceLast < 1.6f)
            {
                RandomIndex = Random.Range(0, telePort.PointCount);
                State = 2;
            }
            else if (navMeshAgent.enabled == true)
            {
                if(LastPos == Vector3.zero)
                {
                    State = 2;
                }
                navMeshAgent.SetDestination(LastPos);
            }
            else
            {
                State = 3;
            }
        }
        if(State == 2)
        {
            Debug.Log(navMeshAgent.enabled);
            if (See == true)
            {
                if (DistancePlayer < 25)
                {
                    State = 0;
                }
                else if (DistancePlayer >= 25)
                {
                    State = 3;
                }
            }
            else if (navMeshAgent.enabled == true)
            {
                
                navMeshAgent.SetDestination(telePort.Point[RandomIndex].position);
            }
            if (navMeshAgent.path == null || navMeshAgent.path.corners.Length == 0)
            {
                RandomIndex += 1;
                if (RandomIndex >= telePort.PointCount)
                {
                    RandomIndex -= telePort.PointCount;
                    RandomTimes += 1;
                }
                else
                {
                    RandomTimes += 1;
                }
                if(RandomTimes == telePort.PointCount)
                {
                    NoSeeTime = 0;
                    State = 3;
                }
            }
            else
            {

                NoSeeTime += Time.deltaTime;
                if(NoSeeTime > 7)
                {
                    NoSeeTime = 0;
                    State = 3;
                }
            }
        }
        if(State == 3)
        {
            StartCoroutine(SearchTeleportPoint());
        }


        if (State == 5)
        {
            State5_Counter += Time.deltaTime;
            anim.SetInteger("Turn", 1);
            if (State5_Counter > 2)
            {
                State5_Counter = 0;
                anim.SetInteger("Turn", 0);
                LastPos = target.position;
                navMeshAgent.enabled = true;
                ChoosePath();
            }
        }

    }
    private Vector3 LastPos;
    private bool See = true;
    private float NoSeeTime = 0;
    float State5_Counter;
    int RandomIndex;
    int RandomTimes = 0;
    Transform TeleportLast;
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Character")
        {
            StartCoroutine(PlayerDead());
        }
    }

    private IEnumerator SearchTeleportPoint()
    {
        State = 4;
        anim.SetInteger("Walk", 0);
        Vector3 DirectionToPlayer;
        Quaternion rotationToFaceAway;
        foreach (Transform teleport_point in telePort.Point)
        {
            navMeshAgent.enabled = false;
            transform.position = teleport_point.position;
            DirectionToPlayer = target.position - transform.position;
            rotationToFaceAway = Quaternion.LookRotation(-DirectionToPlayer);
            transform.rotation = rotationToFaceAway;
            yield return null;
            navMeshAgent.enabled = true;
            if (navMeshAgent.enabled == true)
            {
                navMeshAgent.SetDestination(target.position);
            }
            if (navMeshAgent.path == null || navMeshAgent.path.corners.Length == 0)
            {

            }
            else if(navMeshAgent.path != null)
            {
                if(teleport_point == TeleportLast)
                {
                    RandomIndex = Random.Range(0, telePort.PointCount);
                    transform.position = telePort.Point[RandomIndex].position;
                    TeleportLast = telePort.Point[RandomIndex];
                    DirectionToPlayer = target.position - transform.position;
                    rotationToFaceAway = Quaternion.LookRotation(-DirectionToPlayer);
                    transform.rotation = rotationToFaceAway;
                }
                TeleportLast = teleport_point;
                navMeshAgent.enabled = false;
                State = 5;
                yield break;
            }
        }
        RandomIndex = Random.Range(0, telePort.PointCount);
        transform.position = telePort.Point[RandomIndex].position;
        TeleportLast = telePort.Point[RandomIndex];
        DirectionToPlayer = target.position - transform.position;
        rotationToFaceAway = Quaternion.LookRotation(-DirectionToPlayer);
        transform.rotation = rotationToFaceAway;
        yield break;
    }



    private void ChoosePath()
    {
        if (See == true)
        {
            if (DistancePlayer < 25)
            {
                State = 0;
            }
            else if (DistancePlayer >= 25)
            {
                State = 3;
            }
        }
        else if (See == false)
        {
            if(LastPos == Vector3.zero)
            {
                State = 2;
            }
            if (DistancePlayer < 25f)
            {
                State = 1;
            }
            else if(DistancePlayer >= 25f)
            {
                State = 3;
            }
        }
    }

    

    private void WalkAnim()
    {
        counter += Time.deltaTime;
        if (navMeshAgent.velocity.magnitude > 0.1f)
        {
            if (counter > 1)
            {
                if (Random.Range(0, 10) < 2)
                {
                    anim.SetInteger("Walk", 2);
                }
                else
                {
                    anim.SetInteger("Walk", 1);
                }
                counter = 0;
            }

        }
        else
        {
            anim.SetInteger("Walk", 0);
        }
    }

    private Light flashlight;
    private CinemachineBrain cinemachineBrain;
    public CinemachineVirtualCamera Dead_Camera;
    private CinemachineVirtualCamera Cm1;
    public static bool Dead = false;
    public GameObject DeadPanel;
    public Transform Hand;
    public Vector3 DeadAngle = new Vector3(0, 90, 0);
    private float Cm_Default_Transition_Time;
    public float CameraChangeDelay = 2;
    public IEnumerator PlayerDead()
    {
        State = 6;
        NoSeeTime = 0;
        RandomTimes = 0;

        flashlight.enabled = false;
        Cm_Default_Transition_Time = cinemachineBrain.m_DefaultBlend.BlendTime;
        Dead = true;
        Character.ActionProhibit = true;
        Character.AllProhibit = true;
        CameraRotate.cameratotate = false;
        navMeshAgent.enabled = false;
        anim.SetInteger("Dead", 1);
        target.GetComponent<CharacterController>().enabled = false;
        StartCoroutine(CharacterFollowBoss());
        yield return new WaitForSeconds(1f);
        StartCoroutine(CharacterLookBoss(3));
        yield return new WaitForSeconds(CameraChangeDelay);
        cinemachineBrain.m_DefaultBlend.m_Time = 0.5f;
        Dead_Camera.Priority = 11;
        int layerIndex = LayerMask.NameToLayer("Character");
        Camera.main.cullingMask |= 1 << layerIndex;
        StartCoroutine(DropHead());
        yield return new WaitForSeconds(0.5f);
        panic2.State = 0;
        yield return new WaitForSeconds(0.5f);
        panic2.State = 0;
        yield return new WaitForSeconds(1.5f);
        SceneManager.sceneLoaded += OnSceneLoaded;
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

    public float HeadTime,HeadTime2;
    public Vector3 HeadDropForce;
    public Transform Hand_L;
    private GameObject RealHead;
    private GameObject FakeHead;
    private IEnumerator DropHead()
    {
        RealHead = target.Find("Head").gameObject;
        FakeHead = target.Find("Head_Fake").gameObject;
        yield return new WaitForSeconds(HeadTime);
        RealHead.SetActive(false);
        FakeHead.SetActive(true);
        FakeHead.transform.SetParent(Hand_L);
        yield return new WaitForSeconds(HeadTime2);
        FakeHead.transform.SetParent(null);
        Rigidbody FakeHeadRb = FakeHead.GetComponent<Rigidbody>();
        FakeHeadRb.useGravity = true;
        FakeHeadRb.isKinematic = false;
        FakeHeadRb.AddForce(transform.rotation * HeadDropForce);
    }

    public float GrabAdjust_y=-0.42f;
    private IEnumerator CharacterFollowBoss()
    {
        SceneLoad = false;
        while (SceneLoad == false)
        {
            Vector3 TargetPos = Hand.position + new Vector3(0, GrabAdjust_y, 0);
            target.position = Vector3.Lerp(target.position, TargetPos, 0.5f);
            yield return null;
        }
    }

    private IEnumerator CharacterLookBoss(float time)
    {
        Vector3 AngleDifference = new Vector3(0, transform.eulerAngles.y - target.eulerAngles.y, 0);
        Quaternion TargetRotation = target.rotation * Quaternion.Euler(0, DeadAngle.y + AngleDifference.y, DeadAngle.z);
        Cm1.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y = 0;
        for (float i =0; i<time; i+=Time.deltaTime)
        {
            target.rotation = Quaternion.Lerp(target.rotation, TargetRotation, 0.5f);
            yield return null;
        }
        //target.rotation = TargetRotation;
    }

    float counter = 0;
    

    bool SceneLoad = false;
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        State = 0;
        See = true;
        anim.SetInteger("Dead", 0);
        cinemachineBrain.m_DefaultBlend.m_Time = Cm_Default_Transition_Time;
        target = GameObject.Find("Character").transform;
        flashlight = target.transform.Find("Head").transform.Find("FlashLight").GetComponent<Light>();
        Cm1 = GameObject.Find("CameraGroup").transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        panic = GameObject.Find("PostProcessing").transform.Find("Effect").transform.Find("Panic").GetComponent<PanicRed_PP>();
        cinemachineBrain = Camera.main.transform.GetComponent<CinemachineBrain>();
        SceneLoad = true;
        navMeshAgent.enabled = true;
        DeadPanel.SetActive(false);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
