using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBreak : MonoBehaviour
{

    private Transform[] children;
    void Start()
    {
        children = new Transform[GlassCaseBreak.transform.childCount];
        for (int i = 0; i < GlassCaseBreak.transform.childCount; i++)
        {
            children[i] = GlassCaseBreak.transform.GetChild(i);
        }


    }
    public GameObject GlassCaseOrigin,GlassCaseBreak,doll;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            GlassCaseOrigin.SetActive(false);
            GlassCaseBreak.SetActive(true);
            doll.SetActive(false);
            foreach (Transform child in children)
            {
                child.SetParent(null);
            }
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }
    void Update()
    {
        
    }
}
