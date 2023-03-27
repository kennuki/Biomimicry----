using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    CharacterController controller;
    public GameObject LookPoint;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        CdHeight = controller.height;
        LookpointOriginY = LookPoint.transform.position.y;
    }

    void FixedUpdate()
    {
        SquatFunction();
        MoveFunction();
        GravityFunction();
        controller.Move(move * Time.deltaTime);
    }

    #region MoveFunction
    public static float speed = 5f;
    public static float imaangle;
    Vector3 move;
    private Vector3 dir;
    float h, j;  //Horizontal input, Vertical input
    private void MoveFunction()
    {
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
        move = dir * speed;
    }
    #endregion
    #region SquatFunction
    int SquatState; //0 = stand, 1 = Squat, 2 = Lie down and look
    float count = 0;
    bool stay = false;
    float CdHeight;
    float LookpointOriginY;
    private void SquatFunction()
    {

        if (Input.GetKey(KeyCode.C))
        {
            count += Time.deltaTime;
            if (count > 0.5f)
            {
                SquatState = 2;
            }
            else if (count > 0.1f && SquatState == 1)
            {
                SquatState = 2;
            }
            else if (stay == false)
            {
                SquatState = Mathf.Abs(SquatState - 1);
                stay = true;
            }
        }
        else
        {
            if (SquatState == 2)
            {
                SquatState = 1;
            }
            count = 0;
            stay = false;
        }
        if (SquatState == 1)
        {
            controller.height = Mathf.Clamp(controller.height - Time.deltaTime, CdHeight / 2, CdHeight);
            controller.center = new Vector3(0, Mathf.Clamp(controller.center.y - Time.deltaTime * 0.5f, 0 - CdHeight * 0.25f, 0), 0);
            LookPoint.transform.position = new Vector3(LookPoint.transform.position.x, Mathf.Clamp(LookPoint.transform.position.y-Time.deltaTime,LookpointOriginY- CdHeight * 0.25f, LookpointOriginY), LookPoint.transform.position.z);
        }
        else if(SquatState == 0)
        {
            controller.height = Mathf.Clamp(controller.height + Time.deltaTime, CdHeight / 2, CdHeight);
            controller.center = new Vector3(0, Mathf.Clamp(controller.center.y + Time.deltaTime * 0.5f, 0 - CdHeight * 0.25f, 0), 0);
            LookPoint.transform.position = new Vector3(LookPoint.transform.position.x, Mathf.Clamp(LookPoint.transform.position.y + Time.deltaTime, LookpointOriginY - CdHeight * 0.25f, LookpointOriginY), LookPoint.transform.position.z);
        }
    }


    #endregion
    #region GravityFunction
    float g = 1f, v1 = 10, c = 0;
    private void GravityFunction()
    {
        c += Time.deltaTime;
        if (!controller.isGrounded)
        {
            v1 = v1 + g;
            c = 0;
        }
        else if (controller.isGrounded && c > 0.05f)
        {
            v1 = 10;
        }
        move.y = move.y - v1 * 5 * Time.deltaTime;
    }
    #endregion
}
