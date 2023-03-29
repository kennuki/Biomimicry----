using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Character : MonoBehaviour
{
    CharacterController controller;
    public GameObject LookPoint;
    public Animator anim;
    public static bool ActionProhibit = false;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        CdHeight = controller.height;
        LookpointOriginY = LookPoint.transform.position.y;
        StartCoroutine(GrabThrowfunction());
    }

    void FixedUpdate()
    {
        if (ActionProhibit == false)
        {
            SquatFunction();
            MoveFunction();
        }
        else move = Vector3.zero;
        GravityFunction();
        controller.Move(move * Time.deltaTime);
        if (Input.GetKey(KeyCode.L))
        {
            anim.SetInteger("HandState", 2);
        }
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
    #region GrabThrowFunction
    public static bool GrabbableItemTouch = false;
    private BoxCollider GrabRange;
    public Transform Cm1;
    private IEnumerator GrabThrowfunction()
    {
        GrabRange = GetChildComponentByName<BoxCollider>("GrabRange");
        while (true)
        {
            if(GrabbableItemTouch == true)
            {
                anim.SetInteger("HandState", 2);
                GrabbableItemTouch = false;
            }
            if(anim.GetInteger("HandState") == 1)
            {
                anim.SetInteger("HandState", 0);
                ActionProhibit = false;
                yield return new WaitForSecondsRealtime(0.5f); //after this delay ,collider enable
                GrabRange.enabled = false;
                yield return new WaitForSecondsRealtime(0.3f);
            }
            else if (anim.GetInteger("HandState") == 3)
            {
                anim.SetInteger("HandState", 0);
                yield return new WaitForSecondsRealtime(0.3f);
                ActionProhibit = false;
                yield return new WaitForSecondsRealtime(0.5f);
            }
            else if (Input.GetKey(KeyCode.F))
            {
                if(anim.GetInteger("HandState") == 0)
                {
                    float AngleX = Cm1.eulerAngles.x;
                    if (AngleX > 180) AngleX -= 360;
                    AngleX = Mathf.Clamp(AngleX, 0, 45);
                    anim.SetFloat("AngleX", Mathf.Clamp((1 - AngleX / 45), 0, 1));

                    ActionProhibit = true;
                    anim.SetInteger("HandState", 1);
                    yield return new WaitForSecondsRealtime(0.2f);
                    GrabRange.enabled = true;
                }
                else if(anim.GetInteger("HandState") == 2)
                {
                    ActionProhibit = true;
                    anim.SetInteger("HandState", 3);
                    GrabItem.ThrowItem = true;
                    yield return new WaitForSecondsRealtime(0.5f);
                }
            }
            yield return new WaitForFixedUpdate();
        }




    }
    #endregion


    private T GetChildComponentByName<T>(string name) where T : Component
    {
        foreach (T component in GetComponentsInChildren<T>(true))
        {
            if (component.gameObject.name == name)
            {
                return component;
            }
        }
        return null;
    }
}
