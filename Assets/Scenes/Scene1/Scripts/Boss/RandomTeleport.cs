using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomTeleport : MonoBehaviour
{
    [System.Serializable]
    public class TelePortPoint
    {
        public int PointCount;
        public Transform[] Point;
        public int[] MaxTeleportTimes;
    }
    private Transform Character;
    private Transform Boss;
    private NavMeshAgent agent;
    public TelePortPoint telePort;
    
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
    float TeleportCD = 3.7f;
    float t = 0;
    float AgentSpeed;
    public void RandomTeleportFunc()
    {
        t += Time.deltaTime;
        if (Vector3.Distance(Character.position, Boss.position) > 10 || t > TeleportCD)
        {
            for(int i = 0;i<telePort.PointCount; i++)
            {
                if (telePort.MaxTeleportTimes[i] > 0)
                {
                    if (Vector3.Distance(telePort.Point[i].position, Boss.position) < 20)
                    {

                        StartCoroutine(Teleport(telePort.Point[i].position));
                        t = 0;
                        TeleportCD = 10;
                        telePort.MaxTeleportTimes[i] -= 1;
                        break;
                    }
                }
            }         
        }
    }
    public IEnumerator Teleport(Vector3 v)
    {
        Debug.Log(v);
        agent.speed = 0;
        agent.enabled = false;
        this.transform.position = v;
        yield return new WaitForSeconds(Time.deltaTime);
        agent.enabled = true;
        agent.speed = AgentSpeed;

    }
}
