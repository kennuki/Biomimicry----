using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class GrabItem : MonoBehaviour
{
    public Transform LookPoint;
    public CharacterController controller;
    public Animator Hand_Anim;
    BoxCollider Range;
    public AudioCharacter audioCharacter;
    AudioSource audioSource;
    AudioClip audioClip;
    public static bool ThrowItem = false;
    void Start()
    {
        if(stageRoutine == null)
        {
           stageRoutine = GameObject.Find("LightGroup").transform.Find("1-5(Stage)").transform.Find("Audience Seat").transform.Find("Ceiling Light").GetComponent<StageRoutine>();
        }
        audioSource = audioCharacter.AudioSources[0];
        Range = GetComponent<BoxCollider>();
        StartCoroutine(GrabThrowfunction());
        ReloadScene();
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
    private void HandAnimeLayer(bool OnOff)
    {
        if(OnOff == true)
        {
            Hand_Anim.SetLayerWeight(2, 1 - Hand_Anim.GetLayerWeight(1));
            Hand_Anim.SetLayerWeight(3, Hand_Anim.GetLayerWeight(1));
        }
        else
        {
            Hand_Anim.SetLayerWeight(2, 0);
            Hand_Anim.SetLayerWeight(3, 0);
        }
    }

    public Transform LeftHand;
    private GameObject GrabbedItem, PushedItem;
    private Rigidbody GrabbedItemRb, PushedItemRb;
    private Collider GrabbedItemCd;
    private PhysicMaterial physicMaterialBox;
    private string ItemName;
    private int ItemIndex;
    public string[] VIP_Item;
    private Collider Interacted_Item;
    private Vector3 GrabbedItemScale;
    bool NoObstacle = true;
    private void OnTriggerEnter(Collider other)
    {

        float Dis = Vector3.Distance(other.transform.position, transform.position);
        Vector3 Dir = other.transform.position - transform.position;
        Ray ray = new Ray(transform.position, Dir);
        RaycastHit hit;
        LayerMask layerMask = 1 << 6 | 1 << 12;
        if (Physics.Raycast(ray, out hit, 6, layerMask))
        {
            if (Vector3.Distance(hit.point, transform.position) > Vector3.Distance(other.transform.position, transform.position))
            {
                if (other.tag != "PostProcess")
                    NoObstacle = true;

            }
            else
            {
                NoObstacle = false;
            }
        }
        else
            NoObstacle = true;
        if (ThrowItem == false)
        {

            if (other.tag == "BrushOff" && GrabAllow == true)
            {
                if(Vector3.Distance(other.transform.position, transform.position) < 1.4f)
                {
                    Range.enabled = false;
                    Character.AllProhibit = true;
                    GrabbedItem = other.gameObject;
                    HandAnimeLayer(true);
                    GrabbedItemRb = GrabbedItem.GetComponent<Rigidbody>();
                    GrabAction = true;
                    StartCoroutine(BrushAway());
                }

            }
            if (NoObstacle == true)
            {
                if (other.tag == "Grabbable" && GrabAllow == true)
                {
                    grabbleItem = other.GetComponent<GrabbleItem>();
                    Range.enabled = false;
                    Character.AllProhibit = true;
                    GrabbedItem = other.gameObject;
                    GrabbedItemScale = GrabbedItem.transform.localScale;
                    HandAnimeLayer(true);
                    GrabbedItemRb = GrabbedItem.GetComponent<Rigidbody>();
                    GrabbedItemCd = GrabbedItem.GetComponent<Collider>();
                    GrabAction = true;
                    ThrowItem = true;
                    StartCoroutine(Grab());
                    foreach (string name in VIP_Item)
                    {
                        if (other.name == name)
                        {
                            ItemName = other.gameObject.name;
                            ItemIndex = other.gameObject.GetComponent<InteractiveObject>().index;
                            StartCoroutine(ChipGetFunction());
                        }
                    }
                }
                if (Character.SquatState == 0 && GrabbedItem == null)
                {
                    if (other.tag == "Pushable" && Range.size.x < 2f)
                    {
                        Hand_Anim.SetLayerWeight(4, 0.8f);
                        Hand_Anim.SetInteger("PushPull", 1);
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
                        //physicMaterialBox.dynamicFriction = 0.1f;
                        StartCoroutine(PushObject());
                        StartCoroutine(PushLookAngleAdjust());
                    }
                    else if (other.tag == "PushOnly" && Range.size.x < 2f && Dis < 1f)
                    {
                        Hand_Anim.SetLayerWeight(4, 0.8f);
                        Hand_Anim.SetInteger("PushPull", 1);
                        Range.enabled = false;
                        PushedItem = other.gameObject;
                        PushedItemRb = PushedItem.GetComponent<Rigidbody>();
                        physicMaterialBox = PushedItem.GetComponent<Collider>().material;
                        Character.AllProhibit = true;
                        Character.MoveOnly = true;
                        CameraRotate.cameratotate = false;
                        //physicMaterialBox.dynamicFriction = 0.4f;
                        StartCoroutine(PushObject2());
                    }
                    else if (other.gameObject.name == "Board" && controller.isGrounded == true&& Character.SquatState == 0)
                    {
                        float PlayerToRod_Y = Mathf.Abs(transform.position.y - other.transform.position.y);
                        DistanceToPushedItem = Vector3.Distance(transform.position, other.transform.position);

                        if (DistanceToPushedItem < 1f && PlayerToRod_Y < 0.4f && DistanceToPushedItem > 0.1f)
                        {

                            StartCoroutine(EventActive(other.gameObject,0,"Rod",1f,0.5f,true,true));
                        }
                        Range.enabled = false;
                    }
                    else if (other.gameObject.name == "Chair")
                    {
                        other.GetComponent<BoxCollider>().enabled = false;
                        StartCoroutine(SitDown());
                    }
                    else if (other.gameObject.name == "ElevatorButton")
                    {
                        float PlayerToTarget_Y = Mathf.Abs(transform.position.y - other.transform.position.y);
                        float PlayerToTarget = Vector3.Distance(transform.position, other.transform.position);
                        if (PlayerToTarget < 1.2f && PlayerToTarget_Y < 0.8f && PlayerToTarget > 0.5f)
                        {
                            StartCoroutine(EventActive(other.gameObject,1.5f,"Press", 1.5f, 0.5f, true,true));

                        }
                    }
                }

            }
        }
        else if (NoObstacle == true)
        {
            Debug.Log(NoObstacle);
            if (other.tag == "Interacted" && Range.size.y<4 && controller.isGrounded == true)
            {
                Interacted_Item = other;
                InteractFunction();
                Range.enabled = false;
            }
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
                                    if (DistanceToPushedItem < 1f && PlayerToScan_Y < 0.8f && DistanceToPushedItem > 0.3f)
                                    {
                                        Hand_Anim.SetLayerWeight(2, 1);
                                        Hand_Anim.SetInteger("Scan", 1);
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
                        if (DistanceToPushedItem < 1f && PlayerToRod_Y < 0.8f && DistanceToPushedItem > 0.3f)
                        {
                            Hand_Anim.SetLayerWeight(2, 1);
                            Hand_Anim.SetInteger("Scan", 1);
                            Range.enabled = false;
                            CameraRotate.cameratotate = false;
                            Character.AllProhibit = true;
                            Character.MoveOnly = false;
                            StartCoroutine(GateOpenFunction(false));
                        }
                        break;
                    }
                case 4:
                    {
                        switch (Interacted_Item.gameObject.GetComponent<InteractiveObject>().index)
                        {
                            case 4:
                                {
                                    
                                    float PlayerToScan_Y = Mathf.Abs(transform.position.y - Interacted_Item.transform.position.y);
                                    DistanceToPushedItem = Vector3.Distance(transform.position, Interacted_Item.transform.position);
                                    if (DistanceToPushedItem < 4f && PlayerToScan_Y < 0.8f && DistanceToPushedItem > 1f)
                                    {
                                        Range.enabled = false;
                                        CameraRotate.cameratotate = false;
                                        Character.AllProhibit = true;
                                        Character.ActionProhibit = true;
                                        //Character.MoveOnly = false;
                                        StartCoroutine(ElectricityRecover());
                                    }
                                    break;
                                }
                            default:
                                break;
                        }
                        break;
                    }
             
                default:
                    break;

            }
        }
    }

    public static bool FlashGet = false;
    public FlashLight flash,flash2;
    private IEnumerator ChipGetFunction()
    {
        while (true)
        {
            switch (ItemIndex)
            {
                case 3:
                    {
                        yield return new WaitForSeconds(0.32f);
                        FlashGet = true;
                        flash.enabled = FlashGet;
                        flash2.enabled = FlashGet;
                        ItemName = null;
                        yield return new WaitForSeconds(Time.deltaTime);
                        ThrowItem = false;
                        GrabAllow = true;
                        GrabbedItem.SetActive(false);
                        yield return new WaitForSeconds(0.22f);
                        Hand_Anim.SetInteger("HandState", 0);
                        Hand_Anim.SetLayerWeight(2, 0);
                        break;
                    }
                default:
                    break;

            }
            yield break;
        }

    }

    private bool GrabAllow = true;
    private bool GrabAction = false;
    private IEnumerator GrabThrowfunction()
    {
        while (true)
        {
            
            if (GrabAction == true)
            {
                if (Hand_Anim.GetInteger("HandState") == 0 && GrabAllow == true)
                {
                    GrabAction = false;
                    GrabAllow = false;
                    Hand_Anim.SetInteger("HandState", 1);
                }
                else if (Hand_Anim.GetInteger("HandState") == 2)
                {
                    Hand_Anim.SetInteger("HandState", 3);
                    GrabAction = false;
                    GrabAllow = true;
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    public GameObject cm1;
    bool IfTriggerDetect = false;
    private IEnumerator TriggerDetect()
    {
        NoObstacle = true;
        Range.size = new Vector3(0.1f, 0.25f, 0.1f);
        Range.enabled = true;
        transform.eulerAngles = new Vector3(cm1.transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z); 
        IfTriggerDetect = true;
        while (Range.size.z < 2f)
        {
            Range.size = Range.size + new Vector3(0, 0, Time.deltaTime * 50);
            Range.center = Range.center + new Vector3(0, 0, Time.deltaTime * 25f);        
            yield return new WaitForSeconds(Time.deltaTime);
        }
        while (Range.size.y < 6f)
        {
            Range.size = Range.size + new Vector3(0, Time.deltaTime * 75f, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        while (Range.size.x < 2.5f)
        {
            Range.size = Range.size + new Vector3(Time.deltaTime * 75f, 0, 0);
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


    public Transform character;
    public Vector3 ThrowForce = new Vector3(50, 80, 160);
    private IEnumerator Throw()
    {
        float ForceAdjust = cm1.transform.eulerAngles.x;
        //float ForceAdjust = 0;
        if (ForceAdjust > 180)
        {
            ForceAdjust -= 360;
        }


        ForceAdjust = ForceAdjust / 45 - 1;
        Vector3 AdjustForce = ThrowForce;
        AdjustForce.x = ThrowForce.x - ForceAdjust * 10;
        AdjustForce.z = ThrowForce.z + ForceAdjust*40;
        AdjustForce.y = ThrowForce.y - ForceAdjust * 35;
        if (GrabbedItem != null)
        {
            ThrowItem = false;
            HandAnimeLayer(true);
            Physics.IgnoreLayerCollision(7, 10);
            ItemName = null;
            ItemIndex = 0;
            Character.AllProhibit = true;
            GrabbedItemRb.useGravity = false;
            GrabAction = true;
            yield return new WaitForSeconds(0.05f);
            if (GrabbedItemCd != null)
            {
                GrabbedItem.GetComponent<Collider>().enabled = true;
            }
            GrabbedItemRb.isKinematic = false;
            yield return new WaitForSeconds(0.4f);
            if (GrabbedItemRb.gameObject.transform.parent != null)
                GrabbedItem.transform.SetParent(null);
            GrabbedItem.transform.localScale = GrabbedItemScale;
            GrabbedItemRb.AddForce(character.rotation*Quaternion.Euler(0,90,0) * AdjustForce *(GrabbedItemRb.mass+0.05f));
            GrabbedItemRb.useGravity = true;
            yield return new WaitForSeconds(0.25f);
            Character.AllProhibit = false;
            GrabbedItem = null;
            Hand_Anim.SetInteger("HandState", 0);
            HandAnimeLayer(false);
            yield return new WaitForSeconds(0.3f);
            Physics.IgnoreLayerCollision(7, 10, false);
            GrabAllow = true;
            GrabbedItemCd = null;
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
        if (GrabbedItemCd != null)
        {
            GrabbedItem.GetComponent<Collider>().enabled = false;
        }
        GrabbedItemRb.isKinematic = true;
        yield return new WaitForSeconds(Time.deltaTime);
        GrabbedItem.transform.SetParent(LeftHand);
        GrabbedItem.transform.position = LeftHand.position + LeftHand.transform.rotation * GrabOffset;
        GrabbedItem.transform.eulerAngles = LeftHand.eulerAngles;
        GrabbedItem.transform.Rotate(AngleOffset, Space.Self);
        yield return new WaitForSeconds(0.1f);
        GrabbedItem.transform.position = LeftHand.position + LeftHand.transform.rotation * GrabOffset;
        yield return new WaitForSeconds(0.1f);
        Character.AllProhibit = false;
        Hand_Anim.SetInteger("HandState", 2);
        HandAnimeLayer(false);
        switch (ItemIndex)
        {
            case 3:
                {
                    ItemIndex = 0;
                    Hand_Anim.SetInteger("HandState", 0);
                    GrabbedItem = null;
                    break;
                }
            default:
                break;

        }
    }
    private IEnumerator BrushAway()
    {      
        yield return new WaitForSeconds(0.1f);
        Character.AllProhibit = false;
        GrabbedItemRb.AddForce(character.rotation * Vector3.forward*10);
        yield return null;
        GrabbedItemRb.AddForce(character.rotation * Vector3.forward * 10);
        yield return new WaitForSeconds(0.4f);
        ThrowItem = false;
        Hand_Anim.SetInteger("HandState", 0);
        HandAnimeLayer(false);
        GrabbedItem = null;
        yield return new WaitForSeconds(0.1f);
        GrabAllow = true;
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
                    Force = transform.rotation * Vector3.forward * PushForce * 1f;
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
                StartCoroutine(PushAnimFix1());             
                Hand_Anim.SetInteger("PushPull", 0);
                CharacterState(false);
                Cursor.lockState = CursorLockMode.Locked;
                Character.speed = OriginSpeed;
                Character.EnergyUse = false;
                yield return new WaitForSeconds(0.2f);
               // physicMaterialBox.dynamicFriction = 2f;
                yield break;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                Hand_Anim.SetLayerWeight(4, 0.8f);
                Hand_Anim.SetInteger("PushPull", 1);

                if (DistanceToPushedItem >= 0.9f)
                {
                    Character.speed = 1+DistanceToPushedItem-0.85f;
                    Force = transform.rotation * Vector3.forward * PushForce * (1 + (0.85f - DistanceToPushedItem) * 8f) * 1.4f;
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));
                }
                else if (DistanceToPushedItem < 0.85f)
                {
                    Character.speed = Mathf.Clamp(DistanceToPushedItem- 085f, 0, 1);
                    Force = transform.rotation * Vector3.forward * PushForce * (1 + (0.85f - DistanceToPushedItem))*1.4f;   
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));
                }
                else
                {
                    Character.speed = 1f;
                    Force = transform.rotation * Vector3.forward * PushForce * (1 + (0.85f - DistanceToPushedItem)*5f) * 1.4f;
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));
                }
                if (DistanceToPushedItem > 1.6f)
                {
                    StartCoroutine(PushAnimFix1());
                    Range.enabled = false;
                    Hand_Anim.SetInteger("PushPull", 0);
                    Character.AllProhibit = false;
                    Character.MoveOnly = false;
                    Character.EnergyUse = false;
                    Character.speed = OriginSpeed;
                    CameraRotate.cameratotate = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    //physicMaterialBox.dynamicFriction = 2f;
                    yield break;
                }
                Character.EnergyUse = true;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                Hand_Anim.SetLayerWeight(4, 0.8f);
                Hand_Anim.SetInteger("PushPull", 2);
                DistanceToPushedItem = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(PushedItem.transform.position.x, PushedItem.transform.position.z));
                float maxSpeed = 1;
                Vector3 vel = PushedItemRb.velocity;
                if (DistanceToPushedItem > 0.5f)
                {
                    Character.speed = 1;
                    Force = transform.rotation * Vector3.back * PushForce * (1 + (DistanceToPushedItem-0.5f) * 5f);
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));

                }
                else if (DistanceToPushedItem < 0.5f)
                {
                    Character.speed = 1 +1f - DistanceToPushedItem;
                }
                if (DistanceToPushedItem > 1.2f)
                {
                    Range.enabled = false;
                    StartCoroutine(PushAnimFix1());
                    Hand_Anim.SetInteger("PushPull", 0);
                    Character.AllProhibit = false;
                    Character.MoveOnly = false;
                    Character.EnergyUse = false;
                    Character.speed = OriginSpeed;
                    CameraRotate.cameratotate = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    //physicMaterialBox.dynamicFriction = 2f;
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

    private IEnumerator PushAnimFix1()
    {
        float counter = 0;
        while (counter<=0.8f)
        {
            Hand_Anim.SetLayerWeight(4, 0.8f-counter);
            counter += Time.deltaTime * 2;
            if (Hand_Anim.GetInteger("PushPull")>0&& counter>0.2f)
            {
                Hand_Anim.SetLayerWeight(4, 0.8f);
                yield break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
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
                    Force = transform.rotation  * Vector3.forward * PushForce * 28f;
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
        Hand_Anim.SetInteger("PushPull", 0);
        StartCoroutine(PushAnimFix1());
        Character.AllProhibit = false;
        Character.MoveOnly = false;
        CameraRotate.cameratotate = true;
        Cursor.lockState = CursorLockMode.Locked;
        yield return new WaitForSeconds(1);
        //physicMaterialBox.dynamicFriction = 2f;
        yield break;

    }

    public ArmRotate armRotate1, armRotate2;
    public ElectricDoorOpen ElectricDoorOpen;
    private IEnumerator DoorOpen1()
    {
        ElectricDoorOpen.enabled = true;
        yield return new WaitForSeconds(1f);
        Hand_Anim.SetInteger("Rod", 0);
        Hand_Anim.SetLayerWeight(2, 0);
        yield return new WaitForSeconds(1.8f);
        armRotate1.enabled = true;
        armRotate2.enabled = true;
        Character.AllProhibit = false;
        Character.MoveOnly = true;
        Cursor.lockState = CursorLockMode.Locked;
        CameraRotate.cameratotate = true;
        GrabbedItem = null;
    }


    public GameObject chair;
    private StageRoutine stageRoutine;
    public PosRotAdjust adjust;
    public static bool ElectricRecover = false;
    private IEnumerator ElectricityRecover()
    {
        adjust.enabled = true;
        while(adjust.arrive == false)
        {
            yield return null;
        }
        Hand_Anim.SetLayerWeight(2, 1);
        Hand_Anim.SetInteger("Unlock", 1);
        ElectricRecover = true;
        chair.SetActive(true);
        stageRoutine.enabled = true;
        yield return new WaitForSeconds(1.1f);
        CameraRotate.cameratotate = true;
        if (GrabbedItemRb.gameObject.transform.parent != null)
            GrabbedItem.transform.SetParent(null);
        GrabbedItem.transform.localScale = GrabbedItemScale;
        GrabbedItemRb.useGravity = false;
        yield return new WaitForSeconds(0.56f);
        Hand_Anim.SetLayerWeight(2, 0);
        Hand_Anim.SetInteger("Unlock", 0);       
        ThrowItem = false;
        GrabAllow = true;
        Hand_Anim.SetInteger("HandState", 0);
        HandAnimeLayer(false);
        yield return new WaitForSeconds(1f);
        Character.ActionProhibit = false;
        Character.AllProhibit = false;
        Cursor.lockState = CursorLockMode.Locked;

        GrabbedItem = null;
    }


    private IEnumerator EventActive(GameObject target,float delayActive, string HandAnime, float WaitTime1, float WaitTime2,bool ColliderClose,bool RotateBody)
    {
        StartCoroutine(DelayActive(target, delayActive));
        if (ColliderClose)
            target.GetComponent<Collider>().enabled = false;
        if (RotateBody)
            target.GetComponent<RotateAdjust>().adjust();
        Range.enabled = false;
        CharacterState(true);
        Hand_Anim.SetLayerWeight(2, 1);
        Hand_Anim.SetInteger(HandAnime, 1);
        yield return new WaitForSeconds(WaitTime1);
        Hand_Anim.SetLayerWeight(2, 0);
        Hand_Anim.SetInteger(HandAnime, 0);
        yield return new WaitForSeconds(WaitTime2);
        CharacterState(false);
        Cursor.lockState = CursorLockMode.Locked;
        GrabbedItem = null;
    }

    private IEnumerator DelayActive(GameObject target,float Delay)
    {
        yield return new WaitForSeconds(Delay);
        target.GetComponent<EventActive>().Active = true;
    }

    private void CharacterState(bool Stop)
    {
        Character.AllProhibit = Stop;
        Character.ActionProhibit = Stop;
        CameraRotate.cameratotate = !Stop;
    }


    public ScanScreen Screen;
    private IEnumerator GateOpenFunction(bool ScanResult)
    {
        yield return new WaitForSeconds(0.5f);
        Screen.Scanning = true;
        Screen.ScanResult = ScanResult;
        yield return new WaitForSeconds(1f);
        Hand_Anim.SetInteger("Scan", 0);
        Hand_Anim.SetLayerWeight(2, 0);
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

    private void ReloadScene()
    {
        flash.enabled = FlashGet;
        flash2.enabled = FlashGet;
        chair.SetActive(ElectricRecover);



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
