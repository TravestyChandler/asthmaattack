using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour {

	private enum STATE {RUN, STAY};
	STATE currentState;

	public float runTimer;
	public float stayTimer;

	// Use this for initialization
	void Start () {
		
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
	}

	private void EnterRunState() {
		currentState = STATE.RUN;
		runTimer = Random.Range (1f, 3f);
		UpdateRun ();
	}
	private void UpdateRun() {
		runTimer -= Time.deltaTime;

		if (runTimer <= 0f) {
			EnterStayState ();
		}
	}

	private void EnterStayState() {
		currentState = STATE.STAY;
		stayTimer = Random.Range (1f, 5f);
		UpdateStay ();
	}
	private void UpdateStay() {
		stayTimer -= Time.deltaTime;

		if (stayTimer <= 0f) {
			EnterRunState ();
		}
	}
}
