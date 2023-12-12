using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioAsset : MonoBehaviour
{
    public static AudioAsset Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public AudioClip tile_walk;
    public AudioClip metal_walk;
    public AudioClip carpet_walk;
    public AudioClip wood_walk;
    public AudioClip wood2_walk;
    public AudioClip wood3_walk;
    public AudioClip stone_walk;
    public AudioClip tile2_walk;
    public AudioClip metal2_walk;
    public AudioClip paper_walk;

    public AudioClip tile_land;
    public AudioClip metal_land;
    public AudioClip metal2_land;
    public AudioClip carpet_land;
    public AudioClip wood_land;
    public AudioClip paper_land;

    public AudioClip box_push1;
    public AudioClip box_push2;
    public AudioClip box_push3;
    public AudioClip door_metal_push;
}
