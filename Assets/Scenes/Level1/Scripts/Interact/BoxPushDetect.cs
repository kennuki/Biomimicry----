using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPushDetect : MonoBehaviour
{
    [SerializeField]
    private BoxCollider[] BoxColliders;

    private Transform Player;
    private void Start()
    {
        Player = GameObject.Find("Character").transform;
    }
    private void Update()
    {
        if(Player==null)
            Player = GameObject.Find("Character").transform;
        float angle = -Player.transform.eulerAngles.y + transform.localEulerAngles.y;
        if (angle < 0)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        for (int i = 0; i < 4; i++)
        {
            if (Mathf.Abs(angle +90 - i * 90) < 20 || (angle+90 - i * 90) > 340)
            {
                BoxColliders[i].enabled = true;
            }
            else
            {
                BoxColliders[i].enabled = false;
            }
        }
    }
}
