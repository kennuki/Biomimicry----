using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoldierAttack : MonoBehaviour
{
    private GameObject player;
    private void Start()
    {
        player = GameObject.Find("Character");
    }
}
