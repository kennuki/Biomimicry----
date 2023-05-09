using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBreak : MonoBehaviour
{
    public AudioSource audioSource;
    private AudioClip audioClip;
    private Transform[] children;
    public BoxCollider colliders;
    bool IfBreak = false;
    private Transform character;
    public Vector2 InitialPos, EndPos;
    void Start()
    {
        character = GameObject.Find("Character").transform;
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
            StartCoroutine(GlassBreakCondition1(30));
        }
    }
    private IEnumerator GlassBreakCondition1(float time)
    {
        StartCoroutine(GlassBreakCondition2());
        colliders.enabled = false;
        yield return new WaitForSeconds(time);
        if(IfBreak == false)
        {
            audioSource.PlayOneShot(audioClip);
            GlassCaseOrigin.SetActive(false);
            GlassCaseBreak.SetActive(true);
            doll.SetActive(false);
            IfBreak = true;
            foreach (Transform child in children)
            {
                child.SetParent(null);
                child.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0, 3), 0, 0));
            }
        }
    }
    private IEnumerator GlassBreakCondition2()
    {
        while (IfBreak == false)
        {
            if (character.position.x > InitialPos.x && character.position.x < EndPos.x && character.position.z > InitialPos.y && character.position.z < EndPos.y)
            {
                if (character.eulerAngles.y > 0 && character.eulerAngles.y < 20 || character.eulerAngles.y < 360 && character.eulerAngles.y > 340)
                {
                    audioSource.PlayOneShot(audioClip);
                    GlassCaseOrigin.SetActive(false);
                    GlassCaseBreak.SetActive(true);
                    doll.SetActive(false);
                    IfBreak = true;
                    foreach (Transform child in children)
                    {
                        child.SetParent(null);
                        child.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0, 3), 0, 0));
                    }
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
