using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CigaretteSpawner : MonoBehaviour {

    public float heightRange = 1f;
    public float spawnTimer = 2.5f;
    public GameObject cigarettePrefab;
    private float internalTimer = 0.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(spawnTimer < internalTimer)
        {
            internalTimer = 0f;
            SpawnCigarette();
        }
        else
        {
            internalTimer += Time.deltaTime;
        }
	}

    public void SpawnCigarette()
    {
        float rand = Random.Range(this.transform.position.y - heightRange, this.transform.position.y + heightRange);
        Vector3 pos = this.transform.position;
        pos.y = rand;
        GameObject cig = Instantiate(cigarettePrefab, pos, Quaternion.identity);
        Destroy(cig, 10f);
    }
}
