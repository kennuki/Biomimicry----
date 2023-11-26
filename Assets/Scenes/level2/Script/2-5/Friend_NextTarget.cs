using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend_NextTarget : MonoBehaviour
{
    public FacePoint_Player Player;

    public List<Transform> NextPoint_self;
    private List<Transform> NextPoint_player;
    public NearArea self, player;
    private Transform player_facePoint;
    private Transform self_point;
    private Transform box1,box2;
    public Transform adads;
    public Transform target;
    private void Update()
    {
        player_facePoint = Player.nextPoint;
        GetPointInfo();
        if(player_facePoint != null&& self_point != null)
        {
            Calculate_NextPoint_player();
            Calculate_NextPoint_self();
            Calculate_BestPoint();
            player.nearPoint.Clear();
            self.nearPoint.Clear();
        }
        Debug.Log(target);
    }
    private void GetPointInfo()
    {
        self_point = AllArea.Instance.friend_point;
        box1 = AllArea.Instance.box1_point;
        box2 = AllArea.Instance.box2_point;
    }
    private void Calculate_NextPoint_self()
    {
        foreach(Transform a in self_point.GetComponent<NearArea>().nearPoint)
        {
            self.nearPoint.Add(a);
        }
        NextPoint_self = self.nearPoint;
        List<Transform> next_self = NextPoint_self;
        foreach (Transform nearPoint in next_self)
        {
            if (nearPoint.name == box1.name || nearPoint.name == box2.name|| nearPoint.name == player_facePoint.name)
            {
                self.nearPoint.Remove(nearPoint);
            }
            //self.nearPoint.Clear();
        }
        Debug.Log("?");   
        self.nearPoint.Clear();
    }
    private void Calculate_NextPoint_player()
    {

        foreach (Transform a in player_facePoint.GetComponent<NearArea>().nearPoint)
        {
            player.nearPoint.Add(a);
        }

        NextPoint_player = player.nearPoint;
        //player.nearPoint = player_facePoint.GetComponent<NearArea>().nearPoint;
        //NextPoint_player = player.nearPoint;
        List<Transform> next_player = NextPoint_player;
        foreach (Transform nearPoint in next_player)
        {
            if (nearPoint.name == box1.name || nearPoint.name == box2.name)
                NextPoint_player.Remove(nearPoint);
        }

    }
    private void Calculate_BestPoint()
    {
        int possibleChoice;
        float average_min = 100;
        float average;
        foreach(Transform near in NextPoint_self)
        {
            possibleChoice = near.GetComponent<NearArea>().nearPoint.Count;
            average = CalculateScore(near, possibleChoice) / NextPoint_player.Count;
            if (average < average_min)
            {
                target = near;
            }
            else if(average == average_min)
            {
                if (Random.Range(0, 10) < 5)
                    target = near;
            }
        }

    }
    private int CalculateScore(Transform next_self, int possible_choice)
    {
        List<Transform> next2point = next_self.GetComponent<NearArea>().nearPoint;
        int score = possible_choice * NextPoint_player.Count;
        foreach (Transform point in NextPoint_player)
        {
            foreach (Transform next2 in next2point)
            {
                if (point.name == next2.name)
                    score -= 1;
            }
        }
        return score;
    }
}
