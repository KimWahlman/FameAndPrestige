using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour {

    public AudioMixerSnapshot normalGameplay, duelGameplay, shameGameplay;
    public AudioClip[] stings;
    public AudioSource stingSource;
    public float bpm = 128;

    private float mTransitionIn, mTransitionOut, mQuarterNote;
	public int mCurrentGameMode = 0;
	// Use this for initialization
	void Start ()
    {
        mQuarterNote = 60 / bpm;
        mTransitionIn = mQuarterNote;
        mTransitionOut = mQuarterNote * 32;
        mCurrentGameMode = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void FixedUpdate()
    {
        if(mCurrentGameMode == 0)
        {
            normalGameplay.TransitionTo(mTransitionIn);
        }
        else if(mCurrentGameMode == 1)
        {
            duelGameplay.TransitionTo(mTransitionIn);
        }
        else if(mCurrentGameMode == 2)
        {
            shameGameplay.TransitionTo(mTransitionIn);
        }
        else
        {
            normalGameplay.TransitionTo(mTransitionOut);
            duelGameplay.TransitionTo(mTransitionOut);
            shameGameplay.TransitionTo(mTransitionOut);
        }
    }

}

