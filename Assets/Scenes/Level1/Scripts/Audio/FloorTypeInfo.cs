using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTypeInfo
{
    public enum floorType
    {
        Tile,
        Metal,
        Wood,
        Carpet
    }
    public static AudioClip GetAudioClipWalk(floorType type)
    {
        switch (type)
        {
            case floorType.Tile:
                return AudioAsset.Instance.tile_walk;
            case floorType.Metal:
                return AudioAsset.Instance.metal_walk;
            case floorType.Wood:
                return AudioAsset.Instance.wood_walk;
            case floorType.Carpet:
                return AudioAsset.Instance.carpet_walk;
            default:
                return null;
        }
    }
    public static AudioClip GetAudioClipLand(floorType type)
    {
        switch (type)
        {
            case floorType.Tile:
                return AudioAsset.Instance.tile_land;
            case floorType.Metal:
                return AudioAsset.Instance.metal_land;
            case floorType.Wood:
                return AudioAsset.Instance.wood_land;
            case floorType.Carpet:
                return AudioAsset.Instance.carpet_land;
            default:
                return null;
        }
    }
}
