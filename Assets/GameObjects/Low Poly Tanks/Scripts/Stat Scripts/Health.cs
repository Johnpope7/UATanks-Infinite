using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField, Tooltip("The ragdoll spawned when a pawn dies")]
    private GameObject tankParts;
    private bool spawnParts = true;

    [Header("Health Values")]
    [SerializeField]
    private float MaxHealth = 100f;
    [SerializeField]
    private float currentHealth = 100f;
    private float percent;


    //the code comments itself lol
    public float GetHealth()
    {
        return currentHealth;
    }

    private void SetHealth(float value)
    {
        currentHealth = value;
    }

    public float GetMaxHealth()
    {
        return MaxHealth;
    }

    private void SetMaxHealth(float value)
    {
        MaxHealth = value;
    }

    public float GetPercent()
    {
        percent = currentHealth / MaxHealth;
        return percent;
    }

    private void SetPercent()
    {
        percent = currentHealth / MaxHealth;
    }

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
