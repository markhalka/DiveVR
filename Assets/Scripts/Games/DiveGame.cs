using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DiveGame : MonoBehaviour
{


    public GameObject fishContainer;
    public GameObject sharkContainer;



    public GameObject menu;
    public Text questionText;

    public GameObject endPanel;
    public Button exitEndPanel;

    GameObject currentFish;




    int minX;
    int maxX;


    bool wrongAnswer = false;
    bool gameOver;


    double perChange = 0;


    Diver diver;
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
        Text fishText;

        public Fish(GameObject sprite, bool right)
        {
            goingRight = right;
            this.sprite = sprite;

            fishContainer.transform.GetChild(i).localScale = new Vector3(-1, 1, 1);
            fishContainer.transform.GetChild(i).GetChild(0).localScale = new Vector3(-1, 1, 1);
        }

        public void setText(string text)
        {
            fishText.text = text;
        }

        public string getText(string text)
        {
            return fishText.text;
        }

        public void update()
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
        }
    }

    public class Bubbles
    {
        bool showingBubbles = false;

        int MAX_HEIGHT = 287;
        int bubbleSpeed = 2;
        int currentTankCount = 140;

        AudioClip bubblesPopping;
        AudioClip bubblesRising;

        GameObject gameObject;

        System.Random random;
        public Bubbles(GameObject bubbleRef)
        {
            gameObject = bubbleRef;
            // gameObject.GetComponent<Button>().onClick.AddListener
            random = new System.Random();
        }

        public void popBubble()
        {
            Audio.play(bubblesPopping);

            currentTankCount += 50;
            if (currentTankCount > 140)
            {
                currentTankCount = 140;
            }
            gameObject.SetActive(false);
        }

        public void startBubble()
        {

            Audio.play(bubblesRising);

            showingBubbles = true;
            gameObject.SetActive(true);
            gameObject.transform.localPosition = new Vector3(random.Next(-356, 280), -236, 0);
        }

        public void update()
        {
            float bubbleHeight = bubble.transform.localPosition.y;
            bubble.transform.localPosition = new Vector2(bubble.transform.localPosition.x, bubbleHeight + fishSpeed);
            if (bubbleHeight >= MAX_HEIGHT)
            {
                showingBubbles = false;
                bubble.SetActive(false);
            }
        }

        public bool getShowingBubbles()
        {
            return showingBubbles;
        }


    }

    public class AirTank
    {
        public GameObject oxygenTank;

        void update()
        {
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
            }
        }
    }

    public class Diver
    {
        bool goingDown = false;
        int currHeight;

        int minHeight;
        int maxHeight;

        public GameObject gameObject;
        public void update()
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
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, currHeight, gameObject.transform.localPosition.z);

        }

    }

    public static class Audio
    {
        public static AudioSource source;
        
        public static void play(AudioClip clip)
        {
            source.clip = clip;
            source.Play();
        }
    }

    public class Background
    {
        public GameObject background;
        float deltaB, deltaG; //for color change
        Text questionText;
        public Background(Text questionTextRef)
        {
            deltaB = 0.1f;
            deltaG = 0.2f;
            questionText = questionTextRef;
        }

        public void changeColor()
        {
            Color currColor = background.GetComponent<Image>().color;
            if (currColor.g < 0.4f)
            {
                //change the text color to white
                questionText.GetComponent<Text>().color = Color.white;
            }
            background.GetComponent<Image>().color = new Color(currColor.r, currColor.g - deltaG, currColor.b - deltaB);
        }
}


    public class Question
    {
        utilities.Question currentQuestion;
        utilities utility;
        int score = 0;

        AudioClip buttonSound;
        AudioClip wrongAnswerSound;
        AudioClip rightAnswerSound;

        Text questionText;

        public Question()
        {
            utility = new utilities();
        }
        public void nextQuestion()
        {
            score++;


            if (differentiator.question.Count > 0)
            {
                differentiator.question = new List<utilities.Question>();
            }

            utility.getQuestion("1", 0.1f);

            currentQuestion = differentiator.question[0];

            questionText.text = currentQuestion.question;
            currentQuestion.setStringAnswer();
        }

        public bool checkAnswer(Fish fish)
        {
            Audio.play(buttonSound);

            if (fish.sprite.transform.GetChild(0).GetComponent<Text>().text == currentQuestion.stringAnswer)
            {
                Audio.play(rightAnswerSound);
                nextQuestion();
                return true;
            }
            else
            {
                Audio.play(wrongAnswerSound);
                return false;
            }
        }
    }




    void initSprites()
    {
        for (int i = 0; i < fishContainer.transform.childCount; i++)
        {
            bool goingRight = i % 2 == 0;
            Fish currFish = new Fish(fishContainer.transform.GetChild(i).gameObject, goingRight);
            fishes.Add(currFish);
            fishContainer.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate { question.checkAnswer(currFish); });
        }
    }


    void Update()
    {

        if (Information.doneLoading)
        {
            SceneManager.LoadScene("Math");
        }
    }

    IEnumerator handleAnimations()
    {
        while (!gameOver)
        {
            updateAirTank();

            if (bubble.getShowingBubbles())
            {
                bubble.update();
            }

            foreach(var f in fishes)
            {
                f.update();
            }

            diver.update();

            yield return new WaitForSeconds(1);
        }
    }







    void generateFish()
    {
        int choiceIndex = 0;
        string[] choices = utility.multipleChoiceNumber(currentQuestion.stringAnswer);
        foreach(var f in fishes)
        {
            //just assign the multple choice to the fish
            f.setText(choices[choiceIndex++]);
            if (choiceIndex > choices.Length - 1)
            {
                choices = utility.multipleChoiceNumber(currentQuestion.stringAnswer); //maybe only have one fish with the right answer?
                choiceIndex = 0;
            }
        }
    }


    void updateFishSpeed()
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
    }



    public GameObject inBetween;
    

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
