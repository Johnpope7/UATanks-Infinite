using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    [Header("Win Variables")]
    public Timer timer;

    [Header("Sound Variables")]
    public AudioSource music; //stores the audio source for music
    public AudioListener master; //stores the audiolistener that is picking up all sounds
    public List<AudioSource> sfx; //lists all sfx sound effects
    public Slider masterSlider; //stores the slider for the master sound control
    public Slider musicSlider;//stores the slider for the music sound control
    public Slider sfxSlider;//stores the slider for the sfx sound control
    [HideInInspector]
    public float masterVolume; //value of the master volume 
    [HideInInspector]
    public float sfxVolume;//value of the sfx volume
    [HideInInspector]
    public float musicVolume; //value of the music volume

    #endregion
    #region BuiltIn Functions
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
        master = FindObjectOfType<AudioListener>();
        music = GameObject.FindWithTag("Music").GetComponent<AudioSource>();
        masterSlider = GameObject.FindWithTag("MasterSlider").GetComponent<Slider>();
        musicSlider = GameObject.FindWithTag("MusicSlider").GetComponent<Slider>();
        sfxSlider = GameObject.FindWithTag("sfxSlider").GetComponent<Slider>();
        LoadOptions();
    }
    // Update is called once per frame
    void Update()
    {

    }
    #endregion
    #region Custom Functions

    void PlayerDeath()
    {
        //DO NOT DESTROY THE PLAYER,
        //JUST DISABLE THEIR COLLIDERS
    }
    //method for updating the players points
    public void UpdateScore(int _points)
    {
        score += _points;
        scoreText.text = "Score: " + score;
    }
    public void Win()
    {
        if (timer.timer == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2); //loads the next scene in the build index0
        }
    }
    #region Sound Functions
    public void LoadOptions()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", masterVolume); 
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", musicVolume);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", sfxVolume);

    }
    public void LoadOptionsGUI()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterSliderValue", masterSlider.value);
        musicSlider.value = PlayerPrefs.GetFloat("MusicSliderValue", musicSlider.value);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXSliderValue", sfxSlider.value);
    }
    public void SaveOptions()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("MasterSliderValue", masterSlider.value);
        PlayerPrefs.SetFloat("MusicSliderValue", musicSlider.value);
        PlayerPrefs.SetFloat("SFXSliderValue", sfxSlider.value);

    }

    public void CheckForVolumeChange()
    {
        masterVolume = masterSlider.value;
        sfxVolume = sfxSlider.value;
        musicVolume = musicSlider.value;
    }

    public void CheckForSoundObjects()
    {
        if (masterSlider == null)
        {
            masterSlider = GameObject.FindWithTag("MasterSlider").GetComponent<Slider>();
        }

        if (musicSlider == null)
        {
            musicSlider = GameObject.FindWithTag("MusicSlider").GetComponent<Slider>();
        }

        if (sfxSlider == null)
        {
            sfxSlider = GameObject.FindWithTag("sfxSlider").GetComponent<Slider>();
        }

        if (music == null)
        {
            music = GameObject.FindWithTag("Music").GetComponent<AudioSource>();
        }
    }
    #endregion
    #endregion
}


