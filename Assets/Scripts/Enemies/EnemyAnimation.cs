using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour
{
    private NavMeshAgent m_NavMeshAgent;
    private Animator m_Animator;

    [SerializeField] GameEvent m_SomeoneStartAttack;
    [SerializeField] GameEvent m_SomeoneEndAttack;

    // Start is called before the first frame update
    void Start()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = m_NavMeshAgent.velocity.magnitude;
        m_Animator.SetFloat("Speed", speed);
    }

    public void OnEnemyNearFromPlayer(Component sender, object data)
    {
        if (!ReferenceEquals(sender.gameObject, gameObject)) return;

        m_Animator.SetBool("Chasing", false);
        m_Animator.SetBool("Fighting", true);
    }

    public void OnEnemyFarFromPlayer(Component sender, object data)
    {
        if (!ReferenceEquals(sender.gameObject, gameObject)) return;

        m_Animator.SetBool("Chasing", true);
        m_Animator.SetBool("Fighting", false);
    }

    public void OnEnemyLostPlayer(Component sender, object data)
    {
        if (!ReferenceEquals(sender.gameObject, gameObject)) return;

        m_Animator.SetBool("Chasing", false);
    }




    public void OnEnemyAttack(Component sender, object data)
    {
        if (!ReferenceEquals(sender.gameObject, gameObject)) return;

        m_Animator.SetTrigger("Attack");
    }

    public void StartAttack()
    {
        m_SomeoneStartAttack.Raise(this);
    }

    public void EndAttack()
    {
        m_SomeoneEndAttack.Raise(this);
    }
}
