﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

[Serializable]
public struct PartCostPair
{
   public GameObject build;
   [Range(0, 100)]
   public int cost;
}

public class HouseController : MonoBehaviour {

    #region visible in inspector

    [Header("Parameters")]
    [Range(1, 60)]
    [Tooltip("Time of upgrading one level of house in seconds")]
    [SerializeField]
    private float buildingTime;

    [Header("References & costs")]
    [SerializeField]
    PartCostPair[] houseSteps;

    [SerializeField]
    ParticleSystem clickParticles;

    [SerializeField]
    AudioSource buildSound;

    public PartCostPair[] HouseSteps {
        get { return houseSteps; }
    }

    public bool Building {
        get { return building; }
    }

    #endregion

    bool building = false;
    int actualStep = 0;

    public int NextStep {
        get { return actualStep + 1; }
    }

    public int Step {
        get { return actualStep; }
    }


    public bool initUpgrade() {
        if (building) return false;
        building = true;
        clickParticles.Play();
        actualStep++;
        houseSteps[actualStep].build.transform.DOMoveY(0.0f, buildingTime).OnComplete(finishBuildStep);
        buildSound.Play();
        return true;
    }

    public void finishBuildStep() {
        if(NextStep > houseSteps.Length - 1) {
            GameManager.Instance.GameWon();
        }
        building = false;
    }

    public bool canBuild(int countOfOwnedWood) {
        return (NextStep < houseSteps.Length &&           houseSteps[NextStep].cost <= countOfOwnedWood);        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
        checkForDebugInput();
#endif
    }

    void checkForDebugInput() {
        if (Input.GetKeyDown("x")) {
            initUpgrade();
        }
    }

}
