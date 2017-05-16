using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
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

    public Slider breathSlider;
	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if(IsGrounded() && canJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        if (IsGrounded())
        {
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
                rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);
            }
        }
        Debug.Log(rb.velocity.x);
        if(Mathf.Abs(rb.velocity.x) > 0.1f)
        {
           breathMeter = Mathf.Clamp(breathMeter - (Time.deltaTime * breathChange), 0f, 100f);
        }
        else
        {
           breathMeter = Mathf.Clamp(breathMeter + (Time.deltaTime * breathChange), 0f, 100f);
        }
        breathSlider.value = breathMeter;
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
    }
}
