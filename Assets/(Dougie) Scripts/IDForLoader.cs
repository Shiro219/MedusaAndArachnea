using System;
using UnityEngine;

public class IDForLoader : MonoBehaviour
{
    [SerializeField] private int levelID;

    private void Awake()
    {
        Loader.setIndex(levelID);
    }
}
