using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region visible in inspector
    [Header("Parameters")]

    [Range(1, 100)]
    [Tooltip("Range of raycasting for targeted object")]
    [SerializeField]
    private int ActionRange;

    [Range(0.1f, 2f)]
    [Tooltip("Time beetween chops")]
    [SerializeField]
    private float chopCoolDown;

    [Header("References")]

    [SerializeField]
    private Text targetActionLabel;

    [SerializeField]
    private Text woodCountLabel;

    [SerializeField]
    private GameObject TreePrefab;

    [SerializeField]
    private AudioSource axeSound;
    #endregion

    private GameObject actualTarget;
    private HouseController houseCtrl = null;
    private CampfireController campFireCtrl = null;


    int countOfWood = 0;

    bool canAddFire = false;
    bool canBuild = false;

    Vector3 plantPosition;
    bool canPlant = false;

    bool canChop = true;

    // Use this for initialization
    void Start() {
        updateWoodCountLabel();
    }

    // Update is called once per frame
    void Update() {
        actualTarget = rayCastForLookTarget();
        updateTargetLabel();
        processInput();
    }

    void processInput() {
        if (Input.GetMouseButtonDown(0)) {
            tryToDoActionToTarget();
        }
    }

    GameObject rayCastForLookTarget() {
        Transform cam = Camera.main.transform;
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, ActionRange)) {
            plantPosition = hit.point;
            return hit.collider.gameObject;
        }
        return null;
    }

    void updateTargetLabel() {
        if (!actualTarget) {
            targetActionLabel.text = "";
            return;
        }

        switch (actualTarget.tag) {
            case "CampFire":
                updateLabelForFire();
                break;
            case "Construction":
                updateLabelForContruction();
                break;
            case "Tree":
                targetActionLabel.text = "Rąb";
                break;
            case "Terrain":
                updateLabelForPlant();
                break;
        }
    }

    private void updateLabelForPlant() {
        if (checkForGameStarted()) return;

        canPlant = countOfWood > 1;
        if (canPlant) {
            targetActionLabel.text = "Posadź drzewo";
        }
        else {
            targetActionLabel.text = "Za mało drewna by posadzić kolejne drzewo";
        }
    }

    private void updateLabelForContruction() {
        if (checkForGameStarted()) return;

        if (houseCtrl == null) {
            houseCtrl = actualTarget.GetComponent<HouseController>();
        }
        canBuild = houseCtrl.canBuild(countOfWood);
        if (canBuild)
            targetActionLabel.text = "Wybuduj następny poziom";
        else
            targetActionLabel.text = "Za mało drewna żeby budować";
    }

    private bool checkForGameStarted() {
        if (!GameManager.Instance.GameStarted) {
            targetActionLabel.text = "Najpierw rozpal ognisko";
            return true;
        }
        return false;
    }

    private void updateLabelForFire() {
        if (campFireCtrl == null) {
            campFireCtrl = actualTarget.GetComponent<CampfireController>();
        }
        canAddFire = countOfWood > 0;
        if (canAddFire)
            targetActionLabel.text = "Dorzuć drewna";
        else
            targetActionLabel.text = "Nie masz drewna do ogniska";
    }

    void tryToDoActionToTarget() {
        if (!actualTarget) return;
        switch (actualTarget.tag) {
            case "CampFire":
                tryToAddFire();
                break;
            case "Construction":
                tryToBuild();
                break;
            case "Tree":
                tryToChop();
                break;
            case "Terrain":
                tryToPlant();
                break;
        }
    }

    private void tryToAddFire() {
        if (canAddFire) {
            campFireCtrl.AddWood(2);
            countOfWood -= 2;
            updateWoodCountLabel();
        }
    }

    private void tryToBuild() {
        if (canBuild && houseCtrl.initUpgrade()) {
            countOfWood -= houseCtrl.HouseSteps[houseCtrl.Step].cost;
            updateWoodCountLabel();
        }
    }

    private void tryToChop() {
        if (canChop) {
            axeSound.Play();
            TreeController tc = actualTarget.GetComponent<TreeController>();
            countOfWood += tc.Chop();
            updateWoodCountLabel();
            StartCoroutine(countCooldownForChop());
        }
    }

    private void tryToPlant() {
        if (canPlant) {
            var newTree = Instantiate(TreePrefab).transform;
            newTree.eulerAngles = new Vector3(0f, Random.Range(0, 359));
            newTree.position = plantPosition;
            countOfWood--;
            updateWoodCountLabel();
        }
    }

    IEnumerator countCooldownForChop() {
        yield return new WaitForSeconds(chopCoolDown);
        canChop = true;
    }

    void updateWoodCountLabel() {
        woodCountLabel.text = (Mathf.Clamp(countOfWood, 0, 1000000).ToString());
    }


}
