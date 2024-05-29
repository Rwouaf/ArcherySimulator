using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyNavigation : MonoBehaviour
{
    Rigidbody m_Rigidbody;

    NavMeshAgent m_NavMeshAgent;

    [Header("Movement Speed")]
    [SerializeField] float m_MovementSpeedOnPath;
    [SerializeField] float m_MovementSpeedOnFighting;


    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void OnEnemyMovesTowardPlayer(Component sender, object data)
    {
        if (!sender.gameObject.Equals(gameObject) || data.ConvertTo<Transform>().gameObject.layer != LayerMask.NameToLayer("Player")) return;

        m_NavMeshAgent.speed = m_MovementSpeedOnFighting;
    }

    public void OnEnemyLostPlayer(Component sender, object data)
    {
        if (!ReferenceEquals(sender.gameObject, gameObject)) return;

        m_NavMeshAgent.speed = m_MovementSpeedOnPath;
    }

}
