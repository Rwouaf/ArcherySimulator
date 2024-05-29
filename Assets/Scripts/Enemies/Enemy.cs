using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    [Header("Stats")]

    [SerializeField] private int m_Health = 100;



    [Header("Events")]

    public GameEvent m_OnEnemyHasDied;
        

    public int Health {  get { return m_Health; } set {  m_Health = value; } }

    

    public void EnemyHasBeenHit(Component sender, object data)
    {
        int damage = data.ConvertTo<int>();

        m_Health = Mathf.Max(m_Health - damage, 0);

        if (m_Health == 0)
            m_OnEnemyHasDied.Raise(this);
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<IWeapon>(out var weapon))
            weapon.Attack(this);
    }

    public void MakeDamage(int damage)
    {
        Debug.Log("damage: " + damage);
        m_Health = Mathf.Max(m_Health - damage, 0);
        Debug.Log("Health: " + m_Health);
        if (m_Health == 0)
            m_OnEnemyHasDied.Raise(this);
    }
}
