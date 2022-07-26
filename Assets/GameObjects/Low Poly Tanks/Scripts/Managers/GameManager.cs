using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour 
{
    //our gamemanager instance
    public static GameManager instance;

    [Header("Player Score")]
    public int score; //the players score
    public Text scoreText;

    [Header("Enemy Variables")]
    public List<GameObject> enemyList; //list all enemies in the level
    public List<GameObject> enemyPrefabs; //lists all enemy prefabs that can spawn as enemies
    public List<GameObject> enemySpawners; //lists all enemy spawners
    public List<Transform> waypoints; //list of waypoints for enemy patrols
    public GameObject playertarget; //stores the game object of the player
    public int currentEnemies; //an int for storing the amount of enemies currently in the level
    public int maxEnemies; //int that stores the maximum amount of enemies allowed per level
    public float enemyspawnDelayTimer; //cooldown timer for enemy spawns

    void PlayerDeath() 
    {
        //DO NOT DESTROY THE PLAYER,
        //JUST DISABLE THEIR COLLIDERS
    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
        //set the initial text in the score ui to say 0
        scoreText.text = "Score: 0" + score;
        //actually set score to zero
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //method for updating the players points
    public void UpdateScore(int _points) 
    {
        score += _points;
        scoreText.text = "Score: " + score;
    }
}
