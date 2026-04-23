using System;
using UnityEngine;
using UnityEngine.AI;
public class Button : MonoBehaviour
{
    [SerializeField] private Door door;
    private int objectsOnButton = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            objectsOnButton++;
            door.SetOpen(true);
        } else if (other.TryGetComponent(out Stone stone))
        {
            objectsOnButton++;
            door.SetOpen(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            objectsOnButton--;

            if (objectsOnButton <= 0)
            {
                door.SetOpen(false);
            }
        }else if (other.TryGetComponent(out Stone stone))
        {
            objectsOnButton--;

            if (objectsOnButton <= 0)
            {
                door.SetOpen(false);
            }
        }
    }
}
