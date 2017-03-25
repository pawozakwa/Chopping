using System.Collections;
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

    public PartCostPair[] HouseSteps {
        get { return houseSteps; }
    }

    public bool Building {
        get { return building; }
    }

    #endregion

    bool building;
    int actualStep = 0;

    public int NextStep {
        get { return actualStep + 1; }
    }
    

    public void initUpgrade() {
        if (building) return;
        actualStep++;
        houseSteps[actualStep].build.transform.DOMoveY(0.0f, buildingTime).OnComplete(finishBuildStep);
    }

    public void finishBuildStep() {
        building = false;
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
        if (Input.GetKey("x")) {
            initUpgrade();
        }
    }

}
