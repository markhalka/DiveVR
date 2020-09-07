using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class QuickGame : MonoBehaviour
{

    static bool gameOver;
    static bool startAnimation;


    public static GameObject fishContainer;
    public static utilities utility;
    public static float amountDown = 100;


    public GameObject lowerBound;
    public GameObject upperBound;

    public GameObject leftBackground;
    public GameObject rightBackground;

    public GameObject leftDiver;
    public GameObject rightDiver;

    public GameObject leftClams;
    public GameObject rightClams;

    public GameObject tempFish;

    public Text leftText;
    public Text rightText;

    int minHeight;
    int maxHeight;

    Player player;
    Player computer;

    public AudioSource source;
    public AudioClip sinking;
    public AudioClip rattle;
    public AudioClip buttonSound;

    public GameObject inbetween;


    void Start()
    {
        Information.isVrMode = false;
        utility = new utilities();
        fishContainer = tempFish;
        player = new Player(leftDiver, leftText, leftBackground, true, leftClams, false);
        computer = new Player(rightDiver, rightText, rightBackground, false, rightClams, true);
        gameOver = false;
        startAnimation = true;
        minHeight = -30;
        maxHeight = 30;
        StartCoroutine(startGameAnimation());

        StartCoroutine(AnimateFish(player));
        StartCoroutine(AnimateFish(computer));

        StartCoroutine(AnimateDiver(player));
        StartCoroutine(AnimateDiver(computer));

        StartCoroutine(AnimateGoingDown(player));
        StartCoroutine(AnimateGoingDown(computer));

        StartCoroutine(hideAnimation(player));

        StartCoroutine(wrongAnswerAnimation(player));
        StartCoroutine(wrongAnswerAnimation(computer));
    }



    public class Fish
    {
        public bool goingRight;
        public GameObject sprite;

        public Fish(GameObject sprite, bool right)
        {
            goingRight = right;
            this.sprite = sprite;
        }
    }


    public class Player : MonoBehaviour
    {


        public GameObject diver;
        public Text questionText;
        public GameObject background;
        GameObject clams;
        public int currHeight;
        public bool goingDown;
        public bool isRight;
        public bool wrongAnswer;
        public bool isComputer;
        public bool isWinner;
        public bool isShaking;
        public Fish[] fishes;
        public Fish[] fishClams;

        public utilities.Question currentQuestion;
        public int score;
        public bool isLeft;



        public Player(GameObject diverObject, Text questionTextObject, GameObject backgroundObject, bool left, GameObject clamsObject, bool computer)
        {
            fishes = new Fish[utility.getRandom(3, 6)];
            fishClams = new Fish[4];
            score = 0;
            currHeight = -15;
            goingDown = false;
            wrongAnswer = false;
            isRight = false;
            isWinner = false;
            isShaking = false;
            isComputer = computer;
            isLeft = left;
            background = backgroundObject;
            diver = diverObject; clams = clamsObject;
            questionText = questionTextObject;
            initSprites();
            initClams();

            nextQuestion();

        }


        void initSprites()
        {

            for (int i = 0; i < fishes.Length; i++)
            {
                bool right = false;
                if (i % 2 == 0)
                {
                    right = true;
                }
                int index = utility.getRandom(0, fishContainer.transform.childCount - 1);
                GameObject newFish = Instantiate(fishContainer.transform.GetChild(index).gameObject, fishContainer.transform.GetChild(index));
                newFish.gameObject.SetActive(true);

                newFish.transform.SetParent(background.transform);
                Fish currFish = new Fish(newFish, right);
                float curr = fishContainer.transform.GetChild(index).transform.localScale.x;
                if (!right)
                {

                    currFish.sprite.transform.localScale = new Vector3(curr * -1, 1, 1);
                }
                else
                {
                    currFish.sprite.transform.localScale = new Vector3(curr, 1, 1);
                }
                fishes[i] = currFish;

                int xPos = 0;
                if (isLeft)
                {
                    xPos = utility.getRandom(-200, 50);

                }
                else
                {
                    xPos = utility.getRandom(-50, 200);
                }
                int yPos = utility.getRandom(1000, -1000);
                currFish.sprite.transform.localPosition = new Vector3(xPos, yPos);
            }


        }

        void initClams()
        {
            for (int i = 0; i < clams.transform.childCount; i++)
            {
                Fish fish = new Fish(clams.transform.GetChild(i).gameObject, true);
                fishClams[i] = fish;
                clams.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate { checkAnswer(fish); });

            }

        }

        void checkAnswer(Fish fish)
        {
            if (!isShaking)
            {
                if (fish.sprite.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text == currentQuestion.stringAnswer)
                {
                    nextQuestion();
                }
                else
                {
                    wrongAnswer = true;
                }
            }
        }


        public void nextQuestion()
        {
            score++;
            if (score > 6)
            {
                gameOver = true;
                isWinner = true;
                return;
            }
            Color currColor = background.GetComponent<Image>().color;
            if (currColor.g < 0.4f)
            {
                questionText.GetComponent<Text>().color = Color.white;
            }

            if (score > 1)
                isRight = true;

            if (differentiator.question.Count > 0)
            {
                differentiator.question = new List<utilities.Question>();
            }

            utility.getQuestion("1", 0.1f);

            currentQuestion = differentiator.question[0];
            questionText.text = currentQuestion.question;
            currentQuestion.setStringAnswer();
            generateFish();

        }

        void generateFish()
        {
            int choiceIndex = 0;
            string[] choices = utility.multipleChoiceNumber(currentQuestion.stringAnswer);
            for (int i = 0; i < clams.transform.childCount; i++)
            {
                clams.transform.GetChild(i).GetComponentInChildren<TMPro.TMP_Text>().text = choices[choiceIndex++];
                if (choiceIndex > choices.Length - 1)
                {
                    choices = utility.multipleChoiceNumber(currentQuestion.stringAnswer);
                    choiceIndex = 0;
                }
            }
        }


    }

    float shakeAmount = 10;
    IEnumerator wrongAnswerAnimation(Player player)
    {
        while (!gameOver)
        {


            if (player.wrongAnswer || player.isComputer && Information.isIncorrect)
            {
                source.clip = rattle;
                source.Play();

                Information.isIncorrect = false;
                player.wrongAnswer = false;
                player.isShaking = true;
                int shakeDuration = 120;
                int length = player.fishClams.Length;
                Vector3[] originalPos = new Vector3[length];
                for (int i = 0; i < length; i++)
                {
                    originalPos[i] = player.fishClams[i].sprite.transform.localPosition;
                }

                while (shakeDuration > 0)
                {
                    for (int i = 0; i < length; i++)
                    {
                        player.fishClams[i].sprite.transform.localPosition = originalPos[i] + Random.insideUnitSphere * shakeAmount;
                    }
                    shakeDuration--;
                    yield return new WaitForSeconds(0.03f);

                }
                player.isShaking = false;
                for (int i = 0; i < length; i++)
                {
                    player.fishClams[i].sprite.transform.localPosition = originalPos[i];
                }

            }
            yield return new WaitForSeconds(0.3f);
        }

    }


    #region points
    public GameObject points;
    public GameObject endPanel;
    public TMPro.TMP_Text winnerText;
    Vector3 pointsStart = new Vector3(0, 0, 0);
    Vector3 pointsEnd = new Vector3(-340, -279, 0);

    public float journeyTime = 1.5f;
    public float speed = 0.1f;

    float startTime;
    Vector3 centerPoint;

    Vector3 startRelCenter;
    Vector3 endRelCenter;


    IEnumerator hideAnimation(Player player)
    {
        while (!gameOver)
        {
            yield return new WaitForSeconds(1);
        }

        points.gameObject.SetActive(true);

        if (!player.isWinner)
        {
            winnerText.text = "Computer Wins!";
            points.SetActive(false);
            yield break;
        }
        else
        {
            winnerText.text = "Player Wins!";
        }

        endPanel.SetActive(true);
        float fraction = 0;

        int count = 0;
        while (count < 2)
        {
            count++;
            yield return new WaitForSeconds(1);
        }

        while (fraction < 100)
        {
            GetCenter(Vector3.up);
            float fracComplete = fraction / 100;
            points.transform.localPosition = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete * speed);
            points.transform.localPosition += centerPoint;

            fraction += 1;
            yield return new WaitForSeconds(0.01f);

        }

        count = 0;
        while (count < 2)
        {
            count++;
            yield return new WaitForSeconds(1);
        }

        inbetween.SetActive(true);



        yield break;
    }


    public void GetCenter(Vector3 direction)
    {
        centerPoint = (pointsStart + pointsEnd) * .5f;
        centerPoint -= direction;
        startRelCenter = pointsStart - centerPoint;
        endRelCenter = pointsEnd - centerPoint;
    }
    #endregion


    IEnumerator AnimateGoingDown(Player player)
    {
        while (!gameOver)
        {

            if (player.isComputer && Information.isCorrect)
            {
                source.clip = sinking;
                source.Play();

                Information.isCorrect = false;
                if (!startAnimation)
                    player.nextQuestion();
            }

            if (player.isRight)
            {
                player.isRight = false;

                source.clip = sinking;
                source.Play();

                int curr = 0;
                while (curr < amountDown)
                {
                    curr++;
                    player.background.transform.Translate(new Vector3(0, 1, 0));
                    yield return new WaitForSeconds(0.01f);
                }

            }
            yield return new WaitForSeconds(0.2f);
        }

    }


    IEnumerator AnimateDiver(Player player)
    {
        while (!gameOver)
        {

            if (player.goingDown)
            {
                player.currHeight -= 2;

                if (player.currHeight <= minHeight)
                {
                    player.goingDown = false;
                }
            }
            else
            {
                player.currHeight += 2;
                if (player.currHeight >= maxHeight)
                {
                    player.goingDown = true;
                }

            }
            player.diver.transform.localPosition = new Vector3(player.diver.transform.localPosition.x, player.currHeight, player.diver.transform.localPosition.z);
            yield return new WaitForSeconds(0.01f);
        }

    }
    //here, make sure the fish dont go past their boundaries 
    int fishSpeed = 2;
    IEnumerator AnimateFish(Player player)
    {
        int maxX = 0;
        int minX = 0;

        if (player.isLeft)
        {
            maxX = 50;
            minX = -200;

        }
        else
        {
            minX = -50;
            maxX = 200;
        }
        while (!gameOver)
        {

            for (int i = 0; i < player.fishes.Length; i++)
            {
                Fish currentFish = player.fishes[i];
                float x = currentFish.sprite.transform.localPosition.x;
                if (currentFish.goingRight)
                {
                    x += fishSpeed;
                    if (x >= maxX)
                    {
                        currentFish.goingRight = false;
                        float curr = currentFish.sprite.transform.localScale.x;
                        currentFish.sprite.transform.localScale = new Vector3(curr * -1, 1, 1);
                    }
                }
                else
                {
                    x -= fishSpeed;
                    if (x <= minX)
                    {
                        currentFish.goingRight = true;
                        float curr = currentFish.sprite.transform.localScale.x;
                        currentFish.sprite.transform.localScale = new Vector3(curr * -1, 1, 1);
                    }

                }
                currentFish.sprite.transform.localPosition = new Vector2(x, currentFish.sprite.transform.localPosition.y);
            }


            yield return new WaitForSeconds(0.01f);
        }

    }


    int initialiPauseCount = 10;
    int diverY = -15;

    public GameObject ai;
    IEnumerator startGameAnimation()
    {
        for (int i = 0; i < initialiPauseCount; i++)
        {
            yield return new WaitForSeconds(0.2f);
        }

        while (leftBackground.GetComponent<RectTransform>().localPosition.y > -900)
        {
            leftBackground.GetComponent<RectTransform>().Translate(new Vector3(0, -10, 0));
            rightBackground.GetComponent<RectTransform>().Translate(new Vector3(0, -10, 0));

            yield return new WaitForSeconds(0.01f);
        }

        while (player.diver.transform.localPosition.y > diverY)
        {
            player.diver.transform.Translate(0, -1, 0);
            computer.diver.transform.Translate(0, -1, 0);
            yield return new WaitForSeconds(0.01f);
        }

        leftBackground.transform.GetChild(0).gameObject.SetActive(true);
        rightBackground.transform.GetChild(0).gameObject.SetActive(true);
        rightBackground.transform.GetChild(1).gameObject.SetActive(false);

        startAnimation = false;
        yield break;

    }


    void Update()
    {
        if (gameOver && ai.GetComponent<MathAI>().isPlaying)
        {
            ai.GetComponent<MathAI>().isPlaying = false;
        }

        if (Information.doneLoading)
        {
            //Information.nextScene++;
            SceneManager.LoadScene("Math");
        }

    }
}
