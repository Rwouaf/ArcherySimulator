using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField]
    private int damage;
    public int Damage { get { return damage; } set { damage = value; } }
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public Weapon(int damage)
    {
        Damage = damage;
    }


    public void Attack(Enemy enemy)
    {
        if (rb.velocity.magnitude > 0.5f)
        {
            if (enemy != null)
            {
                enemy.Health = Mathf.Max(enemy.Health - Damage, 0);

                if (enemy.Health == 0)
                    enemy.m_OnEnemyHasDied.Raise(this);

                Debug.Log($"Enemy HP: {enemy.Health}");
            }
        }
    }
}
