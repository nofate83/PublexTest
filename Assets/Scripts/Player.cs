using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
internal class Player : MonoBehaviour
{
    [SerializeField] private InputController _inputController;
    private Camera _camera;
    private NavMeshAgent _agent;
    
    void Awake()
    {
        _camera = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
        
    }

    public void StartGame()
    {
        _agent.ResetPath();
        _inputController.GetPoint += SetNewDestination;
        _agent.isStopped = false;
    }

    public void StopGame()
    {
        if(_agent.hasPath)
        {
            _agent.isStopped = true;
        }
        _inputController.GetPoint -= SetNewDestination;
    }


    private void SetNewDestination(Vector2 point)
    {
        RaycastHit hit;
        Physics.Raycast(_camera.ScreenPointToRay(point), out hit, 1000, 256);
        _agent.SetDestination(hit.point);
    }
}
