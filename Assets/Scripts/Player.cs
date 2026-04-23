using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Player : MonoBehaviour{
    public enum Character
    {
        Arachnea,
        Medusa,
    } 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float groundAcceleration = 15f;
    public float apexHeight = 40f;
    public float apexTime = .5f;
    public float gravityMod = 1f;
    public bool isOnWall = false;
    public bool faceRight = true;
    public float dashTime= .5f;
    public float dashSpeed = 6f;
    public float dashGrav = .125f;
    public bool isDashing = false;
    private Vector3 deltaPosition;
    Vector2 _velocity;
    Quaternion facingRight;
    Quaternion facingLeft;
    public float direction;
    private CharacterController controller;
    private InputAction up;
    private InputAction down;
    private InputAction left;
    private InputAction right;
    private InputAction dash;
    private InputAction interact;
    private Controls controls;
    


    public Character character;
    void Awake()
    {
        controls = new Controls();
        controls.Player.Enable();
        if (character == Character.Arachnea)
        {
            up = controls.Player.ArachneaJump;
            down = controls.Player.ArachneaDrop;
            left = controls.Player.ArachneaWalkLeft;
            right = controls.Player.ArachneaWalkRight;
            dash = controls.Player.ArachneaDash;
            interact = controls.Player.ArachneaInteract;
        }
        else
        {
            up = controls.Player.MedusaJump;
            down = controls.Player.MedusaDrop;
            left = controls.Player.MedusaWalkLeft;
            right = controls.Player.MedusaWalkRight;
            dash = controls.Player.MedusaDash;
            interact = controls.Player.MedusaInteract;
        }
    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
        facingRight = Quaternion.Euler(0f,0f,0f);
        facingLeft = Quaternion.Euler(0f,180f,0f);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        float direction = 0f;
        if(right.IsPressed()) direction += 1f;
        if(left.IsPressed()) direction -= 1f;
        bool jumpPressedThisFrame = up.WasPressedThisFrame();
        bool jumpHeld = up.IsPressed();
        bool dashPressedThisFrame = dash.WasPerformedThisFrame();


        if (!isOnWall)
        {
            DoWalk(direction);    
            if (controller.isGrounded)
            {
                if (jumpPressedThisFrame)
                {

                    _velocity.y = 2f*apexHeight/apexTime;
                }
            }
            else
            {
                if (!jumpHeld&&!isDashing)
                {
                    gravityMod = 2f;
                }
            }

            if (!controller.isGrounded)
            {
                float gravity = 2f*apexHeight/(apexTime*apexTime);
                _velocity.y -= gravity*gravityMod*Time.deltaTime;
            }
        }
        else
        {
            if (down.IsPressed())
            {
                _velocity.y -= 1f;
            }

            if (up.IsPressed())
            {
                _velocity.y += 1f;
            }
            if (direction!= 0f)
            {
                if (Mathf.Sign(direction) != Mathf.Sign(_velocity.x))
                {
                    _velocity.x = 0f;
                }
                
                
                _velocity.x += direction*groundAcceleration * Time.deltaTime;
                _velocity.x = Mathf.Clamp(_velocity.x,-walkSpeed,walkSpeed);

                transform.rotation = (direction >0f) ? facingRight : facingLeft;
            }
            else
            {
                _velocity.x = Mathf.MoveTowards(_velocity.x,0f,groundAcceleration*Time.deltaTime);
            }
        }

        if (dashPressedThisFrame)
        {
            StartCoroutine(Dash());
        }
        float deltaX = _velocity.x*Time.deltaTime;
        float deltaY = _velocity.y*Time.deltaTime;
        deltaPosition = new Vector3(0f,deltaY,deltaX);
        transform.position += deltaPosition;
        controller.Move(deltaPosition);

        if (interact.IsPressed())
        {
            Interact();
        }
    }
    private IEnumerator Dash()
    {
        isDashing = true;
        walkSpeed*=dashSpeed;
        gravityMod = dashGrav;
        yield return new WaitForSeconds(dashTime);
        walkSpeed/=dashSpeed;
        gravityMod = 1f;
        isDashing = false;
    }
    public virtual void Jump()
    {
        Debug.Log("not supposed to print");
    }
    public virtual void Interact()
    {
        Debug.Log("not supposed to print");
    }

    private void DoWalk(float direction)
    {
        if (direction!= 0f)
        {
            if (Mathf.Sign(direction) != Mathf.Sign(_velocity.x))
            {
                _velocity.x = 0f;
                faceRight = !faceRight;
            }
                
            _velocity.x += direction*groundAcceleration * Time.deltaTime;
            _velocity.x = Mathf.Clamp(_velocity.x,-walkSpeed,walkSpeed);

            transform.rotation = (direction >0f) ? facingRight : facingLeft;
        }
        else
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x,0f,groundAcceleration*Time.deltaTime);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody box = hit.collider.attachedRigidbody;
        if (!hit.collider.TryGetComponent(out Stone stone)) return;
        if (hit.moveDirection.y < -0.3) return;
        Vector3 pushDir = new Vector3(0, 0, hit.moveDirection.z);
        box.linearVelocity +=pushDir;
    }


}