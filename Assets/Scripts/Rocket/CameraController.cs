using UnityEngine;

/// <summary>
/// CameraController - script that manages camera movement.
/// </summary>
public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    public GameObject InformationPanel;
    // The object we are tracking with the camera
    [SerializeField]
    GameObject target;

    // y-offset from our target
    [SerializeField]
    float offset = 0;



    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        Reset();
    }

    /// <summary>
    /// Resets the camera's position and state
    /// </summary>
    public void Reset()
    {
        /* Information.flying = false;
         Information.hasFuel = true;*/ // i donto think i need these here 
        RenderSettings.fog = false;

        var newPos = transform.position;
        // Track x-position
        newPos.x = target.transform.position.x;

        // Track y-position + offset
        newPos.y = target.transform.position.y + offset;

        transform.position = newPos;
    }

    void Update()
    {

        // Track x-position if the rocket is still flying
        var newPos = transform.position;
        newPos.x = target.transform.position.x;

        // Screen-shake horizontally if using fuel
        if (Information.hasFuel && !InformationPanel.activeSelf) //here check if it is in the panel 
        {
            newPos.x += Random.Range(-0.15f, 0.15f);
        }

        // Track y-position if the rocket is still flying
        newPos.y = target.transform.position.y + offset;
        transform.position = newPos;

    }


    void SetTarget(GameObject inTarget)
    {
        target = inTarget;
    }
}
