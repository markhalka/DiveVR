using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;



//maybe make it more interactive (e.g draw the force vector of gravity)
//draw the direction of xx
//or have questions show up mid flight (e.g if the rocket is travelling at 5m/s for 10 seconds how far has it travelled)
//then they can click a hint button and get the equation shown to them (then there will be another button that says show answer, or just skip question)

//you can extend this idea to molecules, so you can calculate conentraion etc. during the actual simulatino



public class RocketController : MonoBehaviour
{


    Rigidbody myRigidbody = null;

    [SerializeField]
    ParticleSystem exhaustParticles = null;

    public AudioClip exhaustAudio = null;
    public AudioSource exhaustSource = null;

    public AudioClip windAudio = null;
    public AudioSource windSource = null;

    [SerializeField]
    float speedForMaxPitch = 100f;
    [SerializeField]
    float speedForMaxVolume = 100f;

    [SerializeField]
    [Range(-3f, 3f)]
    float windMinPitch, windMaxPitch;
    [SerializeField]
    [Range(0f, 1f)]
    float windMinVolume, windMaxVolume;

    float startFuel;
    float currentFuel;
    float initialMass;

    float impulse;
    bool showExhaust = false;

    float maxHeight;
    float maxSpeed;

    public GameObject endPanel;


    float centerOfPressure = 0.45f;

    float centerOfMass = 0.55f;

    float bodyLength = 1;


    int notFlyingCount = 0;
    Vector3 startPosition;

    public Button launch;


    void Awake()
    {
        startFuel = 1f;
        initialMass = 1f;
        impulse = 1f;
    }


    void Start()
    {
        isQuiz = false;

        exhaustSource.clip = exhaustAudio;
        windSource.clip = windAudio;

        myRigidbody = GetComponent<Rigidbody>();
        startPosition = myRigidbody.position;


        bodyLength = GetComponent<CapsuleCollider>().height;
        myRigidbody.centerOfMass = new Vector3(0f, centerOfMass * bodyLength, 0f);


        launch.onClick.AddListener(delegate { takeLaunch(); });

        InformationPanel.transform.parent.GetComponent<InformationPanel>().simpleClose = true;

        ParseData.parseModel();
        initPanelTimes();
        initButtons();

        if (Information.isQuiz == 0)
            showPanel();


        Information.flying = false;
        Information.hasFuel = false;

        Information.currentScene = "Rocket";


    }

