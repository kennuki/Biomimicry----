using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItem : MonoBehaviour
{
    public Animator AnimL,AnimR;
    BoxCollider Range;
    public static bool ThrowItem = false;
    void Start()
    {
        Range = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (IfTriggerDetect == false && Input.GetKeyDown(KeyCode.F) && ThrowItem == false && Character.AllProhibit == false && Character.GrabProhibit == false)
        {
            StartCoroutine(TriggerDetect());
        }
        else if (Input.GetKeyDown(KeyCode.F) && ThrowItem == true && Character.AllProhibit == false && Character.GrabProhibit == false)
        {
            StartCoroutine(Throw());
        }
    }

    public Transform LeftHand;
    private GameObject GrabbedItem, PushedItem;
    private Rigidbody GrabbedItemRb, PushedItemRb;
    public PhysicMaterial physicMaterialBox;
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Grabbable")
        {
            Range.enabled = false;
            Character.AllProhibit = true;
            GrabbedItem = other.gameObject;
            GrabbedItemRb = GrabbedItem.GetComponent<Rigidbody>();
            Character.GrabAllow = true;
            ThrowItem = true;
            StartCoroutine(Grab());

        }
        else if (other.tag == "Pushable" && Range.size.x < 0.4f)
        {
            AnimL.SetInteger("PushPull", 1);
            AnimR.SetInteger("PushPull", 1);
            Range.enabled = false;
            PushedItem = other.gameObject;
            PushedItemRb = PushedItem.GetComponent<Rigidbody>();
            Character.AllProhibit = true;
            Character.MoveOnly = true;
            CameraRotate.cameratotate = false;
            physicMaterialBox.dynamicFriction = 0.2f;
            InteractFix1.maxspeed = 1.5f;
            StartCoroutine(PushObject());
        }
        else if (other.tag == "PushOnly" && Range.size.x < 0.2f)
        {
            AnimL.SetInteger("PushPull", 1);
            AnimR.SetInteger("PushPull", 1);
            Range.enabled = false;
            PushedItem = other.gameObject;
            PushedItemRb = PushedItem.GetComponent<Rigidbody>();
            Character.AllProhibit = true;
            Character.MoveOnly = true;
            CameraRotate.cameratotate = false;
            physicMaterialBox.dynamicFriction = 0.2f;
            InteractFix1.maxspeed = 1.5f;
            StartCoroutine(PushObject2());
        }
        else if (other.tag == "Rod" && Range.size.y < 5f)
        {
            PushedItem = other.gameObject;
            DistanceToPushedItem = Vector3.Distance(transform.position, PushedItem.transform.position);
            Debug.Log(DistanceToPushedItem);
            if (DistanceToPushedItem < 1f)
            {
                Range.enabled = false;
                Character.AllProhibit = true;
                GrabbedItem = other.gameObject;
                GrabbedItem.GetComponent<BoxCollider>().enabled = false;
                StartCoroutine(DoorOpen1());
            }
            Range.enabled = false;
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
            Range.size = Range.size + new Vector3(0, 0, Time.deltaTime * 200);
            Range.center = Range.center + new Vector3(0, 0, Time.deltaTime * 100);        
            yield return new WaitForSeconds(Time.deltaTime);
        }
        while (Range.size.y < 8f)
        {
            Range.size = Range.size + new Vector3(0, Time.deltaTime * 200, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        while (Range.size.x < 3f)
        {
            Range.size = Range.size + new Vector3(Time.deltaTime * 200, 0, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Range.center = Vector3.zero;
        IfTriggerDetect = false;
        Range.enabled = false;
    }
    private IEnumerator Throw()
    {

        if (GrabbedItem != null)
        {
            Character.AllProhibit = true;
            ThrowItem = false;
            Character.GrabAllow = true;
            yield return new WaitForSeconds(0.05f);
            GrabbedItemRb.isKinematic = false;
            yield return new WaitForSeconds(0.35f);
            if (GrabbedItemRb.gameObject.transform.parent != null)
                GrabbedItemRb.AddForce(this.transform.rotation * new Vector3(60, 250, 190));
            GrabbedItem.transform.SetParent(null);
            yield return new WaitForSeconds(0.3f);
            Character.AllProhibit = false;
        }
        yield return new WaitForSeconds(Time.deltaTime);
    }
    public Vector3 GrabOffset = new Vector3(0.2f, -0.01f, 0.1f);
    private IEnumerator Grab()
    {
        yield return new WaitForSeconds(0.32f);
        GrabbedItemRb.isKinematic = true;
        GrabbedItem.transform.SetParent(LeftHand);
        GrabbedItem.transform.position = LeftHand.position + transform.rotation * GrabOffset;
        GrabbedItem.transform.eulerAngles = LeftHand.eulerAngles;
        GrabbedItem.transform.Rotate(new Vector3(0, 90, 180), Space.Self);
        yield return new WaitForSeconds(0.3f);
        Character.AllProhibit = false;
    }

    private float DistanceToPushedItem;
    public float PushForce = 10f;
    private IEnumerator PushObject()
    {
        Vector3 Force = Vector3.zero;
        DistanceToPushedItem = Vector3.Distance(transform.position, PushedItem.transform.position);
        for (float i = 0; i <= 0.25f; i += Time.deltaTime)
        {
            if(DistanceToPushedItem < 1.0f)
            {
                Force = transform.rotation * Vector3.forward * PushForce * 1.2f;
                PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        while (true)
        {
            DistanceToPushedItem = Vector3.Distance(transform.position, PushedItem.transform.position);
            Debug.Log(DistanceToPushedItem);
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.F)|| DistanceToPushedItem > 1.5f)
            {
                Range.enabled = false;
                AnimL.SetInteger("PushPull", 0);
                AnimR.SetInteger("PushPull", 0);
                Character.AllProhibit = false;
                Character.MoveOnly = false;
                CameraRotate.cameratotate = true;
                Cursor.lockState = CursorLockMode.Locked;
                physicMaterialBox.dynamicFriction = 4000f;
                yield break;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                AnimL.SetInteger("PushPull", 1);
                AnimR.SetInteger("PushPull", 1);

                if (DistanceToPushedItem > 1.2f)
                {
                    Character.speed = 1+DistanceToPushedItem-1f;
                }
                else if (DistanceToPushedItem < 1.1f)
                {
                    Character.speed = Mathf.Clamp(DistanceToPushedItem- 1.2f, 0, 1);
                    Force = transform.rotation * Vector3.forward * PushForce * (1 + (1.2f - DistanceToPushedItem)*1.2f)*1.2f;   
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));
                }
                else
                {
                    Character.speed = 1f;
                    Force = transform.rotation * Vector3.forward * PushForce * (1 + (1.2f - DistanceToPushedItem)) * 1.2f;
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                AnimL.SetInteger("PushPull", 2);
                AnimR.SetInteger("PushPull", 2);
                DistanceToPushedItem = Vector3.Distance(transform.position, PushedItem.transform.position);
                float maxSpeed = 1;
                Vector3 vel = PushedItemRb.velocity;
                if (DistanceToPushedItem > 1.1f)
                {
                    Character.speed = 1;
                    Force = transform.rotation * Vector3.back * PushForce * (1 + (DistanceToPushedItem-1.1f) * 4f);
                    PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));

                }
                else if (DistanceToPushedItem < 1.1f)
                {
                    Character.speed = 1 +1.1f - DistanceToPushedItem;
                }

                if (PushedItemRb.velocity.magnitude > maxSpeed&& DistanceToPushedItem < 1.2f)
                {
                    PushedItemRb.velocity = vel.normalized * maxSpeed;
                }
            }
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            
        }
    }
    private IEnumerator PushObject2()
    {
        Vector3 Force = Vector3.zero;
        DistanceToPushedItem = Vector3.Distance(transform.position, PushedItem.transform.position);
        for (float i = 0; i <= 0.25f; i += Time.deltaTime)
        {
            if (DistanceToPushedItem < 1.0f)
            {
                Force = transform.rotation * Vector3.forward * PushForce * 1.2f;
                PushedItemRb.AddForce(new Vector3(Force.x, 0, Force.z));
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
        physicMaterialBox.dynamicFriction = 4000f;
        yield break;

    }

    public ElectricDoorOpen ElectricDoorOpen;
    private IEnumerator DoorOpen1()
    {
        ElectricDoorOpen.enabled = true;
        yield return new WaitForSeconds(1f);
        Character.AllProhibit = false;
    }
}
