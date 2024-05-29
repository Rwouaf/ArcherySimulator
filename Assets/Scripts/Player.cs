using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IDestroyable
{

    public float maxHealth = 10f;
    private float currentHealth;

    public float maxStrenght = 10f;
    private float currentStrenght;

    //RespawnPoint coord
    [SerializeField] private GameObject initialSpawn;
    private UnityEngine.Vector3 respawnPoint;

    void Awake()
    {
        //la vie du joueur
        currentHealth = maxHealth;
        currentStrenght = maxStrenght;

        //Place le player sur le point de spawn donné
        UnityEngine.Vector3 initialSpawnPos = initialSpawn.transform.position;
        transform.position = initialSpawnPos;
        respawnPoint = initialSpawnPos;
    }

    public void Dead(Component sender, object data)
    {
        Debug.Log("Player Dead");
        transform.position = respawnPoint;
        respawnPoint.Set(transform.position.x, transform.position.y, transform.position.z);
    }


    public void reachCheckPoint(Component sender, object data)
    {
        Debug.Log("Player as reached CheckPoint");
        respawnPoint.Set(transform.position.x, transform.position.y, transform.position.z);
    }

    public void Heal(Component sender, object data)
    {
        if (data is not float value) return;
        
        if (value < 0.0f)
        {
            Debug.LogError("Player heal event should pass a positive value (received value = "+ value +")");
            return;
        }
        
        Debug.Log("Healing player for "+ value + "hp");
        SetCurrentHealth(currentHealth + value);
    }

    private void SetCurrentHealth(float value)
    {
        if (value < 0)
        {
            Debug.LogError("Player health can't be lower than 0");
            return;
        }

        currentHealth = value;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void MakeDamage(int damage)
    {
        int healthAfterDamage = (int)Math.Max(currentHealth - damage, 0);
        
        SetCurrentHealth(healthAfterDamage);

    }
}
