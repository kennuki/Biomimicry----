using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AnimeTrigger : EventTriggerFunction
{
    public PlayableDirector director;
    public override void Enter()
    {
        if (Trigger)
        {
            director.Play();
            Character.ActionProhibit = true;
            Character.AllProhibit = true;
            int layerIndex = LayerMask.NameToLayer("Character");
            Camera.main.cullingMask |= 1 << layerIndex;
            this.enabled = false;
        }
    }
}
