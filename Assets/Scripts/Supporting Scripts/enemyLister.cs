using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyLister : MonoBehaviour
{
    
    void Start()
    {
        if (GameManager.instance)
        {
            GameManager.instance.enemyList.Add(this.gameObject);//adds the enemy to the list of Enemies
        }
    }
}
