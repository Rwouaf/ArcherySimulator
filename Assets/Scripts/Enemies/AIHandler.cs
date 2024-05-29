using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;
using Unity.VisualScripting;

public class AIHandler : MonoBehaviour
{
    [Header("Navigation")]

    [SerializeField] List<Transform> m_Itinerary;
    [SerializeField] bool m_RegenerateItineraryWhenCompleted;
    [SerializeField] int m_CurrentWaypointIndex = 0;
    NavMeshAgent m_NavMeshAgent;



    [Header("Player Detection")]

    [SerializeField] float m_DetectionRadius = 10f; 
    [SerializeField] LayerMask m_PlayerLayer;  
    [SerializeField] bool m_IsChasingPlayer = false;
    [SerializeField] Transform m_PlayerTransform = null;
    [SerializeField] float m_MaxDistanceFromCurrentWaypointBeforeLeavesFocus;
    [SerializeField] float m_MaxDistanceFromPlayerBeforeLeavesFocus;


    [Header("Fight")]

    [SerializeField] bool m_IsDamageInitializedByEvent = true;

    [SerializeField] float m_RadiusSphereCastPlayerOnFight;
    [SerializeField] bool m_IsFighting;
    [SerializeField] float m_MaxDistanceNearAttack;
    [SerializeField] float m_MinDistanceNearAttack;

    [SerializeField] bool m_CanAttack;

    [SerializeField] float m_LastTimeAttack;

    [SerializeField] int m_AttackCount;

    [SerializeField] int m_AttackIndex;

    [SerializeField] int m_DamageOnHit;

    [SerializeField] float m_AttackRate;


    [Header("Events")]

    [SerializeField] GameEvent m_OnPlayerHasBeenSeen; 
    [SerializeField] GameEvent m_OnEnemyMovesTowardTarget;
    [SerializeField] GameEvent m_OnPlayerHasBeenLost;
    [SerializeField] GameEvent m_OnEnemyAttacks;
    [SerializeField] GameEvent m_EnemyIsFarFromPlayer;
    [SerializeField] GameEvent m_EnemyIsNearFromPlayer;
    [SerializeField] GameEvent m_OnInitDamageMakerByEvent;

    public List<Transform> Itinerary
    {
        get => m_Itinerary;
        set => m_Itinerary = value;
    }

    private void Start()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();

        GenerateRandomItinerary();

        if (m_Itinerary.Count > 0)
            SetNextWaypoint();


