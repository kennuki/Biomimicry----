using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactive : MonoBehaviour
{
    public GameObject Boss;
    private void Awake()
    {
        Boss.SetActive(false);
    }
    public void Active()
    {
        Boss.SetActive(true);
    }
}
