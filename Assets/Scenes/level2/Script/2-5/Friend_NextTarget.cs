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
    private Transform box1;
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
       // Debug.Log(target);
    }
    private void GetPointInfo()
    {
        self_point = AllArea.Instance.friend_point;
        box1 = AllArea.Instance.box1_point;
    }
    private void Calculate_NextPoint_self()
    {
        foreach(Transform a in self_point.GetComponent<NearArea>().nearPoint)
        {
            self.nearPoint.Add(a);
        }
        NextPoint_self = self.nearPoint;
        self.nearPoint.Remove(box1);
        self.nearPoint.Remove(player_facePoint);
    }
    private void Calculate_NextPoint_player()
    {

        foreach (Transform a in player_facePoint.GetComponent<NearArea>().nearPoint)
        {
            player.nearPoint.Add(a);
        }
        NextPoint_player = player.nearPoint;
        player.nearPoint.Remove(box1);
    }
    private void Calculate_BestPoint()
    {
        float possibleChoice;
        float average_max = -1;
        float average;
        foreach(Transform near in NextPoint_self)
        {
            possibleChoice = near.GetComponent<NearArea>().nearPoint.Count;
            foreach(Transform near2 in near.GetComponent<NearArea>().nearPoint)
            {
                if (near2.name == box1.name)
                    possibleChoice -= 1;
            }
            average = CalculateScore(near, possibleChoice) / NextPoint_player.Count;
            if (average > average_max)
            {
                target = near;
                average_max = average;
            }
        }
        AllArea.Instance.friend_next_point = target;
    }
    private float CalculateScore(Transform next_self, float possible_choice)
    {
        List<Transform> next2point = next_self.GetComponent<NearArea>().nearPoint;
        float score = possible_choice * NextPoint_player.Count;
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
