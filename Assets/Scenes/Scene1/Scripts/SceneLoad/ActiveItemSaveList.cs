using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItemSaveList : MonoBehaviour
{
    public List<GameObject> ObjectsToSave = new List<GameObject>();
    private static bool[] activeStates;
    private void Start()
    {
        RestoreActiveState();
    }
    public void SaveActiveState()
    {
        if (activeStates == null)
        {
            activeStates = new bool[ushort.MaxValue];
        }
        for (int i = 0; i < ObjectsToSave.Count; i++)
        {
            activeStates[i] = ObjectsToSave[i].activeSelf;
            Debug.Log(activeStates[i] + " " + i);
        }

    }
    public void RestoreActiveState()
    {
        if(activeStates != null)
        {
            for (int i = 0; i < ObjectsToSave.Count; i++)
            {
                ObjectsToSave[i].SetActive(activeStates[i]);
                Debug.Log(activeStates[i] + " " + i);
            }
        }

    }
    private void Update()
    {
        
    }
}