        if (m_IsDamageInitializedByEvent)
            m_OnInitDamageMakerByEvent.Raise(this, m_DamageOnHit);
    }

    private void Update()
    {
        m_CanAttack = Time.time >= m_LastTimeAttack + m_AttackRate;
    }
    private void FixedUpdate()
    {
        if (m_IsChasingPlayer)
            StartCoroutine(ChasePlayer());
        else
            Patrol();
    }

    void Patrol()
    {
        if (m_NavMeshAgent.remainingDistance <= m_NavMeshAgent.stoppingDistance)
            SetNextWaypoint();

        if (!m_IsChasingPlayer && CanSeePlayer(out Transform playerTransform))
            m_OnPlayerHasBeenSeen.Raise(this, playerTransform);
    }

    private void SetNextWaypoint()
    {
        if (m_Itinerary.Count == 0)
            return;

        m_NavMeshAgent.SetDestination(m_Itinerary[m_CurrentWaypointIndex].position);

        if (m_CurrentWaypointIndex + 1 == m_Itinerary.Count && m_RegenerateItineraryWhenCompleted)
            GenerateRandomItinerary();

        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % m_Itinerary.Count;
    }


    private void GenerateRandomItinerary()
    {
        for (int i = 0; i < m_Itinerary.Count; i++)
        {
            int randomIndex = Random.Range(i, m_Itinerary.Count);
            (m_Itinerary[randomIndex], m_Itinerary[i]) = (m_Itinerary[i], m_Itinerary[randomIndex]); // Echange de valeurs
        }
    }

    private bool CanSeePlayer(out Transform playerTransform)
    {
        playerTransform = null;

        Collider[] playerCollider = new Collider[1];

        int numbCollider = Physics.OverlapSphereNonAlloc(transform.position, m_DetectionRadius, playerCollider, m_PlayerLayer);

        if (numbCollider == 0) return false;

        
        playerTransform = playerCollider
                            .DefaultIfEmpty(null)
                            .FirstOrDefault()
                            .transform;

        Vector3 playerDirection = playerTransform.position - transform.position;

        // S'assurer que le player, une fois détécté, est directement visible par l'ennemi et non pas séparé par un mur par exemple
        // Pour cela, on tire un rayon vers le player et si le rayon touche autre chose que le layer player, alors il y a un obstacle et le player n'est finalement pas visible par l'ennemi
        // Egalement, on s'assure que le rayon tiré se fait dans la direction forward de l'ennemi. (Si le joueur est derrière l'ennemi, il n'est pas visible)
        if (
                Physics.Raycast(transform.position, playerDirection, out RaycastHit hit, m_DetectionRadius * 2) 
                &&
                (m_PlayerLayer & (1 << hit.transform.gameObject.layer)) != 0
                && 
                Vector3.Dot(transform.TransformDirection(Vector3.forward), hit.transform.position - transform.position) > 0
            )
            return true;
        else
            return false;
    }

    IEnumerator ChasePlayer()
    {
        if (m_PlayerTransform != null)
            m_NavMeshAgent.SetDestination(m_PlayerTransform.position);

        if (m_NavMeshAgent.remainingDistance <= m_MaxDistanceNearAttack)
        {
            m_EnemyIsNearFromPlayer.Raise(this);
            StartCoroutine(Fighting()) ;
        }
        else
            m_EnemyIsFarFromPlayer.Raise(this);


        yield return null;
    }

    public void PlayerHasBeenSeen(Component sender, object data)
    {
        if (Vector3.Distance(transform.position, sender.transform.position) > m_DetectionRadius * 10) return;

        Transform playerTransform = data.ConvertTo<Transform>();

        m_IsChasingPlayer = true;
        m_PlayerTransform = playerTransform;

        m_OnEnemyMovesTowardTarget.Raise(this, playerTransform);
    }

    public void PlayerHasBeenLost(Component sender, object data)
    {
        if (!sender.gameObject.Equals(gameObject)) return;

        m_PlayerTransform = null;
        m_IsChasingPlayer = false;

        m_NavMeshAgent.SetDestination(m_Itinerary[m_CurrentWaypointIndex].position);
    }

    private void OnDrawGizmos()
    {

        if (m_PlayerTransform != null)
        {
            Gizmos.color = Color.black;

            Gizmos.DrawRay(transform.position, m_PlayerTransform.position - transform.position);
        }
    }


    IEnumerator Fighting()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, m_PlayerTransform.position);
        float distanceFromCurrentWaypoint = Vector3.Distance(transform.position, m_Itinerary[m_CurrentWaypointIndex].transform.position);

        if (distanceFromCurrentWaypoint > m_MaxDistanceFromCurrentWaypointBeforeLeavesFocus
            || distanceFromPlayer > m_MaxDistanceFromPlayerBeforeLeavesFocus)
        {
            m_OnPlayerHasBeenLost.Raise(this);
            m_LastTimeAttack = 0f;

            yield break;
        }

        if (!m_CanAttack)
            yield return null;

        if (distanceFromPlayer > m_MaxDistanceNearAttack)
            yield return null;


        Vector3 directionToPlayer = (m_PlayerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 15f);

        int randomIndex = Random.Range(0, m_AttackCount);

        m_OnEnemyAttacks.Raise(this, new {Damage = m_DamageOnHit, AttackIndex = randomIndex});

        m_LastTimeAttack = Time.time;

        yield return null;
    }

}

