using UnityEngine;

public class BallScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("started");

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("collision enter");
        Information.currentBox = other.gameObject;
    }
}
