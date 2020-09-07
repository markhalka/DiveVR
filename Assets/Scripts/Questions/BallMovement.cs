using UnityEngine;

public class BallMovement : MonoBehaviour
{

    Rigidbody2D myRigidBody;
    Vector2 inputVector;        // Will remember the direction I want to move

    // Use this for initialization
    void Start()
    {
        // Assign reference automatically IF this script is on the same object as RigidBody2D
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        inputVector = new Vector2(0f, 0f);      // First, always reset the input vector

        if (Input.GetKey(KeyCode.W))
        {
            inputVector += new Vector2(0f, 1f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector += new Vector2(0f, -1f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputVector += new Vector2(-1f, 0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector += new Vector2(1f, 0f);
        }

        // To normalize a vector, divide it by its length (or "magnitude")
        // To find the length, use the Pythagorean Theorem: c^2 = a^2 + b^2
        //		float cSquared = (inputVector.x * inputVector.x) + (inputVector.y * inputVector.y);
        //		float c = Mathf.Sqrt (cSquared);	// Find square root
        //		inputVector = inputVector / c;		// Normalize vector

        // In practice, just do this, and Unity does the calculation for you
        inputVector = inputVector.normalized;

    }

    // FixedUpdate is once per PHYSICS FRAME, which is how fast physics runs
    // All physics code should go here. Input should stay in regular update
    void FixedUpdate()
    {
        myRigidBody.velocity = inputVector * 5f;
    }
}
