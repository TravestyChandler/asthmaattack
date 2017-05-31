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
	public float breathInhalerChange = 10f;
    public RectTransform gameOverPanel;
    public RectTransform levelCompletePanel;
    public Slider breathSlider;

	//INHALER
	public int inhalerCharges = 2;
	private bool takingInhaler = false;
	private float hazardDecrement = 0f;
	private float hazardTimer = 0.5f;
	public float inhalerTimer = 5.0f;
	public bool inhalerTaken = false;
	//INHALER UI
	public Text inhalerText;

	//ANIMATION
	private Animator anim;
	public GameObject player;
	// Use this for initialization
	void Start () {
        currentPhase = GamePhase.Playing;
        rb = this.GetComponent<Rigidbody2D>();
		anim = this.GetComponentInChildren<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool ("Inhaler", takingInhaler);
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
//                breathMeter = Mathf.Clamp(breathMeter + (Time.deltaTime * breathChange), 0f, 100f);
            }
            breathSlider.value = breathMeter;
            if (Input.GetKeyDown(KeyCode.Backslash))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

			//INHALER FUNCTIONALITY
			if (Input.GetKeyDown (KeyCode.Q)) {
				if (IsGrounded ()) {
					if (inhalerCharges > 0) {
						takingInhaler = true;
						inhalerTaken = true;
//						if (breathMeter + 50 >= 100) {
//							breathMeter = 100;
//						} else {
//							breathMeter = breathMeter + 50;
//						}
						inhalerCharges--;
					}
					anim.SetTrigger ("ExitInhaler");
					Invoke ("WaitForInhaler", 1.0f);
				}
			}

			if (inhalerTaken == true) {
				breathMeter = Mathf.Clamp(breathMeter + (Time.deltaTime * breathInhalerChange), 0f, 100f);
				Invoke ("InhalerDuration", inhalerTimer);
			}

			//INHALER UI
			if (inhalerCharges == 2) {
				inhalerText.text = "x 2";
			} else if (inhalerCharges == 1) {
				inhalerText.text = "x 1";
			} else if (inhalerCharges == 0) {
				inhalerText.text = "x 0";
			}

			//HAZARD DECREMENT (every second your inside)
			hazardTimer -= Time.deltaTime;
			if (hazardTimer <= 0) {
				breathMeter -= hazardDecrement;
				hazardTimer = 0.5f;
			}


			//ANIMATIONS
			anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
			if (rb.velocity.x < -0.1f) {
				if (player.transform.localScale.x >= 0.0f) {
					player.transform.localScale = new Vector3 (-(player.transform.localScale.x), player.transform.localScale.y, player.transform.localScale.z);
				}
			}
			if (rb.velocity.x > 0.1f) {
				print ("FLIP");
				if (player.transform.localScale.x <= 0.0f) {
					player.transform.localScale = new Vector3 (-(player.transform.localScale.x), player.transform.localScale.y, player.transform.localScale.z);
				}
				player.transform.localScale = new Vector3 ((player.transform.localScale.x), player.transform.localScale.y, player.transform.localScale.z);
			}
			anim.SetBool ("Grounded", IsGrounded ());
			anim.SetFloat ("Breath", breathMeter);
        }
	}
	public void WaitForInhaler() {
		takingInhaler = false;
	}
	public void InhalerDuration() {
		inhalerTaken = false;
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
		if (col.tag == "hazard") {
			hazardDecrement = 15f;
		}
        if(col.tag == "endoflevel")
        {
            currentPhase = GamePhase.Victory;
            LevelComplete();
        }
    }

	public void OnTriggerExit2D(Collider2D col) {

		if (col.tag == "hazard") {
			hazardDecrement = 0f;
		}
	}
}
