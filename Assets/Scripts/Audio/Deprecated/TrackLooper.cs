using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackLooper : MonoBehaviour
{
    [SerializeField]
    private AudioClip startingTrack;

    [SerializeField]
    private AudioClip loopingTrack;

    [SerializeField]
    private AudioSource player;
    // Start is called before the first frame update

    private bool looping;
    void Awake()
    {
        player.clip = startingTrack;
        player.Play();
        looping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!looping && !player.isPlaying)
        {
            player.clip = loopingTrack;
            player.Play();
            player.loop = true;
            looping = true;
        }
    }
}
