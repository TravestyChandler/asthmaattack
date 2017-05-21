using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public enum GamePhase
    {
        Playing,
        Victory,
        GameOver,
        Invalid
    }

    public GamePhase currentPhase;
    public float movementSpeed = 5f;
    public float stopSpeed = 0.5f;
    public float jumpSpeed = 5f;
    public bool canJump = true;
    public LayerMask groundLayers;
    public Transform circleCastPosition;
    public float circleCastRadius = 0.5f;
    private Rigidbody2D rb;

    public float breathMeter = 100f;
    public float breathChange = 0.5f;
    public RectTransform gameOverPanel;
    public RectTransform levelCompletePanel;
    public Slider breathSlider;

	//ANIMATION
	private Animator anim;
	// Use this for initialization
	void Start () {
        currentPhase = GamePhase.Playing;
        rb = this.GetComponent<Rigidbody2D>();
		anim = this.GetComponentInChildren<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
        
        if (currentPhase == GamePhase.Playing)
        {
            if (breathMeter <= 0f)
            {
                currentPhase = GamePhase.GameOver;
                GameOver();
                return;
            }
            if (IsGrounded() && canJump)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }
            }
            float speedMutliplier = (IsGrounded()) ? 1f : 0.5f;
            if (Input.GetKeyUp(KeyCode.D))
            {
                rb.velocity = new Vector2(stopSpeed, rb.velocity.y);
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                rb.velocity = new Vector2(-stopSpeed, rb.velocity.y);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(movementSpeed * speedMutliplier, rb.velocity.y);
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-movementSpeed * speedMutliplier, rb.velocity.y);
            }
            Debug.Log(rb.velocity.x);
            if (Mathf.Abs(rb.velocity.x) > 0.1f)
            {
                breathMeter = Mathf.Clamp(breathMeter - (Time.deltaTime * breathChange), 0f, 100f);
            }
            else
            {
                breathMeter = Mathf.Clamp(breathMeter + (Time.deltaTime * breathChange), 0f, 100f);
            }
            breathSlider.value = breathMeter;
            if (Input.GetKeyDown(KeyCode.Backslash))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

			//ANIMATIONS
			anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
			if (rb.velocity.x < -0.1f) {
				transform.localScale = new Vector3 (-(transform.localScale.x), transform.localScale.y, transform.localScale.z);
			}
			if (rb.velocity.x > 0.1f) {
				print ("FLIP");
				transform.localScale = new Vector3 ((transform.localScale.x), transform.localScale.y, transform.localScale.z);
			}
        }
	}
    public void LevelComplete()
    {
        StartCoroutine(LevelCompleteScreen());
    }

    public IEnumerator LevelCompleteScreen()
    {
        float timer = 0f;
        while (timer < .5f)
        {
            float scaleVal = Mathf.Lerp(0f, 1f, timer / 0.5f);
            levelCompletePanel.localScale = Vector3.one * scaleVal;
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void GameOver()
    {
        StartCoroutine(ShowGameOverScreen());
    }
    public IEnumerator ShowGameOverScreen()
    {
        float timer = 0f;
        while(timer < .5f)
        {
            float scaleVal = Mathf.Lerp(0f, 1f, timer / 0.5f);
            gameOverPanel.localScale = Vector3.one * scaleVal;
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public bool IsGrounded()
    {
        return (Physics2D.OverlapCircle(circleCastPosition.position, circleCastRadius, groundLayers)) != null;
    }

    public void Jump()
    {
        canJump = false;
        rb.velocity += new Vector2(0f, jumpSpeed);
        StartCoroutine(JumpWait());
    }

    public IEnumerator JumpWait()
    {
        float timer = 0f;
        float maxTime = 0.1f;
        while (timer < maxTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        canJump = true;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "collectible")
        {
            GameObject game = col.gameObject;
            Collectible collect = game.GetComponent<Collectible>();
            if(collect.collType == Collectible.CollectibleType.Good)
            {
                breathMeter += collect.collectibleBreathChange;
            }
            if (collect.collType == Collectible.CollectibleType.Bad)
            {
                breathMeter -= collect.collectibleBreathChange;
            }
            Destroy(game);
        }
        if(col.tag == "endoflevel")
        {
            currentPhase = GamePhase.Victory;
            LevelComplete();
        }
    }
}