    void initButtons()
    {
        endPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { takeQuiz(); });
        endPanel.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(delegate { takeAgain(); });
        endPanel.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(delegate { takeExit(); });
        videoPlayer.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { closeVideo(videoPlayer); });

    }

    void closeVideo(VideoPlayer p)
    {
        videoPlayer.gameObject.SetActive(false);
        Time.timeScale = 1;

    }

    void takeQuiz()
    {
        Information.isQuiz = 1;
        //startQuiz();
    }

    public GameObject quiz;
    void startQuiz()
    {
        isQuiz = true;
        endPanel.gameObject.SetActive(false);
        quiz.SetActive(true);
    }


    void takeAgain()
    {
        Reset();
    }

    void takeExit()
    {
        SceneManager.LoadScene("ModuleMenu");
    }


    void takeLaunch()
    {
        Information.panelIndex = fuelStartOffset + rocketFuels.Length;
        launch.gameObject.SetActive(false);
        StartFlying();
    }

    public Button[] rocketMaterials;
    public Button[] rocketFuels;


    List<RocketFuel> fuels;
    List<RocketMaterial> materials;
    void initMaterialButtons()
    {
        materials = new List<RocketMaterial>();
        RocketMaterial steel = new RocketMaterial();
        steel.weight = 4;
        steel.cost = 1;
        rocketMaterials[0].onClick.AddListener(delegate { initMaterial(0); });
        materials.Add(steel);

        RocketMaterial aluminium = new RocketMaterial();
        aluminium.weight = 3;
        aluminium.cost = 1;
        rocketMaterials[1].onClick.AddListener(delegate { initMaterial(1); });
        materials.Add(aluminium);

        RocketMaterial titanium = new RocketMaterial();
        titanium.weight = 2;
        titanium.cost = 10;
        rocketMaterials[2].onClick.AddListener(delegate { initMaterial(2); });
        materials.Add(titanium);

        RocketMaterial carbon = new RocketMaterial();
        carbon.weight = 1;
        carbon.cost = 20;
        rocketMaterials[3].onClick.AddListener(delegate { initMaterial(3); });
        materials.Add(carbon);

        for (int i = 0; i < rocketMaterials.Length; i++)
        {
            rocketMaterials[i].gameObject.SetActive(true);
        }


    }

    void initFuelButtons()
    {
        fuels = new List<RocketFuel>();
        fuelStartOffset = 1 + rocketMaterials.Length; //where to start the panel index
        RocketFuel spring = new RocketFuel();
        spring.impulsePerWeight = 25000;
        spring.costPerWeight = 1;
        spring.fuelAmount = 1;
        fuels.Add(spring);
        rocketFuels[0].onClick.AddListener(delegate { initFuel(0); });

        RocketFuel compressedAir = new RocketFuel();
        compressedAir.impulsePerWeight = 30000;
        compressedAir.costPerWeight = 1;
        compressedAir.fuelAmount = 1;
        fuels.Add(compressedAir);
        rocketFuels[1].onClick.AddListener(delegate { initFuel(1); });

        RocketFuel Solid = new RocketFuel();
        Solid.impulsePerWeight = 18000;
        Solid.costPerWeight = 1;
        Solid.fuelAmount = 20;
        fuels.Add(Solid);
        rocketFuels[2].onClick.AddListener(delegate { initFuel(2); });

        RocketFuel Liquid = new RocketFuel();
        Liquid.impulsePerWeight = 24000;
        Liquid.costPerWeight = 1;
        Liquid.fuelAmount = 20;
        fuels.Add(Liquid);
        rocketFuels[3].onClick.AddListener(delegate { initFuel(3); });



        for (int i = 0; i < rocketFuels.Length; i++)
        {
            rocketFuels[i].gameObject.SetActive(true);
        }
    }


    int cost = 0;
    int materialStartOffset = 1;
    int fuelStartOffset = 0;
    void initMaterial(int n)
    {
        initialMass = materials[n].weight;
        cost += materials[n].cost;
        Information.panelIndex = materialStartOffset + n;
        showPanel();
        initFuelButtons();

        for (int i = 0; i < rocketMaterials.Length; i++)
        {
            rocketMaterials[i].gameObject.SetActive(false);
        }

    }

    void initFuel(int n)
    {

        impulse = fuels[n].impulsePerWeight;
        cost += fuels[n].costPerWeight;

        startFuel = fuels[n].fuelAmount;
        for (int i = 0; i < rocketFuels.Length; i++)
        {
            rocketFuels[i].gameObject.SetActive(false);
        }

        launch.gameObject.SetActive(true);
        if (n > 1)
        {
            showExhaust = true;
        }
        Information.panelIndex = fuelStartOffset + n;
        showPanel();

    }





    float time = 0;

    void panelManager()
    {

        if (Information.flying)
        {
            time += Time.deltaTime;
        }

        if (Information.panelIndex < panelTimes.Count && panelTimes[Information.panelIndex] > 0 && time > panelTimes[Information.panelIndex] && !InformationPanel.activeSelf)
        {
            Information.lableIndex = 0;
            showPanel();
        }
    }



    int currentPanel = 0;

    List<int> panelTimes;
    void initPanelTimes()
    {


        int[] activeLearning = new int[] { 5, 10, 15, 20, 25 };
        panelTimes = new List<int>();
        for (int i = 0; i < rocketMaterials.Length + rocketFuels.Length + 1; i++)
        {
            panelTimes.Add(-1);
        }

        for (int i = 0; i < activeLearning.Length; i++)
        {
            panelTimes.Add(activeLearning[i]);
        }

    }
    public GameObject InformationPanel;
    bool wasFlying;
    public VideoPlayer videoPlayer;

    void showPanel()
    {
        Debug.LogError("opendd");
        if (!InformationPanel.gameObject.activeSelf)
        {
            InformationPanel.gameObject.SetActive(true);
        }


        wasFlying = Information.flying;

        if (wasFlying)
            Time.timeScale = 0;

        Information.flying = false;
        //   lableIndex = 1;
        panelOpend = true;
        if (Information.panelIndex == panelTimes.Count - 1)
        {
            videoPlayer.gameObject.SetActive(true);
            videoPlayer.loopPointReached += closeVideo;
            // Time.timeScale = 0; 
        }

        //  InformationPanel.transform.GetChild(0).gameObject.SetActive(true);
    }
    bool panelOpend = false;
    void showEndPanel()
    {
        endPanel.SetActive(true);

        Time.timeScale = 0; //this should pause the physics
        if (time < panelTimes[panelTimes.Count - 1])
        {
            endPanel.transform.GetChild(2).gameObject.SetActive(true);
        }
        if (maxHeight > 1300)
        {
            endPanel.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().color = Color.white;
            endPanel.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().color = Color.white;

        }
        endPanel.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Max height: " + (int)maxHeight;
        endPanel.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = "Max speed: " + (int)maxSpeed;

    }



    void hidePanel()
    {
        if (!videoPlayer.isPlaying)
            Time.timeScale = 1;

        Information.flying = wasFlying;
        panelOpend = false;
        Debug.LogError(Information.panelIndex + " this is the panel index when closed");
        if (Information.panelIndex == 0)
        {
            initMaterialButtons();
        }
        Information.panelIndex++; //so that next time it shows the next panel
        InformationPanel.SetActive(false);

    }





    public void Reset()
    {
        exhaustParticles.Stop();
        exhaustParticles.Clear();

        myRigidbody.transform.position = startPosition;
        myRigidbody.transform.rotation = Quaternion.identity;
        myRigidbody.velocity = Vector3.zero;
        myRigidbody.rotation = Quaternion.identity;
        myRigidbody.angularVelocity = Vector3.zero;
        myRigidbody.isKinematic = true;

        Information.flying = false;
        Information.hasFuel = false;
        Information.doneLoading = false;

        notFlyingCount = 0;
        maxSpeed = 0;
        maxHeight = 0;

        windSource.Stop();
        endPanel.gameObject.SetActive(false);

        Information.lableIndex = 0;
        Information.panelIndex = 0;

        time = 0;
        showPanel();
    }

    private void FixedUpdate()
    {
        if (myRigidbody.velocity.y < 0)
        {
            Information.flying = false;


            notFlyingCount++;
            if (notFlyingCount > 100)
            {
                showEndPanel();

            }
        }
        if (Information.hasFuel)
        {
            Thrust();
            if (!exhaustParticles.isPlaying && showExhaust)
            {
                exhaustParticles.Play();

            }
            if (!exhaustSource.isPlaying)
            {
                exhaustSource.Play();
            }
        }
        else
        {
            if (exhaustParticles.isPlaying)
            {
                exhaustParticles.Stop();
            }
            if (exhaustSource.isPlaying)
            {
                exhaustSource.Stop();
            }
        }
    }




    void Thrust()
    {

        if (!Information.flying)
        {
            Information.flying = true;
        }
        myRigidbody.AddRelativeForce(Vector3.up * Time.deltaTime * impulse);
        if (myRigidbody.velocity.y > maxSpeed)
        {
            maxSpeed = myRigidbody.velocity.y;
        }
        if (myRigidbody.transform.position.y > maxHeight)
        {
            maxHeight = myRigidbody.transform.position.y;
        }

        if (currentFuel < 0)
        {

            Information.hasFuel = false;
        }
        else
        {
            currentFuel -= Time.deltaTime;

        }
    }

    bool isInGame;
    bool isQuiz;
    void Update()
    {

        if (!Information.isInMenu)
        {
            checkQuiz();
        }

        if (panelOpend && !InformationPanel.activeSelf)
        {

            hidePanel();
        }


        if (Information.flying)
        {

            //	update the amount of fuel left

            windSource.pitch = Mathf.Lerp(windMinPitch, windMaxPitch, myRigidbody.velocity.magnitude / speedForMaxPitch);
            windSource.volume = Mathf.Lerp(windMinVolume, windMaxVolume, myRigidbody.velocity.magnitude / speedForMaxVolume);
            if (isInGame != windSource.isPlaying)
            {
                windSource.Play();
            }
        }


        if (Information.interactiveLesson && !isQuiz)
        {
            panelManager();
        }

        if (Information.doneLoading)
        {

            SceneManager.LoadScene("ScienceMain");
        }

    }
    void checkQuiz()
    {
        if (!isQuiz)
        {
            if (Information.isQuiz == 1)
            {
                startQuiz();
            }
        }
        else
        {
            if (Information.isQuiz == 0)
            {
                endQuiz();
            }
        }
    }
    void endQuiz()
    {
        isQuiz = false;

        Debug.LogError("ended quiz");
        if (Information.wasPreTest)
        {
            Information.panelIndex = 0;
            Information.lableIndex = 0;
            showPanel(); //???
            return;
        }

        quiz.GetComponent<QuizMenu>().endQuiz();
    }


    public void StartFlying()
    {

        Information.hasFuel = true;
        myRigidbody.isKinematic = false;
        myRigidbody.mass = initialMass + startFuel;
        currentFuel = startFuel;
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (!Information.hasFuel && !collision.gameObject.CompareTag("Player"))
        {
            myRigidbody.velocity = Vector3.zero;
        }
    }
}
