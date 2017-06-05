using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGM : MonoBehaviour {

	public static StartGM instance = null;

	//MAIN MENU
	public GameObject startButton;
	public GameObject controlButton;
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


	}

	// Update is called once per frame
	void Update () {

	}

	public void ShowLevelSelect() {
		levelSelectButton.SetActive (false);
		controlButton.SetActive (false);
		levelSelectMenu.SetActive (true);
	}

	public void LoadLevel(string levelName) {
		SceneManager.LoadScene (levelName);
	}

	public void BackToHome() {
		controlButton.SetActive (true);
		levelSelectButton.SetActive (true);
		levelSelectMenu.SetActive (false);
	}
}
