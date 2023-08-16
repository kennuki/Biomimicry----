using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBreak : MonoBehaviour
{
    public GameObject Card;
    public PanicRed_PP panicRed_PP;
    public AudioSource audioSource;
    public AudioClip[] audioClip;
    private Transform[] children;
    public BoxCollider colliders;
    bool IfBreak = false;
    private Transform character;
    public Vector2 InitialPos, EndPos;
    void Start()
    {
        character = GameObject.Find("Character").transform;
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
            StartCoroutine(GlassBreakCondition1(Random.Range(10,25)));
        }
    }
    private IEnumerator GlassBreakCondition1(float time)
    {
        StartCoroutine(GlassBreakCondition2());
        colliders.enabled = false;
        yield return new WaitForSeconds(time);
        if(IfBreak == false)
        {
            panicRed_PP.State = 0;
            panicRed_PP.RunTime = 2;
            audioSource.PlayOneShot(audioClip[0]);
            GlassCaseOrigin.SetActive(false);
            GlassCaseBreak.SetActive(true);
            IfBreak = true;
            foreach (Transform child in children)
            {
                child.SetParent(null);
                child.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0, 3), 0, 0));
            }
            doll.SetActive(false);
            Card.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            audioSource.PlayOneShot(audioClip[1]);
            yield return new WaitForSeconds(0.1f);
            panicRed_PP.State = 0;
            panicRed_PP.RunTime = 3f;
            yield return new WaitForSeconds(0.4f);
            doll.transform.position = character.position + character.rotation * new Vector3(0, -1f, 1.5f);
            doll.transform.SetParent(character);
            yield return new WaitForSeconds(Time.deltaTime);
            doll.SetActive(true);
            yield return new WaitForSeconds(Time.deltaTime * 1);
            doll.SetActive(false);
            doll.transform.SetParent(null);
        }
    }
    private IEnumerator GlassBreakCondition2()
    {
        while (IfBreak == false)
        {
            if (character.position.x > InitialPos.x && character.position.x < EndPos.x && character.position.z > InitialPos.y && character.position.z < EndPos.y)
            {
                if (character.eulerAngles.y > 90 && character.eulerAngles.y < 120)
                {
                    yield return new WaitForSeconds(0.5f);
                    audioSource.PlayOneShot(audioClip[0],1.5f);
                    GlassCaseOrigin.SetActive(false);
                    GlassCaseBreak.SetActive(true);
                    IfBreak = true;
                    foreach (Transform child in children)
                    {
                        child.SetParent(null);
                        child.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0, 3), 0, 0));
                    }
                    doll.SetActive(false);
                    Card.SetActive(true);
                    yield return new WaitForSeconds(0.1f);
                    audioSource.PlayOneShot(audioClip[1]);
                    yield return new WaitForSeconds(0.1f);
                    panicRed_PP.State = 0;
                    panicRed_PP.RunTime = 3f;
                    yield return new WaitForSeconds(0.4f);
                    doll.transform.position = character.position + character.rotation * new Vector3(0,0.7f,1f);
                    doll.transform.SetParent(character);
                    yield return new WaitForSeconds(Time.deltaTime);
                    doll.SetActive(true);
                    yield return new WaitForSeconds(Time.deltaTime*1);
                    doll.SetActive(false);
                    doll.transform.SetParent(null);
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
