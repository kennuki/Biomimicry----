using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart2_5 : EventTriggerFunction
{
    public ThirdView thirdView;
    public Friend_Move friend;
    public AudioSource source;
    public override void Enter()
    {
        if (Trigger)
        {
            source.Play();
            Trigger = false;
            thirdView.enabled = true;
            friend.enabled = true;
            Character.LookState = 3;
            this.enabled = false;
        }
    }
}
