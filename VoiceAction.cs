using UnityEngine;
using System.Collections;

public class VoiceAction : MonoBehaviour {

    AudioSource audioSource = null;
    AudioClip tapSound = null;
    AudioClip ringBoxSound = null;
    AudioClip moveHoldSound = null;
    AudioClip largerSound = null;
    AudioClip smallerSound = null;
    AudioClip pinkDiamond = null;
    AudioClip incubator = null;
    AudioClip treeSound = null;
    AudioClip girlSound = null;
    AudioClip incubatorNext = null;
    AudioClip bigRing = null;
    AudioClip bloodSound = null;

    bool spinObject = false;
    bool incubatorOnce = false;

    private AudioSource[] allAudioSources;
    private GameObject[] allRotatingObjets;

    // Use this for initialization
    void Start () {

        // Add an AudioSource component and set up some defaults
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialize = false;
        audioSource.spatialBlend = 1.0f;
        audioSource.dopplerLevel = 0.0f;
        audioSource.rolloffMode = AudioRolloffMode.Custom;

        // Load the Sphere sounds from the Resources folder
        tapSound = Resources.Load<AudioClip>("Select13");
        ringBoxSound = Resources.Load<AudioClip>("SlotPrize2");
        pinkDiamond = Resources.Load<AudioClip>("voice5a");
        moveHoldSound = Resources.Load<AudioClip>("Select17");
        smallerSound = Resources.Load<AudioClip>("Shrink2");
        largerSound = Resources.Load<AudioClip>("Grow");
        incubator = Resources.Load<AudioClip>("voice3a");
        treeSound = Resources.Load<AudioClip>("voice7ab");
        girlSound = Resources.Load<AudioClip>("IVONAa");
        bigRing = Resources.Load<AudioClip>("IVONA2a");
        incubatorNext = Resources.Load<AudioClip>("INC");
        bloodSound = Resources.Load<AudioClip>("sociallyresponsible");

    }

    //This spin the ringbox
    IEnumerator RotateForSeconds() //Call this method with StartCoroutine(RotateForSeconds());
    {
        float time = 2;     //How long will the object be rotated?
        while (time > 0)     //While the time is more than zero...
        {
            transform.Rotate(Vector3.up, Time.deltaTime * 800);     //...rotate the object.
            time -= Time.deltaTime;     //Decrease the time- value one unit per second.

            yield return null;     //Loop the method.
        }
        Quaternion toQuat = Camera.main.transform.localRotation;
        toQuat.x = 0;
        toQuat.z = 0;
        transform.rotation = toQuat;
    }

    void StopAllAudio()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
    }

    void PlayAudio()
    {
        StopAllAudio();
        spinObject = !spinObject;
        audioSource.clip = moveHoldSound;
        audioSource.Play();

        if (this.CompareTag("INC_Tag"))
        {
            if (!incubatorOnce)
            {
                audioSource.clip = incubator;
                incubatorOnce = true;
            } else
            {
                audioSource.clip = incubatorNext;
            }
            audioSource.Play();
        }

        if (this.CompareTag("pinkDiamond"))
        {
            audioSource.clip = pinkDiamond;
            audioSource.Play();
        }

        if (this.CompareTag("RingBox"))
        {
            audioSource.clip = ringBoxSound;
            audioSource.Play();
            StartCoroutine(RotateForSeconds());
        }

        if (this.CompareTag("Tree"))
        {
            audioSource.clip = treeSound;
            audioSource.Play();
        }

        if (this.CompareTag("BloodDiamond"))
        {
            audioSource.clip = bloodSound;
            audioSource.Play();
        }

        if (this.CompareTag("Girl"))
        {
            audioSource.clip = girlSound;
            audioSource.Play();
        }

        if (this.CompareTag("BigRing"))
        {
            audioSource.clip = bigRing;
            audioSource.Play();
        }

    }

    // Called by SpeechManager when the user says the command
    void Grow()
    {
        transform.localScale += new Vector3(.5F, .5F, .5F);
        audioSource.clip = largerSound;
        audioSource.Play();
    }

    // Called by SpeechManager when the user says the command
    void Shrink()
    {
        transform.localScale += new Vector3(-.5F, -.5F, -.5F);
        audioSource.clip = smallerSound;
        audioSource.Play();
    }

    // Called by GazeGestureManager when the user performs a Hold
    void StopSpin()
    {
        if (this.CompareTag("BigRing"))
        {
            //Do nothing
        }
        else if (this.CompareTag("Tree"))
        {
            //Do nothing
        }
        else
        {
            // Rotate object to face user.
            Quaternion toQuat = Camera.main.transform.localRotation;
            toQuat.x = 0;
            toQuat.z = 0;
            transform.rotation = toQuat;
        }
        spinObject = !spinObject;
        audioSource.clip = moveHoldSound;
        audioSource.Play();
    }



    // Update is called once per frame
    void Update () {

        if (spinObject)
        {
            if (!this.CompareTag("RingBox"))
            {
                transform.Rotate(Vector3.up, Time.deltaTime * 50);
            }
        }
    }



}
