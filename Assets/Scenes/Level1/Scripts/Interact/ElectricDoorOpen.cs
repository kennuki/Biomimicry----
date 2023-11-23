using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class ElectricDoorOpen : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public PlayableAsset yourTimelineAsset;
    public Animator Rod;
    private EventActive Event;
    private void Start()
    {
        Event = this.GetComponent<EventActive>();
        CameraRotate = GameObject.Find("CM vcam1").GetComponent<CameraRotate>();
    }
    private void Update()
    {
        if (Event.Active)
        {
            StartCoroutine(Open(1.4f));
            Event.Active = false;
        }
    }
    public CinemachineVirtualCamera Rod_Camera;
    public GameObject door;
    private CameraRotate CameraRotate;

    private IEnumerator Open(float second)
    {
        playableDirector.playableAsset = yourTimelineAsset;
        playableDirector.Play();
        Rod.enabled = true;
        yield return new WaitForSeconds(1f);
        StartCoroutine(CameraRotate.CameraShake());
        int layerIndex = LayerMask.NameToLayer("Character");
        Camera.main.cullingMask |= 1 << layerIndex;
        Rod_Camera.Priority = 11;
        for (float i = 0; i < second; i += Time.deltaTime)
        {
            door.transform.Translate(0, 2 * Time.deltaTime, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Rod_Camera.Priority = 0;
        yield return new WaitForSeconds(2f);
        Camera.main.cullingMask &= ~(1 << layerIndex);
    }
}
