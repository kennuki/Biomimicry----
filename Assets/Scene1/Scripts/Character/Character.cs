using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    CharacterController controller;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveFunction();
        move.y = move.y - v1 * 15 * Time.deltaTime;
        controller.Move(move * Time.deltaTime);
    }
    public static float speed = 5f;
    public static float imaangle;
    Vector3 move;
    private Vector3 dir;
    float h;
    float j;
    float a = 0;
    float g = 1f, v1 = 4, c = 0;
    private void MoveFunction()
    {
        a += Time.deltaTime;
        h = Input.GetAxis("Horizontal");
        j = Input.GetAxis("Vertical");
        imaangle = transform.eulerAngles.y * Mathf.Deg2Rad;
        float jx = j * Mathf.Sin(imaangle);
        float jz = j * Mathf.Cos(imaangle);
        float hx = h * Mathf.Sin(imaangle + 0.5f * Mathf.PI);
        float hz = h * Mathf.Cos(imaangle + 0.5f * Mathf.PI);
        dir = new Vector3(jx + hx, dir.y, jz + hz);
        if (j != 0 && h != 0)
        {
            dir /= Mathf.Sqrt(2);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            a = 1;
        }
        move = dir * speed;
    }
}
