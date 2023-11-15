using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanCamera : MonoBehaviour
{
    private Camera Cm;
    private Transform CharacterCamera;
    public Transform TargetGlass;
    public Transform Reflector;
    private Quaternion GlassRotate;
    private void Start()
    {
        Cm = GetComponent<Camera>();
        CharacterCamera = GameObject.Find("CM vcam1").transform;
        Field_Of_View = Cm.fieldOfView;
        NearPlane = Cm.nearClipPlane;
        GlassRotate = TargetGlass.rotation;
        OriginRotation = Quaternion.Euler(0, 0, 0);
        CameraOriginPos = transform.position;
        ReflectorScaleX = Reflector.localScale.x;
    }
    private void Update()
    {

        CameraFix();
        CameraFix2();      
    }
    public float NearPlaneFixRate = 1;
    public float MirrorFixRate = 0.5f;
    public float CameraY_Offset = 0.1f;
    private float Field_Of_View;
    private float NearPlane;
    private Vector3 CameraOriginPos;
    public float MirrorFocusPoint = 2f;
    public float MirrorFocusRate = 0.5f;
    public float Field_Of_View_Discrete_Point = 8;
    public float Field_Of_View_Discrete_Rate = 4;
    public float Mirror_Camera_Y_ShakeRate = 1;
    private Quaternion RotationAmount;
    private Quaternion InverseY;
    private float ReflectorScaleX;
    private void CameraFix()
    {
        GlassRotate = TargetGlass.rotation;
        Vector3 ABVector = transform.position - CharacterCamera.position;
        Vector3 ABOriginVectorNoY = new Vector3(CameraOriginPos.x, transform.position.y, CameraOriginPos.z) - CharacterCamera.position;
        Vector3 ABOriginVector = CameraOriginPos - CharacterCamera.position;
        Vector3 MirrorFacingVector = Reflector.up;
        Vector3 MIrrorHeightVector = Reflector.forward;
        Vector3 Projection_Z;
        float angle = Vector3.Angle(Reflector.up, ABOriginVector);
        if (angle < 90)
        {
            MirrorFacingVector = -Reflector.up;
            MirrorFocusRate = -Mathf.Abs(MirrorFocusRate);
            InverseY = Quaternion.Euler(0, 180, 0);
            Reflector.localScale = new Vector3(-ReflectorScaleX, Reflector.localScale.y, Reflector.localScale.z);

        }
        else
        {
            MirrorFacingVector = Reflector.up;
            MirrorFocusRate = Mathf.Abs(MirrorFocusRate);
            InverseY = Quaternion.Euler(0, 0, 0);
            Reflector.localScale = new Vector3(ReflectorScaleX, Reflector.localScale.y, Reflector.localScale.z);
        }
        Vector3 Projection_Y = Vector3.Project(ABVector, MirrorFacingVector);
        Vector3 Projection_Y_Target;
        Vector3 Projection_Y2 = Vector3.Project(ABOriginVectorNoY, MirrorFacingVector);
        Vector3 Projection_X2 = ABOriginVectorNoY - Projection_Y2;
        float DistanceY = Projection_Y.magnitude;
        float DistanceY2 = Projection_Y2.magnitude;
        float Distance_MirrorCm_Mirror = Vector3.Distance(transform.position,CameraOriginPos);
        Vector3 TargetPos2;
        RotationAmount = Quaternion.FromToRotation(-ABVector, MirrorFacingVector);

        if (DistanceY < Field_Of_View_Discrete_Point)
        {
            Cm.fieldOfView = Field_Of_View + (Mathf.Abs(DistanceY - Field_Of_View_Discrete_Point) * Field_Of_View_Discrete_Rate); 
        }
        else
        {
            Cm.fieldOfView = Field_Of_View;
        }
        Projection_Z = Vector3.Project(ABOriginVector, Reflector.forward);
        if (DistanceY2 < MirrorFocusPoint && DistanceY < DistanceY2 + (MirrorFocusPoint - DistanceY2) * MirrorFocusRate)
        {
            Projection_Y_Target = Projection_Y2 - TargetGlass.rotation * new Vector3(0, 0, ((MirrorFocusPoint - DistanceY2) * MirrorFocusRate));
            TargetPos2 = CameraOriginPos - (Projection_X2 * 0.5f) + (Projection_Y_Target - Projection_Y2);
            transform.position = TargetPos2;
            transform.position = TargetPos2 + Projection_Z * MirrorFixRate / Mathf.Sqrt(DistanceY) * Mirror_Camera_Y_ShakeRate +CameraY_Offset* Projection_Z;
        }
        else
        {

            Projection_Y_Target = Projection_Y2 - TargetGlass.rotation * new Vector3(0, 0, ((MirrorFocusPoint - DistanceY2) * MirrorFocusRate));
            TargetPos2 = CameraOriginPos - (Projection_X2 * 0.5f) + (Projection_Y_Target - Projection_Y2);
            transform.position = TargetPos2;
            //transform.position = new Vector3(TargetPos2.x, CameraOriginPos.y + y_change * Mathf.Clamp((MirrorFixRate / Mathf.Sqrt(DistanceY))* Mirror_Camera_Y_ShakeRate, 0, 1) + CameraY_Offset, TargetPos2.z);
            transform.position = TargetPos2 + Projection_Z * MirrorFixRate / Mathf.Sqrt(DistanceY) * Mirror_Camera_Y_ShakeRate + CameraY_Offset * Projection_Z;

        }
        float MagnitudeX = Vector3.Project(Projection_X2, Reflector.right).magnitude;
        Cm.nearClipPlane = NearPlane + Distance_MirrorCm_Mirror * NearPlaneFixRate - MagnitudeX * 0.2f;
    }

    private Quaternion OriginRotation;
    private void CameraFix2()
    {
        Quaternion rotationDifference = Quaternion.Inverse(OriginRotation) * GlassRotate;
        transform.rotation = OriginRotation * RotationAmount  * rotationDifference * InverseY;
    }
}
