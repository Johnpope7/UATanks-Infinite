using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    #region Variables
    [Header("Spawner Settings")]
    public Transform tf; //holds the spawners transform
    [SerializeField, Tooltip("This is how long it takes for the next enemy to spawn.")]
    public float nextSpawnTime;//adjust the time between enemy spawns
    [SerializeField, Tooltip("Toggle the spawners ability to move")]
    public bool isMoving = false;//allows the spawner to move to random locations
    [SerializeField, Tooltip("Minimum x-axis value on the spawners range of movement.")]
    public float minX = 0f; //the minimum range of the spawners x value movement
    [SerializeField, Tooltip("Maximum x-axis value on the spawners range of movement.")]
    public float maxX = 10f; //the maximum range of the spawners x value movement
    [SerializeField, Tooltip("Minimum z-axis value on the spawners range of movement.")]
    public float minZ = 0f; //the minimum range of the spawners z value movement
    [SerializeField, Tooltip("Maximum z-axis value on the spawners range of movement.")]
    public float maxZ = 10f; //the maximum range of the spawners z value movement
    [SerializeField, Tooltip("Y value for the random spawner, keep positive and not randomized, prefurably close to 1 or 2.")]
    public float setY = 2f; //where the psawner will be on thw Y plane
    #endregion
    #region Functions
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        tf = GetComponent<Transform>();
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isMoving)
        {
            //randomly change position
            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(minZ, maxZ);
            tf.position = new Vector3(randomX, setY, randomZ);
        }
    }

    protected virtual void OnDestroy()
    {
        //saved for the child scripts
    }
    #endregion
}
