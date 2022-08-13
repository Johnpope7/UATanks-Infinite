using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField, Tooltip("The ragdoll spawned when a pawn dies")]
    private GameObject tankParts;
    public bool spawnParts = true;

    [Header("Health Values")]
    [SerializeField]
    public float MaxHealth = 100f;
    [SerializeField]
    public float currentHealth = 100f;
    public float percent;



    public void Heal(float heal)
    {
        heal = Mathf.Max(heal, 0f);
        currentHealth = Mathf.Clamp(currentHealth + heal, 0f, MaxHealth);
        SendMessage("onHeal", SendMessageOptions.DontRequireReceiver);
    }

    public void FullHeal()
    {
        currentHealth = MaxHealth;
    }

    public void Damage(float damage)
    {
        damage = Mathf.Max(damage, 0f);
        currentHealth = Mathf.Clamp(currentHealth - damage, 0f, MaxHealth);
    }

    public void Kill()
    {
        currentHealth = 0;
        Destroy(gameObject);
    }

    public void SpawnTankParts()
    {
        //if we can spawn a ragdoll
        if (spawnParts == true)
        {
            //then spawn one at our current position and rotation
            Instantiate(tankParts, transform.position, transform.rotation);
        }
        //set ragdoll spawning to false
        spawnParts = false;
    }

}
