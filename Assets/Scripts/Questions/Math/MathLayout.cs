using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MathLayout : MonoBehaviour
{


    public TMP_Text questionText;

    public GameObject cursor;

    public GameObject fractionBox;
    public GameObject plainBox;
    public GameObject exponentBox;
    public GameObject dragAndDrop;

    public GameObject numbers;
    public GameObject romanNumbers;
    public GameObject varContainer;

    public Button currentBox;
    public Button tempClick;
    public Material mat;

    utilities.Question question;
    utilities utility;

    bool isSingleDigit = false;

    List<GameObject> numberButtons;
    List<GameObject> multipleButtons;

    TMP_Text currentDad = null;

    bool isOpen = false;

    GameObject[] userBoxes;

    public AudioSource source;
    public AudioClip buttonSound;
    public AudioClip DadEnter;
    public AudioClip DadExit;
    public AudioClip correct;
    public AudioClip inCorrect;


    void Start()
    {
        Information.isSelect = false;
        Information.isVrMode = false;

        tempClick.onClick.AddListener(delegate { checkAnswer(true); });
        initRomanNumbers();
        Information.click2d = true;
        Information.isSelect = true;
        time.onValueChanged.AddListener(delegate { checkAnswer(false); });

    }


    private void OnEnable()
    {

        if (numberButtons == null)
            initNumberBar();

        utility = new utilities();
        if (differentiator.question.Count > 0)
        {
            question = differentiator.question[0];
        }
        else
        {
            Debug.LogError("no question");
            return;
        }

        questionText.gameObject.SetActive(true);

        if (question.isClickable)
        {
            questionText.text = question.question + " " + question.stringAnswer;
            Debug.Log("returning, because it is clickable");
            return; //because you dont need any of that 
            //this part doesnt seem to work 
        }

        question.setStringAnswer();
        questionText.text = question.question; //+ " " + question.stringAnswer;


        if (question.isDad)
        {
            createDragAndDrop();
        }
        else
        {

            if (question.isRomam) //show the roman numerals only, then create the box
            {

                romanNumbers.SetActive(true);
                createBoxQuestion();
            }
            else if (question.isExpression)
            {
                initVariables();
                createBoxQuestion();

            }
            else
            {


                if (question.multipleChoice || utility.toss() && !question.isOrdering)
                {
                    question.multipleChoice = true;
                    if (question.isFraction)
                    {
                        string[] temp = utility.multipleChoiceFraction(question.stringAnswer);
                        question.choices = temp.ToList<string>();
                    }
                    else if (question.isTime)
                    {
                        question.choices = utility.multipleChoiceTime(question.stringAnswer).ToList<string>();
                    }
                    createMultipleChoice();
                }
                else
                {
                    createBoxQuestion();
                }

            }

        }

        Information.updateEntities = userBoxes;
    }



    void deleteVariables()
    {
        if (userVariables == null || userVariables.Count == 0)
            return;

        for (int i = userVariables.Count - 1; i >= 0; i--)
        {
            numberButtons.Remove(userVariables[i]);
            Destroy(userVariables[i]);
        }
    }
    List<GameObject> userVariables;
    void initVariables()
    {
        userVariables = new List<GameObject>();
        if (numberButtons == null)
        {
            initNumberBar();
        }
        Vector2 offset = new Vector2(0, 0);
        List<char> varialbes = new List<char>();
        varialbes.Add('+');
        varialbes.Add('^');

        for (int i = 0; i < question.stringAnswer.Length; i++)
        {
            if (char.IsLetter(question.stringAnswer[i]))
            {
                if (!varialbes.Contains(question.stringAnswer[i]))
                {
                    varialbes.Add(question.stringAnswer[i]);
                }
            }
        }

        for (int i = 0; i < varialbes.Count; i++)
        {
            GameObject curr = Instantiate(varContainer, varContainer.transform, true);
            curr.transform.SetParent(curr.transform.parent.parent);
            curr.transform.GetComponent<Button>().onClick.AddListener(delegate { buttonClick(curr.transform.GetComponent<Button>()); });
            curr.transform.GetChild(0).GetComponent<TMP_Text>().text = varialbes[i].ToString();
            curr.transform.Translate(offset);
            offset.y += 15;
            curr.SetActive(true);
            userVariables.Add(curr.gameObject);
            numberButtons.Add(curr.gameObject);
        }
    }

    void initRomanNumbers()
    {
        int length = romanNumbers.transform.childCount;
        for (int i = 0; i < length - 1; i++)
        {
            Button curr = romanNumbers.transform.GetChild(i).GetComponentInChildren<Button>();
            numberButtons.Add(curr.gameObject);
            curr.onClick.AddListener(delegate { buttonClick(curr); });
            curr.gameObject.SetActive(true);
        }
        romanNumbers.transform.GetChild(length - 1).GetComponent<Button>().onClick.AddListener(delegate { backButton(); });
    }


    void initNumberBar()
    {
        numberButtons = new List<GameObject>();
        int length = numbers.transform.childCount;
        for (int i = 0; i < length - 3; i++)
        {
            Button curr = numbers.transform.GetChild(i).GetComponentInChildren<Button>();
            numberButtons.Add(curr.gameObject);
            curr.onClick.AddListener(delegate { buttonClick(curr); });
        }

        numbers.transform.GetChild(length - 1).GetComponent<Button>().onClick.AddListener(delegate { backButton(); });
        numbers.transform.GetChild(length - 2).GetComponent<Button>().onClick.AddListener(delegate { negativeButton(); });
        numbers.transform.GetChild(length - 3).GetComponent<Button>().onClick.AddListener(delegate { bracketButton(); });
    }

    bool isNegative = false;
    void negativeButton()
    {

        if (currentBox != null)
        {
            string text = currentBox.GetComponentInChildren<TMP_Text>().text;
            if (!question.isExpression)
            {
                if (!isNegative)
                {
                    currentBox.GetComponentInChildren<TMP_Text>().text = "-" + text;
                    isNegative = true;
                }
                else
                {
                    if (text.Length > 1)
                        text = text.Substring(1);

                    currentBox.GetComponentInChildren<TMP_Text>().text = text;
                    isNegative = false;
                }
            }
            else
            {
                currentBox.GetComponentInChildren<TMP_Text>().text += "-";
            }

        }
    }


    void bracketButton()
    {

        if (currentBox != null)
        {
            if (!isOpen)
            {
                isOpen = true;
                currentBox.GetComponentInChildren<TMP_Text>().text += "(";
            }
            else
            {
                isOpen = false;
                currentBox.GetComponentInChildren<TMP_Text>().text += ")";
            }
        }
    }



    void checkAnswer(bool click)
    {
        int count = 0;
        string currAnswer = "";
        if (question.multipleChoice)
        {
            if (currentBox != null)
            {
                checkAnswer(currentBox.GetComponentInChildren<TMP_Text>().text, true);
            }

            return;
        }
        for (int i = 0; i < userBoxes.Length; i++) //this is for multiple boxes
        {
            if (question.isFraction)
            {
                if (question.isMixed)
                {
                    currAnswer += userBoxes[i].transform.GetChild(5).GetChild(0).GetComponent<TMP_Text>().text + " ";
                }
                currAnswer += userBoxes[i].transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>().text + "/" + userBoxes[i].transform.GetChild(4).GetChild(0).GetComponent<TMP_Text>().text;

                if (userBoxes[i].transform.GetChild(4).GetChild(0).GetComponent<TMP_Text>().text.Length > 0)
                {
                    count++;
                }
            }
            else if (question.isExponent)
            {
                currAnswer += userBoxes[i].transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>().text + "^" + userBoxes[i].transform.GetChild(4).GetChild(0).GetComponent<TMP_Text>().text;

                if (userBoxes[i].transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>().text.Length > 0)
                {
                    count++;
                }
            }
            else
            {
                currAnswer += userBoxes[i].transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>().text;
                if (userBoxes[i].transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>().text.Length > 0)
                {
                    count++;
                }
            }


            if (i != userBoxes.Length - 1)
            {
                currAnswer += ",";
            }

        }
        if (question.isTime)
        {
            if (time.isOn)
            {
                currAnswer += " AM";
            }
            else
            {
                currAnswer += " PM";
            }

        }

        if (count == userBoxes.Length)
        {
            checkAnswer(currAnswer, click);
        }

    }

    void checkAnswer(string answer, bool click)
    {
        string[] currNumbers = question.stringAnswer.Split(',');
        string[] userNumbers = answer.Split(',');
        if (question.isOrdering) //order matters
        {
            if (answer == question.stringAnswer)
            {
                Information.isCorrect = true;

            }
            else
            {

                if (click || Information.isCurriculum)
                {
                    source.clip = inCorrect;
                    source.Play();

                    Information.isCorrect = false;
                    Information.isIncorrect = true;

                }
                return;
            }
            return;
        }
        else if (question.isExpression)
        {
            currNumbers = getTerms(question.stringAnswer).ToArray();
            userNumbers = getTerms(answer).ToArray();
            if (userNumbers.Length != currNumbers.Length)
            {
                if (click || Information.isCurriculum)
                {
                    source.clip = inCorrect;
                    source.Play();

                    Information.isCorrect = false;
                    Information.isIncorrect = true;
                }
                return;
            }
            for (int i = 0; i < currNumbers.Length; i++)
            {
                bool found = false;
                for (int j = 0; j < userNumbers.Length; j++)
                {
                    if (compareFactors(currNumbers[i], userNumbers[j]))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    if (click || Information.isCurriculum)
                    {
                        source.clip = inCorrect;
                        source.Play();

                        Information.isCorrect = false;
                        Information.isIncorrect = true;
                    }
                    return;
                }
            }

            source.clip = correct;
            source.Play();

            Information.isCorrect = true;



        }
        else
        {
            if (compareArrays(currNumbers, userNumbers))
            {
                Information.isCorrect = true;
            }
            else
            {
                if (click || Information.isCurriculum)
                {
                    source.clip = inCorrect;
                    source.Play();

                    Information.isCorrect = false;
                    Information.isIncorrect = true;
                    return;
                }
            }
        }
    }

    bool compareArrays(string[] currNumbers, string[] userNumbers)
    {
        if (currNumbers.Length != userNumbers.Length)
        {
            return false;
        }
        bool[] incl = new bool[userNumbers.Length];
        for (int i = 0; i < incl.Length; i++)
        {
            incl[i] = false;
        }
        for (int i = 0; i < currNumbers.Length; i++)
        {

            int j = 0;
            for (j = 0; j < userNumbers.Length; j++)
            {

                if (currNumbers[i] == userNumbers[j] && incl[j] == false)
                {
                    incl[j] = true;
                    break;
                }
            }

            if (j == currNumbers.Length)
            {
                return false;
            }

        }
        return true;
    }


    bool compareFactors(string a, string b)
    {
        List<string> aList = getInnerTerm(a);
        List<string> bList = getInnerTerm(b);

        return compareArrays(aList.ToArray(), bList.ToArray());
    }

    List<string> getInnerTerm(string str)
    {
        string currNumber = "";
        List<string> output = new List<string>();
        for (int i = 0; i < str.Length; i++)
        {
            if (char.IsNumber(str[i]) || str[i] == '.')
            {
                currNumber += str[i];

            }
            else
            {
                if (currNumber != "")
                {
                    output.Add(currNumber);
                    currNumber = "";
                }


                output.Add(str[i].ToString());
            }
        }
        if (currNumber != "")
            output.Add(currNumber);
        return output;

    }


    List<string> getTerms(string str)
    {
        List<string> output = new List<string>();
        int termStart = 0;
        string curr = "";
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == ' ')
                continue;
            if (str[i] == '+' || str[i] == '-' || str[i] == ')')
            {

                if (termStart != i)
                {
                    output.Add(curr);
                    termStart = i + 1;
                    curr = "";
                }
            }
            if (str[i] != '+' && str[i] != '(' && str[i] != ')')
                curr += str[i];
        }
        if (curr != "")
        {
            output.Add(curr);
        }
        return output;
    }


    void checkDad()
    {
        string checkAnswer = "";
        int dadClick = 0;
        for (int i = 1; i < dragAndDrop.transform.childCount; i++)
        {
            string currText = dragAndDrop.transform.GetChild(i).GetComponentInChildren<TMP_Text>().text;
            if (dragAndDrop.transform.GetChild(i).GetChild(0).gameObject.activeSelf && currText.Length > 0)
            {
                dadClick++;
            }
            checkAnswer += currText;

            if (i != dragAndDrop.transform.childCount - 1)
            {
                checkAnswer += ",";
            }
        }
        if (checkAnswer == question.stringAnswer)
        {
            source.clip = correct;
            source.Play();

            Information.isCorrect = true;

        }
        else
        {
            if (dadClick >= dragAndDrop.transform.childCount - 1)
            {
                source.clip = inCorrect;
                source.Play();
                Information.isIncorrect = true;
            }
        }
    }



    public void clearPrevious()
    {
        if (userBoxes != null)
        {


            for (int i = 0; i < userBoxes.Length; i++)
            {
                userBoxes[i].SetActive(false);
            }
        }

        questionText.gameObject.SetActive(false);
        numbers.SetActive(false);
        romanNumbers.SetActive(false);
        dragAndDrop.SetActive(false);
        time.gameObject.SetActive(false);
        deleteVariables();

    }

    int getBoxIndex()
    {
        if (currentBox != null)
        {
            for (int i = 1; i < boxes.transform.childCount; i++)
            {
                if (boxes.transform.GetChild(i).transform == currentBox.transform)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    int getUserBoxIndex(GameObject curr)
    {
        for (int i = 0; i < userBoxes.Length; i++)
        {
            if (userBoxes[i].transform == curr.transform)
            {
                return i;
            }
        }
        return -1;
    }


    public GameObject boxes;
    void createMultipleChoice()
    {

        boxes.SetActive(true);

        if (question.choices.Count > 0)
        {
            createMultipleChoice(question.choices);
        }
        else
        {
            string[] num = utility.multipleChoiceNumber(question.stringAnswer);
            createMultipleChoice(new List<string>(num));
        }
    }
    float multipleChoiceOffset = 40;
    void createMultipleChoice(List<string> choices)
    {

        boxes.SetActive(true);

        string[] num = choices.ToArray();
        float newOffset = 0 - (num.Length - 1) / (float)2 * multipleChoiceOffset;
        userBoxes = new GameObject[num.Length];
        for (int i = 0; i < num.Length; i++)
        {
            GameObject curr = null;
            curr = Instantiate(boxes.transform.GetChild(0).gameObject, boxes.transform.GetChild(0), true);
            curr.transform.SetParent(boxes.transform);
            curr.gameObject.SetActive(true);
            curr.transform.Translate(new Vector2(newOffset + multipleChoiceOffset * i, 0));
            curr.transform.GetChild(0).GetComponent<TMP_Text>().text = num[i];
            curr.GetComponent<Button>().onClick.AddListener(delegate { onClick(curr.GetComponent<Button>()); });
            curr.SetActive(true);
            userBoxes[i] = curr;
        }
    }


    public Toggle time;
    public Canvas canvas;
    public void createBoxQuestion()
    {
        if (!question.isRomam)
        {
            numbers.SetActive(true);
        }

        if (question.isTime)
        {
            time.gameObject.SetActive(true);
        }

        string[] userNumbers = question.stringAnswer.Split(',');

        float boxOffset = plainBox.transform.GetComponent<RectTransform>().sizeDelta.x * 0.3f;
        userBoxes = new GameObject[userNumbers.Length];

        float newOffset = 0 - (userNumbers.Length - 1) / (float)2 * boxOffset;

        for (int i = 0; i < userNumbers.Length; i++)
        {
            GameObject curr = null;

            if (question.isFraction)
            {
                curr = Instantiate(fractionBox, fractionBox.transform, true);

                if (question.isMixed)
                {
                    curr.transform.GetChild(5).gameObject.SetActive(true);
                }

                curr.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { userBoxClick(curr.transform.GetChild(3).gameObject); });
                curr.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(delegate { userBoxClick(curr.transform.GetChild(4).gameObject); });
                curr.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(delegate { userBoxClick(curr.transform.GetChild(5).gameObject); });

            }
            else if (question.isExponent)
            {
                curr = Instantiate(exponentBox, exponentBox.transform, true);
                curr.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { userBoxClick(curr.transform.GetChild(3).gameObject); });
                curr.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(delegate { userBoxClick(curr.transform.GetChild(4).gameObject); });

            }
            else
            {
                curr = Instantiate(plainBox, plainBox.transform, true);
                curr.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { userBoxClick(curr.transform.GetChild(2).gameObject); });
                currentBox = curr.transform.GetChild(2).GetComponent<Button>();
            }
            curr.transform.SetParent(curr.transform.parent.parent);

            curr.transform.Translate(new Vector2(newOffset + boxOffset * i, 0));
            curr.transform.GetChild(0).GetComponent<TMP_Text>().text = question.before;
            curr.transform.GetChild(1).GetComponent<TMP_Text>().text = question.after;

            if (i != userNumbers.Length - 1)
            {
                if (question.inBetween != "")
                    curr.transform.GetChild(1).GetComponent<TMP_Text>().text = question.inBetween;

                curr.transform.Translate(new Vector2((question.inBetween.Length - 1) * (-2f), 0));
            }

            curr.SetActive(true);
            userBoxes[i] = curr;
        }
    }

    void userBoxClick(GameObject curr)
    {
        source.clip = buttonSound;
        source.Play();

        currentBox = curr.GetComponent<Button>();
    }


    public void createDragAndDrop()
    {
        dragAndDrop.gameObject.SetActive(true);
        string[] tempNumbers = question.stringAnswer.Split(',');
        //you just need to mix it up
        System.Random rnd = new System.Random();
        string[] numbers = tempNumbers.OrderBy(x => rnd.Next()).ToArray();
        userBoxes = new GameObject[numbers.Length];

        float boxOffset = plainBox.transform.GetComponent<RectTransform>().sizeDelta.x * 0.3f;
        userBoxes = new GameObject[userBoxes.Length];

        float newOffset = 0 - (userBoxes.Length - 1) / (float)2 * boxOffset;


        for (int i = 0; i < numbers.Length; i++)
        {
            GameObject curr = Instantiate(dragAndDrop.transform.GetChild(0).gameObject, dragAndDrop.transform.GetChild(0), true);
            curr.transform.SetParent(curr.transform.parent.parent);
            curr.transform.Translate(new Vector2(newOffset + boxOffset * i, 0));

            curr.transform.GetChild(0).GetComponent<TMP_Text>().text = numbers[i];

            curr.SetActive(true);

            curr.GetComponent<Button>().onClick.AddListener(delegate { dadEnterClick(curr.transform.GetChild(0).GetComponent<TMP_Text>()); });
            curr.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { dadExitClick(curr.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>()); });

            userBoxes[i] = curr;
        }
    }


    void onClick(Button curr)
    {
        source.clip = buttonSound;
        source.Play();

        currentBox = curr;
        if (question.multipleChoice)
        {
            checkAnswer(true);
        }
        else
        {
            if (!numbers.activeSelf)
            {
                curr.GetComponentInChildren<TMP_Text>().text = "";
                numbers.SetActive(true);
            }
        }
    }

    void backButton()
    {
        source.clip = buttonSound;
        source.Play();

        if (currentBox != null && currentBox.transform.GetChild(0).GetComponent<TMP_Text>().text.Length > 0)
        {
            string curr = currentBox.transform.GetChild(0).GetComponent<TMP_Text>().text;
            currentBox.transform.GetChild(0).GetComponent<TMP_Text>().text = curr.Substring(0, curr.Length - 1);
        }
    }



    void buttonClick(Button curr)
    {
        source.clip = buttonSound;
        source.Play();

        if (currentBox != null)
        {
            currentBox.GetComponentInChildren<TMP_Text>().text += curr.GetComponentInChildren<TMP_Text>().text;

            Debug.LogError(curr.transform + " " + curr.GetComponentInChildren<TMP_Text>().text);
            checkAnswer(false);

        }
    }

    void dadEnterClick(TMP_Text curr)
    {
        source.clip = DadEnter;
        source.Play();

        currentDad = curr;
    }


    int dadCount = 0;
    void dadExitClick(TMP_Text curr)
    {
        source.clip = DadExit;
        source.Play();

        if (currentDad == null)
        {


            for (int i = 0; i < dragAndDrop.transform.childCount; i++)
            {
                if (dragAndDrop.transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text == curr.text && !dragAndDrop.transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>().gameObject.activeSelf)
                {
                    dragAndDrop.transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>().gameObject.SetActive(true);
                    break;
                }
            }

            curr.text = "";
        }
        else
        {

            if (!currentDad.gameObject.activeSelf)
                return;

            string pastText = curr.text;
            curr.text = currentDad.text;
            currentDad.gameObject.SetActive(false);
            checkDad();


            currentDad = null;
            if (pastText != "")
            {
                for (int i = 0; i < dragAndDrop.transform.childCount; i++)
                {
                    if (dragAndDrop.transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text == pastText && !dragAndDrop.transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>().gameObject.activeSelf)
                    {
                        dragAndDrop.transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>().gameObject.SetActive(true);
                        break;

                    }
                }
            }
        }
    }



    int getNumberAnimationIndex(GameObject curr)
    {
        for (int i = 0; i < numberButtons.Count; i++)
        {
            if (curr == numberButtons[i])
                return i;
        }
        return -1;
    }

    int getMultipleAnimationIndex(GameObject curr)
    {
        for (int i = 0; i < multipleButtons.Count; i++)
        {
            if (curr == multipleButtons[i])
                return i;
        }
        return -1;
    }
}
