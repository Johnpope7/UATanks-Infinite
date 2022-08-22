using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour 
{
    #region Variables
    //our gamemanager instance
    public static GameManager instance;

    [Header("Player Variables")]
    public int score; //the players score
    public Text scoreText;
    public int currentPlayers; //the current amount of players spawned
    public int maxPlayers; //the maximum amount of players in the game
    public float playerSpawnDelayTimer;//how long until the player spawns
    public List<GameObject> playerSpawners; //list of all player spawners
    public List<GameObject> playerPrefabs; //list of all possible prefabs for the player
    public List<GameObject> playerList; //list of all players in the game

    [Header("Enemy Variables")]
    public List<GameObject> enemyList; //list all enemies in the level
    public List<GameObject> enemyPrefabs; //lists all enemy prefabs that can spawn as enemies
    public List<GameObject> enemySpawners; //lists all enemy spawners
    public GameObject playertarget; //stores the game object of the player
    public int currentEnemies; //an int for storing the amount of enemies currently in the level
    public int maxEnemies; //int that stores the maximum amount of enemies allowed per level
    [SerializeField]
    public float enemySpawnDelayTimer; //cooldown timer for enemy spawns

    [Header("Pickup Variables")]
    public float currentPickUps; //stores the amount of powerups on the map
    public List<GameObject> livePickUps; //list of pickups on the ground
    public List<Transform> pickupSpawners;
    public List<GameObject> pickupPrefabs;
    public float pickupSpawnDelay = 10.0f;
    public float maxPickUps = 4.0f;

    [Header("Map Variables")]
    [SerializeField]
    public MapGenerator mapGenerator;
    public Room[,] grid;//map tile grid array
    public bool isMapOfTheDay = false;//bool for map of the day seed
    public bool isRandomMap = true; //bool for random map seed

    #endregion
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
