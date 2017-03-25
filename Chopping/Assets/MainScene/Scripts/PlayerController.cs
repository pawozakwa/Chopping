using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    #region visible in inspector
    [Header("Parameters")]

    [Range(1, 100)]
    [Tooltip("Range of raycasting for targeted object")]
    [SerializeField]
    private int ActionRange;

    [Header("References")]

    [SerializeField]
    private Text targetActionLabel;

    [SerializeField]
    private Text woodCountLabel;

    private GameObject actualTarget;

    private HouseController houseCtrl = null;



    #endregion

    int countOfWood = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        actualTarget = rayCastForLookTarget();
	}

    GameObject rayCastForLookTarget() {
        Transform cam = Camera.main.transform;
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 500)) {
            return hit.collider.gameObject;
        }

        return null;
    }

    void updateTargetLabel(GameObject target) {
        if (!target) targetActionLabel.text = "";

        switch (target.tag) {
            case "CampFire":
                if (countOfWood > 0)
                    targetActionLabel.text = "Dorzuć drewna";
                break;
            case "Construction":
                if(houseCtrl.HouseSteps[houseCtrl.NextStep].cost <= countOfWood)
                    targetActionLabel.text = "Wybuduj następny poziom";
                break;
            case "Tree":
                targetActionLabel.text = "Rąb";
                break;
            case "Terrain":
                targetActionLabel.text = "Posadź drzewo";
                break;
            default:

                break;
        }

    }

    void updateWoodCountLabel() {

    }
    

}
