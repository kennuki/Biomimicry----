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
        StartCoroutine(GrabbedItemPhysicstTrue());
    }

    void Update()
    {
        if (Range.enabled == true && IfTriggerDetect == false)
        {
            StartCoroutine(TriggerDetect());
        }
       
    }

    public Transform LeftHand;
    private GameObject GrabbedItem;
    private Rigidbody GrabbedItemRb;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Grabbable")
        {
            Character.GrabbableItemTouch = true;

            GrabbedItem = other.gameObject;
            GrabbedItemRb = GrabbedItem.GetComponent<Rigidbody>();
            GrabbedItemRb.isKinematic = true;
            GrabbedItem.transform.SetParent(LeftHand);
            GrabbedItem.transform.position = LeftHand.position + transform.rotation * new Vector3(0.2f, -0.01f, 0.1f);
            GrabbedItem.transform.eulerAngles = LeftHand.eulerAngles;
            GrabbedItem.transform.Rotate(new Vector3(0, 90, 180), Space.Self); 
        }
    }

    bool IfTriggerDetect = false;
    private IEnumerator TriggerDetect()
    {
        IfTriggerDetect = true;
        while (Range.size.z < 3.5f)
        {
            Range.size = Range.size + new Vector3(0, 0, Time.deltaTime * 50);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        while (Range.size.y < 8f)
        {
            Range.size = Range.size + new Vector3(0, Time.deltaTime * 60, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        while (Range.size.x < 4f)
        {
            Range.size = Range.size + new Vector3(Time.deltaTime * 45, 0, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Range.size = new Vector3(1, 1, 1);
        IfTriggerDetect = false;
        Range.enabled = false;
    }
    private IEnumerator GrabbedItemPhysicstTrue()
    {
        while (true)
        {
            if (ThrowItem == true && GrabbedItem != null)
            {
                yield return new WaitForSeconds(0.01f);
                GrabbedItemRb.isKinematic = false;
                yield return new WaitForSeconds(0.39f);
                GrabbedItem.transform.SetParent(null);
                GrabbedItemRb.AddForce(this.transform.rotation * new Vector3(60, 250, 190));
                ThrowItem = false;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }

    }
}
