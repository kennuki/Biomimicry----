using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanCamera : MonoBehaviour
{
    private Camera Cm;
    private Transform CharacterCamera;
    public Transform TargetGlass;
    private Quaternion GlassRotate;
    private void Start()
    {
        Cm = GetComponent<Camera>();
        CharacterCamera = GameObject.Find("CM vcam1").transform;
        Field_Of_View = Cm.fieldOfView;
        if(FixAxis == "X")
        {
            OriginAxisPosition = transform.position;
            MaxP = MirrorScale / 2;
            MinP = -MirrorScale / 2;
        }
        else if (FixAxis == "Z")
        {
            OriginAxisPosition = transform.position;
            MaxP = MirrorScale / 2;
            MinP = -MirrorScale / 2;
        }
        transform.Rotate(AngleOffset, 0, 0);
        GlassRotate = TargetGlass.rotation;
        OriginTargetRotation = TargetGlass.eulerAngles;
        OriginRotation = transform.eulerAngles;
    }
    public float AngleOffset = -45;
    private void Update()
    {
        CameraFix();
        transform.LookAt(new Vector3(CharacterCamera.transform.position.x, CharacterCamera.transform.position.y+ CameraY_Offset, CharacterCamera.transform.position.z));
        CameraFix2();

    }

    public string FixAxis = "X";
    public float MirrorScale = 1;
    private Vector3 OriginAxisPosition;
    private float MaxP,MinP;
    public float CameraY_Offset = 0.1f;
    private float Field_Of_View;
    private void CameraFix()
    {
        GlassRotate = TargetGlass.rotation;
        float Distance = Vector3.Distance(transform.position, CharacterCamera.transform.position);
        Vector3 ABVector = transform.position - CharacterCamera.transform.position;
        ABVector.y = 0;
        float angle = Vector3.Angle(ABVector, (GlassRotate*Vector3.right));
        Debug.Log(angle);
        if (FixAxis == "X")
        {
            if (Distance < 8)
            {
                Cm.fieldOfView = Field_Of_View + (Mathf.Abs(Distance - 8) * 8f);
                float TargetP = (CharacterCamera.transform.position.x - transform.position.x) * (1 / Distance);
                transform.position = Vector3.Lerp(transform.position, new Vector3(Mathf.Clamp(CharacterCamera.transform.position.x, OriginAxisPosition.x - MinP * (Mathf.Clamp((1 / Distance),0,4)), OriginAxisPosition.x - MaxP * (Mathf.Clamp((1 / Distance), 0, 1))), transform.position.y, transform.position.z), 1f);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, OriginAxisPosition, 0.1f);
                Cm.fieldOfView = Field_Of_View;
            }
            if (Distance < 0.8f)
            {
                //transform.position = OriginAxisPosition+new Vector3(0,0,)
            }
        }
        else if (FixAxis == "Z")
        {
            if (Vector3.Distance(transform.position, CharacterCamera.transform.position) < MirrorScale+1)
            {
                transform.position = Vector3.Lerp(transform.position,new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(CharacterCamera.transform.position.z, MinP, MaxP)),1f);
                Cm.fieldOfView = Field_Of_View + Mathf.Abs((Vector3.Distance(transform.position, CharacterCamera.transform.position) - 2) * 25);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, OriginAxisPosition, 0.1f);
                Cm.fieldOfView = Field_Of_View;
            }
        }
        transform.position = new Vector3(transform.position.x, CharacterCamera.transform.position.y + CameraY_Offset, transform.position.z);
    }

    private Vector3 OriginTargetRotation;
    private Vector3 OriginRotation;
    private void CameraFix2()
    {
        transform.eulerAngles = OriginRotation + (TargetGlass.eulerAngles - OriginTargetRotation);
    }
}
