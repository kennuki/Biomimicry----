using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CameraRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        sensitivitySlider.value = Setting.Sensitive;
        OringinPos = LookPoint.transform.localPosition;
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameratotate = true;
        StartCoroutine(LockCursorToMiddle());
        sensitivitySlider.value = Setting.Sensitive;
        sensitivity = sensitivitySlider.value;  
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
    }

    // Update is called once per frame
    void Update()
    {
        CursorHide();
        if (cameratotate == true&&Character.LookState == 1)
        {
            CameraRotateFunction();
        }
        if (shake)
        {
            CameraShake2();
        }
        else LookPoint.transform.localPosition = OringinPos;
        //Debug.LogFormat("{0:F4}, {1:F4}", va2.x, va2.y);
    }

    public bool state = false;
    public static bool cameratotate = true;
    private IEnumerator LockCursorToMiddle()
    {
        float waittime;
        while (true)
        {
            state = !state;
            if (cameratotate == false)
            {
                waittime = 0f;
                state = false;
            }
            else if (state == true)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                va2 = Vector2.zero;
                vt2 = vn2;
                //vt2 = new Vector2(0.4992765f, 0.5012655f);
                waittime = 0f;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                vn2 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                waittime = 0.1f;
            }
            yield return new WaitForSeconds(waittime);
        }
    }
    private void CursorHide()
    {
        if (Input.GetKeyDown(KeyCode.B) && cameratotate == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cameratotate = true;
        }
        else if (Input.GetKeyDown(KeyCode.B) && cameratotate == true)
        {

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cameratotate = false;
        }
    }



    public CinemachineVirtualCamera CM1;
    public GameObject LookPoint;
    //public GameObject PlayerRotate2;
    public GameObject PlayerRotate;  //control character's rotation that follow the camera
    private float sensitivity;
    public Slider sensitivitySlider;
    public float CmRotateRate = 1f;  //rotate speed rate
    public Vector2 va2 = Vector2.zero;      //offset of the cursor
    Vector2 vt2 = Vector2.zero;   //temp that store the cursor's position in the screen
    Vector2 v2 = new Vector2(0.5f, 0.5f);
    Vector2 vn2 = Vector2.zero;
    private void CameraRotateFunction()
    {
        float h = Input.GetAxis("Horizontal");
        float j = Input.GetAxis("Vertical");
        var transposer = CM1.GetCinemachineComponent<CinemachineTransposer>();
        if (Input.GetKeyDown(KeyCode.F7))
        {
            transposer.m_FollowOffset = new Vector3(0f, 0f, -0.13f);
        }
        v2 = Camera.main.ScreenToViewportPoint(Input.mousePosition); //cursor's position in screen(0~1)
        va2 = v2 - vt2;
        if (Mathf.Abs(va2.x) > 0f)
        {
            this.transform.Rotate(Vector3.up * va2.x * CmRotateRate* sensitivity * 80, Space.World);
            PlayerRotate.transform.Rotate(Vector3.up * va2.x * CmRotateRate* sensitivity * 80, Space.World);
            /*if (h == 0 && j == 0)
            {
                PlayerRotate2.transform.Rotate(Vector3.up * -va2.x * CmRotateRate* sensitivity * 80, Space.World);
            }*/
        }
        float CameraXAngle = this.transform.eulerAngles.x;
        if (CameraXAngle > 180)
        {
            CameraXAngle -= 360;
        }
        float faceAngle;
        Vector2 PlayerXZ = new Vector2(LookPoint.transform.position.x, LookPoint.transform.position.z);
        Vector2 CameraXZ = new Vector2(transform.position.x, transform.position.z);
        float XZDistance = Vector2.Distance(PlayerXZ, CameraXZ);
        faceAngle = Mathf.Atan2(XZDistance, LookPoint.transform.position.y - transform.position.y) * Mathf.Rad2Deg;
        if (Mathf.Abs(va2.y) >= 0f)
        {
            if (va2.y > 0 && CameraXAngle > -70f)
            {
                transposer.m_FollowOffset += new Vector3(0, -va2.y * 0.13f, 0) * CmRotateRate* sensitivity / 30;
                Quaternion targetRotation = Quaternion.Euler(faceAngle - 90, transform.eulerAngles.y, transform.eulerAngles.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.2f);



            }
            else if (va2.y <= 0 && CameraXAngle < 70f)
            {
                transposer.m_FollowOffset += new Vector3(0, -va2.y * 0.13f, 0) * CmRotateRate* sensitivity / 30;
                Quaternion targetRotation = Quaternion.Euler(faceAngle - 90, transform.eulerAngles.y, transform.eulerAngles.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.2f);
            }
        }
        va2 = Vector2.zero;
        vt2 = v2;
    }
    void OnSensitivityChanged(float value)
    {
        sensitivity = value;
        Setting.Sensitive = sensitivitySlider.value;
    }

    public AnimationCurve Curve;
    public AnimationCurve Curve2;
    float ShakeDuration = 1.4f;
    public IEnumerator CameraShake()
    {
        float ShakeTime = 0;
        while (ShakeTime < ShakeDuration)
        {
            ShakeTime += Time.deltaTime;
            float strength = Curve.Evaluate(ShakeTime / ShakeDuration);
            LookPoint.transform.localPosition = OringinPos + Random.insideUnitSphere * strength;
            yield return null;
        }
        LookPoint.transform.localPosition = OringinPos;
    }
    public bool shake = false;
    public float Amplitude = 1;
    float ShakeTime2 = 0;
    Vector3 OringinPos;
    public void CameraShake2()
    {
        if(ShakeTime2 < ShakeDuration)
        {
            ShakeTime2 += Time.deltaTime;
            float strength = Curve2.Evaluate(ShakeTime2 / ShakeDuration);
            LookPoint.transform.localPosition = OringinPos + Random.insideUnitSphere * strength* Amplitude;
        }
        else
        {
            ShakeTime2 = 0;
            LookPoint.transform.localPosition = OringinPos;
        }

    }
}
