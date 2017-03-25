using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {

    private static GameManager instance;

    bool gameStarted;

    public static GameManager Instance {
        get {
            if (instance = null)
                instance = Instantiate(Resources.Load("GameManager") as GameManager);
            return instance;
        }
    }

    public void GameWon() {
        print("ZWYCIĘSTWO!");
        throw new NotImplementedException();
    }

    public void GameLost() {
        print("PRZEGRANA!");
        throw new NotImplementedException();
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
