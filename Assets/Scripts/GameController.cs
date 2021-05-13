/*
Name: Marc Domingo
Student ID: 2346778
Chapman Email: mdomingo@chapman.edu
Course Number and Section: 236-03
Assignment: Final Project
This is my own work, and I did not cheat on this assignment.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

// The following class represents the functionality of a 2D Platformer's UI and contains functions to simulate the aspects of the game such as scorekeeping, button functionality, and records the player's stats for an attempt on a text file.
public class GameController : MonoBehaviour
{
    public GameObject playerEntity;
    public PlayerCheckpoint startingPosition;
    public PlayerController player;
    public List<EnemyController> enemyList = new List<EnemyController>();
    public List<PlayerCheckpoint> checkpointList = new List<PlayerCheckpoint>();
    public Text deathCounter;
    public Text timeCounter;
    public Text alertedCounter;

    private int numberOfEnemiesAlerted;
    private bool hasFinishedCounting;
    private static string combinationFileName = "Recorded_Attempts.txt";
    private static string combinationFolderName = "Assets/Text";
    private static string combinationPath
    {
        get
        {
            return (Path.Combine(combinationFolderName, combinationFileName));
        }
    }

    private float timeSinceStart;
    private bool gameIsActive;
    private bool attemptWasRecorded;

    public GameObject playButton;
    public GameObject resetButton;
    public GameObject quitButton;

    // Start is called before the first frame update
    void Start()
    {
        numberOfEnemiesAlerted = 0;
        hasFinishedCounting = false;
        EnsureFilepathExists();
        startingPosition.isActive = true;
        playerEntity.SetActive(false);
        resetButton.SetActive(false);
        alertedCounter.text = "Alerted: counting...";
        Debug.Log("Press 'Play' to Start!");
    }

    // Update is called once per frame
    void Update()
    {

        if (gameIsActive == true && player.hasEscaped == false)
        {
            StopAllCoroutines();
            StartCoroutine(inGameTimer());
        }

        deathCounter.text = "Deaths: " + player.numberOfDeaths;
        timeCounter.text = "Time: " + timeSinceStart;

        if (player.hasEscaped == true)
        {
            if (hasFinishedCounting == false)
            {
                countAlertedEnemies();
            }
            alertedCounter.text = "Alerted: " + numberOfEnemiesAlerted + " of 5";
            resetButton.SetActive(true);
            if (attemptWasRecorded == false)
            {
                WriteFile(timeSinceStart, player.numberOfDeaths, numberOfEnemiesAlerted);
            }
        }
    }

    public void OnPlayButtonPress()
    {
        timeSinceStart = 0f;
        numberOfEnemiesAlerted = 0;
        hasFinishedCounting = false;
        gameIsActive = true;
        attemptWasRecorded = false;
        playerEntity.SetActive(true);
        player.resetPlayer();
        alertedCounter.text = "Alerted: counting...";
        quitButton.SetActive(false);
        playButton.SetActive(false);
        resetButton.SetActive(true);

        foreach (EnemyController enemy in enemyList)
        {
            enemy.resetEnemy();
        }

        foreach (PlayerCheckpoint checkpoint in checkpointList)
        {
            checkpoint.isActive = false;
        }
    }

    public void OnResetButtonPress()
    {
        playerEntity.SetActive(false);
        playButton.SetActive(true);
        quitButton.SetActive(true);
        gameIsActive = false;
    }

    public void OnQuitButtonPress()
    {
        playerEntity.SetActive(false);
        playButton.SetActive(false);
        resetButton.SetActive(false);
        quitButton.SetActive(false);
        gameIsActive = false;
        Debug.Log("You have quit the application.");
        Application.Quit();
    }

    IEnumerator inGameTimer()
    {
        while (gameIsActive == true && player.hasEscaped == false)
        {
            timeSinceStart += Time.deltaTime;
            yield return null;
        }
    }
    private static void EnsureFilepathExists()
    {
        if (!Directory.Exists(combinationFolderName))
            Directory.CreateDirectory(combinationFolderName);
    }

    private void WriteFile(float timeTaken, int numberOfDeaths, int numberOfAlertedEnemies)
    {
        using (StreamWriter writer = new StreamWriter(combinationPath, true))
        {
            writer.WriteLine("Attempt at: " + DateTime.Now);
            writer.WriteLine("Time taken to escape: " + timeTaken + " seconds");
            writer.WriteLine("Number of deaths before escaping: " + numberOfDeaths);
            writer.WriteLine("Number of enemies alerted before escaping: " + numberOfAlertedEnemies + " of 5 enemies");
            writer.WriteLine();
            attemptWasRecorded = true;
        }
    }

    private void countAlertedEnemies()
    {
        foreach (EnemyController enemy in enemyList)
        {
            if (enemy.hasBeenAlerted == true)
            {
                numberOfEnemiesAlerted++;
            }

            else
            {
                continue;
            }
        }
        hasFinishedCounting = true;
    }
}

