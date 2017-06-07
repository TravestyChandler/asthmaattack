using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager Instance;
    public GameObject sfxPrefab, musicPrefab;
	// Use this for initialization
	void Start () {
	    if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySFX(AudioClip clip)
    {
        GameObject game = Instantiate(sfxPrefab, this.transform, false);
        AudioSource sour = game.GetComponent<AudioSource>();
        sour.clip = clip;
        sour.Play();
        Destroy(game, clip.length);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (GameObject.Find(clip.name) != null)
        {
            return;
        }
        else
        {
            GameObject game = Instantiate(musicPrefab, this.transform, false);
            AudioSource sour = game.GetComponent<AudioSource>();
            sour.clip = clip;
            sour.Play();
            game.name = clip.name;
            return;
        }
    }
}
