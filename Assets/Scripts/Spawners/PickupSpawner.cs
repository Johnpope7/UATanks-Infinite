using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : Spawner
{
    protected override void Awake() 
    {
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        //and if the time is greater than or equal to the next set spawn time
        if (Time.time >= nextSpawnTime)
        {
            //declare a random int between 0 and the max number of pickup prefabs in their list
            int random = Random.Range(0, GameManager.instance.pickupPrefabs.Count);
            //instantiate a pickup using our random int at this objects position
            GameObject item = Instantiate(GameManager.instance.pickupPrefabs[random], tf.position, tf.rotation);
            //and the number it is in the current pickup count
            item.name = GameManager.instance.pickupPrefabs[random] + "_" + GameManager.instance.currentPickUps;
            //add it to the GameManager's list of pickups
            GameManager.instance.livePickUps.Add(item);
            //set the next spawn time equal to now plus pickup spawn delay
            nextSpawnTime = Time.time + GameManager.instance.pickupSpawnDelay;
            //increment the number of current pickups
            GameManager.instance.currentPickUps++;
        }
        base.Update();
    }
    protected override void OnDestroy()
    {
        GameManager.instance.pickupSpawners.Remove(transform);
        base.OnDestroy();
    }
}

