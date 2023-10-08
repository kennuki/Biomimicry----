using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private InteractObjects useInteractObjects;
    [SerializeField] private InteractObjects interactObjects;

    [SerializeField] private Animator animator;

    private void Start()
    {
        Range = GetComponent<BoxCollider>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && interactObjects)
        {
            useInteractObjects = interactObjects;



            switch (interactObjects.type)
            {
                case InteractObjectsType.PushOnly:
                    useInteractObjects.SetImpact(transform.right * -200);
                    animator.SetTrigger("Push");

                    break;
                default:
                    break;
            }

            useInteractObjects.Enter();
        }

        if (Input.GetKeyUp(KeyCode.F) && useInteractObjects)
        {
            useInteractObjects.Exit();

            useInteractObjects = null;
        }

        if(useInteractObjects)
        {
            useInteractObjects.Tick();
        }
    }

    public CharacterController controller;
    public bool ThrowItem = false;
    private void OnTriggerEnter(Collider other)
    {

        other.TryGetComponent<InteractObjects>(out interactObjects);


        if (ThrowItem!)
        {
            if(ObstacleDetect()> Vector3.Distance(other.transform.position, transform.position))
            {

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit " + other.name);

        if (other.GetComponent<InteractObjects>())
        {
            interactObjects = null;
        }
    }

    private bool IfTriggerDetect = false;
    public GameObject cm1;
    private BoxCollider Range;
    private Transform Obstacle;
    private IEnumerator TriggerDetect()
    {
        Obstacle = null;
        Range.size = new Vector3(0.1f, 0.25f, 0.1f);
        Range.enabled = true;
        transform.eulerAngles = new Vector3(cm1.transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        IfTriggerDetect = true;
        while (Range.size.z < 2f)
        {
            Range.size = Range.size + new Vector3(0, 0, Time.deltaTime * 50);
            Range.center = Range.center + new Vector3(0, 0, Time.deltaTime * 25);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        while (Range.size.y < 6f)
        {
            Range.size = Range.size + new Vector3(0, Time.deltaTime * 75, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        while (Range.size.x < 2.5f)
        {
            Range.size = Range.size + new Vector3(Time.deltaTime * 75, 0, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }
       /* if (Interacted_Item == null && ThrowItem == true && Character.AllProhibit == false && Character.GrabProhibit == false)
        {
            StartCoroutine(Throw());
        }*/
        Range.center = Vector3.zero;
        IfTriggerDetect = false;
        Range.enabled = false;

    }


    private float ObstacleDetect()
    {
        Transform raycastOrigin = transform;
        Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
        RaycastHit hit;
        LayerMask layerMask = 1 << 6 | 1 << 12;
        if (Physics.Raycast(ray, out hit, 6, layerMask))
        {
            return Vector3.Distance(hit.point, transform.position);
        }
        return 0;
    }

    private void PlayAnimator()
    {
        Character.AllProhibit = true;
        Character.MoveOnly = true;
        CameraRotate.cameratotate = false;
    }
}
