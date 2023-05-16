using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGame : MonoBehaviour
{
    public void Quit()
    {

        UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();

    }
}
