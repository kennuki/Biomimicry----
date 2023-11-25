using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointInfo : MonoBehaviour
{
    public static PointInfo Instance;
    public List<Transform> AllPoint;
    private void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        Instance = this;
        DontDestroyOnLoad(this);
        foreach (Transform point in this.transform)
        {
            AllPoint.Add(point);
        }
    }
}
