using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dismember : MonoBehaviour
{
    public GameObject[] PlayerObject; 
    public GameObject FakePlayer;
    public GameObject FakeBody;
    public Vector3 force = new Vector3(0, 0, 0);
    /*private IEnumerator dismember()
    {
        foreach(GameObject gameObject in PlayerObject)
        {
            gameObject.SetActive(false);
        }
        FakeBody.GetComponent<>
        FakeHead.transform.SetParent(null);
        Rigidbody FakeHeadRb = FakeHead.GetComponent<Rigidbody>();
        FakeHeadRb.useGravity = true;
        FakeHeadRb.isKinematic = false;
        FakeHeadRb.AddForce(transform.rotation * HeadDropForce);
    }*/
}
