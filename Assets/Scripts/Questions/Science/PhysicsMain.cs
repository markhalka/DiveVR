using UnityEngine;

public class PhysicsMain : MonoBehaviour
{
    // Start is called before the first frame update

    //this will handle which ones to call

    //get this one working 


    void Start()
    {
        openModule();
    }


    public GameObject staticModule;
    public GameObject currentModule;
    public GameObject goldRubergModule;
    public GameObject magnetsModule;


    //you also need to make the whole container true


    public void openModule()
    {
        switch (Information.nextScene)
        {
            case 7: //magnets
                magnetsModule.gameObject.SetActive(true);
                break;
            case 28: //gold ruberg
                goldRubergModule.gameObject.SetActive(true);
                break;
            case 40: //current electricity
                currentModule.gameObject.SetActive(true);
                break;
            case 43: //static electircity
                staticModule.gameObject.SetActive(true);
                break;
        }
        gameObject.SetActive(false); //i mean you don't really need it anymore...
    }



    // Update is called once per frame
    void Update()
    {

    }
}
