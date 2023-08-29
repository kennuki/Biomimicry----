using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomTeleport : MonoBehaviour
{
    private Transform Character;
    private Transform Boss;
    private NavMeshAgent agent;
    public Vector3[] Point;
    private void Start()
    {
        Boss = GameObject.Find("Boss").transform;
        Character = GameObject.Find("Character").transform;
        agent = Boss.GetComponent<NavMeshAgent>();
        AgentSpeed = agent.speed;
    }
    private void Update()
    {
        if (Boss == null||Character == null)
        {
            Boss = GameObject.Find("Boss").transform;
            Character = GameObject.Find("Character").transform;
            agent = Boss.GetComponent<NavMeshAgent>();
            AgentSpeed = agent.speed;
        }
        RandomTeleportFunc();
    }
    float TeleportCD = 3;
    float t = 0;
    float AgentSpeed;
    public void RandomTeleportFunc()
    {
        t += Time.deltaTime;
        if (Vector3.Distance(Character.position, Boss.position) > 10 || t > TeleportCD)
        {
            foreach(Vector3 vector in Point)
            {
                if (Vector3.Distance(vector, Boss.position) < 20)
                {
                    StartCoroutine(Teleport(vector));
                    t = 0;
                    TeleportCD = 10;
                    break;
                }
            }
        }
    }
    public IEnumerator Teleport(Vector3 v)
    {
        agent.speed = 0;
        this.transform.position = v;
        yield return new WaitForSeconds(1.5f);
        agent.speed = AgentSpeed;
    }
}
