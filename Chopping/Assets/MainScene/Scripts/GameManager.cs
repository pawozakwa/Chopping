using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {

    private static GameManager instance;
    #region visible in inspector
    [Header("References")]

    [SerializeField]
    private GameObject winScreen;

    [SerializeField]
    private GameObject loseScreen;

    [SerializeField]
    private GameObject Player;

    #endregion


    bool gameStarted;
    bool gameFinished;

    GameManager() {
        instance = this;
    }

    public static GameManager Instance {
        get {
            return instance;
        }
    }

    public bool GameStarted {
        get {
            return gameStarted;
        }
    }

    public void StartGame() {
        gameStarted = true;
        gameFinished = false;
    }

    public void GameWon() {
        if (gameFinished) return;
        print("ZWYCIĘSTWO!");
        winScreen.SetActive(true);
    }

    public void GameLost() {
        if (gameFinished) return;
        print("PRZEGRANA!");
        loseScreen.SetActive(true);
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
