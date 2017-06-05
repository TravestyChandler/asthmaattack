using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour {

	private enum STATE {RUN, STAY};
	STATE currentState;

	public float runTimer;
	public float stayTimer;

	public Rigidbody2D rb;
	Vector3 currentDirection = new Vector3 (-1.0f, 0f, 0f);
	public float speed = 1f;

	//ANIMATION
	private Animator anim;
	public GameObject dog;

	// Use this for initialization
	void Start () {
		currentState = STATE.RUN;
		rb = this.GetComponent<Rigidbody2D> ();
		anim = this.GetComponentInChildren<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
		switch (currentState) {
		case STATE.RUN:
			UpdateRun ();
			break;
		case STATE.STAY:
			UpdateStay ();
			break;
		}

		//ANIMATIONS
		anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
		if (rb.velocity.x > 0.1f) {
			if (dog.transform.localScale.x >= 0.0f) {
				dog.transform.localScale = new Vector3 (-(dog.transform.localScale.x), dog.transform.localScale.y, dog.transform.localScale.z);
			}
		}
		if (rb.velocity.x < -0.1f) {
			print ("FLIP");
			if (dog.transform.localScale.x <= 0.0f) {
				dog.transform.localScale = new Vector3 (-(dog.transform.localScale.x), dog.transform.localScale.y, dog.transform.localScale.z);
			}
			dog.transform.localScale = new Vector3 ((dog.transform.localScale.x), dog.transform.localScale.y, dog.transform.localScale.z);
		}
	}

	private void EnterRunState() {
		currentState = STATE.RUN;
//		runTimer = Random.Range (1f, 3f);
		runTimer = 3f;
		currentDirection = -(currentDirection);
		print (currentDirection);
		UpdateRun ();
	}
	private void UpdateRun() {
		runTimer -= Time.deltaTime;
		print ("IM RUNNING");
//		currentDirection = new Vector3 (-1.0f, 0f, 0f);
		rb.velocity = currentDirection * speed;

		if (runTimer <= 0f) {
			EnterStayState ();
		}
	}

	private void EnterStayState() {
		currentState = STATE.STAY;
		rb.velocity = Vector2.zero;
//		stayTimer = Random.Range (1f, 5f);
		stayTimer = 2f;
		UpdateStay ();
	}
	private void UpdateStay() {
		stayTimer -= Time.deltaTime;

		if (stayTimer <= 0f) {
			EnterRunState ();
		}
	}

	void OnCollisionEnter2D(Collision2D c) {
		if (c.gameObject.tag == "obstacle") {
			EnterStayState ();
		}
	}

}
