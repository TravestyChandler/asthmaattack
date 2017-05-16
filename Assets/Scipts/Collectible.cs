using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {
    public enum CollectibleType
    {
        Good,
        Bad,
        Invalid
    }

    public CollectibleType collType = CollectibleType.Good;
    public float collectibleBreathChange = 20f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
