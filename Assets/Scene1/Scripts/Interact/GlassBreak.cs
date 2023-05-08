using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBreak : MonoBehaviour
{
    public AudioSource audioSource;
    private AudioClip audioClip;
    private Transform[] children;
    void Start()
    {
        audioClip = audioSource.clip;
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
            audioSource.PlayOneShot(audioClip);
            GlassCaseOrigin.SetActive(false);
            GlassCaseBreak.SetActive(true);
            doll.SetActive(false);
            foreach (Transform child in children)
            {
                child.SetParent(null);
                child.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0, 3), 0,0));
            }
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }
    void Update()
    {
        
    }
}
