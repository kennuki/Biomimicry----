using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBySavePoint : MonoBehaviour
{
    public int savepoint = 0;
    private void Start()
    {
        if (SavePointSerial.CurrentSavePointIndex >= savepoint)
            this.gameObject.SetActive(false);
    }
}
