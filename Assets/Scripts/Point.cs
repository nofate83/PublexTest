using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    [Tooltip("For Patrol Type only")]
    public List<Point> patrolRoute; 
    public PointType pointType;
    public EnemyType enemyType;

    public Vector3 pos =>  transform.position;

    private void Start()
    {
        gameObject.SetActive(false);
    }
}
