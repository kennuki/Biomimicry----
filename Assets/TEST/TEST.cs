using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TEST : MonoBehaviour
{
    public abstract void OnGrasp();
    public abstract void Tick();
    public abstract void OnDisGrasp();
}
