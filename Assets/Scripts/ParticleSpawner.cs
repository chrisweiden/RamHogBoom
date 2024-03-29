﻿using UnityEngine;
using System.Collections;

public class ParticleSpawner : MonoBehaviour {


    //0 = Explosion
    //1 = Lightning
    //2 = Confusion
    //3 = ??
    public GameObject[] particles;

    private static ParticleSpawner instance;

   



    // Use this for initialization
    void Start () {
        instance = this;
	}

    void Awake()
    {
        instance = this;
    }


    public static ParticleSpawner Instance
    {
        get
        {
            return instance;
        }
    }

    public void SpawnParticleSystem(Transform particlePosition, int particleN)
    {
        GameObject newParticleSystem = GameObject.Instantiate(particles[particleN]);
        newParticleSystem.transform.position = particlePosition.position;

       

        StartCoroutine(DestroyParticleSystem (newParticleSystem , (int)newParticleSystem.GetComponent<ParticleSystem>().duration));

    }


    IEnumerator DestroyParticleSystem(GameObject pSystem, int t)
    {
        yield return new WaitForSeconds(t);


        if (pSystem != null)
        {
            GameObject.Destroy(pSystem);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
