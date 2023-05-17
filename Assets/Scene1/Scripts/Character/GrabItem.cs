using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GrabItem : MonoBehaviour
{
    public Transform LookPoint;
    public CharacterController controller;
    public Animator AnimL,AnimR;
    BoxCollider Range;
    public AudioCharacter audioCharacter;
    AudioSource audioSource;
    AudioClip audioClip;
    public static bool ThrowItem = false;
    void Start()
    {
        audioSource = audioCharacter.AudioSources[0];
        Range = GetComponent<BoxCollider>();
        StartCoroutine(GrabThrowfunction());
    }

    void Update()
    {

        if (IfTriggerDetect == false && Input.GetKeyDown(KeyCode.F) && ThrowItem == false && Character.AllProhibit == false && Character.GrabProhibit == false)
        {
            StartCoroutine(TriggerDetect());
        }
        else if (Input.GetKeyDown(KeyCode.F) && ThrowItem == true && Character.AllProhibit == false && Character.GrabProhibit == false)
        {
            Interacted_Item = null;
            StartCoroutine(TriggerDetect());
            if (ItemName != null)
            {               
                
            }
            else

                StartCoroutine(Throw());
        }
    }

    public Transform LeftHand;
    private GameObject GrabbedItem, PushedItem;
    private Rigidbody GrabbedItemRb, PushedItemRb;
    private PhysicMaterial physicMaterialBox;
    private string ItemName;
    private int ItemIndex;
    public GameObject[] VIP_Item;
    private Collider Interacted_Item;
    private Transform Obstacle;
    private void OnTriggerEnter(Collider other)
    {
        if (ThrowItem == false)
        {
            Transform raycastOrigin = transform;
            Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
            RaycastHit hit;
            bool NoObstacle = true;
            LayerMask layerMask = 1 << 6 | 1 << 12;

            if (Physics.Raycast(ray, out hit,6, layerMask))
            {
                Vector3 collisionPoint = hit.point;
                if (Vector3.Distance(hit.point, transform.position) > Vector3.Distance(other.transform.position, transform.position))
                {
                    NoObstacle = true;
                }
                else
                {
                    NoObstacle = false;
                }
            }
            if (Obstacle != null && NoObstacle == true || Obstacle == null)
            {

               
                
                if (other.tag == "Grabbable")
                {

                    grabbleItem = other.GetComponent<GrabbleItem>();
                    Range.enabled = false;
                    Character.AllProhibit = true;
                    GrabbedItem = other.gameObject;
                    GrabbedItemRb = GrabbedItem.GetComponent<Rigidbody>();
                    GrabAllow = true;
                    ThrowItem = true;
                    StartCoroutine(Grab());
                    foreach (GameObject item in VIP_Item)
                    {
                        if (other.name == item.name)
                        {
                            ItemName = other.gameObject.name;
                            ItemIndex = other.gameObject.GetComponent<InteractiveObject>().index;
                        }
                    }
                }
                else if (other.tag == "Pushable" && Range.size.x < 0.2f)
                {
                    AnimL.SetInteger("PushPull", 1);
                    AnimR.SetInteger("PushPull", 1);
                    Range.enabled = false;
                    PushedItem = other.gameObject;
                    PushedItemRb = PushedItem.GetComponent<Rigidbody>();
                    physicMaterialBox = PushedItem.GetComponentInChildren<BoxCollider>().material;
                    if (physicMaterialBox == null)
                    {
                        physicMaterialBox = PushedItem.GetComponentInChildren<MeshCollider>().material;
                    }
                    Character.AllProhibit = true;
                    Character.MoveOnly = true;
                    CameraRotate.cameratotate = false;
                    physicMaterialBox.dynamicFriction = 0.1f;
                    StartCoroutine(PushObject());
                    StartCoroutine(PushLookAngleAdjust());
                }
                else if (other.tag == "PushOnly" && Range.size.x < 0.2f)
                {
                    AnimL.SetInteger("PushPull", 1);
                    AnimR.SetInteger("PushPull", 1);
                    Range.enabled = false;
                    PushedItem = other.gameObject;
                    PushedItemRb = PushedItem.GetComponent<Rigidbody>();
                    physicMaterialBox = PushedItem.GetComponent<Collider>().material;
                    Character.AllProhibit = true;
                    Character.MoveOnly = true;
                    CameraRotate.cameratotate = false;
                    physicMaterialBox.dynamicFriction = 0.4f;
                    StartCoroutine(PushObject2());
                }
                else if (other.tag == "Rod" && Range.size.y < 0.5f && controller.isGrounded == true&&new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")).magnitude ==0)
                {
                    PushedItem = other.gameObject;
                    float PlayerToRod_Y = Mathf.Abs(transform.position.y - PushedItem.transform.position.y);
                    DistanceToPushedItem = Vector3.Distance(transform.position, PushedItem.transform.position);
                    //Debug.Log(DistanceToPushedItem +" "+ PlayerToRod_Y);
                    if (DistanceToPushedItem < 1f && PlayerToRod_Y < 0.4f && DistanceToPushedItem > 0.3f)
                    {
                        armRotate1.enabled = false;
                        armRotate2.enabled = false;
                        AnimR.SetInteger("Rod", 1);
                        Range.enabled = false;
                        CameraRotate.cameratotate = false;
                        Character.AllProhibit = true;
                        Character.MoveOnly = false;
                        GrabbedItem = other.gameObject;
                        GrabbedItem.GetComponent<BoxCollider>().enabled = false;
                        StartCoroutine(DoorOpen1());
                    }
                    Range.enabled = false;
                }
                else if (other.gameObject.name == "Board" && Range.size.y < 0.5f && controller.isGrounded == true &&other.tag != "Rod")
                {
                    PushedItem = other.gameObject;
                    float PlayerToRod_Y = Mathf.Abs(transform.position.y - PushedItem.transform.position.y);
                    DistanceToPushedItem = Vector3.Distance(transform.position, PushedItem.transform.position);
                    //Debug.Log(DistanceToPushedItem +" "+ PlayerToRod_Y);
                    if (DistanceToPushedItem < 1f && PlayerToRod_Y < 0.4f && DistanceToPushedItem > 0.3f)
                    {
                        AnimR.SetInteger("Rod", 1);
                        Range.enabled = false;
                        CameraRotate.cameratotate = false;
                        Character.AllProhibit = true;
                        Character.MoveOnly = false;
                        GrabbedItem = other.gameObject;
                        GrabbedItem.GetComponent<BoxCollider>().enabled = false;
                        StartCoroutine(ElectricityRecover());
                    }
                    Range.enabled = false;
                }
                else if(other.gameObject.name == "Chair")
                {
                    GrabbedItem.GetComponent<BoxCollider>().enabled = false;
                    StartCoroutine(SitDown());
                }
            }
            if (other.tag == "Obstacle")
            {
              
                Obstacle = other.gameObject.transform;
            }
        }

        else if (other.tag == "Interacted" && Range.size.y < 2f && controller.isGrounded == true)
        {
            Interacted_Item = other;
            InteractFunction();
            Range.enabled = false;
        }
    }
    private void InteractFunction()
    {
        if (Interacted_Item != null)
        {
            switch (ItemIndex)
            {
                case 1:
                    {
                        switch (Interacted_Item.gameObject.GetComponent<InteractiveObject>().index)
                        {
                            case 1:
                                {
                                    float PlayerToScan_Y = Mathf.Abs(transform.position.y - Interacted_Item.transform.position.y);
                                    DistanceToPushedItem = Vector3.Distance(transform.position, Interacted_Item.transform.position);
                                    if (DistanceToPushedItem < 1f && PlayerToScan_Y < 0.2f && DistanceToPushedItem > 0.6f)
                                    {
                                        AnimL.SetInteger("Scan", 1);
                                        Range.enabled = false;
                                        CameraRotate.cameratotate = false;
                                        Character.AllProhibit = true;
                                        Character.MoveOnly = false;
                                        StartCoroutine(GateOpenFunction(true));
                                    }
                                    break;
                                }                          
                            default:
                                break;
                        }
                        break;
                    }
                case 2:
                    {
                        float PlayerToRod_Y = Mathf.Abs(transform.position.y - Interacted_Item.transform.position.y);
                        DistanceToPushedItem = Vector3.Distance(transform.position, Interacted_Item.transform.position);
                        if (DistanceToPushedItem < 1f && PlayerToRod_Y < 0.4f && DistanceToPushedItem > 0.6f)
                        {
                            AnimL.SetInteger("Scan", 1);
                            Range.enabled = false;
                            CameraRotate.cameratotate = false;
                            Character.AllProhibit = true;
                            Character.MoveOnly = false;
                            StartCoroutine(GateOpenFunction(false));
                        }
                        break;
                    }
                default:
                    break;

            }
        }
    }

    private bool GrabAllow = false;
    private IEnumerator GrabThrowfunction()
    {
        while (true)
        {
            if (AnimL.GetInteger("HandState") == 1)
            {
                AnimL.SetInteger("HandState", 2);
            }
            else if (AnimL.GetInteger("HandState") == 3)
            {
                AnimL.SetInteger("HandState", 0);
            }
            else if (GrabAllow == true)
            {
                if (AnimL.GetInteger("HandState") == 0)
                {
                    GrabAllow = false;
                    float AngleX = cm1.transform.eulerAngles.x;
                    if (AngleX > 180) AngleX -= 360;
                    AngleX = Mathf.Clamp(AngleX, 0, 45);
                    AnimL.SetFloat("AngleX", Mathf.Clamp((1 - AngleX / 45), 0, 1));
                    AnimL.SetInteger("HandState", 1);
                }
                else if (AnimL.GetInteger("HandState") == 2)
                {
                    GrabAllow = false;
                    AnimL.SetInteger("HandState", 3);
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    public GameObject cm1;
    bool IfTriggerDetect = false;
    private IEnumerator TriggerDetect()
    {
        Obstacle = null;
        Range.size = new Vector3(0.1f, 0.25f, 0.1f);
        Range.enabled = true;
        transform.eulerAngles = new Vector3(cm1.transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z); 
        IfTriggerDetect = true;
        while (Range.size.z < 3f)
        {
            Range.size = Range.size + new Vector3(0, 0, Time.deltaTime * 100);
            Range.center = Range.center + new Vector3(0, 0, Time.deltaTime * 50);        
            yield return new WaitForSeconds(Time.deltaTime);
        }
        while (Range.size.y < 8f)
        {
            Range.size = Range.size + new Vector3(0, Time.deltaTime * 150, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        while (Range.size.x < 3f)
        {
            Range.size = Range.size + new Vector3(Time.deltaTime * 150, 0, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if (Interacted_Item == null && ThrowItem == true && Character.AllProhibit == false && Character.GrabProhibit == false)
        {
            StartCoroutine(Throw());
        }
        Range.center = Vector3.zero;
        IfTriggerDetect = false;
        Range.enabled = false;

    }



    public Vector3 ThrowForce = new Vector3(50, 80, 160);
    private IEnumerator Throw()
    {
        float ForceAdjust = LookPoint.eulerAngles.x;
        if (ForceAdjust > 180)
        {
            ForceAdjust -= 360;
        }

        ForceAdjust = ForceAdjust / 45 + 1;
        Vector3 AdjustForce = ThrowForce;
        AdjustForce.x = ThrowForce.x / ForceAdjust;
        AdjustForce.y = ThrowForce.y - ForceAdjust * 25;
        if (GrabbedItem != null)
        {
            Physics.IgnoreLayerCollision(7, 10);
            ItemName = null;
            ItemIndex = 0;
            Character.AllProhibit = true;
            GrabAllow = true;
            GrabbedItemRb.useGravity = false;
            yield return new WaitForSeconds(0.05f);
            GrabbedItemRb.isKinematic = false;
            yield return new WaitForSeconds(0.35f);
            if (GrabbedItemRb.gameObject.transform.parent != null)
                GrabbedItem.transform.SetParent(null);
            GrabbedItemRb.AddForce(LookPoint.rotation * AdjustForce * ForceAdjust*(GrabbedItemRb.mass+0.05f));
            GrabbedItemRb.useGravity = true;
            yield return new WaitForSeconds(0.3f);
            Character.AllProhibit = false;
            yield return new WaitForSeconds(0.3f);
            Physics.IgnoreLayerCollision(7, 10, false);
            ThrowItem = false;
            AnimL.SetInteger("HandState", 0);
        }
        yield return new WaitForSeconds(Time.deltaTime);
    }

    private GrabbleItem grabbleItem;
    private Vector3 GrabOffset = new Vector3(0.2f, -0.01f, 0.1f);
    private Vector3 AngleOffset = new Vector3(0, 90, 180);
    private IEnumerator Grab()
    {
        AngleOffset = grabbleItem.Angle;
        GrabOffset = grabbleItem.Offset;
        yield return new WaitForSeconds(0.32f);
        GrabbedItemRb.isKinematic = true;
        yield return new WaitForSeconds(Time.deltaTime);
        GrabbedItem.transform.SetParent(LeftHand);
        GrabbedItem.transform.position = LeftHand.position + transform.rotation * GrabOffset;
        GrabbedItem.transform.eulerAngles = LeftHand.eulerAngles;
        GrabbedItem.transform.Rotate(AngleOffset, Space.Self);
        yield return new WaitForSeconds(0.1f);
        GrabbedItem.transform.position = LeftHand.position + transform.rotation * GrabOffset;
        yield return new WaitForSeconds(0.2f);
        AnimL.SetInteger("HandState", 2);
        Character.AllProhibit = false;
    }

    float OriginSpeed = Character.speed;
    private float DistanceToPushedItem;
    public float PushForce = 10f;
    private IEnumerator PushObject()
    {      
        Vector3 Force = Vector3.zero;
        DistanceToPushedItem = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(PushedItem.transform.position.x, PushedItem.transform.position.z));
        Character.speed = 0;
        for (float i = 0; i <= 0.25f; i += Time.deltaTime)
        {
            if(DistanceToPushedItem < 1.0f)
            {
                if (DistanceToPushedItem - 0.3f < i * 4)
                {
                    Force = transform.rotation * Vector3.forward * PushForce * 1.3f;
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        while (true)
        {
            DistanceToPushedItem = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(PushedItem.transform.position.x, PushedItem.transform.position.z));
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.F)|| DistanceToPushedItem > 2f||Character.NoEnergy == true)
            {
                Range.enabled = false;
                AnimL.SetInteger("PushPull", 0);
                AnimR.SetInteger("PushPull", 0);
                Character.AllProhibit = false;
                Character.MoveOnly = false;
                CameraRotate.cameratotate = true;
                Cursor.lockState = CursorLockMode.Locked;
                Character.speed = OriginSpeed;
                Character.EnergyUse = false;
                yield return new WaitForSeconds(0.2f);
                physicMaterialBox.dynamicFriction = 2f;
                yield break;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                AnimL.SetInteger("PushPull", 1);
                AnimR.SetInteger("PushPull", 1);

                if (DistanceToPushedItem >= 0.7f)
                {
                    Character.speed = 1+DistanceToPushedItem-0.65f;
                    Force = transform.rotation * Vector3.forward * PushForce * (1 + (0.65f - DistanceToPushedItem) * 8f) * 1.4f;
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));
                }
                else if (DistanceToPushedItem < 0.65f)
                {
                    Character.speed = Mathf.Clamp(DistanceToPushedItem- 0.7f, 0, 1);
                    Force = transform.rotation * Vector3.forward * PushForce * (1 + (0.65f - DistanceToPushedItem))*1.4f;   
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));
                }
                else
                {
                    Character.speed = 1f;
                    Force = transform.rotation * Vector3.forward * PushForce * (1 + (0.65f - DistanceToPushedItem)*5f) * 1.4f;
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));
                }
                if (DistanceToPushedItem > 1.6f)
                {
                    Range.enabled = false;
                    AnimL.SetInteger("PushPull", 0);
                    AnimR.SetInteger("PushPull", 0);
                    Character.AllProhibit = false;
                    Character.MoveOnly = false;
                    Character.EnergyUse = false;
                    Character.speed = OriginSpeed;
                    CameraRotate.cameratotate = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    physicMaterialBox.dynamicFriction = 2f;
                    yield break;
                }
                Character.EnergyUse = true;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                AnimL.SetInteger("PushPull", 2);
                AnimR.SetInteger("PushPull", 2);
                DistanceToPushedItem = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(PushedItem.transform.position.x, PushedItem.transform.position.z));
                float maxSpeed = 1;
                Vector3 vel = PushedItemRb.velocity;
                if (DistanceToPushedItem > 0.65f)
                {
                    Character.speed = 1;
                    Force = transform.rotation * Vector3.back * PushForce * (1 + (DistanceToPushedItem-0.65f) * 5f);
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));

                }
                else if (DistanceToPushedItem < 0.65f)
                {
                    Character.speed = 1 +1f - DistanceToPushedItem;
                }
                if (DistanceToPushedItem > 1.2f)
                {
                    Range.enabled = false;
                    AnimL.SetInteger("PushPull", 0);
                    AnimR.SetInteger("PushPull", 0);
                    Character.AllProhibit = false;
                    Character.MoveOnly = false;
                    Character.EnergyUse = false;
                    Character.speed = OriginSpeed;
                    CameraRotate.cameratotate = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    physicMaterialBox.dynamicFriction = 2f;
                    yield break;
                }
                if (PushedItemRb.velocity.magnitude > maxSpeed&& DistanceToPushedItem < 0.7f)
                {
                    PushedItemRb.velocity = vel.normalized * maxSpeed;
                }
                Character.EnergyUse = true;
            }
            else
            {
                Character.EnergyUse = false;
            }
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            
        }
    }
    private IEnumerator PushObject2()
    {
        Vector3 Force = Vector3.zero;
        DistanceToPushedItem = Vector2.Distance(new Vector2(transform.position.x, transform.position.z) , new Vector2(PushedItem.transform.position.x, PushedItem.transform.position.z));
        for (float i = 0; i <= 0.25f; i += Time.deltaTime)
        {
            if (DistanceToPushedItem < 1.0f)
            {
                if (DistanceToPushedItem - 0.3f < i * 4)
                {
                    Force = transform.rotation * Vector3.forward * PushForce * 1.4f;
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));
                    int clip = Random.Range(3, 6);
                    audioClip = audioCharacter.AudioClip[clip];
                    if(audioSource.isPlaying == false)
                    {
                        audioSource.PlayOneShot(audioClip);
                    }

                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Range.enabled = false;
        AnimL.SetInteger("PushPull", 0);
        AnimR.SetInteger("PushPull", 0);
        Character.AllProhibit = false;
        Character.MoveOnly = false;
        CameraRotate.cameratotate = true;
        Cursor.lockState = CursorLockMode.Locked;
        yield return new WaitForSeconds(1);
        physicMaterialBox.dynamicFriction = 2f;
        yield break;

    }

    public ArmRotate armRotate1, armRotate2;
    public ElectricDoorOpen ElectricDoorOpen;
    private IEnumerator DoorOpen1()
    {
        ElectricDoorOpen.enabled = true;
        yield return new WaitForSeconds(1f);
        AnimR.SetInteger("Rod", 0);
        yield return new WaitForSeconds(1.8f);
        armRotate1.enabled = true;
        armRotate2.enabled = true;
        Character.AllProhibit = false;
        Character.MoveOnly = true;
        Cursor.lockState = CursorLockMode.Locked;
        CameraRotate.cameratotate = true;
    }

    public GameObject chair;
    public StageRoutine stageRoutine;
    private IEnumerator ElectricityRecover()
    {
        chair.SetActive(true);
        stageRoutine.enabled = true;
        yield return new WaitForSeconds(1f);
        AnimR.SetInteger("Rod", 0);
        yield return new WaitForSeconds(1f);
        Character.AllProhibit = false;
        Character.MoveOnly = true;
        Cursor.lockState = CursorLockMode.Locked;
        CameraRotate.cameratotate = true;
    }

    public ScanScreen Screen;
    private IEnumerator GateOpenFunction(bool ScanResult)
    {
        yield return new WaitForSeconds(0.5f);
        Screen.Scanning = true;
        Screen.ScanResult = ScanResult;
        yield return new WaitForSeconds(1f);
        AnimL.SetInteger("Scan", 0);
        yield return new WaitForSeconds(1.8f);
        Interacted_Item = null;
        Character.AllProhibit = false;
        Character.MoveOnly = true;
        Cursor.lockState = CursorLockMode.Locked;
        CameraRotate.cameratotate = true;
        yield break;
    }

    public MoveToTarget moveToTarget;
    private IEnumerator SitDown()
    {
        yield return new WaitForSeconds(0.5f);
        moveToTarget.enabled = true;

    }



    private IEnumerator PushLookAngleAdjust()
    {
        Vector3 direction = PushedItem.transform.position - transform.position;
        Vector3 normalizedDirection = direction.normalized;
        float elevationAngle = Mathf.Atan2(normalizedDirection.y, Mathf.Sqrt(normalizedDirection.x * normalizedDirection.x + normalizedDirection.z * normalizedDirection.z)) * Mathf.Rad2Deg;
        Vector3 TargetEulerAngle =  new Vector3(0-elevationAngle-5, cm1.transform.localEulerAngles.y, cm1.transform.localEulerAngles.z);
        float Cm1AngleX = cm1.transform.localEulerAngles.x;
        if(Cm1AngleX > 180)
        {
            Cm1AngleX -= 360;
        }
        while (Mathf.Abs(Cm1AngleX - TargetEulerAngle.x) > 5)
        {
            direction = PushedItem.transform.position - transform.position;
            normalizedDirection = direction.normalized;
            elevationAngle = Mathf.Atan2(normalizedDirection.y, Mathf.Sqrt(normalizedDirection.x * normalizedDirection.x + normalizedDirection.z * normalizedDirection.z)) * Mathf.Rad2Deg;
            TargetEulerAngle = new Vector3(0 - elevationAngle - 18, cm1.transform.localEulerAngles.y, cm1.transform.localEulerAngles.z);
            Cm1AngleX = cm1.transform.localEulerAngles.x;
            if (Cm1AngleX > 180)
            {
                Cm1AngleX -= 360;
            }
            var transposer = cm1.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>();
            transposer.m_FollowOffset.y += Mathf.Clamp((Cm1AngleX - TargetEulerAngle.x), -1, 1) * -0.0005f;
            yield return new WaitForSeconds(Time.deltaTime);
        }

    }

    private IEnumerator AudioPlay(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(audioClip);
    }

}
