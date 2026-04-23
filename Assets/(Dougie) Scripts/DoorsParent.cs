using System;
using System.Collections;
using UnityEngine;

public class DoorsParent : MonoBehaviour
{
    [SerializeField] private IndividualDoor medusaDoor;
    [SerializeField] private IndividualDoor arachneaDoor;
    private bool medusaExit = false;
    private bool arachneaExit = false;

    private event EventHandler checkIfFinished;
    private void Start()
    {
        medusaDoor.MedusaOnDoor += MedusaDoor_OnEntrance;
        medusaDoor.MedusaOffDoor += MedusaDoor_OnExit;
        arachneaDoor.ArachnaeOffDoor += ArachneaDoor_OnExit;
        arachneaDoor.ArachnaeOnDoor += ArachneaDoor_OnEntrance;
        checkIfFinished += CheckIfFinished;


    }



    private void ArachneaDoor_OnEntrance(object sender, EventArgs e)
    {
        arachneaExit = true;
        Debug.Log("Arachnea is on her door");
        checkIfFinished?.Invoke(this, EventArgs.Empty);
    }

    private void ArachneaDoor_OnExit(object sender, EventArgs e)
    {
        arachneaExit = false;
        Debug.Log("Arachnea is off her door");
        checkIfFinished?.Invoke(this, EventArgs.Empty);
        
        
    }

    private void MedusaDoor_OnExit(object sender, EventArgs e)
    {
        medusaExit = false;
        Debug.Log("Medusa is off her door");
        checkIfFinished?.Invoke(this, EventArgs.Empty);
        
    }

    private void MedusaDoor_OnEntrance(object sender, EventArgs e)
    {
        medusaExit = true;
        Debug.Log("Medusa is on her door");
        checkIfFinished?.Invoke(this, EventArgs.Empty);
        
    }
    
    
    private void CheckIfFinished(object sender, EventArgs e)
    {
        if (medusaExit && arachneaExit)
        {
            Debug.Log("BOTH CHARACTERS ARE ON THEIR EXITS!!!");
            StartCoroutine(TransitionToNextLevel());
        }
    }

    IEnumerator TransitionToNextLevel()
    {
        yield return new WaitForSeconds(3);
        Debug.Log("Changing Level!");
        Loader.Load(Loader.Scene.LevelOne);
    }
}
