using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    List<Enemy> m_EnemyList;

    List <GameObject> m_Waypoints;

    [SerializeField]  bool m_DestroyWhenEmpty;

    [SerializeField] GameObject m_EnemyPrefab;

    [SerializeField] int m_EnemySpawnCount;

    

    // Start is called before the first frame update
    void Awake()
    {
        m_Waypoints = gameObject
                .GetComponentsInChildren<Transform>()
                .Select(c => c.gameObject)
                .Where(go => go.layer.Equals(LayerMask.NameToLayer("Waypoint")) && !go.Equals(gameObject))
                .ToList();

        m_EnemyList = new List<Enemy>();    

        for (int i = 0; i < m_EnemySpawnCount; i++)
        {
            AIHandler currentEnemy = m_EnemyPrefab.GetComponent<AIHandler>();

            currentEnemy.Itinerary = m_Waypoints
                                        .Select(w => w.transform)
                                        .ToList();

            int randomIndex = Random.Range(0, m_Waypoints.Count - 1);

            Instantiate(currentEnemy, m_Waypoints[randomIndex].transform.position, Quaternion.identity);


            m_EnemyList.Add(currentEnemy.GetComponent<Enemy>());   
        }
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;

        gameObject
            .GetComponentsInChildren<Transform>()
            .Select(c => c.gameObject)
            .Where(go => go.layer.Equals(LayerMask.NameToLayer("Waypoint")) && !go.Equals(gameObject))
            .ToList()
            .ForEach(w => Gizmos.DrawSphere(w.transform.position, 1.2f));
    }

    #region Event's Callback 

    public void EnemyHasDied(Component sender, object data) 
    {
        bool isEnemyBelongsToThisSpawner = sender.TryGetComponent(out Enemy enemy) && m_EnemyList.Contains(enemy);

        if (!isEnemyBelongsToThisSpawner) return;

        m_EnemyList.Remove(enemy);

        if (m_DestroyWhenEmpty)
            Destroy(gameObject);

    }

    #endregion
}
