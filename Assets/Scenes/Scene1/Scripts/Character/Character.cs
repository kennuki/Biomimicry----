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
    void Start()
    {
        controller = GetComponent<CharacterController>();
        CdHeight = controller.height;
        OffsetLookpointToCharacterY = transform.position.y-LookPoint.transform.position.y;
        Origin_speed = speed;
    }

    void FixedUpdate()
    {
        //Debug.Log(speed);
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
    }
    private void Update()
    {
        EnergyUseFunction();
    }



    #region MoveFunction
    public static float speed = 3f;
    private float Origin_speed;
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
        if (Input.GetKey(KeyCode.LeftShift) && Energy > 0 && dir.magnitude>0 && MoveOnly == false)
        {
            EnergyUse = true;
            speed = Origin_speed + 1.5f;
        }
        else if(MoveOnly == false)
        {
            EnergyUse = false;
            speed = Origin_speed;
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
        if (SquatState == 1 && count < 1)
        {
            speed = 2;
            controller.height = Mathf.Clamp(controller.height - Time.deltaTime, CdHeight / 2, CdHeight);
            controller.center = new Vector3(0, Mathf.Clamp(controller.center.y - Time.deltaTime * 0.5f, 0 - CdHeight * 0.25f, 0), 0);
            LookPoint.transform.position = new Vector3(LookPoint.transform.position.x, Mathf.Clamp(LookPoint.transform.position.y - Time.deltaTime, transform.position.y - OffsetLookpointToCharacterY - CdHeight * 0.25f, transform.position.y - OffsetLookpointToCharacterY), LookPoint.transform.position.z);
        }
        else if (SquatState == 0 && count < 1)
        {
            speed = Origin_speed;
            controller.height = Mathf.Clamp(controller.height + Time.deltaTime * 1.5f, CdHeight / 2, CdHeight);
            controller.center = new Vector3(0, Mathf.Clamp(controller.center.y + Time.deltaTime * 0.75f, 0 - CdHeight * 0.375f, 0), 0);
            LookPoint.transform.position = new Vector3(LookPoint.transform.position.x, Mathf.Clamp(LookPoint.transform.position.y + Time.deltaTime, transform.position.y - OffsetLookpointToCharacterY - CdHeight * 0.375f, transform.position.y - OffsetLookpointToCharacterY), LookPoint.transform.position.z);
        }
    }
    public void Squat() 
    {
        SquatState = Mathf.Abs(SquatState - 1);
        count = 0;

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
            if (anim.GetInteger("HandState") == 0 || anim.GetInteger("HandState") == 3) ;
            //MoveOnly = true;
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
    public void Jump(float intensity)
    {
        if (controller.isGrounded == true)
        {
            LookPoint.transform.position = new Vector3(LookPoint.transform.position.x, transform.position.y - OffsetLookpointToCharacterY, LookPoint.transform.position.z);
            SquatState = 0;
            c = 0;
            v1 = -55*intensity;
        }
    }


    #endregion


    public static bool NoEnergy = false;
    public static bool EnergyUse = false;
    private float EnergyUseRate = 12;
    private float ChargeRate = 15;
    public float MaxEnergy = 100;
    public float Energy = 100;
    private float ChargeDelay = 1f;
    float ChargeCounter = 0;
    private void EnergyUseFunction()
    {
        ChargeCounter += Time.deltaTime;
        if (EnergyUse == true && Energy > 0)
        {
            Energy -= EnergyUseRate * Time.deltaTime;
            ChargeCounter = 0;
        }
        else if (EnergyUse == false && ChargeCounter > ChargeDelay && Energy <= MaxEnergy)
        {
            Energy += ChargeRate * Time.deltaTime;
        }
        else if (Energy <= 0)
        {
            NoEnergy = true;
        }
        else
        {
            NoEnergy = false;
        }
    }

    private Rigidbody TouchedObjectRb;
    private void OnTriggerStay(Collider other)
    {
        TouchedObjectRb = other.gameObject.GetComponent<Rigidbody>();
        if (TouchedObjectRb != null)
        {
            if (other.tag != "PushOnly"&& other.tag != "Pushable")
            {
                Vector2 toObject = new Vector2(other.transform.position.x, other.transform.position.z) - new Vector2(this.transform.position.x, this.transform.position.z);
                Vector2 move2 = new Vector2(move.x, move.z);
                toObject.Normalize();
                move2.Normalize();
                float anglebias = 0;
                float angleDifference = Vector2.SignedAngle(move2, toObject);
                if (other.gameObject.GetComponent<GateRotate>() != null)
                {
                    anglebias = other.gameObject.GetComponent<GateRotate>().AngleBias;
                }
                if (angleDifference <= 40 + anglebias && angleDifference > -40 + anglebias && Input.GetKey(KeyCode.S)==false)
                {
                    TouchedObjectRb.AddForce(move *7.5f);
                }
                else
                {

                }
            }          
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        TouchedObjectRb = other.gameObject.GetComponent<Rigidbody>();
        if (TouchedObjectRb != null)
        {

            Vector2 toObject = new Vector2(other.transform.position.x, other.transform.position.z) - new Vector2(this.transform.position.x, this.transform.position.z);
            Vector2 move2 = new Vector2(move.x, move.z);
            toObject.Normalize();
            move2.Normalize();
            float anglebias = 0;
            float angleDifference = Vector2.SignedAngle(move2, toObject);
            if (other.gameObject.GetComponent<GateRotate>() != null)
            {
                anglebias = other.gameObject.GetComponent<GateRotate>().AngleBias;
            }
            if (angleDifference <= 40 + anglebias && angleDifference > -40 + anglebias)
            {
                TouchedObjectRb.AddForce(move * 7.5f);
            }
            else
            {

            }

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
