using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactive : MonoBehaviour
{
    public GameObject Boss;
    public void Active()
    {
        Boss.SetActive(true);
    }
}
