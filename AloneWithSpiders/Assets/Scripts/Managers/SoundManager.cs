using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioMixerGroup mixerGroup;

    public AudioSource source;
    public AudioClip footsteps;
    public Transform playerPos;


    private Vector3 previousPlayerPos = Vector3.zero;
    private float footstepInterval = 1f;
    private float lastFootstep = 1.5f;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        source.outputAudioMixerGroup = mixerGroup;
    }

    private void Update(){
        if(lastFootstep>0){
            lastFootstep= lastFootstep- Time.deltaTime;
            if(playerPos.position == previousPlayerPos){
                source.Stop();
                lastFootstep=0f;
            }
        }
        else{
            if(playerPos.position != previousPlayerPos){
                previousPlayerPos = playerPos.position;

                activateFootsteps();
            }
        }
        

    }

    public void activateFootsteps(){
        lastFootstep=footstepInterval;
        source.PlayOneShot(footsteps);
    }
}
