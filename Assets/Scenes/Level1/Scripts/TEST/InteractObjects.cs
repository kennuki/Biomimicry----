using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractObjectsType
{
    PushOnly
}

public abstract class InteractObjects : MonoBehaviour
{ 
    public InteractObjectsType type;

    public Vector3 impact;
    public void SetImpact(Vector3 impact_)
    {
        impact = impact_;
    }

    public abstract void Enter();
    public abstract void Tick();
    public abstract void Exit();
}
