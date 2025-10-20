// using UnityEngine;
// using UnityEngine.InputSystem;
// public class PlayerMovement : MonoBehaviour
// {
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     public Rigidbody2D rb;
//     public float moveSpeed = 5f;
//     public float jumpPower = 10f;
//     float horizontalMovement;
//     private bool isJumping = false;
//     public float jumpHoldForce = 2f;   // extra force while holding


   
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        
//     }

//     public void Move(InputAction.CallbackContext context)
//     {
//         horizontalMovement = context.ReadValue<Vector2>().x;
//     }

//     public void Jump(InputAction.CallbackContext context)
//     {
//         if (context.performed)    // on press
//         {
//             isJumping = true;
//             // Debug.Log("Jump pressed");
//         }
//         else if (context.canceled) // on release
//         {
//             isJumping = false;
//             // Debug.Log("Jump released");
//         }
//     }





// }
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float jumpPower = 10f;
    private float horizontalMovement;
    private bool isJumping = false;

    void Update()
    {
        // horizontal movement
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);

        // keep moving up while jump key is held
        if (isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isJumping = true;  // start flying up
        }
        else if (context.canceled)
        {
            isJumping = false; // stop flying
        }
    }
}
