using System;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    public Action GameWin;
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            GameWin?.Invoke();
        }
    }
}
