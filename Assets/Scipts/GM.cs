using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour {

	public static GM instance = null;
	public GameObject startScreen;


	//MAIN MENU
	public GameObject startButton;
	public GameObject levelSelectButton;
	public GameObject levelSelectMenu;

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

	public void LoadLevelSelect() {
		SceneManager.LoadScene ("StartMenu");
	}
//
//	public void ShowLevelSelect() {
//		levelSelectButton.SetActive (false);
//		startButton.SetActive (false);
//		levelSelectMenu.SetActive (true);
//	}
//
//	public void LoadLevel(string levelName) {
//		SceneManager.LoadScene (levelName);
//	}
}
