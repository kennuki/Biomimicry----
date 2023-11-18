using System.Collections;
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

    public Animator anim;
    public Transform IdlePointList;
    public TelePortPoint telePort;

    private List<Transform> IdlePoint = new List<Transform>();
    private List<Transform> PointCache = new List<Transform>();
    private PanicRed_PP panic;
    private PanicRed_PP panic2;
    private NavMeshAgent navMeshAgent;
    private Renderer[] renderers;
    private Transform target; 
    private Transform IdleTarget_Point;
    private Transform TargetTeleportPoint;
    private Vector3 LastPos=Vector3.zero;
    private bool See = true;

    private float distance;
    private float Teleport_CD = 7;
    private float WalkChange_Counter = 0;
    private float Anime_Teleport_Counter;
    private float DistanceLast;
    private float DistancePlayer;
    private enum ChasingState
    {
        Chase,
        ChaseTrack,
        WanderStart,
        Wandering,
        TeleportPointSearch,
        TeleportStart,
        Teleport
    } 
    private void Start()
    {
        FindFunction();
        cinemachineBrain = Camera.main.transform.GetComponent<CinemachineBrain>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        renderers = GetComponentsInChildren<Renderer>();
        AddIdlePoint();
        navMeshAgent.enabled = true;
    }

    private void FindFunction()
    {
        target = GameObject.Find("Character").transform;
        flashlight = target.transform.Find("Head").transform.Find("FlashLight").GetComponent<Light>();
        panic = GameObject.Find("PostProcessing").transform.Find("Effect").transform.Find("Panic").GetComponent<PanicRed_PP>();
        panic2 = GameObject.Find("PostProcessing").transform.Find("Effect").transform.Find("Panic2").GetComponent<PanicRed_PP>();
        Dead_Camera = this.transform.Find("Cm_Dead").GetComponent<CinemachineVirtualCamera>();
        Cm1 = GameObject.Find("CameraGroup").transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
    }



    public void AddIdlePoint()
    {
        if (IdlePointList != null)
        {
            foreach (Transform child in IdlePointList)
            {
                IdlePoint.Add(child);
            }
        }
        else
        {
            Debug.LogError("Target object not assigned!");
        }
    }

    public void AddCachePoint(Transform target)
    {
        bool InCache = false;
        foreach (Transform point in PointCache)
        {
            if (target == point)
            {
                InCache = true;
            }
        }
        if (InCache==false)
        {
            if (PointCache.Count > 5)
            {
                PointCache.Add(target);
                PointCache.RemoveAt(0);
            }
            else
            {
                PointCache.Add(target);
            }
        }

    }


    private ChasingState State = ChasingState.Chase;

    private void Update()
    {
        if (Input.GetKey(KeyCode.O))
        {
            Debug.Log(State);
        }

        if (IdleTarget_Point!= null)
        {
            string A="";
            foreach (Transform point in PointCache)
            {
                A = A + point.name;
            }
                //Debug.Log(A);

        }
     
        WalkAnim();
        BossChasing();

    }
    private void BossChasing()
    {
        BossChasing_Detect();
        BossChasing_Tick();
        if (State == ChasingState.Chase)
        {
            if (navMeshAgent.enabled == true)
            {
                navMeshAgent.SetDestination(LastPos);
                ChoosePath();
            }
        }
        if (State == ChasingState.ChaseTrack)
        {
            Debug.Log("1");
            if (See == true)
            {
                ChoosePath();
            }
            if (navMeshAgent.path == null || navMeshAgent.path.corners.Length == 0)
            {
                State = ChasingState.WanderStart;
            }
            else if (DistanceLast < 2f)
            {
                State = ChasingState.WanderStart;
            }
            else if (navMeshAgent.velocity.magnitude>0.01f)
            {
                
                navMeshAgent.SetDestination(LastPos);
            }
            else
            {
                State = ChasingState.WanderStart;
            }
        }
        if (State == ChasingState.WanderStart)
        {
            LastPos = Vector3.zero;
            //Debug.Log(navMeshAgent.enabled);
            if (See == true && navMeshAgent.enabled == true)
            {
                if (DistancePlayer < 25)
                {
                    State = ChasingState.Chase;
                }
                else if (DistancePlayer >= 25)
                {
                    if (Teleport_CD > 8)
                    {
                        State = ChasingState.TeleportPointSearch;
                    }
                    else
                    {
                        StartCoroutine(BossIdle());
                        State = ChasingState.Wandering;
                    }
                }
            }
            else if (navMeshAgent.enabled == true)
            {
                if (Teleport_CD > 10)
                {
                    State = ChasingState.TeleportPointSearch;
                }
                else
                {
                    StartCoroutine(BossIdle());
                    State = ChasingState.Wandering;
                }


            }
            /*if (navMeshAgent.path == null || navMeshAgent.path.corners.Length == 0)
            {
                NoSeeTime = 0;
                State = 3;
            }
            else
            {
                NoSeeTime += Time.deltaTime;
                if (NoSeeTime > 7)
                {
                    NoSeeTime = 0;
                    State = 3;
                }
            }*/
        }
        if (State == ChasingState.TeleportPointSearch)
        {
            StartCoroutine(SearchTeleportPoint());
        }
        if (State == ChasingState.Teleport)
        {
            Anime_Teleport_Counter += Time.deltaTime;

            if (Anime_Teleport_Counter > 2)
            {
                Anime_Teleport_Counter = 0;
                anim.SetInteger("Turn", 0);
                LastPos = target.position;
                navMeshAgent.enabled = true;
                Teleport_CD = 0;
                ChoosePath();
            }
        }
    }
    private void BossChasing_Detect()
    {
        Vector3 direction = target.position - transform.position;
        direction.Normalize();
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.name == "Character")
            {
                LastPos = hit.point;
                See = true;
            }
            else
                See = false;
        }
    }
    private void BossChasing_Tick()
    {
        Teleport_CD += Time.deltaTime;
        DistanceLast = Vector3.Distance(LastPos, transform.position);
        DistancePlayer = Vector3.Distance(target.position, transform.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Character")
        {
            StartCoroutine(PlayerDead());
        }
    }

    private void ChoosePath()
    {
        if (See)
        {
            if (DistancePlayer < 25)
                State = ChasingState.Chase;
            else
            {
                if (Teleport_CD > 8)
                    State = ChasingState.TeleportPointSearch;
                else
                    State = ChasingState.WanderStart;
            }
        }
        else if (!See)
        {
            if (DistancePlayer < 25f)
            {
                if (Teleport_CD > 8)
                    State = ChasingState.TeleportPointSearch;
                else
                    State = ChasingState.ChaseTrack;

            }


            else
            {
                if (Teleport_CD > 8)
                    State = ChasingState.TeleportPointSearch;
                else
                    State = ChasingState.WanderStart;
            }
        }
    }

    private IEnumerator SearchTeleportPoint()
    {
        Vector3 DirectionToPlayer;
        Quaternion rotationToFaceAway;
        float DistanceToPlayerCache = Vector3.Distance(transform.position,target.position);
        State = ChasingState.TeleportStart;
        anim.SetInteger("Walk", 0);
        navMeshAgent.enabled = false;
        foreach (Transform teleport_point in telePort.Point)
        {
            float DistanceToPlayer = Vector3.Distance(teleport_point.position, target.position);
            if (DistanceToPlayer < DistanceToPlayerCache)
            {
                if(TargetTeleportPoint == null)
                {
                    TargetTeleportPoint = teleport_point;
                    DistanceToPlayerCache = DistanceToPlayer;
                }
                else if(DistanceToPlayer < DistanceToPlayerCache)
                {
                    TargetTeleportPoint = teleport_point;
                    DistanceToPlayerCache = DistanceToPlayer;
                }
            }
        }

        if (TargetTeleportPoint != null)
        {
            panic2.State = 0;
            anim.SetInteger("Turn", 1);
            yield return new WaitForSeconds(Time.deltaTime * 3);
            transform.position = TargetTeleportPoint.position;
            DirectionToPlayer = target.position - transform.position;
            rotationToFaceAway = Quaternion.LookRotation(DirectionToPlayer);
            transform.rotation = rotationToFaceAway;
            navMeshAgent.enabled = true;
            State = ChasingState.Teleport;
            //navMeshAgent.SetDestination(target.position);
        }
        else
        {
            navMeshAgent.enabled = true;
            //Teleport_CD = 0;
            State = ChasingState.WanderStart;
        }
        

        
        yield break;
    }

    private IEnumerator BossIdle()
    {
        distance = 100;
        float time = 0;
        int i;
        for (i = 0; i < IdlePoint.Count; i++)
        {
            if (Vector3.Distance(IdlePoint[i].position, this.transform.position) < distance)
            {

                    IdleTarget_Point = IdlePoint[i];
                    distance = Vector3.Distance(IdlePoint[i].position, this.transform.position);
 
            }
        }

        NearPoint near = IdleTarget_Point.GetComponent<NearPoint>();
        int nearpoint_count = near.pointInfo.NearPoint.Length;
        List<Transform> NearPoint_fix = new List<Transform>();
        AddCachePoint(IdleTarget_Point);

        while (time < 5)
        {
            if(See == true)
            {
                State = 0;
                yield break;
            }
            if(Vector3.Distance(IdleTarget_Point.position,transform.position) > 2)
            {
                time += Time.deltaTime;
                navMeshAgent.SetDestination(IdleTarget_Point.position);
                yield return null;
            }
            else
            {

                for (int j=0;j< nearpoint_count; j++)
                {
                    bool InCache = false;
                    foreach (Transform point in PointCache)
                    {
                        if(point == near.pointInfo.NearPoint[j])
                        {
                            InCache = true;
                        }
                    }
                    if (!InCache)
                    {
                        NearPoint_fix.Add(near.pointInfo.NearPoint[j]);
                    }
                }
                if (NearPoint_fix.Count == 0)
                {
                    PointCache.Clear();
                }
                else
                {
                    IdleTarget_Point = NearPoint_fix[Random.Range(0, NearPoint_fix.Count)];
                    AddCachePoint(IdleTarget_Point);
                    near = IdleTarget_Point.GetComponent<NearPoint>();
                    nearpoint_count = near.pointInfo.NearPoint.Length;
                    NearPoint_fix.Clear();
                }
                navMeshAgent.SetDestination(IdleTarget_Point.position);

            }
        }
        State = ChasingState.WanderStart;
        PointCache.RemoveAt(PointCache.Count - 1);
        yield break;

    }

    private void WalkAnim()
    {
        WalkChange_Counter += Time.deltaTime;
        if (navMeshAgent.velocity.magnitude > 0.1f)
        {
            if (WalkChange_Counter > 1)
            {
                if (Random.Range(0, 10) < 4)
                {
                    anim.SetInteger("Walk", 2);
                }
                else
                {
                    anim.SetInteger("Walk", 1);
                }
                WalkChange_Counter = 0;
            }

        }
        else
        {
            anim.SetInteger("Walk", 0);
        }
    }







    


    public static bool Dead = false;
    public GameObject DeadPanel;
    public Transform Hand;
    public Transform Hand_L;
    public CinemachineVirtualCamera Dead_Camera;
    public Vector3 DeadAngle = new Vector3(0, 90, 0);
    public Vector3 HeadDropForce;
    public float CameraChangeDelay = 2;
    public float HeadTime,HeadTime2;
    public float GrabAdjust_y=-0.42f;

    private Light flashlight;
    private CinemachineBrain cinemachineBrain;
    private CinemachineVirtualCamera Cm1;
    private GameObject RealHead;
    private GameObject FakeHead;
    private float Cm_Default_Transition_Time;
    private bool SceneLoad = false;


    public IEnumerator PlayerDead()
    {
        flashlight.enabled = false;
        Dead = true;
        Cm_Default_Transition_Time = cinemachineBrain.m_DefaultBlend.BlendTime;
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LastPos = target.position;
        PointCache.Clear();
        State = ChasingState.ChaseTrack;
        See = true;
        SceneLoad = true;
        navMeshAgent.enabled = true;
        cinemachineBrain.m_DefaultBlend.m_Time = Cm_Default_Transition_Time;
        anim.SetInteger("Dead", 0);
        target = GameObject.Find("Character").transform;
        flashlight = target.transform.Find("Head").transform.Find("FlashLight").GetComponent<Light>();
        Cm1 = GameObject.Find("CameraGroup").transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        panic = GameObject.Find("PostProcessing").transform.Find("Effect").transform.Find("Panic").GetComponent<PanicRed_PP>();
        cinemachineBrain = Camera.main.transform.GetComponent<CinemachineBrain>();
        PointCache.Clear();
        DeadPanel.SetActive(false);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
