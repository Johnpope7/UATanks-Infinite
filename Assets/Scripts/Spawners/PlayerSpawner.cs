using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : Spawner
{
    #region Functions
    #region Builtin Functions
    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        if (GameManager.instance)
        {
            nextSpawnTime = Time.time + GameManager.instance.playerSpawnDelayTimer;
            GameManager.instance.playerSpawners.Add(gameObject);
        }
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        //and if the time is greater than or equal to the next set spawn time
        if (GameManager.instance.currentPlayers < GameManager.instance.maxPlayers)
        {
            if (Time.time >= nextSpawnTime)
            {
                //declare a random int between 0 and the max number of enemy prefabs in their list
                int random = Random.Range(0, GameManager.instance.playerSpawners.Count);
                //instantiate an enemy using our random int at this objects position
                GameObject player = Instantiate(GameManager.instance.playerPrefabs[random], tf.position, tf.rotation);
                //name it something meaningful, in this case, the name of the prefab it chose
                //and the number it is in the current enemies count
                player.name = GameManager.instance.enemyPrefabs[random] + "_" + GameManager.instance.currentEnemies;
                //add it to the GameManager's list of enemies
                GameManager.instance.playerList.Add(player);
                //set the next spawn time equal to now plus enemy spawn delay
                //nextSpawnTime = Time.time + GameManager.instance.playerSpawnDelayTimer;
                GameManager.instance.currentPlayers++;
            }

        }

        base.Update();
    }
    #endregion
    #region Custom Functions
    protected override void OnDestroy()
    {
        GameManager.instance.playerSpawners.Remove(gameObject);
        base.OnDestroy();
    }
    #endregion
    #endregion
}
