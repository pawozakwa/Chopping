using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireController : MonoBehaviour {

    #region visible in inspector 
    [Header("Parameters")]

    [Range(1, 10)]
    [Tooltip("Count of wood for one level of visualisation")]
    [SerializeField]
    private int WoodPerLevel;

    [Range(0.05f, 10.0f)]
    [Tooltip("Time of burning one piece of wood")]
    [SerializeField]
    private float TimeOfBurning;


    [Header("References")]
    [SerializeField]
    private GameObject[] woodParts;

    [SerializeField]
    private Light fireLight;

    [SerializeField]
    private AudioSource fireSound;
    #endregion

    float countOfWood;
    bool burning = false;

    public bool Burning {
        get { return burning; }
    }

    // Use this for initialization
    void Start () {
		
	}

    public void SetAFire() {
        GameManager.Instance.StartGame();
        burning = true;
        fireLight.gameObject.SetActive(true);
        fireSound.Play();
        StartCoroutine(fireCoroutine());
    }

    public void AddWood(int wood) {
        countOfWood += wood;
        if (!burning) {
            SetAFire();
        }
        updateState();
    }

    void updateState() {
        int fireLevel = (int)((float)countOfWood / (float)WoodPerLevel);
        if (countOfWood > 0 && fireLevel == 0) {
            fireLevel = 0;
            fireSound.Stop();
        }
        for (int i = 0; i < woodParts.Length; i++) {
                woodParts[i].SetActive(i < fireLevel);
        }
        fireLight.range = fireLevel * 10;
    }

    IEnumerator fireCoroutine() {
        while(countOfWood > 0) {
            yield return new WaitForSeconds(TimeOfBurning);            
            countOfWood--;
            updateState();
        }
        updateState();
        GameManager.Instance.GameLost();
    }
    	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
        checkForDebugInput();
#endif
    }

    void checkForDebugInput() {
        if (Input.GetKey("f")) {
            if (!burning)
                SetAFire();
            else
                AddWood(1);
        }
    }
}
