using UnityEngine;
using System.Collections;
using UnityEngine.Audio;


/*
 * TODO (KIM): Fix so the audio isn't playing all the time? 
 */

public class MusicController : MonoBehaviour {

    public AudioMixerSnapshot normalGameplay, duelGameplay, shameGameplay, silence;
    //public AudioClip[] stings;
    public AudioSource[] mSource;
    public float bpm = 128;
    private float mTransitionIn, mTransitionOut, mQuarterNote;
    public string mCurrentGameMode;
	// Use this for initialization
	void Start ()
    {
        mQuarterNote = 60 / bpm;
        mTransitionIn = mQuarterNote;
        mTransitionOut = mQuarterNote * 32;
        mCurrentGameMode = "normal";
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    void FixedUpdate()
    {
        if (mCurrentGameMode == "normal") // 0
        {
            
            normalGameplay.TransitionTo(mTransitionIn);
        }
        else if (mCurrentGameMode == "duel") // 1
        {

            duelGameplay.TransitionTo(mTransitionIn);
        }
        else if (mCurrentGameMode == "shame") // 2
        {
            shameGameplay.TransitionTo(mTransitionIn);
        }
        else
        {
            Debug.Log("Hmpf");
            silence.TransitionTo(0.25f);
        }
    }

}

