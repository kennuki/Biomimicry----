using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Character : MonoBehaviour
{
    CharacterController controller;
    public GameObject LookPoint;
    public Animator anim;
    public static bool ActionProhibit = false, GrabProhibit = false, AllProhibit = false, MoveOnly = false;    //ActionProhibt effect squat
    public static bool GrabAllow = false;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        CdHeight = controller.height;
        OffsetLookpointToCharacterY = transform.position.y-LookPoint.transform.position.y;
        StartCoroutine(GrabThrowfunction());
    }

    void FixedUpdate()
    {
        if (AllProhibit == false)
        {
            if (ActionProhibit == false)
            {
                SquatFunction();
            }
            MoveFunction();
            JumpFunction();
        }
        else if(MoveOnly == true)
        {
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
    public static float speed = 4f;
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
        if(InteractFix == true)
        {
            speed = 0.5f;
            InteractFix = false;
        }
        move = dir * speed;
    }
    #endregion
    #region SquatFunction
    int SquatState = 0; //0 = stand, 1 = Squat, 2 = Lie down and look
    float count = 0;
    float CdHeight;
    float OffsetLookpointToCharacterY;
    private void SquatFunction()
    {
        count += Time.deltaTime;
        if (Input.GetKey(KeyCode.C))
        {
            if(count > 0.5f)
            {
                SquatState = Mathf.Abs(SquatState - 1);
                count = 0;
            }
        }
        if (count <= 0.5f)
        {
            GrabProhibit = true;
            MoveOnly = true;
        }
        else
        {
            GrabProhibit = false;
            MoveOnly = false;
        }
        if (SquatState == 1)
        {
            speed = 2;
            controller.height = Mathf.Clamp(controller.height - Time.deltaTime, CdHeight / 2, CdHeight);
            controller.center = new Vector3(0, Mathf.Clamp(controller.center.y - Time.deltaTime * 0.5f, 0 - CdHeight * 0.25f, 0), 0);
            LookPoint.transform.position = new Vector3(LookPoint.transform.position.x, Mathf.Clamp(LookPoint.transform.position.y - Time.deltaTime, transform.position.y - OffsetLookpointToCharacterY - CdHeight * 0.25f, transform.position.y - OffsetLookpointToCharacterY), LookPoint.transform.position.z);
        }
        else if (SquatState == 0)
        {
            speed = 4;
            controller.height = Mathf.Clamp(controller.height + Time.deltaTime * 1.5f, CdHeight / 2, CdHeight);
            controller.center = new Vector3(0, Mathf.Clamp(controller.center.y + Time.deltaTime * 0.75f, 0 - CdHeight * 0.375f, 0), 0);
            LookPoint.transform.position = new Vector3(LookPoint.transform.position.x, Mathf.Clamp(LookPoint.transform.position.y + Time.deltaTime, transform.position.y - OffsetLookpointToCharacterY - CdHeight * 0.375f, transform.position.y - OffsetLookpointToCharacterY), LookPoint.transform.position.z);
        }
    }


    #endregion
    #region GravityFunction
    float g = 3.5f, v1 = 5, c = 0;
    private void GravityFunction()
    {
        c += Time.deltaTime;
        if (!controller.isGrounded)
        {
            MoveOnly = true;
            ActionProhibit = true;
            v1 = v1 + g;
            c = 0;
        }
        else if (controller.isGrounded && c > 0.05f)
        {
            if (anim.GetInteger("HandState") == 0 || anim.GetInteger("HandState") == 3)
                MoveOnly = true;
            else
                MoveOnly = false;
            ActionProhibit = false;
            v1 = 5;
        }
        move.y = move.y - v1 * 5 * Time.deltaTime;
    }
    #endregion
    #region JumpGunction
    private void JumpFunction()
    {
        if (Input.GetKey(KeyCode.Space)&&controller.isGrounded == true)
        {
            LookPoint.transform.position = new Vector3(LookPoint.transform.position.x, transform.position.y - OffsetLookpointToCharacterY, LookPoint.transform.position.z);
            SquatState = 0;
            c = 0;
            v1 = -55;
        }
    }


    #endregion
    #region GrabThrowFunction
    public Transform Cm1;
    private IEnumerator GrabThrowfunction()
    {
        while (true)
        {
            if (anim.GetInteger("HandState") == 1)
            {
                anim.SetInteger("HandState", 2);
            }
            else if (anim.GetInteger("HandState") == 3)
            {
                anim.SetInteger("HandState", 0);
            }
            else if (GrabAllow == true)
            {
                if (anim.GetInteger("HandState") == 0)
                {
                    GrabAllow = false;
                    float AngleX = Cm1.eulerAngles.x;
                    if (AngleX > 180) AngleX -= 360;
                    AngleX = Mathf.Clamp(AngleX, 0, 45);
                    anim.SetFloat("AngleX", Mathf.Clamp((1 - AngleX / 45), 0, 1));
                    anim.SetInteger("HandState", 1);
                }
                else if (anim.GetInteger("HandState") == 2)
                {
                    GrabAllow = false;
                    anim.SetInteger("HandState", 3);
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }




    }
    #endregion
    private bool InteractFix = false;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Pushable")
        {
            InteractFix = true;
        }
    }

    private Rigidbody TouchedObjectRb;
    private void OnTriggerEnter(Collider other)
    {
        TouchedObjectRb = other.gameObject.GetComponent<Rigidbody>();
        if (TouchedObjectRb != null)
        {
            TouchedObjectRb.AddForce(transform.rotation * move * 10f);
        }
    }

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
