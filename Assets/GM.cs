using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour {

	public static GM instance = null;
	public GameObject startScreen;

	void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
	}

	// Use this for initialization
	void Start () {
		startScreen.SetActive (true);
		Time.timeScale = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

	}
    public void ShowStartGame()
    {
        startScreen.SetActive(true);
        Time.timeScale = 0.0f;
    }

	public void StartGame() {
		startScreen.SetActive (false);
        PlayerController.Instance.currentPhase = PlayerController.GamePhase.Playing;
		Time.timeScale = 1.0f;
	}
}
