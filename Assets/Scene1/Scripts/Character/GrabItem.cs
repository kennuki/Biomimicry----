using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItem : MonoBehaviour
{
    BoxCollider Range;
    public static bool ThrowItem = false;
    void Start()
    {
        Range = GetComponent<BoxCollider>();

    }

    void Update()
    {
        if (IfTriggerDetect == false && Input.GetKeyDown(KeyCode.F) && ThrowItem == false && Character.AllProhibit == false)
        {
            StartCoroutine(TriggerDetect());
        }
        else if (Input.GetKeyDown(KeyCode.F) && ThrowItem == true && Character.AllProhibit == false)
        {
            StartCoroutine(Throw());
        }
    }

    public Transform LeftHand;
    private GameObject GrabbedItem;
    private Rigidbody GrabbedItemRb;
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
}
