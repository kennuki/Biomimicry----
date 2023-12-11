using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    public static bool push = false;
    CharacterController controller;
    public GameObject LookPoint;
    public GameObject Left_Hand;
    public Animator anim;
    public static bool ActionProhibit = false, GrabProhibit = false, AllProhibit = false, MoveOnly = false;    //ActionProhibt effect squat
   
    void Start()
    {
        ActionProhibit = false;
        GrabProhibit = false;
        AllProhibit = false;
        MoveOnly = false;
        SquatState = 0;
        controller = GetComponent<CharacterController>();
        CdHeight = controller.height;
        OffsetLookpointToCharacterY = LookPoint.transform.localPosition.y;
        OffsetLookpointToCharacterX = LookPoint.transform.localPosition.x;
        Origin_speed = Originspeed;
        speed = Origin_speed;
        CenterOrigin = controller.center.y;
        float a;
        CalculateCollisionVelocities(5, 10, 1, 0, out a);
    }

    void FixedUpdate()
    {
        Debug.Log(LoadScene.SceneWillChange);
        if (AllProhibit == true)
            EnergyUse = false;
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (LookState == 3)
            {
                LookState = 0;
            }
            LookState = 3;
        }
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
        else
        {
            move = Vector3.zero;
            anim.SetInteger("Run", 0);
        }

        GravityFunction();
        controller.Move(move * Time.deltaTime);
    }
    private bool start_active = false;
    bool ScenesIsloading()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (!SceneManager.GetSceneAt(i).isLoaded)
                return true;
        }
        return false;
    }
    private void Update()
    {
        EnergyUseFunction();
        if (!start_active)
        {
            if (ScenesIsloading())
                controller.enabled = false;
            else if (!ScenesIsloading())
            {
                controller.enabled = true;
                start_active = true;
            }
        }
        Debug.Log(SavePointSerial.CurrentSavePointIndex);
    }




    #region MoveFunction
    public static int LookState = 1;
    public static float speed = 3f;
    public float Origin_speed = 3f;
    public static float Originspeed = 3f;
    public static float imaangle;
    Vector3 move;
    private Vector3 dir;
    float h, j;  //Horizontal input, Vertical input
    int RunDir;
    private void MoveFunction()
    {
        j = Input.GetAxis("Horizontal");
        h = -Input.GetAxis("Vertical");
        if (h <= 0)
        {
            RunDir = -1;
        }
        else
        {
            RunDir = 1;
        }
        imaangle = transform.eulerAngles.y * Mathf.Deg2Rad;
        float jx = j * Mathf.Sin(imaangle);
        float jz = j * Mathf.Cos(imaangle);
        float hx = h * Mathf.Sin(imaangle + 0.5f * Mathf.PI);
        float hz = h * Mathf.Cos(imaangle + 0.5f * Mathf.PI);
        if (LookState == 1)
            dir = new Vector3(jx + hx, dir.y, jz + hz);
        else if (LookState == 3)
            dir = new Vector3(-h, dir.y, -j);
        anim.SetFloat("RunSpeed",  -RunDir * speed*0.4f);
        if (j != 0 && h != 0)
        {
            dir /= Mathf.Sqrt(2);
        }
        if (dir.magnitude > 0.1f)
        {
            anim.SetInteger("Run", 1);
        }
        else
        {
            anim.SetInteger("Run", 0);
        }
        if (Input.GetKey(KeyCode.LeftShift) && Energy > 0 && dir.magnitude>0 && MoveOnly == false && SquatState == 0)
        {
            EnergyUse = true;
            speed = Origin_speed + 1.5f;
        }
        else if(MoveOnly == false)
        {
            //Debug.Log(EnergyUse);
            EnergyUse = false;
            if (SquatState == 1)
            {
                speed = Origin_speed * 0.5f;
            }
            else
                speed = Origin_speed;
        }
        else if (MoveOnly)
        {
            EnergyUse = false;
            speed = Origin_speed;
        }

        move = dir * speed;
    }
    #endregion
    #region SquatFunction
    public static int SquatState = 0; //0 = stand, 1 = Squat, 2 = Lie down and look
    float count = 0;
    float CdHeight;
    float OffsetLookpointToCharacterY;
    float OffsetLookpointToCharacterX;
    float CenterOrigin;
    public static bool If_Squat = false;
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
        else if(count>1f)
        {
            GrabProhibit = false;
            MoveOnly = false;
        }
        if (SquatState == 1 && count <= 1)
        {
            If_Squat = true;
            speed = 2;
            controller.height = Mathf.Clamp(controller.height - Time.deltaTime* CdHeight, CdHeight / 2, CdHeight);
            controller.center = new Vector3(0, Mathf.Clamp(controller.center.y - Time.deltaTime * CdHeight/2, CenterOrigin - CdHeight * 0.25f, CenterOrigin), 0);
            LookPoint.transform.localPosition = new Vector3(Mathf.Clamp(LookPoint.transform.localPosition.x - Time.deltaTime*0.35f, OffsetLookpointToCharacterX  - CdHeight * 0.13f, OffsetLookpointToCharacterX), Mathf.Clamp(LookPoint.transform.localPosition.y - Time.deltaTime, OffsetLookpointToCharacterY - CdHeight * 0.32f, OffsetLookpointToCharacterY), LookPoint.transform.localPosition.z);
            anim.SetLayerWeight(1, Mathf.Clamp(count * 2.15f,0,1));
        }
        else if (SquatState == 0 && count <= 1)
        {
            If_Squat = false;
            anim.SetLayerWeight(1, Mathf.Clamp(1 - count * 2.15f, 0, 1));
            speed = Origin_speed;
            controller.height = Mathf.Clamp(controller.height + Time.deltaTime * CdHeight, CdHeight / 2, CdHeight);
            controller.center = new Vector3(0, Mathf.Clamp(controller.center.y + Time.deltaTime * CdHeight / 2, CenterOrigin - CdHeight * 0.375f, CenterOrigin), 0);
            LookPoint.transform.localPosition = new Vector3(Mathf.Clamp(LookPoint.transform.localPosition.x + Time.deltaTime * 0.35f, OffsetLookpointToCharacterX - CdHeight * 0.13f, OffsetLookpointToCharacterX), Mathf.Clamp(LookPoint.transform.localPosition.y + Time.deltaTime, OffsetLookpointToCharacterY - CdHeight * 0.35f, OffsetLookpointToCharacterY), LookPoint.transform.localPosition.z);
        }

    }
    public void Squat() 
    {
        SquatState = 1;
        count = 0;

    }


    #endregion
    #region GravityFunction
    float g = 3.5f, c = 0;
    float v1 = 5;
    public float OriginV1 = 5;
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
            v1 = OriginV1;
        }
        move.y = move.y - v1 * 5 * Time.deltaTime;
    }
    #endregion
    #region JumpGunction
    private void JumpFunction()
    {
        if (Input.GetKey(KeyCode.Space)&&controller.isGrounded == true)
        {
            //LookPoint.transform.position = new Vector3(LookPoint.transform.position.x, transform.position.y - OffsetLookpointToCharacterY, LookPoint.transform.position.z);
            if(SquatState == 1)
            {
                //SquatState = 0;
            }
            //anim.SetLayerWeight(1, 0);
            c = 0;
            v1 = -55;
        }
    }

    public void Jump(float intensity)
    {
        if (controller.isGrounded == true)
        {
            //LookPoint.transform.position = new Vector3(LookPoint.transform.position.x, transform.position.y - OffsetLookpointToCharacterY, LookPoint.transform.position.z);
            if (SquatState == 1)
            {
                //SquatState = 0;
            }
            //anim.SetLayerWeight(1, 0);
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
    public FlashLight flashLight;
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
            NoEnergy = false;
            Energy += ChargeRate * Time.deltaTime;
        }
        else if (Energy <= 0&&NoEnergy == false)
        {
            NoEnergy = true;
            StartCoroutine(flashLight.LightWeak());
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
                Vector3 collisionPoint = other.ClosestPointOnBounds(transform.position);
                Vector3 vectorA = move;
                Vector3 vectorB = transform.position - collisionPoint;
                float angle = Vector3.Angle(vectorA, vectorB);
                if (angle > 90)
                {
                    float TargetForce;
                    CalculateCollisionVelocities(10, Vector3.Magnitude(move), TouchedObjectRb.mass, Vector3.Magnitude(Vector3.Project(TouchedObjectRb.velocity, Vector3.Normalize(move))), out TargetForce);
                    if(other.tag == "Joint")
                    {

                        TouchedObjectRb.AddForceAtPosition(TargetForce * Vector3.Normalize(controller.velocity) * 0.05f, collisionPoint, ForceMode.VelocityChange);
                    }
                    else
                    {
                        Vector3 Targetvelocity = move;
                        if (Vector3.Magnitude(controller.velocity) < 0.1f)
                        {
                            Targetvelocity.y = 0;
                        }
                        TouchedObjectRb.AddForceAtPosition(TargetForce * Vector3.Normalize(Targetvelocity) * 0.05f, collisionPoint, ForceMode.VelocityChange);
                        //Debug.Log(TargetForce * Vector3.Normalize(controller.velocity) + " " + other.name);
                    }
                }

            }          
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        TouchedObjectRb = other.gameObject.GetComponent<Rigidbody>();
        if (TouchedObjectRb != null)
        {
            Vector3 collisionPoint = other.ClosestPointOnBounds(transform.position);
            Vector3 vectorA = move;
            Vector3 vectorB = transform.position - collisionPoint;
            float angle = Vector3.Angle(vectorA, vectorB);
            float TargetForce;
            CalculateCollisionVelocities(10, Vector3.Magnitude(controller.velocity), TouchedObjectRb.mass, Vector3.Magnitude(Vector3.Project(TouchedObjectRb.velocity, Vector3.Normalize(controller.velocity))), out TargetForce);
            if (other.tag == "PushOnly" || other.tag == "Pushable")
            {
                    TouchedObjectRb.AddForceAtPosition(TargetForce * Vector3.Normalize(controller.velocity) * 0.01f, collisionPoint, ForceMode.VelocityChange);
            }
            else if(other.tag == "Joint")
            {
                TouchedObjectRb.AddForceAtPosition(TargetForce * Vector3.Normalize(controller.velocity) * 0.02f, collisionPoint, ForceMode.VelocityChange);
            }
            else if (other.gameObject.layer == 18)
            {
                
            }
            else
            {
                TouchedObjectRb.AddForceAtPosition(TargetForce * Vector3.Normalize(controller.velocity) * 0.2f* other.material.bounciness, collisionPoint, ForceMode.VelocityChange);
            }


        }
    }
  



    //完全彈性碰撞公式解
    //m1 = player質量，v1 = 玩家速度，m2 = 被碰撞物體質量 ， v2 = 被碰撞物體速度 ，v2a = 碰撞後物體的速度解
    private void CalculateCollisionVelocities(float m1, float v1, float m2, float v2 ,out float v2a)
    {
        v2a = ((m1 + m1) * v1 + (m2 - m1) * v2) / (m1 + m2);
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
