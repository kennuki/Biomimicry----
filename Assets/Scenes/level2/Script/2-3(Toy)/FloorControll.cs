using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorControll : MonoBehaviour
{
    public static FloorControll Instance;
    public enum FloorColor
    {
        red,green,blue,white
    }
    public FloorColor floor;
    private void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        Instance = this;
        DontDestroyOnLoad(this);
    }


}
