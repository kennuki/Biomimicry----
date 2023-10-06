using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item01 : TEST, Crawl
{
    public Vector3 Vector3 { get; set; }

    public override void OnGrasp()
    {
    }

    public override void Tick()
    {
        transform.position = Vector3;
    }

    public override void OnDisGrasp()
    {
    }

}
