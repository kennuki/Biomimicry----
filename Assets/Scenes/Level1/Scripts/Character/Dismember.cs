using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dismember : MonoBehaviour
{
    public GameObject[] PlayerObject; 
    public GameObject FakePlayer;
    public GameObject FakeBody;
    public GameObject[] FakeComponent;
    public Vector3 force = new Vector3(0, 0, 0);
    public Vector3 RandomForce = new Vector3(0, 0, 0);
    public void dismember()
    {
        foreach(GameObject gameObject in PlayerObject)
        {
            gameObject.SetActive(false);
        }
        FakePlayer.SetActive(true);
        FakeBody.GetComponent<Rigidbody>().AddForce(transform.rotation*force);
        RandomForce = new Vector3(RandomForce.x, RandomForce.y, Random.Range(-RandomForce.z, RandomForce.z));
        foreach (GameObject gameObject in FakeComponent)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(transform.rotation * (force / 2 + RandomForce));
        }
    }
}
