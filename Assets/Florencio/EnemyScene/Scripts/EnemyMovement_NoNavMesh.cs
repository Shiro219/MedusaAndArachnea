using UnityEngine;
public class EnemyMovement_NoNavMesh : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Speed at which the enemy moves between points
    [SerializeField] private Transform pointA; // Initial starting point
    [SerializeField] private Transform pointB; // Initial target point
    [SerializeField] private Transform pointC; // Position when door is open
    [SerializeField] private Transform arachneaSpawn; // Respawn position for arachnea
    [SerializeField] private Transform medusaSpawn; // Respawn position for arachnea


    private float movementTimer = 0f; // Timer to track waiting time at each point
    
    private bool isMoving = false; // Flag to indicate whether the enemy is currently moving or waiting
    
    private Transform currentTarget; // Current target point the enemy is moving towards


    private bool usePointC = false; // Represents the door's state, flase = not open
    private bool waitingForOpenDoor = false; // Enemy remains stuck at point C when door is closed
    private bool justLeftC = false; // Prevents enemy from going back to C after just leaving it
    
    private void Awake()
    {
        transform.position = pointA.position; // Start at point A
        currentTarget = pointB; // Set initial target to point B
    }
    
    private void Update()
    {
        //Debug.Log("WaitTimer: " + movementTimer); // Log the current value of the movement timer for debugging purposes

        // IF enemy is at C and waiting for hte door
        if(waitingForOpenDoor)
        {
            movementTimer += Time.deltaTime;
            if (movementTimer >= 3f)
            {
                if (usePointC) // Leaving point C, going to A
                {
                    waitingForOpenDoor = false;
                    movementTimer = 0f;
                    currentTarget = pointA;
                    isMoving = true;
                    justLeftC = true;
                }
            }

            return;
        }

        if (!isMoving) // If the enemy is not currently moving, increment the movement timer
        {
            movementTimer += Time.deltaTime; // Increment the movement timer by the time elapsed since the last frame
        
            if (movementTimer >= 3f) // If the movement timer has reached or exceeded 3 seconds, start moving towards the current target
            {
                isMoving = true; // Set the isMoving flag to true to indicate that the enemy is now moving
                movementTimer = 0f; // Reset the movement timer to 0 for the next waiting period after reaching the target
            }
        }
        else
        {
            MoveToPosition(currentTarget); // Move the enemy towards the current target position
        
            if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f) // If the enemy is close enough to the target position, stop moving and switch to the other target
            {
                isMoving = false; // Set the isMoving flag to false to indicate that the enemy has reached the target and will now wait before moving again
            
                movementTimer = 0f; // Reset the movement timer to 0 for the next waiting period at the new target position

                ChooseTarget();
            }
        }
    }


    private void ChooseTarget()
    {
        if (currentTarget == pointA)
        {
            if (justLeftC) // After leaving C and going to A, go to B
            {
                currentTarget = pointB;
                justLeftC = false;
            }
            else if (usePointC) // Go to point C if door is open
            {
                currentTarget = pointC;
            }
            else
            {
                currentTarget = pointB;
            }
        }
        else if (currentTarget == pointB)
        {
            currentTarget = pointA;
        }
        else if (currentTarget == pointC)
        {
            waitingForOpenDoor = true;
            movementTimer = 0f;
        }
    }

    public void SetDoorOpen(bool open)
    {
        usePointC = open; // Used in Door script

    }

    
    // Method to move the enemy towards a specified position at a defined speed
    private void MoveToPosition(Transform position)
    {
        transform.position = Vector3.MoveTowards(transform.position, position.position, speed * Time.deltaTime); // Move the enemy's position towards the target position at the defined speed, taking into account the time elapsed since the last frame
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arachnea"))
        {
            CharacterController cc = other.GetComponent<CharacterController>();

            // Teleport Arachnea
            if(cc != null)
            {
                cc.enabled = false;
            }
            other.transform.position = arachneaSpawn.position;
            if(cc!= null)
            {
                cc.enabled = true;
            }
        }

        if (other.CompareTag("Medusa"))
        {
            CharacterController cc = other.GetComponent<CharacterController>();
            // Teleport Medusa
            if (cc != null)
            {
                cc.enabled = false;
            }
            other.transform.position = medusaSpawn.position;
            if (cc != null)
            {
                cc.enabled = true;
            }
        }
    }
}