using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItem : MonoBehaviour
{
    public Transform LookPoint;
    public CharacterController controller;
    public Animator AnimL,AnimR;
    BoxCollider Range;
    public static bool ThrowItem = false;
    void Start()
    {
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
    private void OnTriggerEnter(Collider other)
    {
        if(ThrowItem == false)
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
            else if (other.tag == "Pushable" && Range.size.x < 0.4f)
            {
                AnimL.SetInteger("PushPull", 1);
                AnimR.SetInteger("PushPull", 1);
                Range.enabled = false;
                PushedItem = other.gameObject;
                PushedItemRb = PushedItem.GetComponent<Rigidbody>();
                physicMaterialBox = PushedItem.GetComponentInChildren<MeshCollider>().material;
                Character.AllProhibit = true;
                Character.MoveOnly = true;
                CameraRotate.cameratotate = false;
                physicMaterialBox.dynamicFriction = 0.4f;
                StartCoroutine(PushObject());
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
            else if (other.tag == "Rod" && Range.size.y < 0.5f && controller.isGrounded == true)
            {
                PushedItem = other.gameObject;
                float PlayerToRod_Y = Mathf.Abs(transform.position.y - PushedItem.transform.position.y);
                DistanceToPushedItem = Vector3.Distance(transform.position, PushedItem.transform.position);
                //Debug.Log(DistanceToPushedItem +" "+ PlayerToRod_Y);
                if (DistanceToPushedItem < 1f && PlayerToRod_Y < 0.4f && DistanceToPushedItem > 0.6f)
                {
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
            else if (other.tag == "Obstacle")
            {
                Range.enabled = false;
            }
        }
        
        else if (other.tag == "Interacted")
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
                                    float PlayerToRod_Y = Mathf.Abs(transform.position.y - PushedItem.transform.position.y);
                                    DistanceToPushedItem = Vector3.Distance(transform.position, PushedItem.transform.position);
                                    //Debug.Log(DistanceToPushedItem +" "+ PlayerToRod_Y);
                                    if (DistanceToPushedItem < 1f && PlayerToRod_Y < 0.4f && DistanceToPushedItem > 0.6f)
                                    {
                                        AnimL.SetInteger("Card", 1);
                                        Range.enabled = false;
                                        CameraRotate.cameratotate = false;
                                        Character.AllProhibit = true;
                                        Character.MoveOnly = false;
                                        GrabbedItem = Interacted_Item.gameObject;
                                        GrabbedItem.GetComponent<BoxCollider>().enabled = false;
                                        StartCoroutine(GateOpenFunction());
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
        Range.size = new Vector3(0.1f, 0.25f, 0.1f);
        Range.enabled = true;
        transform.eulerAngles = new Vector3(cm1.transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z); 
        IfTriggerDetect = true;
        while (Range.size.z < 3f)
        {
            Range.size = Range.size + new Vector3(0, 0, Time.deltaTime * 150);
            Range.center = Range.center + new Vector3(0, 0, Time.deltaTime * 75);        
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

    public Vector3 ThrowForce = new Vector3(60, 100, 200);
    private IEnumerator Throw()
    {
        float ForceAdjust = LookPoint.eulerAngles.x;
        if (ForceAdjust > 180)
        {
            ForceAdjust -= 360;
        }

        ForceAdjust = ForceAdjust / -45 + 1;
        Vector3 AdjustForce = ThrowForce;
        AdjustForce.x = ThrowForce.x / ForceAdjust;
        AdjustForce.y = ThrowForce.y - ForceAdjust * 20;

        if (GrabbedItem != null)
        {
            ItemName = null;
            ItemIndex = 0;
            Character.AllProhibit = true;
            GrabAllow = true;
            yield return new WaitForSeconds(0.05f);
            GrabbedItemRb.isKinematic = false;
            yield return new WaitForSeconds(0.3f);
            if (GrabbedItemRb.gameObject.transform.parent != null)
                GrabbedItemRb.AddForce(LookPoint.rotation * AdjustForce * ForceAdjust);
            GrabbedItem.transform.SetParent(null);
            yield return new WaitForSeconds(0.3f);
            Character.AllProhibit = false;
            yield return new WaitForSeconds(0.3f);
            ThrowItem = false;
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
        GrabbedItem.transform.SetParent(LeftHand);
        GrabbedItem.transform.position = LeftHand.position + transform.rotation * GrabOffset;
        GrabbedItem.transform.eulerAngles = LeftHand.eulerAngles;
        GrabbedItem.transform.Rotate(AngleOffset, Space.Self);
        yield return new WaitForSeconds(0.3f);
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
                    Force = transform.rotation * Vector3.forward * PushForce * 1.5f;
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

                if (DistanceToPushedItem > 1.1f)
                {
                    Character.speed = 1+DistanceToPushedItem-0.9f;
                }
                else if (DistanceToPushedItem < 1f)
                {
                    Character.speed = Mathf.Clamp(DistanceToPushedItem- 1.1f, 0, 1);
                    Force = transform.rotation * Vector3.forward * PushForce * (1 + (1.1f - DistanceToPushedItem)*1.1f)*1.3f;   
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));
                }
                else
                {
                    Character.speed = 1f;
                    Force = transform.rotation * Vector3.forward * PushForce * (1 + (1.1f - DistanceToPushedItem)) * 1.3f;
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));
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
                if (DistanceToPushedItem > 0.9f)
                {
                    Character.speed = 1;
                    Force = transform.rotation * Vector3.back * PushForce * (1 + (DistanceToPushedItem-0.9f) * 4f);
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));

                }
                else if (DistanceToPushedItem < 0.9f)
                {
                    Character.speed = 1 +1f - DistanceToPushedItem;
                }
                if (DistanceToPushedItem > 1.32f)
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
                if (PushedItemRb.velocity.magnitude > maxSpeed&& DistanceToPushedItem < 1f)
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
                    Force = transform.rotation * Vector3.forward * PushForce * 1f;
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));
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

    public ElectricDoorOpen ElectricDoorOpen;
    private IEnumerator DoorOpen1()
    {
        ElectricDoorOpen.enabled = true;
        yield return new WaitForSeconds(1f);
        AnimR.SetInteger("Rod", 0);
        yield return new WaitForSeconds(1.8f);
        Character.AllProhibit = false;
        Character.MoveOnly = true;
        Cursor.lockState = CursorLockMode.Locked;
        CameraRotate.cameratotate = true;
    }

    public GateOpen GateOpen;
    private IEnumerator GateOpenFunction()
    {
        yield break;
    }
}
