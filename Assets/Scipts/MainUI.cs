using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainUI : MonoBehaviour {
    public static MainUI Instance;
    public RectTransform gameOverPanel;

	// Use this for initialization
	void Start () {
        if(Instance == null)
        {
            Instance = this;
//            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ShowGameStartPanel();
    }

    public void GameOver()
    {
        StartCoroutine(ShowGameOverScreen());
    }

    public IEnumerator ShowGameOverScreen()
    {
        float timer = 0f;
        while (timer < .5f)
        {
            float scaleVal = Mathf.Lerp(0f, 1f, timer / 0.5f);
            gameOverPanel.localScale = Vector3.one * scaleVal;
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void RestartGame()
    {
        StartCoroutine(HideGameOverScreen());
    }

    public IEnumerator HideGameOverScreen()
    {
        float timer = 0f;
        while (timer < .5f)
        {
            float scaleVal = Mathf.Lerp(1f, 0f, timer / 0.5f);
            gameOverPanel.localScale = Vector3.one * scaleVal;
            timer += Time.deltaTime;
            yield return null;
        }
        ReloadScene();
    }

    public void ShowGameStartPanel()
    {
        GM.instance.ShowStartGame();
    }
}
