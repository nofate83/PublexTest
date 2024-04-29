using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
internal class Player : MonoBehaviour
{
    [SerializeField] private InputController _inputController;
    private Camera _camera;
    private NavMeshAgent _agent;
    private bool _active = false;
    
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
        _active = true;
        StartCoroutine("CameraHandler");
    }

    public void StopGame()
    {
        _active = false;
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

    private IEnumerator CameraHandler()
    {
        while(_active)
        {
            Debug.Log("Camera hander");
            _camera.transform.position = Vector3.Lerp(new Vector3(transform.position.x, _camera.transform.position.y, transform.position.z), _camera.transform.position, 0.3f);
            yield return null;
        }
        yield break;
    }
}
