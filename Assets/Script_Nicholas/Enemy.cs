using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public void TurnToStone(Transform stonePrefab)
    {
        var obj = Instantiate(stonePrefab);
        obj.transform.position = transform.position;
        Destroy(gameObject);
    }
}
