using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;


[Serializable]
internal class Enemy : MonoBehaviour
{
    [SerializeField] EnemyType _enemyType;
    [SerializeField] int _fov;
    [SerializeField] float _speed;

    private NavMeshAgent _agent;
    private Transform _target;
    Vector3[] _patrolRoute;
    private IEnumerator _job;
    private bool _active;
    private int _routePointNum;

    public Action TargetCatched;
    
    
    void Awake()
    {
        _active = false;
        _agent = GetComponent<NavMeshAgent>();
     }

    public void StartGame()
    {
        
        _active = true;
        switch (_enemyType)
        {
            case EnemyType.Guard:
                _job = Guard();
                break;
            case EnemyType.Patrol:
                _job = Patrol();
                break;
        }
        StopAllCoroutines();
        StartCoroutine(_job);
    }

    public void StopGame()
    {
        _active = false;
    }
    
    public void SetEnemyType(EnemyType enemyType)
    {
        _enemyType = enemyType;
    }

    public void SetPatrolRoute(List<Point> route)
    {
        _patrolRoute = route.Select(x => x.transform.position).ToArray();
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private bool InLineSight()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit);
        
        if (hit.transform.tag != "Player")
        {
            return false;
        }
        float angle = Vector3.Angle(transform.forward, hit.transform.position - transform.position);
        if(gameObject.name == "Enemy2")
        {
           // Debug.Log($"angle is {angle}");
        }
        return Math.Abs(angle) < _fov / 2;
    }

    private IEnumerator Guard()
    {
        while (_active)
        {
            transform.Rotate(Vector3.up, Time.deltaTime * 41);
            if(InLineSight())
            {
                StartCoroutine("Hunt");
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator Patrol()
    {
        if(_patrolRoute.Length == 0)
        {
            yield break;
        }
        while(_active)
        {
            if(Vector3.Distance(transform.position, _patrolRoute[_routePointNum]) < 10)
            {
                _routePointNum++;
                if(_routePointNum == _patrolRoute.Length)
                {
                    _routePointNum = 0;
                }
            }

            if(!_agent.hasPath)
            {
                _agent.SetDestination(_patrolRoute[_routePointNum]);
            }

            if (InLineSight())
            {
                StartCoroutine("Hunt");
                yield break;
            }
            yield return null;
        }
        
    }

    private IEnumerator Hunt()
    {
        while (_active)
        {
            if (InLineSight())
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * _speed);
            }
            else
            {
                StartCoroutine(_job);
                yield break;
            }
            
            if(Vector3.Distance(transform.position, _target.position) < 10)
            {
                TargetCatched?.Invoke();
                yield break;
            }
            yield return null;
        }
    }

}