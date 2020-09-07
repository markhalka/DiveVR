using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DiveGame : MonoBehaviour
{


    public GameObject fishContainer;
    public GameObject sharkContainer;

    public GameObject diver;
    public GameObject background;
    public GameObject menu;
    public Text questionText;

    public GameObject endPanel;
    public Button exitEndPanel;

    GameObject currentFish;




    float deltaB, deltaG; //for color change

    int minHeight;
    int maxHeight;

    int minX;
    int maxX;

    bool goingDown = false;
    bool wrongAnswer = false;

    bool gameOver;



    int score = -1;

    double perChange = 0;
    int currHeight;



    Diver player;
    Bubbles bubble;
    Question question;


    List<Fish> fishes;

    void Start()
    {
        player = new Diver();
        bubble = new Bubbles();
        question = new Question();

        Information.score = 0;
        Information.acheivment = "";

        deltaB = 0.1f;
        deltaG = 0.2f;

        minHeight = -20;
        maxHeight = 20;
        currHeight = 0;
        minX = -170;
        maxX = 350;


        gameOver = false;
        fishes = new List<Fish>();
        initSprites();

        StartCoroutine(clock());
        StartCoroutine(AnimateDiver());
        StartCoroutine(AnimateFish());
        exitEndPanel.onClick.AddListener(delegate { takeExit(); });

        question.nextQuestion();

        Information.currentScene = "DiveGame";
    }

    public class Fish
    {
        int fishSpeed = 2;
        public bool goingRight;
        public GameObject sprite;
        public Fish(GameObject sprite, bool right)
        {
            goingRight = right;
            this.sprite = sprite;
        }

        public void onClick()
        {

        }
    }

    public class Bubbles
    {
        bool showingBubbles = false;

        int MAX_HEIGHT = 287;
        int bubbleSpeed = 2;
        int currentTankCount = 140;

        AudioSource source;
        GameObject bubble;
        public Bubbles(GameObject bubbleRef)
        {
            bubble = bubbleRef;
        }


        void bubbles()
        {
            source.clip = bubblesPopping;
            source.Play();

            currentTankCount += 50;
            if (currentTankCount > 140)
            {
                currentTankCount = 140;
            }
            bubble.SetActive(false);
        }
    }

    public class AirTank
    {
        public GameObject oxygenTank;

    }

    public class Diver
    {




    }

    public class Audio
    {
        public AudioSource source;
        public AudioClip backgroundSong;
        public AudioClip rightAnswerSound;
        public AudioClip wrongAnswerSound;
        public AudioClip buttonSound;

        public void play(AudioClip clip)
        {
            source.clip = clip;
            source.Play();
        }
    }


    public class Question
    {
        utilities.Question currentQuestion;
        utilities utility;

        public Question()
        {
            utility = new utilities();
        }
        public void nextQuestion()
        {
            score++;
            Color currColor = background.GetComponent<Image>().color;
            if (currColor.g < 0.4f)
            {
                //change the text color to white
                questionText.GetComponent<Text>().color = Color.white;
            }
            background.GetComponent<Image>().color = new Color(currColor.r, currColor.g - deltaG, currColor.b - deltaB);

            if (differentiator.question.Count > 0)
            {
                differentiator.question = new List<utilities.Question>();
            }

            utility.getQuestion("1", 0.1f);

            source.clip = bubblesRising;
            source.Play();

            showingBubbles = true;
            bubble.SetActive(true);
            bubble.transform.localPosition = new Vector3(utility.getRandom(-356, 280), -236, 0);
            StartCoroutine(AnimateBubbles());
            currentQuestion = differentiator.question[0];

            questionText.text = currentQuestion.question;
            currentQuestion.setStringAnswer();
            generateFish();

        }

        bool checkAnswer(Fish fish)
        {
            source.clip = buttonSound;
            source.Play();

            if (!Information.isVrMode)
            {
                if (fish.sprite.transform.GetChild(0).GetComponent<Text>().text == currentQuestion.stringAnswer)
                {
                    source.clip = rightAnswerSound;
                    source.Play();

                    nextQuestion();
                }
                else
                {
                    source.clip = wrongAnswerSound;
                    source.Play();
                    wrongAnswer = true;
                }
            }
            else
            {
                if (currentFish.GetComponent<Text>() != null && currentFish.GetComponentInChildren<Text>().text == currentQuestion.stringAnswer)
                {
                    source.clip = rightAnswerSound;
                    source.Play();
                    //then its right, call next question
                    nextQuestion();
                }
                else
                {
                    source.clip = wrongAnswerSound;
                    source.Play();
                    wrongAnswer = true;
                }
            }
        }
    }




    void initSprites()
    {

        for (int i = 0; i < fishContainer.transform.childCount; i++)
        {

            bool right = false;
            if (i % 2 == 0)
            {
                right = true;

            }
            else
            {
                fishContainer.transform.GetChild(i).localScale = new Vector3(-1, 1, 1);
                fishContainer.transform.GetChild(i).GetChild(0).localScale = new Vector3(-1, 1, 1); //if they go left, you need to filp the sprite and the text
            }
            Fish currFish = new Fish(fishContainer.transform.GetChild(i).gameObject, right);
            fishes.Add(currFish);
            fishContainer.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate { checkAnswer(currFish); });
        }

        bubble.GetComponent<Button>().onClick.AddListener(delegate { bubbles(); });
    }


    void Update()
    {

        if (Information.doneLoading)
        {
            // Information.nextScene++;
            SceneManager.LoadScene("Math");
        }
    }


    IEnumerator clock()
    {
        while (!gameOver)
        {
            updateAirTank();
            yield return new WaitForSeconds(1);
        }

    }




    #region animations
    IEnumerator Aniamte()
    {

        while (!gameOver && showingBubbles)
        {
            float bubbleHeight = bubble.transform.localPosition.y;
            bubble.transform.localPosition = new Vector2(bubble.transform.localPosition.x, bubbleHeight + fishSpeed);
            if (bubbleHeight >= MAX_HEIGHT)
            {
                showingBubbles = false;
                bubble.SetActive(false);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator AnimateDiver()
    {
        while (!gameOver)
        {
            if (goingDown)
            {
                currHeight -= 2;

                if (currHeight <= minHeight)
                {
                    goingDown = false;
                }
            }
            else
            {
                currHeight += 2;
                if (currHeight >= maxHeight)
                {
                    goingDown = true;
                }

            }
            diver.transform.localPosition = new Vector3(diver.transform.localPosition.x, currHeight, diver.transform.localPosition.z);
            yield return new WaitForSeconds(0.01f);
        }

    }
    IEnumerator AnimateFish()
    {
        while (!gameOver)
        {

            for (int i = 0; i < fishes.Count; i++)
            {
                Fish currentFish = fishes[i];
                float x = currentFish.sprite.transform.localPosition.x;
                if (currentFish.goingRight)
                {
                    x += fishSpeed;
                    if (x >= maxX)
                    {

                        currentFish.goingRight = false;
                        float curr = currentFish.sprite.transform.localScale.x;
                        currentFish.sprite.transform.localScale = new Vector3(curr * -1, 1, 1);
                        currentFish.sprite.transform.GetChild(0).localScale = new Vector3(curr * -1, 1, 1);
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
                        currentFish.sprite.transform.GetChild(0).localScale = new Vector3(curr * -1, 1, 1);
                    }

                }
                currentFish.sprite.transform.localPosition = new Vector2(x, currentFish.sprite.transform.localPosition.y);
            }


            yield return new WaitForSeconds(0.01f);
        }

    }
    #endregion





    void generateFish()
    {
        int choiceIndex = 0;
        string[] choices = utility.multipleChoiceNumber(currentQuestion.stringAnswer);
        for (int i = 0; i < fishContainer.transform.childCount; i++)
        {
            //just assign the multple choice to the fish
            fishContainer.transform.GetChild(i).GetComponentInChildren<Text>().text = choices[choiceIndex++];
            if (choiceIndex > choices.Length - 1)
            {
                choices = utility.multipleChoiceNumber(currentQuestion.stringAnswer); //maybe only have one fish with the right answer?
                choiceIndex = 0;
            }
        }
    }






    public GameObject inBetween;
    void updateAirTank()
    {
        int changeAmount = 2;
        if (score < 5 && score > 2)
        {
            changeAmount = 5;
            fishSpeed = 3;
        }
        else if (score < 7 && score >= 5)
        {
            fishSpeed = 4;
            changeAmount = 10;
        }
        else if (score >= 7)
        {
            changeAmount = 15;
            fishSpeed = 6;
        }

        if (currentTankCount < 50 || wrongAnswer)
        {
            oxygenTank.GetComponent<Image>().color = Color.red;
        }
        else
        {
            oxygenTank.GetComponent<Image>().color = Color.green;
        }


        currentTankCount -= changeAmount;
        if (wrongAnswer)
        {
            currentTankCount -= 10;
            wrongAnswer = false;
        }
        RectTransform currSize = oxygenTank.GetComponent<RectTransform>();

        oxygenTank.GetComponent<RectTransform>().sizeDelta = new Vector2(currSize.sizeDelta.x, currentTankCount);


        if (currentTankCount <= 0)
        {
            gameOver = true;


            endPanel.SetActive(true);
            endPanel.transform.GetChild(2).GetComponent<TMPro.TMP_Text>().text = "Game Over! You scored: " + score;


        }
    }

    void takeExit()
    {
        source.clip = buttonSound;
        source.Play();

        inBetween.SetActive(true);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Information.isInMenu)
        {

            if (collision.gameObject.GetComponent<Button>() != null)
                collision.gameObject.GetComponent<Button>().Select();
        }
        else
        {
            currentFish = collision.gameObject;
            currentFish.GetComponent<Button>().Select();
        }

    }
}
