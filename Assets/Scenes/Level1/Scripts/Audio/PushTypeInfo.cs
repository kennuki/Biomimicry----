using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushTypeInfo : MonoBehaviour
{
    public enum pushType
    {
        box1,
        box2,
        box3,
        metal_door1
    }
    public static AudioClip GetAudioClip(pushType type)
    {
        switch (type)
        {
            case pushType.box1:
                return AudioAsset.Instance.box_push1;
            case pushType.box2:
                return AudioAsset.Instance.box_push2;
            case pushType.box3:
                return AudioAsset.Instance.box_push3;
            case pushType.metal_door1:
                return AudioAsset.Instance.door_metal_push;
            default:
                return null;
        }
    }
}
