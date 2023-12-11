using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GrabItem : MonoBehaviour
{
    public static bool ThrowItem = false;
    public Transform LookPoint;
    public CharacterController controller;
    public Animator Hand_Anim;
    public AudioCharacter audioCharacter;
    public Transform LeftHand;
    public string[] VIP_Item;
    private GameObject GrabbedItem, PushedItem;
    private Rigidbody GrabbedItemRb, PushedItemRb;
    private Collider GrabbedItemCd;
    private Vector3 triggerPoint;
    private string ItemName;
    private int ItemIndex;
    private Collider Interacted_Item;
    private Vector3 GrabbedItemScale;
    bool NoObstacle = true;
    BoxCollider Range;
    AudioSource audioSource;
    AudioClip audioClip;
    void Start()
    {
        ThrowItem = false;
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

    private void OnTriggerEnter(Collider other)
    {
            triggerPoint = other.bounds.ClosestPoint(transform.position);
        float Dis = Vector3.Distance(other.transform.position, transform.position);
        Vector3 Dir = other.transform.position - transform.position;
        Ray ray = new Ray(transform.position, Dir);
        RaycastHit hit;
        LayerMask layerMask = 1 << 6 | 1 << 12;
        if (Physics.Raycast(ray, out hit, 6, layerMask))
        {
            if (Vector3.Distance(hit.point, transform.position) > Vector3.Distance(triggerPoint, transform.position))
            {
                if (other.tag != "PostProcess")
                    NoObstacle = true;
                if (other.name == "Cake")
                {
                    Debug.Log(hit.transform.gameObject.name);
                    Debug.Log(Vector3.Distance(hit.point, transform.position));
                    Debug.Log(Vector3.Distance(triggerPoint, transform.position));
                }
            }
            else
            {
                if(other.name == "Cake")
                {
                    Debug.Log(hit.transform.gameObject.name);
                    Debug.Log(Vector3.Distance(hit.point, transform.position));
                    Debug.Log(Vector3.Distance(other.transform.position, transform.position));
                }
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
                        Character.AllProhibit = true;
                        Character.MoveOnly = true;
                        CameraRotate.cameratotate = false;
                        StartCoroutine(PushObject(triggerPoint));
                        StartCoroutine(PushLookAngleAdjust());
                    }
                    else if (other.tag == "PushOnly" && Range.size.x < 2f && Dis < 2f)
                    {
                        Hand_Anim.SetLayerWeight(4, 0.8f);
                        Hand_Anim.SetInteger("PushPull", 1);
                        Range.enabled = false;
                        PushedItem = other.gameObject;
                        PushedItemRb = PushedItem.GetComponent<Rigidbody>();
                        Character.AllProhibit = true;
                        Character.MoveOnly = true;
                        CameraRotate.cameratotate = false;
                        StartCoroutine(PushObject2());
                    }
                    else if (other.gameObject.name == "Board" && controller.isGrounded == true&& Character.SquatState == 0)
                    {
                        float PlayerToRod_Y = Mathf.Abs(transform.position.y - other.transform.position.y);
                        dis_to_target = Vector3.Distance(transform.position, other.transform.position);

                        if (dis_to_target < 1f && PlayerToRod_Y < 0.4f && dis_to_target > 0.1f)
                        {

                            StartCoroutine(EventActive(other.gameObject,0,"Rod",1f,0.5f,true,true,true));
                        }
                        Range.enabled = false;
                    }
                    else if (other.gameObject.name == "Chair")
                    {
                        other.GetComponent<BoxCollider>().enabled = false;
                        other.GetComponent<EventActive>().Active = true;
                    }
                    else if (other.gameObject.name == "ElevatorButton")
                    {
                        float PlayerToTarget_Y = Mathf.Abs(transform.position.y - other.transform.position.y);
                        float PlayerToTarget = Vector3.Distance(transform.position, other.transform.position);
                        if (PlayerToTarget < 1.2f && PlayerToTarget_Y < 0.8f && PlayerToTarget > 0.5f)
                        {
                            StartCoroutine(EventActive(other.gameObject,1.5f,"Press", 1.5f, 0.5f, true,true,true));

                        }
                    }
                }

            }
        }
        else if (NoObstacle == true)
        {
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
                                    dis_to_target = Vector3.Distance(transform.position, Interacted_Item.transform.position);
                                    if (dis_to_target < 1f && PlayerToScan_Y < 0.8f && dis_to_target > 0.3f)
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
                        dis_to_target = Vector3.Distance(transform.position, Interacted_Item.transform.position);
                        if (dis_to_target < 1f && PlayerToRod_Y < 0.8f && dis_to_target > 0.3f)
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
                                    dis_to_target = Vector3.Distance(transform.position, Interacted_Item.transform.position);
                                    if (dis_to_target < 4f && PlayerToScan_Y < 0.8f && dis_to_target > 1f)
                                    {
                                        GrabbedItemCd.enabled = false;
                                        Range.enabled = false;
                                        StartCoroutine(ElectricityRecover(Interacted_Item.gameObject));
                                    }
                                    break;
                                }
                            default:
                                break;
                        }
                        break;
                    }
                case 5:
                    {
                        switch (Interacted_Item.gameObject.GetComponent<InteractiveObject>().index)
                        {
                            case 5:
                                {
                                    StartCoroutine(EventActive(Interacted_Item.gameObject, 1.4f, "Scan", 1.7f, 0.5f, false, false, false));
                                    break;
                                }
                            default:
                                break;
                        }
                        break;
                    }
                case 6:
                    {
                        switch (Interacted_Item.gameObject.GetComponent<InteractiveObject>().index)
                        {
                            case 6:
                                {
                                    StartCoroutine(EventActive(Interacted_Item.gameObject, 1.4f, "Scan", 1.7f, 0.5f, false, false, false));
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
    public GameObject LeftClick;
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
                        LeftClick.SetActive(true);      
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
                    if (!grabbleItem.put)
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
    public AudioSource throw_item;
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
            Range.enabled = false;
            ThrowItem = false;
            Physics.IgnoreLayerCollision(7, 10);
            ItemName = null;
            ItemIndex = 0;
            Character.AllProhibit = true;
            GrabbedItemRb.useGravity = false;
            GrabAction = true;
            HandAnimeLayer(true);
            yield return new WaitForSeconds(0.05f);
            if (GrabbedItemCd != null)
            {
                GrabbedItem.GetComponent<Collider>().isTrigger = false;
            }
            GrabbedItemRb.isKinematic = false;
            if (grabbleItem.put)
            {
                if (GrabbedItemRb.gameObject.transform.parent != null)
                    GrabbedItem.transform.SetParent(null);
                GrabbedItem.transform.localScale = GrabbedItemScale;
                GrabbedItemRb.useGravity = true;
                Character.AllProhibit = false;
                GrabbedItem = null;
                Hand_Anim.SetInteger("HandState", 0);
                HandAnimeLayer(false);
                Physics.IgnoreLayerCollision(7, 10, false);
                GrabbedItemCd = null;
                yield return new WaitForSeconds(1f);
                GrabAllow = true;
                yield break;
            }
            yield return new WaitForSeconds(0.4f);
            if (!grabbleItem.put)
                throw_item.Play();
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
            GrabbedItemCd = null;
            yield return new WaitForSeconds(1f);
            GrabAllow = true;
        }
        yield return new WaitForSeconds(Time.deltaTime);
    }

    public AudioSource pickup;
    private GrabbleItem grabbleItem;
    private Vector3 GrabOffset = new Vector3(0.2f, -0.01f, 0.1f);
    private Vector3 AngleOffset = new Vector3(0, 90, 180);
    private IEnumerator Grab()
    {
        AngleOffset = grabbleItem.Angle;
        GrabOffset = grabbleItem.Offset;
        yield return new WaitForSeconds(0.2f);
        pickup.Play();
        yield return new WaitForSeconds(0.12f);
        if (GrabbedItemCd != null)
        {
            GrabbedItem.GetComponent<Collider>().isTrigger = true;
        }
        GrabbedItemRb.isKinematic = true;
        yield return new WaitForSeconds(Time.deltaTime);
        GrabbedItem.transform.SetParent(LeftHand,true);
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
    private float dis_to_target;
    public float PushForce = 10f;
    private IEnumerator PushObject(Vector3 trigger_point)
    {
        Character.push = true;
        Vector3 Force;
        float dis_point_center = Distance2D(trigger_point, PushedItem.transform.position);
        dis_to_target = Distance2D(transform.position, PushedItem.transform.position) - dis_point_center;
        for (float i = 0; i <= 0.2f; i += Time.deltaTime)
        {
            if(dis_to_target < 1.0f)
            {
                if (dis_to_target - 0.3f < i * 4)
                {
                    Force = transform.rotation * Vector3.forward * PushForce * 0.8f;
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));
                }
            }
            yield return null;
        }
        while (true)
        {
            
            dis_to_target = Distance2D(transform.position, PushedItem.transform.position) - dis_point_center;
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            float amount = new Vector2(h, v).magnitude;
            Vector3 move_dir = character.rotation * new Vector3(h, 0, v); ;
            Vector3 player_dir = character.rotation * Vector3.forward;
            Quaternion rotation_player = transform.rotation;
            if (Character.LookState == 3)
            {
                move_dir = new Vector3(-h, 0, -v);
            }

            float angle = Vector3.Angle(player_dir, move_dir);
            if (Mathf.Abs(angle-90) < 60 || Input.GetKey(KeyCode.F)|| dis_to_target > 2.5f||Character.NoEnergy == true)
            {
                Range.enabled = false;
                StartCoroutine(PushAnimFix1());             
                Hand_Anim.SetInteger("PushPull", 0);
                CharacterState(false);
                Character.push = false;
                Cursor.lockState = CursorLockMode.Locked;
                Character.speed = OriginSpeed;
                Character.EnergyUse = false;
                Character.push = false;
                yield return new WaitForSeconds(0.2f);
               // physicMaterialBox.dynamicFriction = 2f;
                yield break;
            }
            else if (Mathf.Abs(angle) <= 30&&amount!=0)
            {
                Hand_Anim.SetLayerWeight(4, 0.8f);
                Hand_Anim.SetInteger("PushPull", 1);

                if (dis_to_target < 1f)
                {
                    Character.speed = 0.5f+dis_to_target;
                    Force = rotation_player * Vector3.forward * PushForce * (1 + (-dis_to_target))*1.4f;   
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));
                }
                if (dis_to_target > 2.5f)
                {
                    StartCoroutine(PushAnimFix1());
                    Range.enabled = false;
                    Hand_Anim.SetInteger("PushPull", 0);
                    Character.push = false;
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
            else if (Mathf.Abs(angle-180) <= 30 && amount != 0)
            {
                Hand_Anim.SetLayerWeight(4, 0.8f);
                Hand_Anim.SetInteger("PushPull", 2);
                dis_to_target = Distance2D(transform.position, PushedItem.transform.position) - dis_point_center;
                Vector3 vel = PushedItemRb.velocity;
                if (dis_to_target >= 0.4f)
                {
                    Character.speed = 1;
                    Force = rotation_player * Vector3.back * PushForce * (1 + (dis_to_target - 0.05f) * 1f);
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));

                }
                else if (dis_to_target < 0.4f)
                {
                    Character.speed = 1 +1f - dis_to_target;
                }
                if (dis_to_target > 1f)
                {
                    Range.enabled = false;
                    StartCoroutine(PushAnimFix1());
                    Hand_Anim.SetInteger("PushPull", 0);
                    Character.push = false;
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
            else
            {
                Character.EnergyUse = false;
            }
            yield return null;
            
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
        dis_to_target = Distance2D(transform.position , PushedItem.transform.position);
        for (float i = 0; i <= 0.25f; i += Time.deltaTime)
        {
            if (dis_to_target < 1.0f)
            {
                if (dis_to_target - 0.3f < i * 4)
                {
                    Force = transform.rotation  * Vector3.forward * PushForce * 10f;
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));
                    PushTypeInfo.pushType type = PushedItem.gameObject.GetComponent<PushType>().pushType;
                    audioClip = PushTypeInfo.GetAudioClip(type);
                    if(audioSource.isPlaying == false)
                    {
                        audioSource.PlayOneShot(audioClip);
                    }

                }
            }
            yield return null;
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
    private IEnumerator ElectricityRecover(GameObject target)
    {
        StartCoroutine(DelayActive(target, 0, false));
        while (!target.GetComponent<EventActive>().ContinuePlayerAction)
        {
            yield return null;
        }
        Hand_Anim.SetLayerWeight(2, 1);
        Hand_Anim.SetInteger("Unlock", 1);
        yield return new WaitForSeconds(1.1f);
        CameraRotate.cameratotate = true;
        if (GrabbedItemRb.gameObject.transform.parent != null)
            GrabbedItem.transform.SetParent(null);
        GrabbedItem.transform.localScale = GrabbedItemScale;
        GrabbedItemRb.useGravity = false;
        yield return new WaitForSeconds(0.56f);
        ThrowItem = false;
        GrabAllow = true;
        Hand_Anim.SetInteger("HandState", 0);
        Hand_Anim.SetLayerWeight(2, 0);
        Hand_Anim.SetInteger("Unlock", 0);       
        HandAnimeLayer(false);
        yield return new WaitForSeconds(1f);
        Character.ActionProhibit = false;
        Character.AllProhibit = false;
        Cursor.lockState = CursorLockMode.Locked;

        GrabbedItem = null;
    }
    private IEnumerator EventActive(GameObject target,float delayActive, string HandAnime, float WaitTime1, float WaitTime2,bool ColliderClose,bool RotateBody,bool ThrowAway)
    {
        StartCoroutine(DelayActive(target, delayActive,false));
        Range.enabled = false;
        if (ColliderClose)
            target.GetComponent<Collider>().enabled = false;
        if (RotateBody)
            target.GetComponent<RotateAdjust>().adjust();
        CharacterState(true);
        Hand_Anim.SetLayerWeight(2, 1);
        Hand_Anim.SetInteger(HandAnime, 1);
        yield return new WaitForSeconds(WaitTime1);
        Hand_Anim.SetLayerWeight(2, 0);
        Hand_Anim.SetInteger(HandAnime, 0);
        yield return new WaitForSeconds(WaitTime2);
        CharacterState(false);
        Cursor.lockState = CursorLockMode.Locked;
        if (ThrowAway)
            GrabbedItem = null;
    }
    private IEnumerator DelayActive(GameObject target,float Delay,bool player_stop)
    {
        if (player_stop)
            target.GetComponent<EventActive>().ContinuePlayerAction = false;
        yield return new WaitForSeconds(Delay);
        target.GetComponent<EventActive>().Active = true;
    }
    private void CharacterState(bool Stop)
    {
        Character.AllProhibit = Stop;
        Character.ActionProhibit = Stop;
        CameraRotate.cameratotate = !Stop;
    }
    private IEnumerator GateOpenFunction(bool ScanResult)
    {
        yield return new WaitForSeconds(0.5f);
        ScanScreen.Scanning = true;
        ScanScreen.ScanResult = ScanResult;
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
    private void ReloadScene()
    {
        flash.enabled = FlashGet;
        flash2.enabled = FlashGet;
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
    public float Distance2D(Vector3 a, Vector3 b)
    {
        a.y = 0;
        b.y = 0;
        return Vector3.Distance(a, b);
    }
}
