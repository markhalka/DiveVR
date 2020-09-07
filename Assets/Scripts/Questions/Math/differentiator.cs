using System.Collections.Generic;
using UnityEngine;

public class differentiator
{


    public static float level = 0;
    public static List<utilities.Question> question = new List<utilities.Question>();

    static System.Random random = new System.Random();

    static utilities utiltiy = new utilities();
    static questionBank questionBankObj = new questionBank();
    //this gets called from utils, the level of difficulty has to be set from utils 

    //ok so here just change question.fourops to take a list, then in there change the question and answer so it depends on the list
    //do the same thing for the other three ops 
    public static void additionLadder(bool multiple)
    { //0 whole, 1 decimals 2 fractions
        float x = 0, y = 0;
        List<float> numbers = new List<float>();
        int length = 4;
        int questionLength = 4;
        //    float newLevel = level - 0.1f; //make it a little easier
        float newLevel = level;
        Debug.Log(newLevel + " new levle");
        switch (newLevel)
        {
            case 0:
                x = utiltiy.getRandom(0, 10);
                y = utiltiy.getRandom(0, 10);
                length = 1;
                questionLength = 2;
                break;
            case 0.1f:
                x = utiltiy.getRandom(0, 10);
                y = utiltiy.getRandom(10, 99);
                length = 2;
                questionLength = 3;
                break;
            case 0.2f:
                x = utiltiy.getRandom(10, 99);
                y = utiltiy.getRandom(10, 99);
                length = 3;
                break;
            case 0.3f:
                x = utiltiy.getRandom(10, 99);
                y = utiltiy.getRandom(100, 999);
                break;
            case 0.4f:
            case 0.5f:
            case 0.6f:
                x = utiltiy.getRandom(100, 999);
                y = utiltiy.getRandom(100, 999);
                break;


            case 0.7f:
                x = (float)utiltiy.getRandom(0, 10) / Mathf.Pow(10, utiltiy.getRandom(1, 2));
                y = (float)utiltiy.getRandom(0, 10) / Mathf.Pow(10, utiltiy.getRandom(1, 2));

                break;
            case 0.8f:
                //  Debug.Log("here " + utiltiy.getRandom(10, 99) + " " + Mathf.Pow(10, utiltiy.getRandom(1, 3)));
                x = (float)utiltiy.getRandom(10, 99) / Mathf.Pow(10, utiltiy.getRandom(1, 3));
                y = (float)utiltiy.getRandom(10, 99) / Mathf.Pow(10, utiltiy.getRandom(1, 3));
                break;
            case 0.9f:
                x = (float)utiltiy.getRandom(100, 999) / Mathf.Pow(10, utiltiy.getRandom(1, 3));
                y = (float)utiltiy.getRandom(100, 999) / Mathf.Pow(10, utiltiy.getRandom(1, 3));
                break;
            case 1:
                x = (float)utiltiy.getRandom(1000, 9999) / Mathf.Pow(10, utiltiy.getRandom(1, 4));
                y = (float)utiltiy.getRandom(1000, 9999) / Mathf.Pow(10, utiltiy.getRandom(1, 4));
                break;
        }
        if (random.NextDouble() < 0.5)
        {
            float temp = y;
            y = x;
            x = temp;
        }

        if (multiple)
        {
            for (int i = 0; i < questionLength - 1; i++)
            {
                numbers.Add(utiltiy.getRandom(0, 5000));
            }
        }

        numbers.Add(x);
        numbers.Add(y);

        Debug.Log(x + " " + y);

        question.Add(questionBankObj.fourOps(numbers, "+"));
    }
    public static void subtractionLadder(bool multiple)
    {
        float x = 0; float y = 0;
        List<float> numbers = new List<float>();
        int questionLength = 4;
        int length = 4;
        switch (level)
        {
            case 0:
                x = utiltiy.getRandom(0, 10);
                y = utiltiy.getRandom(0, 10);
                length = 1;
                questionLength = 2;
                break;
            case 0.1f:
                x = utiltiy.getRandom(0, 10);
                y = utiltiy.getRandom(10, 100);
                questionLength = 3;
                length = 2;
                break;
            case 0.2f:
                x = utiltiy.getRandom(10, 100);
                y = utiltiy.getRandom(10, 100);
                questionLength = 4;
                length = 3;
                break;
            case 0.3f:
                x = utiltiy.getRandom(10, 100);
                y = utiltiy.getRandom(100, 1000);
                break;
            case 0.4f:
                x = utiltiy.getRandom(100, 1000);
                y = utiltiy.getRandom(100, 1000);
                break;
            case 0.5f:
                x = utiltiy.getRandom(300, 1000);
                y = utiltiy.getRandom(300, 1000);
                break;
            case 0.6f:
                x = utiltiy.getRandom(500, 1000);
                y = utiltiy.getRandom(500, 1000);
                break;
            case 0.7f:
                x = utiltiy.getRandom(0, 10) / Mathf.Pow(10, utiltiy.getRandom(1, 2));
                y = utiltiy.getRandom(0, 10) / Mathf.Pow(10, utiltiy.getRandom(1, 2));
                break;
            case 0.8f:
                x = utiltiy.getRandom(10, 100) / Mathf.Pow(10, utiltiy.getRandom(1, 3));
                y = utiltiy.getRandom(10, 100) / Mathf.Pow(10, utiltiy.getRandom(1, 3));
                break;
            case 0.9f:
                x = utiltiy.getRandom(100, 1000) / Mathf.Pow(10, utiltiy.getRandom(1, 3));
                y = utiltiy.getRandom(100, 1000) / Mathf.Pow(10, utiltiy.getRandom(1, 3));
                break;
            case 1:
                x = utiltiy.getRandom(1000, 10000) / Mathf.Pow(10, utiltiy.getRandom(1, 4));
                y = utiltiy.getRandom(1000, 10000) / Mathf.Pow(10, utiltiy.getRandom(1, 4));
                break;
        }


        if (multiple)
        {
            for (int i = 0; i < questionLength - 1; i++)
            {
                numbers.Add(utiltiy.getRandom(0, 5000));
            }
        }
        numbers.Add(utiltiy.roundError(Mathf.Max(x, y)));
        numbers.Add(utiltiy.roundError(Mathf.Min(x, y)));




        question.Add(questionBankObj.fourOps(numbers, "-"));

    }
    public static void multiplicationLadder(bool multiple)
    {
        float x = 0; float y = 0;
        int length = 2;
        List<float> numbers = new List<float>();
        int questionLength = 4;

        switch (level)
        {
            case 0:
                x = utiltiy.getRandom(0, 5);
                y = utiltiy.getRandom(0, 5);
                length = 1;
                questionLength = 2;
                break;
            case 0.1f:
                x = utiltiy.getRandom(1, 9);
                y = utiltiy.getRandom(3, 10);
                questionLength = 3;
                break;
            case 0.2f:
                x = utiltiy.getRandom(1, 9);
                y = utiltiy.getRandom(5, 12);
                questionLength = 4;
                break;
            case 0.3f:
                x = utiltiy.getRandom(2, 9);
                y = utiltiy.getRandom(5, 15);
                break;
            case 0.4f:
                x = utiltiy.getRandom(11, 99);
                y = utiltiy.getRandom(1, 10);
                break;
            case 0.5f:
                x = utiltiy.getRandom(11, 99);
                y = utiltiy.getRandom(1, 12);
                break;
            case 0.6f:
                x = utiltiy.getRandom(1, 9) / Mathf.Pow(10, utiltiy.getRandom(0, 1));
                y = utiltiy.getRandom(1, 9) / Mathf.Pow(10, utiltiy.getRandom(1, 2));
                break;
            case 0.7f:
                x = utiltiy.getRandom(1, 9) / Mathf.Pow(10, utiltiy.getRandom(0, 1));
                y = utiltiy.getRandom(11, 99) / Mathf.Pow(10, utiltiy.getRandom(1, 2));
                break;
            case 0.8f:
                x = utiltiy.getRandom(5, 99) / Mathf.Pow(10, utiltiy.getRandom(0, 1));
                y = utiltiy.getRandom(1, 10) / Mathf.Pow(10, utiltiy.getRandom(1, 2));
                break;
            case 0.9f:
                x = utiltiy.getRandom(10, 99) / Mathf.Pow(10, utiltiy.getRandom(0, 2));
                y = utiltiy.getRandom(10, 99) / Mathf.Pow(10, utiltiy.getRandom(1, 2));
                break;
            case 1:
                x = utiltiy.getRandom(10, 99) / Mathf.Pow(10, utiltiy.getRandom(0, 2));
                y = utiltiy.getRandom(10, 99) / Mathf.Pow(10, utiltiy.getRandom(1, 2));
                break;
        }
        if (random.NextDouble() < 0.5)
        {
            float temp = y;
            y = x;
            x = temp;
        }



        if (multiple)
        {
            for (int i = 0; i < questionLength - 1; i++)
            {
                numbers.Add(utiltiy.getRandom(1, 12));
            }
        }
        numbers.Add((int)utiltiy.roundError(x));
        numbers.Add((int)utiltiy.roundError(y));



        question.Add(questionBankObj.fourOps(numbers, "*"));
    }


    public static void divisionLadder()
    {
        float x = 0; float y = 0;
        switch (level)
        {
            case 0:
                y = utiltiy.getRandom(1, 5);
                x = y * utiltiy.getRandom(1, 5);
                break;
            case 0.1f:
                y = utiltiy.getRandom(2, 5);
                x = y * utiltiy.getRandom(1, 9);
                break;
            case 0.2f:
                y = utiltiy.getRandom(2, 9);
                x = y * utiltiy.getRandom(2, 12);
                break;
            case 0.3f:
                y = utiltiy.getRandom(2, 9);
                x = y * utiltiy.getRandom(2, 15);
                break;
            case 0.4f:
                y = utiltiy.getRandom(2, 9);
                x = y * utiltiy.getRandom(2, 20);
                break;
            case 0.5f:
                y = utiltiy.getRandom(11, 19);
                x = y * utiltiy.getRandom(5, 30);
                break;
            case 0.6f:
                y = utiltiy.getRandom(2, 9);
                x = y * utiltiy.getRandom(1, 9) / Mathf.Pow(10, utiltiy.getRandom(1, 2));
                break;
            case 0.7f:
                y = utiltiy.getRandom(2, 9);
                x = y * utiltiy.getRandom(2, 10) / Mathf.Pow(10, utiltiy.getRandom(1, 2));
                break;
            case 0.8f:
                y = utiltiy.getRandom(2, 9);
                x = y * utiltiy.getRandom(2, 15) / Mathf.Pow(10, utiltiy.getRandom(1, 2));
                break;
            case 0.9f:
                y = utiltiy.getRandom(11, 19);
                x = y * utiltiy.getRandom(2, 20) / Mathf.Pow(10, utiltiy.getRandom(1, 2));
                break;
            case 1:
                y = utiltiy.getRandom(11, 19) / Mathf.Pow(10, utiltiy.getRandom(1, 2));
                x = y * utiltiy.getRandom(5, 30) / Mathf.Pow(10, utiltiy.getRandom(1, 2));
                break;
        }
        List<float> numbers = new List<float>();
        numbers.Add((int)utiltiy.roundError(x));
        numbers.Add((int)utiltiy.roundError(y));

        question.Add(questionBankObj.fourOps(numbers, "/"));
    }
    public static void fourOpsLadder(bool mulitple)
    {
        // if its multple just change the length done 

        switch (utiltiy.getRandom(0, 3))
        {
            case 0:
                additionLadder(mulitple);
                break;
            case 1:
                subtractionLadder(mulitple);
                break;
            case 2:
                multiplicationLadder(mulitple);
                break;
            case 3:
                divisionLadder();
                break;
        }
    }

    //----------------------------------NEED TO BE TESTED---------------------------------



    //ok, now just generate example numbers, and in question bank set up the question, include it in utitlity and data, than done 

    public static void getLaws()
    {
        int a = utiltiy.getRandom(10, 30);
        int b = utiltiy.getRandom(10, 30);
        int c = utiltiy.getRandom(1, 9);
        string currQuestion = "";
        string answer = "";
        List<string> choices = new List<string>(new string[] { "commutative", "associative", "distributive" });
        switch (utiltiy.getRandom(0, 4))
        {
            case 0: //commutative (addition and multiplication)
                    //a + b = b + a
                currQuestion = a + " + " + b + " = " + b + " + " + a;
                answer = choices[0];
                break;
            case 1: //associative (a + b) + c = a + (b + c) (addition and multiplication)
                currQuestion = "(" + a + " + " + b + ") + " + c + " = " + a + "(" + b + " + " + a + ")";
                answer = choices[1];
                break;
            case 2: //distributive (a * (b + c) = a * b + a * c
                currQuestion = "(" + a + " + " + b + ") * " + c + " = " + a + " * " + c + " + " + b + " * " + c;
                answer = choices[2];
                break;
            case 3: //identity (when you multiply by 1) (or maybe its called unity)
                break;
        }

        question.Add(questionBankObj.lawsQuestion(currQuestion, answer, choices));

    }

    public static void getOperationTerminology()
    {
        int a = utiltiy.getRandom(10, 30);
        int b = utiltiy.getRandom(0, 10);
        int c = 0;
        string numbers = ""; //just 3 numbers seperated by commas
        string currQuestion = "";
        //then pass the right list of names, pick a random one than use the rest for choices 
        List<string> addition = new List<string>(new string[] { "addend", "addend", "sum" });
        List<string> subtraction = new List<string>(new string[] { "minuend", "subtrahed", "difference" });
        List<string> multiplication = new List<string>(new string[] { "factor", "factor", "product" });
        List<string> division = new List<string>(new string[] { "dividend", "divisor", "quotiant" });
        List<string> choices = null;


        switch (utiltiy.getRandom(0, 4))
        {
            case 0: //subtraction 
                    //minuend -  subtrahed = difference
                c = a - b;
                numbers = a + "," + b + "," + c;
                currQuestion = a + " - " + b + " = " + c;
                choices = addition;
                break;
            case 1: //addition
                c = a + b;
                numbers = a + "," + b + "," + c;
                currQuestion = a + " + " + b + " = " + c;
                choices = subtraction;
                //addend + addend = sum
                break;
            case 2: //multiplication
                c = a * b;
                numbers = a + "," + b + "," + c;
                currQuestion = a + " * " + b + " = " + c;
                choices = multiplication;
                //facotr * factor = product
                break;
            case 3: //division

                c = a * b;
                numbers = c + "," + b + "," + a;
                currQuestion = c + " / " + b + " = " + a;
                choices = division;
                // dividend / divisor = quotiant remainder
                break;
            case 4: //fraction
                //numerator / denominaotr 
                Debug.LogError("fraction...");
                break;

        }

        question.Add(questionBankObj.termonlogigyQuestion(currQuestion, numbers, choices));
    }

    //ok so here, just
    public static void placeValueLadder() //you need to fix Minor spelling and grammar errors 
    {
        switch (utiltiy.getRandom(0, 3))
        {
            case 0:
                identifyPlaceValueLadder();
                break;
            case 1:
                convertingBetweenPlaceValueLadder();
                break;
            case 2:
                expandedFormLadder(); //for this one you can switch the question and answer to go from simple to expanded form
                break;
            case 3:
                writtenToNumberLadder();
                break;

        }

        //identifyPlaceValue
        //convertingBetweenPlaceValue
        //expandedForm
        //writtenToNumber


    }

    public static void identifyPlaceValueLadder()
    {

        int i = 0;
        string num = "";
        int length = 0;
        int deicmalLength = 0;
        i = utiltiy.getRandom(0, num.Length - 1);
        switch (level)
        {
            case 0.1f:
            case 0.2f:
            case 0.3f:
                length = utiltiy.getRandom(4, 6);
                break;
            case 0.4f:
            case 0.5f:
                length = utiltiy.getRandom(5, 7);
                break;
            case 0.6f:
            case 0.7f:
                length = utiltiy.getRandom(6, 9);
                deicmalLength = 3;
                break;
            case 0.8f:
            case 0.9f:
            case 1f:
                length = utiltiy.getRandom(9, 10);
                deicmalLength = 5;
                break;
        }

        num = utiltiy.getNumberWithLength(length, deicmalLength);
        float curr = float.Parse(num);
        int currNum = (int)curr;
        question.Add(questionBankObj.identifyPlaceValue(currNum, i));
    }

    public static void convertingBetweenPlaceValueLadder()
    {
        int co = 0;
        int a = 0, b = 0;
        switch (level)
        {
            case 0.1f:
            case 0.2f:
            case 0.3f:
                a = utiltiy.getRandom(1, 2);
                b = utiltiy.getRandom(a, 4);
                co = utiltiy.getRandom(1, 9);
                break;
            case 0.4f:
            case 0.5f:
                a = utiltiy.getRandom(1, 5);
                b = utiltiy.getRandom(a, 6);
                co = utiltiy.getRandom(1, 15);
                break;
            case 0.6f:
            case 0.7f:
            case 0.8f:
                a = utiltiy.getRandom(2, 5);
                b = utiltiy.getRandom(a, 9);
                co = utiltiy.getRandom(5, 20);
                break;
            case 0.9f:
            case 1f:
                a = utiltiy.getRandom(2, 5);
                b = utiltiy.getRandom(a, 10);
                co = utiltiy.getRandom(7, 30);
                break;


        }
        question.Add(questionBankObj.convertingBetweenPlaceValue(co, b, a));
    }

    public static void expandedFormLadder()
    {
        int x = 0;
        switch (level)
        {
            case 0.1f:
            case 0.2f:
            case 0.3f:
                x = utiltiy.getRandom(0, 100);
                break;
            case 0.4f:
            case 0.5f:
                x = utiltiy.getRandom(0, 10000);
                break;
            case 0.6f:
            case 0.7f:
            case 0.8f:
                x = utiltiy.getRandom(0, 70000);
                break;
            case 0.9f:
            case 1:
                x = utiltiy.getRandom(0, 700000);
                break;

        }
        question.Add(questionBankObj.expandedForm(x));
    }

    public static void writtenToNumberLadder()
    {
        string number = "";
        int length = 0;
        string input = "";
        switch (level)
        {
            case 0.1f:
            case 0.2f:
            case 0.3f:
                length = utiltiy.getRandom(0, 3);
                break;
            case 0.4f:
            case 0.5f:
                length = utiltiy.getRandom(0, 5);

                break;
            case 0.6f:
            case 0.7f:
            case 0.8f:
                length = utiltiy.getRandom(0, 7);
                break;
            case 0.9f:
            case 1:
                length = utiltiy.getRandom(0, 9);
                break;
        }
        for (int i = length + 1; i >= 0; i--)
        {
            int curr = utiltiy.getRandom(0, 9);
            string ending = curr == 1 ? " " : "'s ";
            input += curr + " " + utiltiy.placeValuePicker(i) + ending;
            if (i == 1)
            {
                input += " and ";
            }
            number += curr.ToString();
        }
        Debug.Log(number + " number for place value");
        question.Add(questionBankObj.writtenToNumber(input, int.Parse(number)));
    }


    public static void comparingNumersLadder()
    {
        int a = 0;
        int b = 0;
        bool isEqual = false;
        if (utiltiy.getRandom(0, 3) == 0)
        {
            isEqual = true;
        }
        switch (level)
        {
            case 0.1f:
            case 0.2f:
            case 0.3f:
                a = utiltiy.getRandom(0, 100);
                if (isEqual)
                {
                    b = a;
                }
                else
                {
                    b = utiltiy.getRandom(0, 100);
                }
                break;

            case 0.4f:
            case 0.5f:

                a = utiltiy.getRandom(0, 1000);
                if (isEqual)
                {
                    b = a;
                }
                else
                {
                    b = utiltiy.getRandom(0, 1000);
                }

                break;
            case 0.6f:
            case 0.7f:
            case 0.8f:
                a = utiltiy.getRandom(-100, 300);
                if (isEqual)
                {
                    b = a;
                }
                else
                {
                    b = utiltiy.getRandom(-100, 300);
                }

                break;
            case 0.9f:
            case 1:
                a = utiltiy.getRandom(-500, 500);
                if (isEqual)
                {
                    b = a;
                }
                else
                {
                    b = utiltiy.getRandom(-500, 500);
                }

                break;


        }

        if (utiltiy.toss())
        {

            question.Add(questionBankObj.choosingNumbers());
            return;
        }
        question.Add(questionBankObj.comparingNumbers(a, b));
    }

    /* public static void comparingMixedLadder() //you should make this one work for numbers, mixed numbers decimals and fractions depending on the leve l
     {
         bool isFraction = false;
         int a = 0;
         int b = 0;
         bool isEqual = false;
         if (utiltiy.getRandom(0, 3) == 0)
         {
             isEqual = true;
         }
         switch (level)
         {
             //comparingNumbers (both of them)
             case 0.5f:

                 a = utiltiy.getRandom(0, 1000);
                 if (isEqual)
                 {
                     b = a;
                 }
                 else
                 {
                     b = utiltiy.getRandom(0, 1000);
                 }

                 break;


         }
         question.Add(questionBankObj.comparingNumbers(a, b));
     }*/


    public static void estimatingLadder()
    {
        char[] signs = new char[] { '+', '-', '*', '/' };
        int index = utiltiy.getRandom(0, 3);
        int a = 0;
        int b = 0;
        int placeValue = 0;
        switch (level)
        {
            case 0.1f:
            case 0.2f:
            case 0.3f:
                a = utiltiy.getRandom(1, 100);
                b = utiltiy.getRandom(1, 100);
                break;
            case 0.4f:
            case 0.5f:
                a = utiltiy.getRandom(1, 1000);
                b = utiltiy.getRandom(1, 1000);
                break;
            case 0.6f:
            case 0.7f:
            case 0.8f:
                a = utiltiy.getRandom(-100, 1000);
                b = utiltiy.getRandom(-100, 1000);
                break;
            case 0.9f:
            case 1:
                a = utiltiy.getRandom(-500, 2000);
                b = utiltiy.getRandom(-500, 2000);
                break;

                //estimating

        }
        int lengthA = a.ToString().Length;
        int lengthB = b.ToString().Length;
        int biggerLength = lengthA > lengthB ? lengthA : lengthB;
        placeValue = utiltiy.getRandom(1, biggerLength - 1);

        question.Add(questionBankObj.estimating(signs[index], a, b, placeValue));
    }

    public static void elapsedTimeLadder() //you need to make all of these waaay easier to do in your head 
    {

        bool am1 = utiltiy.toss();
        bool am2 = false;

        int h1 = utiltiy.getRandom(1, 12);
        int h2 = utiltiy.getRandom(1, 12);
        int m1 = utiltiy.getRandom(0, 59);
        int m2 = utiltiy.getRandom(0, 59);

        switch (level)
        {
            case 0.1f:
            case 0.2f:
            case 0.3f:
            case 0.4f:
            case 0.5f:
                am2 = am1;
                break;
            case 0.6f:
            case 0.7f:
            case 0.8f:
            case 0.9f:
            case 1:
                am2 = !am1;
                break;

                //estimating

        }
        question.Add(questionBankObj.elapsedTime(h1, m1, am1, h2, m2, am2));
    }
    /*               case 0.1f:
    case 0.2f:
    case 0.3f:
    break;
    case 0.4f:
*         
*         case 0.6f:
    case 0.7f:
    case 0.8f:
        break;
    case 0.9f:
    case 1:
        break;*/
    public static void timeProblemLadder()
    {
        //elapsedTime
        //timePassed
        int h1 = 0, m1 = 0, p1 = 0, p2 = 0;
        h1 = utiltiy.getRandom(1, 12);
        m1 = utiltiy.getRandom(0, 59);

        p1 = utiltiy.getRandom(1, 12);
        p2 = utiltiy.getRandom(0, 59);
        question.Add(questionBankObj.timePassed(h1, m1, p1, p2));
    }

    public static void convertingTimeLadder24()
    {
        //elapsedTime
        //timePassed
        bool is24 = utiltiy.toss();
        bool am = utiltiy.toss();
        int h1 = 0, m1 = 0;
        m1 = utiltiy.getRandom(0, 59);
        if (is24)
        {
            h1 = utiltiy.getRandom(1, 24);
        }
        else
        {
            h1 = utiltiy.getRandom(1, 12);
        }

        question.Add(questionBankObj.twelveTo24hour(h1, m1, am, is24));
    }

    //after braket make sure its a sign
    //make sure there are not ending brakets or signs 


    public static void bedmasLadder() //make sure there is a sign between number and bracket 
    {
        //ok, so here just generate some random bedmass question
        //so just have number sign braket or number sign number bracket or sign number bracket or sign number 
        int terms = 10;

        switch (level)
        {
            case 0.5f:
                break;
            case 1:
                break;

        }
        string expression = "";
        int type = 0; // 0 number 1 sign or bracket 2 exponent 
        bool openBracket = false;
        for (int i = 0; i < terms; i++)
        {
            if (type == 0) //number
            {
                type = 1;
                expression += utiltiy.getRandom(1, 20);
                if (utiltiy.toss())
                {
                    if (openBracket) //if its in a bracket, just close it
                    {
                        type = 3;
                    }
                    else
                    {
                        type = 2;
                    }

                }
                else
                {
                    type = 1;
                }
            }
            else if (type == 1) // sign
            {

                switch (utiltiy.getRandom(0, 2)) //leave division for now 
                {
                    case 0:
                        expression += "+";
                        break;
                    case 1:
                        expression += "-";
                        break;
                    case 2:
                        expression += "*";
                        break;
                }
                if (utiltiy.toss())
                {
                    type = 0;
                }
                else
                {
                    if (!openBracket) //if its in a braket then after a sign you cant have a braket
                    {
                        type = 3; //braket
                    }
                    else
                    {
                        type = 0; //number
                    }



                }

            }
            else if (type == 2) //exponent
            {
                expression += "^" + utiltiy.getRandom(1, 2);
                type = 1;
            }
            else if (type == 3) //braket 
            {
                if (openBracket)
                {
                    //check to see 
                    if (char.IsNumber(expression[expression.Length - 1])) //if there is a number then you can close it
                    {
                        expression += ")";
                        type = 1; // sign

                    }
                    else //add a number dont close it
                    {
                        type = 0;
                    }


                }
                else
                {
                    expression += "(";
                    type = 0; //number

                }
                openBracket = !openBracket;

            }

        }
        if (openBracket) //here check to see if the last char is a sign, in which case get rid of it 
                         //then check to see if the one before that is a bracket, in which case get rid of that bracket and the sign before it
        {
            if (char.IsNumber(expression[expression.Length - 1]))
            {
                expression += ")";
            }
            else //get rid of the opening bracket and the sign before it 
            {
                expression = expression.Substring(0, expression.Length - 1);
            }

        }
        int j = 0;
        for (j = expression.Length - 1; j >= 0; j--)
        {
            //ok so here make sure that the last thing is either a closing braket or a number
            if (expression[j] == ')' || char.IsNumber(expression[j]))
            {
                break;
            }
        }
        expression = expression.Substring(0, j + 1);
        question.Add(questionBankObj.bedmas(expression));

    }


    public static void primeCompositeLadder()
    {
        bool isPrime = utiltiy.toss();
        int num = utiltiy.getRandom(0, 1000);
        switch (level)
        {
            case 0.1f:
            case 0.2f:
            case 0.3f:
                num = utiltiy.getRandom(0, 50);
                break;
            case 0.4f:
            case 0.5f:
                num = utiltiy.getRandom(0, 100);
                break;
            case 0.6f:
            case 0.7f:
            case 0.8f:
                num = utiltiy.getRandom(10, 120);
                break;

            case 0.9f:
            case 1:
                num = utiltiy.getRandom(50, 200);
                break;

        }
        if (isPrime)
        {
            while (!utiltiy.isPrime(num))
            {
                num = utiltiy.getRandom(0, 1000);
            }

        }
        else
        {
            while (utiltiy.isPrime(num))
            {
                num = utiltiy.getRandom(0, 1000);
            }
        }
        switch (level)
        {
            case 0.5f:

                break;
        }
        question.Add(questionBankObj.primeComposite(num));
    }

    //ok so in this one you need to find all of the prime factors of a number 
    public static void primeFactorizationLadder()
    {
        int n = 0;
        if (level < 0.4)
        {
            n = utiltiy.getRandom(0, 100);
        }
        else
        {
            n = utiltiy.getRandom(50, 200);
        }
        question.Add(questionBankObj.primeFactorization(n));

    }

    public static void shapeQuestion(string topic)
    {
        utilities.Question currQuestion = new utilities.Question();
        //   currQuestion.question = "What shape is shown below?";
        currQuestion.isShape = true;
        currQuestion.multipleChoice = true;
        currQuestion.topicIndex = int.Parse(topic);
        question.Add(currQuestion);


    }

    //ok so here pick a number between 0 and 50, 
    public static void arabicToRomanLadder()
    {
        int num = 0;
        int max = 0, min = 1;
        switch (level)
        {
            case 0.1f:
                max = 10;
                min = 1;
                break;
            case 0.2f:
                max = 20;
                min = 5;
                break;
            case 0.3f:
                max = 40;
                break;
            case 0.4f:
                max = 100;
                break;
            case 0.5f:
                max = 500;
                break;
            case 0.6f:
                max = 1000;
                break;
            case 0.7f:
                max = 5000;
                break;
            case 0.8f:
                max = 10000;
                break;
            case 0.9f:
                max = 100000;
                break;
            case 1:
                break;

        }
        num = utiltiy.getRandom(1, max);
        question.Add(questionBankObj.arabicToRoman(num));

    }

    public static void romanToArabicLadder()
    {
        int num = 0;
        int max = 0, min = 1;
        switch (level)
        {
            case 0.1f:
                max = 10;
                min = 1;
                break;
            case 0.2f:
                max = 20;
                min = 5;
                break;
            case 0.3f:
                max = 40;
                break;
            case 0.4f:
                max = 100;
                break;
            case 0.5f:
                max = 500;
                break;
            case 0.6f:
                max = 1000;
                break;
            case 0.7f:
                max = 5000;
                break;
            case 0.8f:
                max = 10000;
                break;
            case 0.9f:
                max = 100000;
                break;
            case 1:
                break;

        }
        num = utiltiy.getRandom(1, max);
        question.Add(questionBankObj.romanToArabic(utiltiy.ArabicToRoman(num)));
    }

    public static void classifyNumbers()
    {
        int type = 0;
        if (level < 0.4)
        {
            type = 2;
        }
        else if (level < 0.7)
        {
            type = 3;
        }
        else
        {
            type = 4;
        }
        question.Add(questionBankObj.classifyNumbers(type));
    }

    //ok, so here you need to add numbers 
    public static void evenAndOdd()
    {
        if (utiltiy.toss())
        {
            char sign = 'a';
            switch (utiltiy.getRandom(0, 4))
            {
                case 0:
                    sign = '+';
                    break;
                case 1:
                    sign = '-';
                    break;
                case 2:
                    sign = '*';
                    break;
                case 3:
                    sign = '/';
                    break;
            }
            int a = utiltiy.getRandom(10, 40);
            int b = utiltiy.getRandom(10, 40);
            question.Add(questionBankObj.evenAndOdd(sign, a, b));
        }
        else
        {
            question.Add(questionBankObj.currentEvenAndOdd());
        }
    }





    //----------------------------------NEED TO BE TESTED---------------------------------


    public static void halvingLadder()
    {
        float x = 0;
        switch (level)
        {
            case 0:
                x = 2 * utiltiy.getRandom(1, 5);
                break;
            case 0.1f:
                x = 2 * utiltiy.getRandom(6, 10);
                break;
            case 0.2f:
                x = 2 * utiltiy.getRandom(11, 20);
                break;
            case 0.3f:
                x = 2 * utiltiy.getRandom(21, 50) + 1;
                break;
            case 0.4f:
                x = 2 * utiltiy.getRandom(51, 100) + 1;
                break;
            case 0.5f:
                x = utiltiy.getRandom(101, 200);
                break;
            case 0.6f:
                x = 2 * utiltiy.getRandom(11, 49) / Mathf.Pow(10, utiltiy.getRandom(1, 2));
                break;
            case 0.7f:
                x = utiltiy.getRandom(11, 50) / Mathf.Pow(10, utiltiy.getRandom(1, 2));
                break;
            case 0.8f:
                x = utiltiy.getRandom(51, 100) / Mathf.Pow(10, utiltiy.getRandom(1, 2));
                break;
            case 0.9f:
                x = utiltiy.getRandom(1001, 3000) / Mathf.Pow(10, utiltiy.getRandom(1, 3));
                break;
            case 1:
                x = utiltiy.getRandom(3001, 4999) / Mathf.Pow(10, utiltiy.getRandom(1, 4));
                break;
        }
        question.Add(questionBankObj.halving(utiltiy.roundError(x)));
    }
    public static void doublingLadder()
    {
        float x = 0;

        switch (level)
        {
            case 0:
                x = utiltiy.getRandom(1, 9);
                break;
            case 0.1f:
                x = utiltiy.getRandom(10, 20);
                break;
            case 0.2f:
                x = utiltiy.getRandom(21, 40);
                break;
            case 0.3f:
                x = utiltiy.getRandom(41, 60);
                break;
            case 0.4f:
                x = (float)utiltiy.getRandom(1, 99) / 10;
                break;
            case 0.5f:
                x = (float)utiltiy.getRandom(11, 99) / 100;
                break;
            case 0.6f:
                x = utiltiy.getRandom(101, 999);
                break;
            case 0.7f:
                x = (float)utiltiy.getRandom(101, 999) / Mathf.Pow(10, utiltiy.getRandom(1, 3));
                break;
            case 0.8f:
                x = (float)utiltiy.getRandom(1001, 9999) / Mathf.Pow(10, utiltiy.getRandom(1, 3));
                break;
            case 0.9f:
                x = (float)utiltiy.getRandom(1000, 9999) / Mathf.Pow(10, utiltiy.getRandom(2, 4));
                break;
            case 1:
                x = (float)utiltiy.getRandom(10001, 99999) / Mathf.Pow(10, utiltiy.getRandom(3, 5));
                break;
        }
        Debug.LogError(x + " x in doubleing");
        question.Add(questionBankObj.doubling(x));
    }
    public static void fractionOfAmountLadder()
    {
        int x = 0;
        int y = 0;
        float amount = 0;
        switch (level)
        {
            case 0:
                x = 1;
                y = 2;
                break;
            case 0.1f:
                x = 1;
                y = 4;
                break;
            case 0.2f:
                y = 4;
                x = utiltiy.getRandom(2, 3);
                break;
            case 0.3f:
                y = 3;
                x = utiltiy.getRandom(1, 2);
                break;
            case 0.4f:
                y = utiltiy.getRandom(3, 10);
                x = utiltiy.getRandom(1, y);
                break;
            case 0.5f:
                y = utiltiy.getRandom(10, 15);
                x = utiltiy.getRandom(1, y);
                break;
            case 0.6f:
                y = utiltiy.getRandom(15, 20);
                x = utiltiy.getRandom(1, y);
                break;
            case 0.7f:
                x = utiltiy.getRandom(2, 5);
                y = utiltiy.getRandom(2, x);
                break;
            case 0.8f:
                x = utiltiy.getRandom(4, 10);
                y = utiltiy.getRandom(2, x);
                break;
            case 0.9f:
                x = utiltiy.getRandom(10, 15);
                y = utiltiy.getRandom(2, x);
                break;
            case 1:
                x = utiltiy.getRandom(15, 30);
                y = utiltiy.getRandom(2, x);
                break;
        }
        if (x % y == 0)
        {
            x++;
        }
        amount = y * utiltiy.getRandom(2, 9) * utiltiy.getRandom(1, (int)(5 * level));
        question.Add(questionBankObj.fractionOfAmount(x, y, amount));
    }
    public static void fractionalChangeLadder()
    {
        float x = 0;
        float y = 0;
        float amount = 0;
        bool decrease = false;
        if (level > 0.1 && random.NextDouble() < 0.5)
        {
            decrease = true;
        }
        switch (level)
        {
            case 0:
                x = 1;
                y = 2;
                break;
            case 0.1f:
                x = 1;
                y = 4;
                break;
            case 0.2f:
                y = 4;
                x = utiltiy.getRandom(2, 3);
                break;
            case 0.3f:
                y = 3;
                x = utiltiy.getRandom(1, 2);
                break;
            case 0.4f:
                y = utiltiy.getRandom(3, 10);
                x = utiltiy.getRandom(1, y);
                break;
            case 0.5f:
                y = utiltiy.getRandom(10, 15);
                x = utiltiy.getRandom(1, y);
                break;
            case 0.6f:
                y = utiltiy.getRandom(15, 20);
                x = utiltiy.getRandom(1, y);
                break;
            case 0.7f:
                x = utiltiy.getRandom(2, 5);
                y = utiltiy.getRandom(2, x);
                break;
            case 0.8f:
                x = utiltiy.getRandom(4, 10);
                y = utiltiy.getRandom(2, x);
                break;
            case 0.9f:
                x = utiltiy.getRandom(10, 15);
                y = utiltiy.getRandom(2, x);
                break;
            case 1:
                x = utiltiy.getRandom(15, 30);
                y = utiltiy.getRandom(2, x);
                break;
        }
        if (x % y == 0)
        {
            x++;
        }
        amount = y * utiltiy.getRandom(2, 9) * utiltiy.getRandom(1, 5 * level);
        question.Add(questionBankObj.fractionalChange(x, y, amount, decrease));
    }
    public static void percentageOfAmountLadder()
    {
        float percentage = 0;
        float amount = 0;
        float[] percent = new float[] { 50, 25, 75, 10, 5, 20, 1 };
        float MinLevel = level * 10 - 3;
        if (MinLevel < 0)
        {
            MinLevel = 0;
        }
        if (level < 0.7)
        {
            percentage = percent[(int)Mathf.Max(MinLevel, utiltiy.getRandom(0, level * 10))];
        }
        switch (level)
        {
            case 0.7f:
                do
                {
                    percentage = 5 * utiltiy.getRandom(3, 19);
                } while (percentage % 10 == 0 || percentage % 25 == 0);
                break;
            case 0.8f:
                do
                {
                    percentage = utiltiy.getRandom(1, 100);
                } while (percentage % 5 == 0 || percentage % 4 == 0);
                break;
            case 0.9f:
                do
                {
                    percentage = utiltiy.getRandom(101, 199);
                } while (percentage % 5 == 0 || percentage % 4 == 0);
                break;
            case 1:
                do
                {
                    percentage = utiltiy.getRandom(200, 399);
                } while (percentage % 5 == 0 || percentage % 4 == 0);
                break;
        }
        amount = utiltiy.getRandom(2, 20 + 20 * level);
        if (level < 0.3 && amount % 4 != 0)
        {
            amount += amount % 4;
        }
        if (level > 0.2 && level < 0.6 && amount % 5 != 0)
        {
            amount += amount % 5;
        }
        question.Add(questionBankObj.percentageOfAmount(percentage, amount));
    }
    public static void percentageIncreaseDecreaseLadder()
    {
        float percentage = 0;
        float amount = 0;
        bool increase = true;
        if (random.NextDouble() < 0.5)
        {
            increase = false;
        }
        float[] percent = new float[] { 50, 25, 75, 10, 5, 20, 1 };
        float MinLevel = level * 10 - 3;
        if (MinLevel < 0)
        {
            MinLevel = 0;
        }
        if (level < 0.7)
        {
            percentage = percent[(int)Mathf.Max(MinLevel, utiltiy.getRandom(0, level * 10))];
        }
        switch (level)
        {
            case 0.7f:
                do
                {
                    percentage = 5 * utiltiy.getRandom(3, 19);
                } while (percentage % 10 == 0 || percentage % 25 == 0);
                break;
            case 0.8f:
                do
                {
                    percentage = utiltiy.getRandom(1, 100);
                } while (percentage % 5 == 0 || percentage % 4 == 0);
                break;
            case 0.9f:
                do
                {
                    percentage = utiltiy.getRandom(101, 199);
                } while (percentage % 5 == 0 || percentage % 4 == 0);
                break;
            case 1:
                do
                {
                    percentage = utiltiy.getRandom(200, 399);
                } while (percentage % 5 == 0 || percentage % 4 == 0);
                break;
        }
        amount = utiltiy.getRandom(2, 20 + 20 * level);
        if (level < 0.3 && amount % 4 != 0)
        {
            amount += amount % 4;
        }
        if (level > 0.2 && level < 0.6 && amount % 5 != 0)
        {
            amount += amount % 5;
        }
        question.Add(questionBankObj.percentageIncreaseDecrease(percentage, amount, increase, false));
    }
    public static void percentageChangeLadder()
    {
        float oldAmount = 0;
        float newAmount = 0;
        float change = 0;
        do
        {
            oldAmount = utiltiy.getRandom(1 + level * 40, 10 + level * 100);
            newAmount = utiltiy.getRandom(1 + level * 40, 10 + level * 100);
            change = utiltiy.roundError(100 * (oldAmount - newAmount) / oldAmount);
        } while (oldAmount == newAmount || (change != Mathf.Round(change) && level < 0.6) || change != Mathf.Round(10 * change) / 10);
        question.Add(questionBankObj.percentageChange(oldAmount, newAmount));
    }
    public static void percentageMultiplierLadder()
    {
        float percentage, type;
        float[] percent = new float[] { 50, 25, 75, 10, 5, 20, 1, 15, 35, 95, 60, 35, 90, 80 };
        percentage = percent[utiltiy.getRandom(0, percent.Length - 1)];
        type = 0;
        if (level > 0)
        {
            type = 1;
        }
        if (level > 0.1)
        {
            type = 2;
        }
        if (level > 0.2)
        {
            type = utiltiy.getRandom(0, 2);
        }
        if (level > 0.4)
        {
            percentage = utiltiy.getRandom(level * 50, 5 + level * 200);
        }
        question.Add(questionBankObj.percentageMultipliers(percentage, type));
    }
    public static void repeatedPercentageChangeLadder()
    {
        float originalAmount = 20 * utiltiy.getRandom(10, 200);
        float[] percent = new float[] { 10, 50, 20, 5, 25, 75, 1, 15, 35, 95, 60, 35, 90, 80 };
        float percentage = percent[utiltiy.getRandom(0, 2)];
        float iterations = utiltiy.getRandom(2, 3 + level * 15);
        bool increase = true;

        if (level > 0.1)
        {
            percentage = percent[utiltiy.getRandom(0, percent.Length - 1)]; //not sure what this is ??
        }
        if (level > 0.3)
        {
            percentage = utiltiy.getRandom(0, 5 + level * 100);
        }
        if (level > 0.4 && utiltiy.toss())
        {
            increase = false;
        }
        if (level < 0.5)
        {
            originalAmount = 100 * utiltiy.getRandom(1, 20);
        }
        if (level < 0.2)
        {
            originalAmount = 200 * utiltiy.getRandom(1, 5);
        }
        if (level > 0.6 && utiltiy.toss())
        {
            percentage += 0.5f;
        }
        if (level > 0.8 && utiltiy.toss())
        {
            percentage /= 10;
        }
        if (level > 0.9 && utiltiy.toss())
        {
            percentage += 100;
        }
        if (percentage > 100 && !increase)
        {
            increase = true;
        }
        question.Add(questionBankObj.repeatedPercentageChange(originalAmount, percentage, iterations, increase));
    }
    public static void reversePercentageLadder()
    {
        float percentage = 0;
        float amount;
        bool increase = true;
        if (random.NextDouble() < 0.5)
        {
            increase = false;
        }
        float[] percent = new float[] { 50, 25, 75, 10, 5, 20, 1 };
        float MinLevel = level * 10 - 3;
        if (MinLevel < 0)
        {
            MinLevel = 0;
        }
        if (level < 0.7)
        {
            percentage = percent[(int)Mathf.Max(MinLevel, utiltiy.getRandom(0, level * 10))];
        }
        switch (level)
        {
            case 0.7f:
                do
                {
                    percentage = 5 * utiltiy.getRandom(3, 19);
                } while (percentage % 10 == 0 || percentage % 25 == 0);
                break;
            case 0.8f:
                do
                {
                    percentage = utiltiy.getRandom(1, 100);
                } while (percentage % 5 == 0 || percentage % 4 == 0);
                break;
            case 0.9f:
                do
                {
                    percentage = utiltiy.getRandom(101, 199);
                } while (percentage % 5 == 0 || percentage % 4 == 0);
                break;
            case 1:
                do
                {
                    percentage = utiltiy.getRandom(200, 399);
                } while (percentage % 5 == 0 || percentage % 4 == 0);
                break;
        }
        amount = 10 * utiltiy.getRandom(2, 20 + 20 * level);
        if (level < 0.3 && amount % 4 != 0)
        {
            amount += amount % 4;
        }
        if (level > 0.2 && level < 0.6 && amount % 5 != 0)
        {
            amount += amount % 5;
        }
        question.Add(questionBankObj.percentageIncreaseDecrease(percentage, amount, increase, true));
    }
    public static void roundingLadder(bool decimals)
    {
        float x = 0;
        float accuracy = 0;
        if (decimals)
        {

            level = utiltiy.getRandom(4, 10) / 10.0f;
        }
        else
        {
            level = utiltiy.getRandom(0, 3) / 10.0f;
        }

        switch (level)
        {
            case 0:
                accuracy = 10;
                x = utiltiy.getRandom(0, 1000);
                break;
            case 0.1f:
                accuracy = 100;
                x = utiltiy.getRandom(0, 1000);
                break;
            case 0.2f:
                accuracy = 1000;
                x = utiltiy.getRandom(0, 10000);
                break;
            case 0.3f:
                accuracy = 1;
                x = utiltiy.getRandom(1000, 10000) / Mathf.Pow(10, utiltiy.getRandom(2, 3));
                break;
            case 0.4f:
                accuracy = 0.1f;
                x = utiltiy.getRandom(10000, 100000) / Mathf.Pow(10, utiltiy.getRandom(3, 4));
                break;
            case 0.5f:
                accuracy = 0.01f;
                x = utiltiy.getRandom(10000, 100000) / Mathf.Pow(10, utiltiy.getRandom(4, 5));
                break;
            case 0.6f:
                accuracy = 1 / Mathf.Pow(10, utiltiy.getRandom(2, 3));
                x = utiltiy.getRandom(100000, 1000000) / Mathf.Pow(10, utiltiy.getRandom(5, 6));
                break;
            case 0.7f:
                accuracy = 1000 * Mathf.Pow(10, utiltiy.getRandom(1, 3));
                x = utiltiy.getRandom(0, 100000000) / Mathf.Pow(10, utiltiy.getRandom(0, 2));
                break;
            case 0.8f:
                accuracy = 1;
                x = utiltiy.getRandom(0, 1000) / Mathf.Pow(10, utiltiy.getRandom(0, 6));
                break;
            case 0.9f:
                accuracy = utiltiy.getRandom(2, 3);
                x = utiltiy.getRandom(0, 100000) / Mathf.Pow(10, utiltiy.getRandom(0, 5));
                break;
            case 1:
                accuracy = 4;
                x = utiltiy.getRandom(0, 1000000) / Mathf.Pow(10, utiltiy.getRandom(0, 6));
                break;
        }

        question.Add(questionBankObj.rounding(utiltiy.roundError(x), accuracy));

        //   question.Add(questionBankObj.rounding(utiltiy.roundError(x), 1));

        if (level < 0.8)
        {

        }
        else
        {
            question.Add(questionBankObj.sigFigs(utiltiy.roundError(x), accuracy));
        }
    }
    public static void addingNegativesLadder()
    {
        float x = 0; float y = 0;
        x = utiltiy.getRandom(1, 5 + level * 40);
        y = utiltiy.getRandom(1, 5 + level * 40);
        x = -Mathf.Abs(x);
        if (level == 0)
        {
            y += Mathf.Abs(x);
        }
        if (level > 0.1 && utiltiy.toss())
        {
            y = -Mathf.Abs(y);
            x = Mathf.Abs(x);
        }
        if (level > 0.3 && utiltiy.toss())
        {
            y = -Mathf.Abs(y);
            x = -Mathf.Abs(x);
        }
        if (level > 0.5 & utiltiy.toss())
        {
            x *= 3;
        }
        if (level > 0.6 & utiltiy.toss())
        {
            y *= 3;
        }
        if (level > 0.8 & utiltiy.toss())
        {
            x /= 10;
        }
        if (level > 0.9 & utiltiy.toss())
        {
            y /= 10;
        }
        List<float> numbers = new List<float>();
        numbers.Add((int)x);
        numbers.Add((int)y);

        question.Add(questionBankObj.fourOps(numbers, "+")); //? idfk
    }
    public static void subtractingNegativesLadder()
    {
        float x = 0; float y = 0;
        x = utiltiy.getRandom(1, 5 + level * 40);
        y = utiltiy.getRandom(1, 5 + level * 40);
        if (level == 0)
        {
            y += Mathf.Abs(x);
        }
        if (level > 0.1 && utiltiy.toss())
        {
            y = -Mathf.Abs(y);
            x = Mathf.Abs(x);
        }
        if (level > 0.3 && utiltiy.toss())
        {
            y = -Mathf.Abs(y);
            x = -Mathf.Abs(x);
        }
        if (level > 0.5 & utiltiy.toss())
        {
            x *= 3;
        }
        if (level > 0.6 & utiltiy.toss())
        {
            y *= 3;
        }
        if (level > 0.8 & utiltiy.toss())
        {
            x /= 10;
        }
        if (level > 0.9 & utiltiy.toss())
        {
            y /= 10;
        }
        if (x > 0 && y > 0 && (x - y > 0))
        {
            x = -x;
        }
        List<float> numbers = new List<float>();
        numbers.Add((int)x);
        numbers.Add((int)y);
        question.Add(questionBankObj.fourOps(numbers, "-"));
    }
    public static void multiplyingDividingNegativesLadder(string type)
    {
        float x = 0; float y = 0;
        x = utiltiy.getRandom(1, 5 + level * 40);
        y = utiltiy.getRandom(1, 5 + level * 40);
        x = -Mathf.Abs(x);
        if (level == 0)
        {
            y += Mathf.Abs(x);
        }
        if (level > 0.1 && utiltiy.toss())
        {
            y = -Mathf.Abs(y);
            x = Mathf.Abs(x);
        }
        if (level > 0.3 && utiltiy.toss())
        {
            y = -Mathf.Abs(y);
            x = -Mathf.Abs(x);
        }
        if (level > 0.5 & utiltiy.toss())
        {
            x *= 3;
        }
        if (level > 0.6 & utiltiy.toss())
        {
            y *= 3;
        }
        if (level > 0.8 & utiltiy.toss())
        {
            x /= 10;
        }
        if (level > 0.9 & utiltiy.toss())
        {
            y /= 10;
        }
        if (type == "/")
        {
            x *= y;
        }
        List<float> numbers = new List<float>();
        numbers.Add((int)utiltiy.roundError(x));
        numbers.Add((int)utiltiy.roundError(y));
        question.Add(questionBankObj.fourOps(numbers, type));
    }
    public static void negativeLadder()
    {
        float x = 0; float y = 0;
        switch (utiltiy.getRandom(0, 3))
        {
            case 0:
                addingNegativesLadder();
                break;
            case 1:
                subtractingNegativesLadder();
                break;
            case 2:
                multiplyingDividingNegativesLadder("*");
                break;
            case 3:
                multiplyingDividingNegativesLadder("/");
                break;
        }
    }
    public static void powersOfTenLadder()
    {
        float x = 0; float y = 0;
        switch (level)
        {
            case 0:
                x = utiltiy.getRandom(1, 100);
                y = 10;
                //   question.Add(questionBankObj.fourOps((int)x, (int)y, "*"));
                break;
            case 0.1f:
                x = 10 * utiltiy.getRandom(1, 100);
                y = 10;
                //   question.Add(questionBankObj.fourOps((int)x, (int)y, "/"));
                break;
            case 0.2f:
                x = utiltiy.getRandom(1, 100);
                y = 100;
                //  question.Add(questionBankObj.fourOps((int)x, (int)y, "*"));
                break;
            case 0.3f:
                x = 100 * utiltiy.getRandom(1, 100);
                y = 100;
                //    question.Add(questionBankObj.fourOps((int)x, (int)y, "/"));
                break;
            case 0.4f:
                x = utiltiy.getRandom(10, 100) / 10;
                y = Mathf.Pow(10, utiltiy.getRandom(1, 2));
                //   question.Add(questionBankObj.fourOps((int)x, (int)y, "*"));
                break;
            case 0.5f:
                x = utiltiy.getRandom(10, 100) / 10;
                y = Mathf.Pow(10, utiltiy.getRandom(1, 2));
                //   question.Add(questionBankObj.fourOps((int)x, (int)y, "/"));
                break;
            case 0.6f:
                x = utiltiy.getRandom(100, 1000) / Mathf.Pow(10, utiltiy.getRandom(1, 2));
                y = Mathf.Pow(10, utiltiy.getRandom(1, 3));
                //   question.Add(questionBankObj.fourOps((int)utiltiy.roundError(x), (int)utiltiy.roundError(y), "*"));
                break;
            case 0.7f:
                x = utiltiy.getRandom(100, 1000) / Mathf.Pow(10, utiltiy.getRandom(1, 2));
                y = Mathf.Pow(10, utiltiy.getRandom(1, 3));
                //    question.Add(questionBankObj.fourOps((int)utiltiy.roundError(x), (int)utiltiy.roundError(y), "/"));
                break;
            case 0.8f:
                x = utiltiy.getRandom(100, 10000) / Mathf.Pow(10, utiltiy.getRandom(1, 3));
                y = Mathf.Pow(10, utiltiy.getRandom(2, 4));
                //  question.Add(questionBankObj.fourOps((int)utiltiy.roundError(x), (int)utiltiy.roundError(y), "*"));
                break;
            case 0.9f:
                x = utiltiy.getRandom(100, 10000) / Mathf.Pow(10, utiltiy.getRandom(1, 3));
                y = Mathf.Pow(10, utiltiy.getRandom(2, 4));
                //     question.Add(questionBankObj.fourOps((int)utiltiy.roundError(x), (int)utiltiy.roundError(y), "/"));
                break;
            case 1:
                x = utiltiy.getRandom(1000, 100000) / Mathf.Pow(10, utiltiy.getRandom(2, 4));
                y = Mathf.Pow(10, utiltiy.getRandom(3, 5));
                /*    if (random.NextDouble() < 0.5) {
                        question.Add(questionBankObj.fourOps((int)utiltiy.roundError(x), (int)utiltiy.roundError(y), "*"));
                    } else {
                        question.Add(questionBankObj.fourOps((int)utiltiy.roundError(x), (int)utiltiy.roundError(y), "/"));
                    } */
                break;
        }
        List<float> numbers = new List<float>();
        numbers.Add(x);
        numbers.Add(y);
        Debug.Log(x + " " + y);
        if (utiltiy.toss())
        {
            question.Add(questionBankObj.fourOps(numbers, "/"));
        }
        else
        {
            question.Add(questionBankObj.fourOps(numbers, "*"));
        }

    }
    public static void ratioShareLadder()
    {
        float amount = 0;
        float parts = 0;
        List<float> ratio = new List<float>();
        float shares = Mathf.Floor(2 + level * 3);
        if (level < 0.4)
        {
            shares = 2;
        }
        for (int i = 0; i < shares; i++)
        {
            ratio.Add(utiltiy.getRandom(1, 3 + level * 12));
            parts += ratio[i];
        }
        if (shares == 2 && ratio[0] == ratio[1])
        {
            ratio[1]++;
            parts++;
        }
        amount = parts * utiltiy.getRandom(2, 5 + level * 14);
        question.Add(questionBankObj.ratioShare(amount, ratio.ToArray()));
    }
    public static void ratioReverseLadder()
    {
        float amount = 0;
        float parts = 0;
        List<float> ratio = new List<float>();
        float shares = Mathf.Floor(2 + level * 3);
        if (level < 0.4)
        {
            shares = 2;
        }
        for (int i = 0; i < shares; i++)
        {
            ratio.Add(utiltiy.getRandom(1, 3 + level * 12));
            parts += ratio[i];
        }
        if (shares == 2 && ratio[0] == ratio[1])
        {
            ratio[1]++;
            parts++;
        }
        amount = parts * utiltiy.getRandom(2, 5 + level * 14);

        question.Add(questionBankObj.ratioReverse(amount, ratio.ToArray()));
    }
    public static void ratioDifferenceLadder()
    {
        float amount = 0;
        float parts = 0;
        List<float> ratio = new List<float>();
        float shares = 2;
        if (level > 0.5)
        {
            shares++;
        }
        if (level < 0.4)
        {
            shares = 2;
        }
        for (int i = 0; i < shares; i++)
        {
            ratio.Add(utiltiy.getRandom(1, 3 + level * 12));
            parts += ratio[i];
        }
        if (shares == 2 && ratio[0] == ratio[1])
        {
            ratio[1]++;
            parts++;
        }
        if (shares == 3)
        {
            do
            {
                ratio[0] = utiltiy.getRandom(1, 3 + level * 12);
                ratio[1] = utiltiy.getRandom(1, 3 + level * 12);
                ratio[2] = utiltiy.getRandom(1, 3 + level * 12);
                parts = ratio[0] + ratio[1] + ratio[2];
            } while (ratio[0] == ratio[1] || ratio[0] == ratio[2] || ratio[1] == ratio[2]);
        }
        amount = parts * utiltiy.getRandom(2, 5 + level * 14);
        question.Add(questionBankObj.ratioDifference(amount, ratio.ToArray()));
    }
    public static void convertingFDPLadder(string from)
    {
        string[] types = new string[] { "FD", "FP", "PD", "PF", "DF", "DP" };
        float num = 0;
        float den = 0;
        string type = "";
        switch (from)
        {
            case "random":
                type = types[utiltiy.getRandom(0, types.Length - 1)];
                break;
            case "fraction":
                type = types[utiltiy.getRandom(0, 1)];
                break;
            case "percentage":
                type = types[utiltiy.getRandom(2, 3)];
                break;
            case "decimal":
                type = types[utiltiy.getRandom(4, 5)];
                break;
        }

        switch (level)
        {
            case 0:
                num = 1;
                den = 2;
                break;
            case 0.1f:
                num = utiltiy.getRandom(1, 3);
                den = 4;
                break;
            case 0.2f:
                num = utiltiy.getRandom(1, 9);
                den = 10;
                break;
            case 0.3f:
                num = utiltiy.getRandom(1, 4);
                den = 5;
                break;
            case 0.4f:
                num = utiltiy.getRandom(1, 19);
                den = 20;
                break;
            case 0.5f:
                num = utiltiy.getRandom(1, 39);
                den = 40;
                break;
            case 0.6f:
                num = utiltiy.getRandom(1, 8);
                den = 8;
                break;
            case 0.7f:
                num = utiltiy.getRandom(1, 100);
                den = 100;
                break;
            case 0.8f:
                num = utiltiy.getRandom(1, 200);
                den = 100;
                break;
            case 0.9f:
                num = utiltiy.getRandom(200, 600);
                den = 100;
                break;
            case 1:
                num = utiltiy.getRandom(500, 1000);
                den = 100;
                break;
        }
        question.Add(questionBankObj.convertFDP(type, num, den));
    }
    public static void collectingTermsLadder()
    {
        string[] letters = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "m", "n", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        List<string> lets = new List<string>();
        List<string> floats = new List<string>();
        List<float> cos = new List<float>();
        float terms;
        int i = 0;
        float co = 0;
        float lastCo = 0;
        int choice = 0;
        switch (level)
        {
            case 0:
                terms = 2;
                lets.Add(letters[utiltiy.getRandom(0, letters.Length - 1)]);
                for (i = 0; i < terms; i++)
                {
                    floats.Add(lets[0]);
                    cos.Add(utiltiy.getRandom(1, 10));
                }
                break;
            case 0.1f:
                terms = 2;
                lets.Add(letters[utiltiy.getRandom(0, letters.Length - 1)]);
                for (i = 0; i < terms; i++)
                {
                    floats.Add(lets[0]);
                }
                cos.Add(utiltiy.getRandom(5, 10));
                cos.Add(utiltiy.getRandom(-5, -1));
                break;
            case 0.2f:
                terms = 3;
                lets.Add(letters[utiltiy.getRandom(0, letters.Length - 1)]);
                for (i = 0; i < terms; i++)
                {
                    floats.Add(lets[0]);
                }
                lastCo = 1;
                for (i = 0; i < terms; i++)
                {
                    co = utiltiy.getRandom(1, 10);
                    if (random.NextDouble() < 0.5 && i > 0)
                    {
                        co = -utiltiy.getRandom(1, lastCo);
                    }
                    cos.Add(co);
                    lastCo = co;
                }
                break;
            case 0.3f:
                {
                    terms = utiltiy.getRandom(3, 4);
                    choice = utiltiy.getRandom(0, letters.Length - 2);
                    lets.Add(letters[choice]);
                    lets.Add(letters[choice + 1]);
                    floats.Add(lets[0]);
                    for (i = 1; i < terms; i++)
                    {
                        floats.Add(lets[utiltiy.getRandom(0, lets.Count - 1)]);
                    }
                    for (i = 0; i < terms; i++)
                    {
                        co = utiltiy.getRandom(1, 10);
                        cos.Add(co);
                    }
                    break;
                }
            case 0.4f:
                {
                    terms = 3;
                    choice = utiltiy.getRandom(0, letters.Length - 2);
                    lets.Add(letters[choice]);
                    lets.Add(letters[choice + 1]);
                    floats.Add(lets[0]);
                    for (i = 1; i < terms; i++)
                    {
                        floats.Add(lets[utiltiy.getRandom(0, lets.Count - 1)]);
                    }
                    for (i = 0; i < terms; i++)
                    {
                        co = utiltiy.getRandom(1, 10);
                        if (random.NextDouble() < 0.5 && i > 0)
                        {
                            co = -co;
                        }
                        cos.Add(co);
                    }
                    break;
                }
            case 0.5f:
                {
                    terms = 4;
                    choice = utiltiy.getRandom(0, letters.Length - 2);
                    lets.Add(letters[choice]);
                    lets.Add(letters[choice + 1]);
                    floats.Add(lets[0]);
                    for (i = 1; i < terms; i++)
                    {
                        floats.Add(lets[utiltiy.getRandom(0, lets.Count - 1)]);
                    }
                    for (i = 0; i < terms; i++)
                    {
                        co = utiltiy.getRandom(1, 10);
                        if (random.NextDouble() < 0.5)
                        {
                            co = -co;
                        }
                        cos.Add(co);
                    }
                    break;
                }
            case 0.6f:
                {
                    terms = 4;
                    choice = utiltiy.getRandom(0, letters.Length - 3);
                    lets.Add(letters[choice] + letters[choice + 1]);
                    lets.Add(letters[choice + 2]);
                    floats.Add(lets[0]);
                    for (i = 1; i < terms; i++)
                    {
                        floats.Add(lets[utiltiy.getRandom(0, lets.Count - 1)]);
                    }
                    for (i = 0; i < terms; i++)
                    {
                        co = utiltiy.getRandom(1, 10);
                        if (random.NextDouble() < 0.5)
                        {
                            co = -co;
                        }
                        cos.Add(co);
                    }
                    break;
                }
            case 0.7f:
                {
                    terms = 4;
                    choice = utiltiy.getRandom(0, letters.Length - 4);
                    lets.Add(letters[choice] + letters[choice + 1]);
                    lets.Add(letters[choice + 2] + letters[choice + 3]);
                    floats.Add(lets[0]);
                    for (i = 1; i < terms; i++)
                    {
                        floats.Add(lets[utiltiy.getRandom(0, lets.Count - 1)]);
                    }
                    for (i = 0; i < terms; i++)
                    {
                        co = utiltiy.getRandom(1, 15);
                        if (random.NextDouble() < 0.5)
                        {
                            co = -co;
                        }
                        cos.Add(co);
                    }
                    break;
                }
            case 0.8f:
                {
                    terms = 4;
                    choice = utiltiy.getRandom(0, letters.Length - 3);
                    lets.Add(letters[choice]);
                    lets.Add(letters[choice + 1]);
                    lets.Add(letters[choice + 2]);
                    floats.Add(lets[0]);
                    for (i = 1; i < terms; i++)
                    {
                        floats.Add(lets[utiltiy.getRandom(0, lets.Count - 1)]);
                    }
                    for (i = 0; i < terms; i++)
                    {
                        co = utiltiy.getRandom(1, 15);
                        if (random.NextDouble() < 0.5)
                        {
                            co = -co;
                        }
                        cos.Add(co);
                    }
                    break;
                }
            case 0.9f:
                {
                    terms = 5;
                    choice = utiltiy.getRandom(0, letters.Length - 4);
                    lets.Add(letters[choice] + letters[choice + 1]);
                    lets.Add(letters[choice + 2] + letters[choice + 3]);
                    floats.Add(lets[0]);
                    for (i = 1; i < terms; i++)
                    {
                        floats.Add(lets[utiltiy.getRandom(0, lets.Count - 1)]);
                    }
                    for (i = 0; i < terms; i++)
                    {
                        co = utiltiy.getRandom(1, 15);
                        if (random.NextDouble() < 0.5)
                        {
                            co = -co;
                        }
                        cos.Add(co);
                    }
                    break;
                }
            case 1:
                {
                    terms = 5;
                    choice = utiltiy.getRandom(0, letters.Length - 3);
                    lets.Add(letters[choice]);
                    lets.Add(letters[choice + 1]);
                    lets.Add(letters[choice + 2]);
                    floats.Add(lets[0]);
                    for (i = 1; i < terms; i++)
                    {
                        floats.Add(lets[utiltiy.getRandom(0, lets.Count - 1)]);
                    }
                    for (i = 0; i < terms; i++)
                    {
                        co = utiltiy.getRandom(1, 15);
                        if (random.NextDouble() < 0.5)
                        {
                            co = -co;
                        }
                        cos.Add(co);
                    }
                    break;
                }
        }
        question.Add(questionBankObj.collectingTerms(lets.ToArray(), floats.ToArray(), cos.ToArray()));
    }
    public static void multiplyingTermsLadder()
    {
        bool negatives = false;
        if (level > 0.5)
        {
            negatives = true;
        }
        float Min = 0;
        if (level > 0.6)
        {
            Min = 3;
        }
        int type = (int)utiltiy.getRandom(Min, level * 6);
        question.Add(questionBankObj.multiplyingTerms(type, negatives));
    }
    public static void factorLadder()
    {
        int MaxFactors = 0;
        int MinNumber = 0;
        int MaxNumber = 0;
        switch (level)
        {
            case 0:
                MaxFactors = 4;
                MinNumber = 1;
                MaxNumber = 25;
                break;
            case 0.1f:
                MaxFactors = 6;
                MinNumber = 5;
                MaxNumber = 30;
                break;
            case 0.2f:
                MaxFactors = 6;
                MinNumber = 10;
                MaxNumber = 40;
                break;
            case 0.3f:
                MaxFactors = 6;
                MinNumber = 10;
                MaxNumber = 60;
                break;
            case 0.4f:
                MaxFactors = 8;
                MinNumber = 20;
                MaxNumber = 80;
                break;
            case 0.5f:
                MaxFactors = 8;
                MinNumber = 40;
                MaxNumber = 100;
                break;
            case 0.6f:
                MaxFactors = 10;
                MinNumber = 50;
                MaxNumber = 200;
                break;
            case 0.7f:
                MaxFactors = 10;
                MinNumber = 100;
                MaxNumber = 400;
                break;
            case 0.8f:
                MaxFactors = 10;
                MinNumber = 100;
                MaxNumber = 800;
                break;
            case 0.9f:
                MaxFactors = 12;
                MinNumber = 400;
                MaxNumber = 1000;
                break;
            case 1:
                MaxFactors = 15;
                MinNumber = 1000;
                MaxNumber = 4000;
                break;
        }
        question.Add(questionBankObj.factors(MaxFactors, MinNumber, MaxNumber));
    }
    public static void multipleLadder()
    {
        int multiple = 0;
        float x = 0;
        switch (level)
        {
            case 0:
                multiple = utiltiy.getRandom(1, 5);
                x = utiltiy.getRandom(1, 10);
                break;
            case 0.1f:
                multiple = utiltiy.getRandom(1, 10);
                x = utiltiy.getRandom(1, 10);
                break;
            case 0.2f:
                multiple = utiltiy.getRandom(1, 15);
                x = utiltiy.getRandom(5, 12);
                break;
            case 0.3f:
                multiple = utiltiy.getRandom(1, 15);
                x = utiltiy.getRandom(5, 15);
                break;
            case 0.4f:
                multiple = utiltiy.getRandom(1, 20);
                x = utiltiy.getRandom(5, 15);
                break;
            case 0.5f:
                multiple = utiltiy.getRandom(1, 20);
                x = utiltiy.getRandom(1, 20);
                break;
            case 0.6f:
                multiple = utiltiy.getRandom(1, 30);
                x = utiltiy.getRandom(5, 15);
                break;
            case 0.7f:
                multiple = utiltiy.getRandom(5, 30);
                x = utiltiy.getRandom(11, 25);
                break;
            case 0.8f:
                multiple = utiltiy.getRandom(10, 20);
                x = utiltiy.getRandom(15, 30);
                break;
            case 0.9f:
                multiple = utiltiy.getRandom(10, 25);
                x = utiltiy.getRandom(20, 50);
                break;
            case 1:
                multiple = utiltiy.getRandom(15, 30);
                x = utiltiy.getRandom(20, 100);
                break;
        }
        question.Add(questionBankObj.multiples(multiple, x));
    }
    public static void hcfLadder()
    {
        float x = 0; float y = 0;
        float z = 0;
        float multiple = utiltiy.getRandom(1, 2 + level * 20);
        x = multiple * utiltiy.getRandom(1, 2 + level * 20);
        y = multiple * utiltiy.getRandom(1, 2 + level * 20);
        while (y == x)
        {
            y = multiple * utiltiy.getRandom(1, 2 + level * 20);
        }
        if (level > 0.7)
        {
            z = multiple * utiltiy.getRandom(1, 2 + level * 20);
            while (z == x || z == y)
            {
                z = multiple * utiltiy.getRandom(1, 2 + level * 20);
            }
        }
        if (z > 0)
        {
            question.Add(questionBankObj.hcf(x, y, z));
        }
        else
        {
            question.Add(questionBankObj.hcf(x, y, -1)); //not sure about this
        }
    }
    public static void lcmLadder()
    {
        float x = 0; float y = 0;
        float z = 0;
        float multiple = 1 + utiltiy.getRandom(0, level * 10);
        x = multiple * utiltiy.getRandom(2, 6 + level * 20);
        y = multiple * utiltiy.getRandom(2, 6 + level * 20);
        while (y == x)
        {
            y = multiple * utiltiy.getRandom(2, 6 + level * 20);
        }
        if (level > 0.7)
        {
            z = multiple * utiltiy.getRandom(2, 6 + level * 10);
            while (z == x || z == y)
            {
                z = multiple * utiltiy.getRandom(2, 6 + level * 10);
            }
        }
        if (z > 0)
        {
            question.Add(questionBankObj.lcm(x, y, z));
        }
        else
        {
            question.Add(questionBankObj.lcm(x, y, -1)); //not sure about that one
        }
    }
    public static void simplifyingRatiosLadder()
    {
        int terms = (int)Mathf.Max(2, Mathf.Round(level * 5));
        int MaxPrime = (int)Mathf.Max(10, Mathf.Round(level * 25));
        question.Add(questionBankObj.simplifyingRatios(terms, MaxPrime));
    }
    public static void simplifyingFractionsLadder()
    {
        int MaxPrime = (int)Mathf.Max(5, Mathf.Round(level * 30));
        question.Add(questionBankObj.simplifyingFractions(MaxPrime));
    }
    public static void nthTermLinearLadder()
    {
        int a = 0;
        int b = 0, c = 0;

        b = (int)utiltiy.getRandom(1, 4 + level * 20);

        if (level > 0)
        {
            c = (int)utiltiy.getRandom(1, 4 + level * 20);
        }
        if (level > 0.2 && utiltiy.toss())
        {
            c *= -1;
        }
        if (level == 0.4)
        {
            b *= -1;
            c = 0;
        }
        if (level > 0.4)
        {
            b *= -1;
        }
        if (level > 0.5)
        {
            b = Mathf.Abs(b) / 2;
        }
        if (level > 0.7)
        {
            b *= -1;
        }
        if (level > 0.8)
        {
            c /= 2;
        }
        if (level > 0.9)
        {
            b /= 2;
            c /= 2;
        }
        question.Add(questionBankObj.nthTermFinding(a, b, c));
    }
    public static void nthTermQuadraticLadder()
    {
        int a, b, c;
        a = 1;
        b = 0;
        c = 0;
        if (level > 0)
        {
            c = (int)utiltiy.getRandom(1, 4 + level * 20);
        }
        if (level > 0.1)
        {
            a = (int)utiltiy.getRandom(1, 4 + level * 20);
        }
        if (level > 0.2)
        {
            b = (int)utiltiy.getRandom(1, 4 + level * 20);
        }
        if (level > 0.3 && utiltiy.toss())
        {
            c *= -1;
        }
        if (level > 0.4 && utiltiy.toss())
        {
            b *= -1;
        }
        if (level > 0.5 && utiltiy.toss())
        {
            a *= -1;
        }
        if (level > 0.6 && utiltiy.toss())
        {
            c /= 2;
        }
        if (level > 0.7 && utiltiy.toss())
        {
            b /= 2;
        }
        if (level > 0.8 && utiltiy.toss())
        {
            a /= 2;
        }
        if (level > 0.9)
        {
            a = -Mathf.Abs(a);
        }
        question.Add(questionBankObj.nthTermFinding(a, b, c));
    }
    public static void sequencesNextTermLadder(bool fraction)
    { //if its a faction you can divide it by anything 
        float a, b, c;
        a = utiltiy.getRandom(1, 3 + level * 5);
        b = utiltiy.getRandom(1, 3 + level * 15);
        c = utiltiy.getRandom(1, 15 + level * 50);
        if (level < 0.7)
        {
            a = 0;
        }
        if (level < 0.1)
        {
            b = utiltiy.getRandom(1, 5);
            if (random.NextDouble() < 0.2)
            {
                b = 10;
            }
        }
        int[] easy = { 2, 4, 5, 8, 10 };

        if (level > 0.3 && random.NextDouble() < 0.7)
        {
            if (fraction)
            {
                c /= easy[utiltiy.getRandom(0, easy.Length - 1)];
            }
            else
            {
                c /= 2;
            }

        }
        if (level > 0.4 && random.NextDouble() < 0.7)
        {
            c = -c;
        }
        if (level > 0.5 && random.NextDouble() < 0.7)
        {
            if (fraction)
            {
                //       c /= utiltiy.getRandom(1, 10);
            }
            else
            {
                c /= 5;
            }

        }
        if (level > 0.2 && random.NextDouble() < 0.7)
        {
            b = -b;
            if (level < 0.5)
            {
                c = utiltiy.getRandom(4, 10) * Mathf.Abs(b);
            }
        }
        if (level > 0.6 && random.NextDouble() < 0.7)
        {

            if (fraction)
            {
                b /= easy[utiltiy.getRandom(0, easy.Length - 1)];
            }
            else
            {
                b /= 2;
            }

        }
        if (level > 0.7 && random.NextDouble() < 0.7)
        {
            a = -a;
        }
        if (level > 0.8 && random.NextDouble() < 0.7)
        {
            if (fraction)
            {
                a /= easy[utiltiy.getRandom(0, easy.Length - 1)];
            }
            else
            {
                a /= 2;
            }

        }
        question.Add(questionBankObj.sequencesNextTerm(a, b, c, fraction));
    }
    public static void nthTermGeneratingLadder()
    {
        int a, b, c;
        a = b = c = 0;
        switch (level)
        {
            case 0:
                b = utiltiy.getRandom(1, 10);
                break;
            case 0.1f:
                do
                {
                    b = utiltiy.getRandom(-10, 10);
                } while (b == 0);
                break;
            case 0.2f:
                b = utiltiy.getRandom(1, 10);
                c = utiltiy.getRandom(1, 10);
                break;
            case 0.3f:
                b = utiltiy.getRandom(1, 10);
                c = utiltiy.getRandom(-15, 15);
                break;
            case 0.4f:
                do
                {
                    b = utiltiy.getRandom(-10, 10);
                    c = utiltiy.getRandom(-10, 10);
                } while (b == 0 || c == 0);
                break;
            case 0.5f:
                do
                {
                    b = utiltiy.getRandom(-15, 15);
                    c = utiltiy.getRandom(-25, 25);
                } while (b == 0 || c == 0);
                break;
            case 0.6f:
                a = utiltiy.getRandom(1, 10);
                break;
            case 0.7f:
                a = utiltiy.getRandom(1, 5);
                c = utiltiy.getRandom(-10, 10);
                break;
            case 0.8f:
                do
                {
                    a = utiltiy.getRandom(-5, 5);
                    b = utiltiy.getRandom(-10, 10);
                } while (a == 0 || b == 0);
                break;
            case 0.9f:
                a = utiltiy.getRandom(1, 5);
                do
                {
                    b = utiltiy.getRandom(-5, 5);
                    c = utiltiy.getRandom(-25, 25);
                } while (b == 0 || c == 0);
                break;
            case 1:
                do
                {
                    a = utiltiy.getRandom(-5, 5);
                    b = utiltiy.getRandom(-10, 10);
                    c = utiltiy.getRandom(-25, 25);
                } while (a == 0 || b == 0 || c == 0);
                break;
        }
        question.Add(questionBankObj.nthTermGenerating(a, b, c));
    }
    public static void addingCoinsLadder()
    {
        int coins = (int)Mathf.Round((level * 8) + (level * 8) + 2);
        question.Add(questionBankObj.addingCoins(coins));
    }
    public static void countingCoinsLadder()
    {
        int coins = (int)utiltiy.getRandom(2 + Mathf.Pow(level * 10, 2), 5 + Mathf.Pow(level * 10, 3));
        question.Add(questionBankObj.countingCoins(coins));
    }
    public static void speedDistTimeLadder()
    {
        level = utiltiy.roundError(level + 0.1f);
        float divisor = 1;
        if (level > 0.4)
        {
            divisor *= 2;
        }
        if (level > 0.8)
        {
            divisor *= 2;
        }
        float speed = utiltiy.getRandom(1 * (level * 4) + 1, 1 + 5 * level * 4) * 10 / divisor;
        float time = utiltiy.getRandom(1 * (level * 4) + 1, 1 + 5 * level * 4) * 10 / divisor;
        question.Add(questionBankObj.speedDistTime(speed, time, utiltiy.getRandom(0, 2)));
    }
    public static void powersAndRootsLadder()
    {
        float x, a, b;
        a = utiltiy.getRandom(1, 5);
        b = utiltiy.getRandom(1, 5);
        x = Mathf.Pow(utiltiy.getRandom(0, 10), b);
        switch (level)
        {
            case 0:
                a = 2;
                b = 1;
                break;
            case 0.1f:
                a = 1;
                b = 2;
                break;
            case 0.2f:
                a = 3;
                b = 1;
                break;
            case 0.3f:
                a = 1;
                b = 3;
                break;
            case 0.4f:
                a = utiltiy.getRandom(0, 3);
                b = 1;
                break;
            case 0.5f:
                a = 1;
                b = utiltiy.getRandom(1, 3);
                break;
            case 0.6f:
                a = utiltiy.getRandom(0, 3);
                b = utiltiy.getRandom(1, 3);
                break;
            case 0.7f:
                a = utiltiy.getRandom(-2, 2);
                b = utiltiy.getRandom(1, 3);
                break;
            case 0.8f:
                a = utiltiy.getRandom(-3, 3);
                b = utiltiy.getRandom(1, 4);
                break;
            case 0.9f:
                a = utiltiy.getRandom(-4, 4);
                b = utiltiy.getRandom(2, 4);
                break;
            case 1:
                a = utiltiy.getRandom(-6, 6);
                b = utiltiy.getRandom(3, 5);
                break;
        }
        if (level > 0.6 && a == 0)
        {
            a--;
        }
        x = Mathf.Pow(utiltiy.getRandom(1, 10), b);
        if (level > 0.6 && a > 0)
        {
            x = Mathf.Pow(utiltiy.getRandom(1, 10) / Mathf.Pow(10, utiltiy.getRandom(0, 1)), b);
        }
        question.Add(questionBankObj.powersAndRoots(utiltiy.roundError(x), a, b));
    }
    public static void orderingLadder()
    {
        int Length = 0;
        bool currDecimal = false;
        bool negative = false;
        bool descending = false;
        float range;
        Length = 3 + (int)Mathf.Round(level * 4);
        range = 10 + (level * 20);
        currDecimal = false;
        negative = false;
        descending = false;
        if (level > 0.1)
        {
            if (random.NextDouble() < 0.5)
            {
                descending = true;
            }
        }
        if (level > 0.2)
        {
            negative = true;
        }
        if (level > 0.4 && level < 0.8)
        {
            currDecimal = true;
            negative = false;
        }
        if (level > 0.7)
        {
            negative = true;
            currDecimal = true;
        }
        question.Add(questionBankObj.ordering(Length, currDecimal, negative, descending, range));
    }


    public static void oneStepEquationLadder(bool inequality, bool isDecimal)
    {
        float x, answer;
        int type = 0;
        x = utiltiy.getRandom(2, 5 + level * 15);
        answer = utiltiy.getRandom(2, 5 + level * 15);
        type = (int)utiltiy.getRandom(0, Mathf.Min(level * 15, 7));
        if (isDecimal)
        {
            x += utiltiy.getRandom(0, 100) / 100;
        }
        if (type == 3 && level < 0.5)
        {
            answer += x;
        }
        if (type == 4 && level < 0.5)
        {
            x += answer;
        }
        if (type == 5)
        {
            answer *= x;
        }
        if (type == 6)
        {
            x *= answer;
        }
        if (type == 7)
        {
            x = 2;
            if (level > 0.8)
            {
                x = utiltiy.getRandom(2, 4);
            }
            if (random.NextDouble() < 0.1)
            {
                x = 1;
            }
        }
        if (level > 0.5 && type != 7)
        {
            if (random.NextDouble() < 0.4)
            {
                x = -x;
            }
            if (random.NextDouble() < 0.4)
            {
                answer = -answer;
            }
        }
        if (level > 0.7 && type != 7)
        {
            if (random.NextDouble() < 0.4)
            {
                x /= 10;
            }
            if (random.NextDouble() < 0.4)
            {
                answer /= 10;
            }
        }
        question.Add(questionBankObj.oneStepEquations(type, x, answer, inequality));
    }
    public static void twoStepEquationLadder(bool inequality, bool isDecimal)
    {
        float x, y, answer;
        int type = 0;
        x = (int)utiltiy.getRandom(2, 4 + level * 12);
        y = (int)utiltiy.getRandom(1, 4 + level * 12);
        answer = (int)utiltiy.getRandom(2, 4 + level * 12);
        type = (int)utiltiy.getRandom(0, Mathf.Min(level * 12, 8));
        if (isDecimal)
        {
            x += utiltiy.getRandom(0, 100) / 100;
            y += utiltiy.getRandom(0, 1000) / 100;

        }
        if (type == 4 && level < 0.5)
        {
            answer += y;
        }
        if (type == 3 || type == 4 || type == 5)
        {
            answer *= x;
        }
        if (type == 7 || type == 8)
        {
            x = 2;
            if (level > 0.8)
            {
                x = utiltiy.getRandom(2, 4);
            }
            if (random.NextDouble() < 0.1)
            {
                x = 1;
            }
        }
        if (level > 0.6 && type != 7 && type != 8)
        {
            if (random.NextDouble() < 0.4)
            {
                x = -x;
            }
            if (random.NextDouble() < 0.4)
            {
                answer = -answer;
            }
        }
        if (level > 0.8 && type != 7 && type != 8)
        {
            if (random.NextDouble() < 0.4)
            {
                x /= 5;
            }
            if (random.NextDouble() < 0.4)
            {
                answer /= 5;
            }
        }
        question.Add(questionBankObj.twoStepEquations(type, x, y, answer, inequality));
    }

    public static void threeStepEquationLadder(bool inequality, bool isDecimal)
    {
        float x, y, z, answer;
        bool reversable = false;
        int count = 0;

        do
        {
            count++;
            if (count > 100)
            {
                Debug.Log("nvm its here");
                return;
            }
            x = (int)utiltiy.getRandom(1, 3 + level * 12);
            y = (int)utiltiy.getRandom(1, 3 + level * 12);
            z = (int)utiltiy.getRandom(1, 3 + level * 12);
            answer = (int)utiltiy.getRandom(2, 4 + level * 12);

            if (isDecimal)
            {
                x += utiltiy.getRandom(0, 100) / 100;
                y += utiltiy.getRandom(0, 1000) / 100;
                z += utiltiy.getRandom(0, 100) / 10;
            }
            if (level > 0.1 && random.NextDouble() < 0.7)
            {
                z = -z;
            }
            if (level > 0.3 && random.NextDouble() < 0.7)
            {
                y = -y;
            }
            if (level > 0.5 && random.NextDouble() < 0.7)
            {
                x = -x;
            }
        } while (x + y == 0 || utiltiy.roundError(x * answer + z) == 0);
        if (level > 0.4 && random.NextDouble() < 0.5)
        {
            reversable = true;
        }
        if (level > 0.6 && random.NextDouble() < 0.7)
        {
            answer /= 2;
        }
        if (level > 0.7 && random.NextDouble() < 0.7)
        {
            answer = -answer;
        }
        if (level > 0.8 && random.NextDouble() < 0.7)
        {
            answer /= 5;
        }
        question.Add(questionBankObj.threeStepEquations(x, y, z, answer, reversable, inequality));
    }
    public static void equationsWithBracketsLadder()
    {
        float x, y, z, answer;
        bool reversable = false;
        x = utiltiy.getRandom(2, 3 + level * 10);
        y = utiltiy.getRandom(1, 4 + level * 10);
        z = utiltiy.getRandom(1, 4 + level * 10);
        answer = utiltiy.getRandom(0, 10 + level * 14);
        if (level > 0.1 && random.NextDouble() < 0.7)
        {
            z = -z;
        }
        if (level < 0.2)
        {
            y = 1;
        }
        if (level > 0.5 && random.NextDouble() < 0.7)
        {
            x = -x;
        }
        if (level > 0.6 && random.NextDouble() < 0.7)
        {
            y = -y;
        }
        if (level > 0.7 && random.NextDouble() < 0.7)
        {
            answer = -answer;
        }
        if (level > 0.8 && random.NextDouble() < 0.7)
        {
            answer /= 2;
        }
        if (level > 0.9 && random.NextDouble() < 0.7)
        {
            answer /= 5;
        }
        if (level > 0.4 && random.NextDouble() < 0.5)
        {
            reversable = true;
        }
        question.Add(questionBankObj.equationsWithBrackets(x, y, z, answer, reversable));
    }
    public static void equationsWithBracketsBothLadder()
    {
        bool reversable = false;
        float a, b, c, d, e, f;
        bool good = true;
        float answer = 0;
        do
        {
            good = true;
            a = utiltiy.getRandom(2, 5 + level * 8);
            b = utiltiy.getRandom(1, 5 + level * 8);
            c = utiltiy.getRandom(1, 8 + level * 25);
            d = utiltiy.getRandom(2, 5 + level * 8);
            e = utiltiy.getRandom(1, 5 + level * 8);
            f = utiltiy.getRandom(1, 8 + level * 25);
            if (level > 0.1 && random.NextDouble() < 0.7)
            {
                c = -c;
            }
            if (level > 0.3 && random.NextDouble() < 0.7)
            {
                f = -f;
            }
            if (level > 0.5 && random.NextDouble() < 0.7)
            {
                b = -b;
            }
            if (level > 0.7 && random.NextDouble() < 0.7)
            {
                e = -e;
            }
            if (level > 0.8 && random.NextDouble() < 0.7)
            {
                d = -d;
            }
            if (level > 0.9 && random.NextDouble() < 0.7)
            {
                a = -a;
            }
            answer = (d * f - a * c) / (a * b - d * e);
            if (level < 0.3 && answer < 0)
            {
                good = false;
            }
            if (level < 0.6 && answer != Mathf.Round(answer))
            {
                good = false;
            }
            if (answer != Mathf.Round(10 * answer) / 10)
            {
                good = false;
            }
        } while (!good || (a * b - d * e) == 0);
        if (level > 0.4 && random.NextDouble() < 0.5)
        {
            reversable = true;
        }
        question.Add(questionBankObj.equationsWithBracketsBoth(a, b, c, d, e, f, answer, reversable));
    }
    public static void equationsMixedLadder(bool isInequality, bool isDecimal)
    {
        switch (utiltiy.getRandom(0, 4))
        {
            case 0:
                oneStepEquationLadder(isInequality, isDecimal); //not sure, but ill just make all of these false ?
                break;
            case 1:
                twoStepEquationLadder(isInequality, isDecimal);
                break;
            case 2:
                threeStepEquationLadder(isInequality, isDecimal);
                break;
            case 3:
                equationsWithBracketsLadder();
                break;
            case 4:
                equationsWithBracketsBothLadder();
                break;
        }
    }
    public static void substitutionLadder()
    {
        bool negatives = false;
        if (level > 0.4f)
        {
            negatives = true;
        }
        float Max = Mathf.Min(level * 15, 8);
        float Min = level * 15 - 3;
        if (Min < 0)
        {
            Min = 0;
        }
        if (Min > Max)
        {
            Min = Max - 1;
        }
        int type = (int)utiltiy.getRandom(Min, Max);
        int v1 = (int)utiltiy.getRandom(1, 6 + 10 * level);
        int v2 = (int)utiltiy.getRandom(1, 6 + 10 * level);
        if (level > 0.5 && random.NextDouble() < 0.7)
        {
            v1 = -v1;
        }
        if (level > 0.8 && random.NextDouble() < 0.7)
        {
            v2 = -v2;
        }
        if (level > 0.6 && random.NextDouble() < 0.7)
        {
            v1 /= 10;
        }
        if (level > 0.9 && random.NextDouble() < 0.7)
        {
            v2 /= 10;
        }
        question.Add(questionBankObj.substitution(type, negatives, v1, v2));
    }
    public static void numberBondsLadder()
    {
        int[] bond = new int[] { 10, 20, 40, 50, 100, 200, 500, 1000, 1, 1, 1 };
        int currentBond = bond[(int)utiltiy.getRandom(Mathf.Max(0, level * 10 - 3), level * 10)];
        int type = (int)utiltiy.getRandom(0, 7);
        if (level < 0.3)
        {
            type = utiltiy.getRandom(0, 3);
        }
        if (level >= 0.3)
        {
            type = utiltiy.getRandom(4, 7);
        }
        float x = utiltiy.getRandom(0, currentBond);
        if (currentBond == 1)
        {
            switch (level)
            {
                case 0.7f:
                    x = utiltiy.getRandom(1, 9) / 10;
                    break;
                case 0.8f:
                    x = utiltiy.getRandom(11, 99) / 100;
                    break;
                case 0.9f:
                    x = utiltiy.getRandom(101, 999) / 1000;
                    break;
                case 1:
                    x = utiltiy.getRandom(1001, 9999) / 10000;
                    break;
            }
        }
        question.Add(questionBankObj.numberBonds(type, currentBond, utiltiy.roundError(x)));
    }
    public static void addSubtractFractionsLadder()
    {
        string w1, w2, w3;
        float n1, n2, n3, d1, d2, d3;
        string o1, o2;
        o1 = "+";
        o2 = "+";
        w1 = w2 = w3 = "0";

        do
        {
            n1 = 1 + utiltiy.getRandom(0, 2 + level * 10);
            n2 = 1 + utiltiy.getRandom(0, 2 + level * 10);
            n3 = 1 + utiltiy.getRandom(0, 2 + level * 10);
            d1 = n1 + 1 + utiltiy.getRandom(0, 1 + level * 10);
            d2 = n2 + 1 + utiltiy.getRandom(0, 1 + level * 10);
            d3 = n3 + 1 + utiltiy.getRandom(0, 1 + level * 10);
            if (level < 0.2f)
            {
                d1 = utiltiy.getRandom(2, 12);
                n1 = utiltiy.getRandom(1, d1);
                n2 = utiltiy.getRandom(1, n1);
            }
        } while (n1 / d1 < n2 / d2);
        if (random.NextDouble() < 0.5 && level > 0)
        {
            o1 = "-";
        }
        if (random.NextDouble() < 0.5)
        {
            o2 = "-";
        }
        if (level > 0.4)
        {
            w1 = utiltiy.getRandom(1, 5 * level).ToString();
        }
        if (level > 0.5)
        {
            w2 = utiltiy.getRandom(1, 5 * level).ToString();
        }
        if (level > 0.6)
        {
            w3 = utiltiy.getRandom(1, 5 * level).ToString();      //this was 5...
        }
        if (level < 0.2)
        {
            d2 = d1;
        }
        if (level < 0.7)
        {
            o2 = "";
        }

        question.Add(questionBankObj.fourOpsFractions(w1, n1, d1, w2, n2, d2, w3, n3, d3, o1, o2));
    }
    public static void multiplyDivideFractionsLadder()
    {
        string w1, w2, w3;
        float n1, n2, n3, d1, d2, d3;
        string o1, o2;
        o1 = "*";
        o2 = "*";
        w1 = w2 = w3 = "0";
        do
        {
            n1 = 1 + utiltiy.getRandom(0, 2 + level * 7);
            n2 = 1 + utiltiy.getRandom(0, 2 + level * 7);
            n3 = 1 + utiltiy.getRandom(0, 2 + level * 7);
            d1 = n1 + 1 + utiltiy.getRandom(0, 2 + level * 5);
            d2 = n2 + 1 + utiltiy.getRandom(0, 2 + level * 5);
            d3 = n3 + 1 + utiltiy.getRandom(0, 2 + level * 5);
        } while (n1 / d1 < n2 / d2);
        if (random.NextDouble() < 0.5 && level > 0.1)
        {
            o1 = "/";
        }
        if (random.NextDouble() < 0.5)
        {
            o2 = "/";
        }
        if (level > 0.4)
        {
            w1 = utiltiy.getRandom(1, 3 * level).ToString();
        }
        if (level > 0.5)
        {
            w2 = utiltiy.getRandom(1, 3 * level).ToString();
        }
        if (level > 0.6)
        {
            w3 = utiltiy.getRandom(1, 3 * level).ToString();
        }
        if (level < 0.2)
        {
            d1 = d2;
        }
        if (level < 0.7)
        {
            o2 = "";
        }
        question.Add(questionBankObj.fourOpsFractions(w1, n1, d1, w2, n2, d2, w3, n3, d3, o1, o2));
    }
    public static void fractionsFourOpsLadder()
    {
        switch (utiltiy.getRandom(0, 1))
        {
            case 0:
                addSubtractFractionsLadder();
                break;
            case 1:
                multiplyDivideFractionsLadder();
                break;
        }
    }


    public static void meanLadder()
    {

        float average = 0;
        List<float> data;
        int count = 0;
        do
        {
            count++;
            if (count > 100)
                return;
            average = 0;
            data = new List<float>();
            float total = 0;
            for (int i = 0; i < 3 + level * 10; i++)
            {
                data.Add(utiltiy.getRandom(1, 10 + level * 50));
                if (level > 0.5)
                {
                    data[i] /= 10;
                }
                if (level > 0.6)
                {
                    data[i] *= -1;
                }
                if (level > 0.8 && random.NextDouble() < 0.5)
                {
                    data[i] *= -1;
                }
                if (level > 0.9 && random.NextDouble() < 0.5)
                {
                    data[i] *= 10;
                }
                total += data[i];
            }
            average = total / data.Count;
        } while (Mathf.Round(10 * average) / 10 != average);
        question.Add(questionBankObj.mean(data.ToArray()));
    }
    public static void medianLadder()
    {
        List<float> data = new List<float>();
        float terms = 3 + level * 10 + utiltiy.getRandom(0, 1);
        if (level < 0.3 && terms % 2 == 0)
        {
            terms--;
        }
        if (level > 0.7 && terms % 2 == 1)
        {
            terms--;
        }
        for (int i = 0; i < terms; i++)
        {
            data.Add(utiltiy.getRandom(1, 10 + level * 50));
            if (level > 0.5)
            {
                data[i] /= 10;
            }
            if (level > 0.6)
            {
                data[i] *= -1;
            }
            if (level > 0.8 && random.NextDouble() < 0.5)
            {
                data[i] *= -1;
            }
            if (level > 0.9 && random.NextDouble() < 0.5)
            {
                data[i] *= 10;
            }
        }
        question.Add(questionBankObj.median(data.ToArray()));
    }
    public static void rangeLadder()
    {
        List<float> data = new List<float>();
        for (int i = 0; i < 3 + level * 10; i++)
        {
            data.Add(utiltiy.getRandom(1, 10 + level * 100));
            if (level > 0.5)
            {
                data[i] /= 10;
            }
            if (level > 0.6)
            {
                data[i] *= -1;
            }
            if (level > 0.8 && random.NextDouble() < 0.5)
            {
                data[i] *= -1;
            }
            if (level > 0.9 && random.NextDouble() < 0.5)
            {
                data[i] *= 20;
            }
        }
        question.Add(questionBankObj.range(data.ToArray()));
    }
    public static void modeLadder()
    { //no clue what to do here
        List<float> data = new List<float>();
        /*  if (level < 0.4 && random.NextDouble() < 0.8) {
              switch (utiltiy.getRandom(0, 4)) {
                 case 0:
                      for (float i = 0; i < 3 + level * 10; i++) {
                          data.Add(utiltiy.colourPicker(-1)); //pretty sure
                      }
                      break;
                  case 1:
                      for (float i = 0; i < 3 + level * 10; i++) {
                          data.Add(utiltiy.utiltiy.fruitPicker(-1));
                      }
                      break;
                  case 2:
                      for (float i = 0; i < 3 + level * 10; i++) {
                          data.Add(utiltiy.itemPicker("small",-1));
                      }
                      break;
                  case 3:
                      for (float i = 0; i < 3 + level * 10; i++) {
                          data.Add(utiltiy.itemPicker("large",-1));
                      }
                      break;
                  case 4:
                      for (float i = 0; i < 3 + level * 10; i++) {
                          data.Add(utiltiy.letterPicker(-1,false)); //pretty sure
                      }
                      break;
              }
          } else {*/ //not sure about this part
        for (int i = 0; i < 3 + level * 10; i++)
        {
            data.Add(utiltiy.getRandom(1, 10 + level * 100));
            if (level > 0.5)
            {
                data[i] /= 10;
            }
            if (level > 0.6)
            {
                data[i] *= -1;
            }
            if (level > 0.8 && random.NextDouble() < 0.5)
            {
                data[i] *= -1;
            }
            if (level > 0.9 && random.NextDouble() < 0.5)
            {
                data[i] *= 20;
            }
            // }
        }
        if (level < 0.8)
        {
            data.Add(data[utiltiy.getRandom(0, data.Count - 1)]);
        }
        if (level >= 0.8 && random.NextDouble() > 0.2)
        {
            data.Add(data[utiltiy.getRandom(0, data.Count - 1)]);
        }
        for (int i = 0; i < data.Count; i++)
        {
            float temp = data[i];
            int choice = (int)utiltiy.getRandom(0, data.Count - 1);
            data[i] = data[choice];
            data[choice] = temp;
        }
        question.Add(questionBankObj.mode(data.ToArray()));
    }
    public static void averagesLadder()
    {
        switch (utiltiy.getRandom(0, 3))
        {
            case 0:
                meanLadder();
                break;
            case 1:
                medianLadder();
                break;
            case 2:
                modeLadder();
                break;
            case 3:
                rangeLadder();
                break;
        }
    }
    public static void multiplyDivideStandardFormLadder(string op)
    {
        utilities.Term x = new utilities.Term();
        utilities.Term y = new utilities.Term();
        x.co = utiltiy.getRandom(1, 3 + level * 20);
        y.co = utiltiy.getRandom(1, 3 + level * 20);
        y.Pow = utiltiy.getRandom(1, 3 + level * 20);
        x.Pow = y.Pow + utiltiy.getRandom(1, 3 + level * 20);
        if (level > 0.7 && random.NextDouble() > 0.5)
        {
            y.Pow = -y.Pow;
        }
        if (level > 0.5 && random.NextDouble() > 0.5)
        {
            x.Pow = -x.Pow;
        }
        if (level > 0.8)
        {
            y.Pow = -Mathf.Abs(y.Pow);
            x.Pow = -Mathf.Abs(x.Pow);
        }
        if (op == "/")
        {
            x.co = x.co * y.co;
        }
        checkCo(x);
        checkCo(y);

        x.co = utiltiy.roundError(x.co);
        y.co = utiltiy.roundError(y.co);
        question.Add(questionBankObj.standardForm(x, y, op));
    }

    public static utilities.Term checkCo(utilities.Term term)
    {
        while (term.co >= 10)
        {
            term.co /= 10;
            term.Pow++;
        }
        while (term.co < 1)
        {
            term.co *= 10;
            term.Pow--;
        }
        return term;
    }

    public static void addSubtractStandardFormLadder(string op)
    {
        utilities.Term x = new utilities.Term();
        utilities.Term y = new utilities.Term();
        y.co = utiltiy.getRandom(1, 9 + level * 10);
        y.Pow = utiltiy.getRandom(1, 2 + level * 10);
        x.co = utiltiy.getRandom(1, 9 + level * 10);
        x.Pow = y.Pow + utiltiy.getRandom(1, 1 + level * 5);
        if (level > 0.7 && random.NextDouble() > 0.5)
        {
            y.Pow = -y.Pow;
        }
        if (level > 0.5 && random.NextDouble() > 0.5)
        {
            x.Pow = -x.Pow;
        }
        if (level > 0.8)
        {
            y.Pow = -Mathf.Abs(y.Pow);
            x.Pow = -Mathf.Abs(x.Pow);
        }
        if (op == "-")
        {
            x.Pow = y.Pow + utiltiy.getRandom(1, 3 + level * 5);
        }
        checkCo(x);
        checkCo(y);

        x.co = utiltiy.roundError(x.co);
        y.co = utiltiy.roundError(y.co);
        question.Add(questionBankObj.standardForm(x, y, op));
    }


    public static void convertingToStandardFormLadder()
    {
        utilities.Term x = new utilities.Term();
        int type = 0;
        x.co = utiltiy.getRandom(1, 9 + level * 50);
        x.Pow = utiltiy.getRandom(0, 3 + level * 3);
        if (level > 0.3 && random.NextDouble() > 0.5)
        {
            x.Pow = -x.Pow;
        }
        if (level > 0.5 && random.NextDouble() > 0.4)
        {
            x.co *= Mathf.Pow(10, utiltiy.getRandom(1, 3));
        }
        if (level > 0.7 && random.NextDouble() > 0.4)
        {
            x.co *= Mathf.Pow(10, -utiltiy.getRandom(1, 3));
        }
        if (level < 0.8)
        {
            checkCo(x);
        }
        if (level >= 0.8)
        {
            type = 2;
            do
            {
                x.co *= Mathf.Pow(10, utiltiy.getRandom(-2, 3));
            } while (x.co >= 1 && x.co < 10);
        }

        x.co = utiltiy.roundError(x.co);
        question.Add(questionBankObj.convertingStandardForm(x, type));
    }


    public static void convertingFromStandardFormLadder()
    {
        utilities.Term x = new utilities.Term();
        int type = 1;
        x.co = utiltiy.getRandom(1, 9 + level * 50);
        x.Pow = utiltiy.getRandom(0, 3 + level * 3);
        if (level > 0.3 && random.NextDouble() > 0.5)
        {
            x.Pow = -x.Pow;
        }
        if (level > 0.5 && random.NextDouble() > 0.4)
        {
            x.co *= Mathf.Pow(10, utiltiy.getRandom(1, 3));
        }
        if (level > 0.7 && random.NextDouble() > 0.4)
        {
            x.co *= Mathf.Pow(10, -utiltiy.getRandom(1, 3));
        }
        checkCo(x);

        x.co = utiltiy.roundError(x.co);
        question.Add(questionBankObj.convertingStandardForm(x, type));
    }

    public static void convertingFractionsLadder()
    {
        bool toMixed = utiltiy.toss();
        float num = 0;
        if (level == 0)
        {
            toMixed = false;
        }
        if (level == 0.1)
        {
            toMixed = true;
        }
        float den = utiltiy.getRandom(2, 4 + level * 40);
        do
        {
            num = den + utiltiy.getRandom(1, 4 + level * 20);
            num *= utiltiy.getRandom(1, level * 20);
        } while (num % den == 0);
        question.Add(questionBankObj.convertingFractions(num, den, toMixed));
    }


    public static void convertingMeticLengthLadder(bool power)
    { //this method is hella fucked up
        float metres;
        int from, to, gap;
        gap = 1;
        if (level > 0.5)
        {
            gap++;
        }
        if (level > 0.8)
        {
            gap++;
        }
        do
        {
            from = utiltiy.getRandom(0, 3);
            if (level < 0.2)
            {
                from = utiltiy.getRandom(1, 2);
            }
            if (random.NextDouble() < 0.5)
            {
                to = from + gap;
            }
            else
            {
                to = from - gap;
            }
            to %= 4;
        } while (Mathf.Abs(to - from) != gap || to < 0);

        if (power)
        {
            metres = (float)utiltiy.getRandom(1, 10) / 10;

        }
        else
        {
            metres = (float)utiltiy.getRandom(10, 100) / 10;
        }

        int currPower = 1;
        if (power)
        {
            currPower = utiltiy.getRandom(2, 3);
        }


        if (from == 0)
        {
            metres /= Mathf.Pow(100, currPower); //100
        }
        if (from == 1)
        {
            metres /= Mathf.Pow(10, currPower);
        }
        if (from == 3)
        {

            metres *= Mathf.Pow(1000, currPower);
        }
        if (to == 3)
        {

            metres *= Mathf.Pow(10, currPower);

        }

        Debug.Log(metres + " meters");
        question.Add(questionBankObj.convertingMetricLength(utiltiy.roundError(metres), from, to, currPower));
    }
    public static void convertingMeticWeightLadder()
    {
        int from, to, gap;
        gap = 1;
        if (level > 0.5)
        {
            gap++;
        }
        if (level > 0.8)
        {
            gap++;
        }
        do
        {
            from = utiltiy.getRandom(0, 3);
            if (level < 0.2)
            {
                from = utiltiy.getRandom(1, 2);
            }
            if (random.NextDouble() < 0.5)
            {
                to = from + gap;
            }
            else
            {
                to = from - gap;
            }
            to %= 4;
        } while (Mathf.Abs(to - from) != gap || to < 0);
        float kg = utiltiy.getRandom(10, 100) / 10;
        if (level < 0.5)
        {
            kg = utiltiy.getRandom(1, 20);
        }
        if (level == 0)
        {
            to = 1;
            from = 2;
        }
        if (level == 0.1)
        {
            to = 2;
            from = 1;
        }
        if (from == 0)
        {
            kg /= 100;
        }
        if (from == 1)
        {
            kg /= 10;
        }
        if (from == 3)
        {
            kg *= 1000;
        }
        if (to == 3)
        {
            kg *= 10;
        }
        question.Add(questionBankObj.convertingMetricWeight(utiltiy.roundError(kg), from, to));
    }
    public static void convertingMeticVolumeLadder()
    {
        int from, to, gap;
        gap = 1;
        if (level > 0.5)
        {
            gap++;
        }
        do
        {
            from = utiltiy.getRandom(0, 2);
            if (level < 0.2)
            {
                from = utiltiy.getRandom(1, 2);
            }
            if (random.NextDouble() < 0.5)
            {
                to = from + gap;
            }
            else
            {
                to = from - gap;
            }
            to %= 3;
        } while (Mathf.Abs(to - from) != gap || to < 0);
        float litres = utiltiy.getRandom(10, 100) / 10;
        if (level < 0.5)
        {
            litres = utiltiy.getRandom(1, 20);
        }
        if (from == 0)
        {
            litres /= 100;
        }
        if (from == 1)
        {
            litres /= 10;
        }
        question.Add(questionBankObj.convertingMetricVolume(utiltiy.roundError(litres), from, to));
    }
    public static void convertingMeticMixedLadder()
    {
        switch (utiltiy.getRandom(0, 2))
        {
            case 0:
                convertingMeticLengthLadder(false);
                break;
            case 1:
                convertingMeticWeightLadder();
                break;
            case 2:
                convertingMeticVolumeLadder();
                break;
        }
    }
    public static void unitaryMethodLadder()
    {
        float cost = 2 * utiltiy.getRandom(3, 10 + level * 50) + 1;
        float newQuantity = 0;
        if (level < 0.4)
        {
            cost = 5 * Mathf.Round(cost / 5);
        }
        if (level < 0.2)
        {
            cost = 5 * Mathf.Round(cost / 5);
        }
        float quantity = utiltiy.getRandom(2 + level * 10, 3 + level * 25);
        if (level == 0)
        {
            quantity = 2;
        }
        do
        {
            newQuantity = quantity + utiltiy.getRandom(1 + level * 10, 2 + level * 25);
        } while (level > 0.3 && newQuantity % quantity == 0);
        question.Add(questionBankObj.unitaryMethod(cost, quantity, newQuantity));
    }
    public static void probabilityBasicLadder()
    {
        int type = 0;
        if (level > 0.2)
        {
            type++;
        }
        if (level > 0.5)
        {
            type++;
        }
        if (level > 0.8)
        {
            type++;
        }
        question.Add(questionBankObj.basicProbability(type));
    }
    public static void expectedFrequencyLadder()
    {
        float trials = 6;
        if (level > 0.1)
        {
            trials *= utiltiy.getRandom(1, 3);
        }
        if (level > 0.3)
        {
            trials *= utiltiy.getRandom(1, 3);
        }
        if (level > 0.5)
        {
            trials *= utiltiy.getRandom(1, 3);
        }
        if (level > 0.7)
        {
            trials *= utiltiy.getRandom(1, 3);
        }
        if (level > 0.9)
        {
            trials *= utiltiy.getRandom(1, 3);
        }
        question.Add(questionBankObj.expectedFrequency(trials));
    }
    public static void differenceLadder()
    {

        float a = utiltiy.getRandom(0, 10 + (level * 20));
        float b = utiltiy.getRandom(0, 10 + (level * 20));
        if (level > 0.2 && random.NextDouble() < 0.7)
        {
            a = -a;
        }
        if (level > 0.4 && random.NextDouble() < 0.7)
        {
            b = -b;
        }
        if (level > 0.7)
        {
            a *= 1.1f;
        }
        if (level > 0.9)
        {
            b *= 1.1f;
        }
        question.Add(questionBankObj.difference(utiltiy.roundError(a), utiltiy.roundError(b)));
    }
    public static void changingTemperaturesLadder()
    {
        float a = utiltiy.getRandom(0, 10 + (level * 20));
        float b = utiltiy.getRandom(0, 10 + (level * 20));
        if (level > 0.2 && random.NextDouble() < 0.7)
        {
            a = -a;
        }
        if (level > 0.4 && random.NextDouble() < 0.7)
        {
            b = -b;
        }
        if (level > 0.7)
        {
            a *= 1.1f;
        }
        if (level > 0.9)
        {
            b *= 1.1f;
        }
        question.Add(questionBankObj.changingTemperatures(utiltiy.roundError(a), utiltiy.roundError(b)));
    }
    public static void polygonSidesLadder()
    {
        int MaxPol = 0;
        if (level > 0.1)
        {
            MaxPol += 5;
        }
        if (level > 0.2)
        {
            MaxPol += 5;
        }
        if (level > 0.3)
        {
            MaxPol += 5;
        }
        if (level > 0.4)
        {
            MaxPol += 5;
        }
        if (level > 0.5)
        {
            MaxPol += 5;
        }
        if (level > 0.6)
        {
            MaxPol += 5;
        }
        if (level > 0.7)
        {
            MaxPol += 5;
        }
        if (level > 0.9)
        {
            MaxPol += 2;
        }
        question.Add(questionBankObj.polygonSides(MaxPol));
    }
    public static void singleBracketsLadder()
    {
        int type = (int)utiltiy.getRandom(level * 2, level * 5);
        bool negatives = false;
        if (level > 0.6)
        {
            negatives = true;
        }
        question.Add(questionBankObj.singleBrackets(type, negatives));
    }
    public static void expandSimplifySingleBracketsLadder()
    {
        int type = (int)Mathf.Round(level * 6);
        int Max = 5 + 4 * (int)level;
        question.Add(questionBankObj.expandSimplifySingleBrackets(type, Max));
    }
    public static void interchangingFDPLadder(string type)
    {
        float[] dec = new float[] { 0.5f, 0.25f, 0.75f, 0.10f, 0.20f, 0.05f, 0.01f, 0.02f, 1.50f, utiltiy.getRandom(0, 200) / 100 };
        float x = 0, y = 0, amount = 0;
        int Max = (int)Mathf.Min(2 + level * 10, dec.Length - 1);
        int Min = Mathf.Max(0, Max - 2);
        int count = 0;
        do
        {
            count++;
            if (count > 50)
            {
                Debug.LogError("FPD failed");
                return;
            }

            x = dec[utiltiy.getRandom(Min, Max)];
            y = dec[utiltiy.getRandom(Min, Max)];
            amount = 10 * utiltiy.getRandom(1, 10 * Max);
            Debug.Log(x + " " + y + " " + amount * x + " " + Mathf.Round(amount * x) + " " + amount * y + " " + Mathf.Round(amount * y));
            Debug.Log(x == y);
            Debug.Log(amount * x != Mathf.Round(amount * x));
            Debug.Log(amount * y != Mathf.Round(amount * y));

        } while (x == y || !Mathf.Approximately(amount * x, Mathf.Round(amount * x)) || !Mathf.Approximately(amount * y, Mathf.Round(amount * y)));

        question.Add(questionBankObj.interchangingFDP(x, y, utiltiy.roundError(amount), type));
    }
    public static void fibonacciLadder()
    {
        int f0 = (int)utiltiy.getRandom(0, 5);
        int f1 = f0 + (int)utiltiy.getRandom(1, 5);
        int given1 = 0;
        int given2 = 1;
        if (level > 0.1 && level < 0.3)
        {
            f0 *= -1;
            if (utiltiy.toss())
            {
                f1 *= -1;
            }
        }
        if (level > 0.2 && level < 0.4)
        {
            f0 /= 2;
            if (utiltiy.toss())
            {
                f1 /= 2;
            }
        }
        if (level > 0.4)
        {
            given2++;
        }
        if (level > 0.6)
        {
            given1 = utiltiy.getRandom(1, 2);
            given2 = given1 + utiltiy.getRandom(1, 2);
        }
        if (level > 0.7)
        {
            given1 = utiltiy.getRandom(2, 4);
            given2 = given1 + utiltiy.getRandom(2, 3);
        }
        if (level > 0.8)
        {
            f0 *= -1;
            if (utiltiy.toss())
            {
                f1 *= -1;
            }
        }
        if (level > 0.9)
        {
            f0 /= 2;
            if (utiltiy.toss())
            {
                f1 /= 2;
            }
        }
        int find = 0;
        do
        {
            find = utiltiy.getRandom(1, given2 + 2);
        } while (find == given1 || find == given2);
        question.Add(questionBankObj.fibonacci(f0, f1, given1, given2, find));
    }
    public static void geometricSequenceLadder()
    {
        float a = utiltiy.getRandom(1, 10);
        float r = utiltiy.getRandom(2, 5);
        float given1 = 0;
        float given2 = 1;
        if (level > 0.2 && utiltiy.toss())
        {
            if (r == 3)
            {
                r = 10;
            }
            a *= Mathf.Pow(r, 3);
            r = 1 / r;
        }
        if (level > 0.4)
        {
            given2++;
        }
        if (level > 0.4 && utiltiy.toss())
        {
            r *= -1;
        }
        if (level > 0.6)
        {
            given1 = utiltiy.getRandom(1, 2);
            given2 = given1 + utiltiy.getRandom(1, 2);
        }
        if (level > 0.7)
        {
            given1 = utiltiy.getRandom(2, 3);
            given2 = given1 + utiltiy.getRandom(1, 2);
        }
        int find = 0;
        do
        {
            find = (int)utiltiy.getRandom(1, given2 + 2);
        } while (find == given1 || find == given2);
        question.Add(questionBankObj.geometricSequence(a, r, given1, given2, find));
    }
    public static void convertingTimeLadder()
    {
        float step = 0;
        int from, to;
        do
        {
            step = 1;
            if (level > 0.5)
            {
                step++;
            }
            if (level > 0.7)
            {
                step++;
            }
            if (level > 0.9)
            {
                step++;
            }
            from = (int)utiltiy.getRandom(0, 4);
            to = (int)utiltiy.getRandom(0, 4);
        } while (Mathf.Abs(from - to) > step || from - to == 0);
        bool currDecimal = false;
        float x = 0;
        do
        {
            currDecimal = false;
            if (level > 0.4)
            {
                currDecimal = true;
            }
            x = 6 * utiltiy.getRandom(1, 2 + level * 15);
            if (step > 2)
            {
                x *= 3;
            }
            if (to > from)
            {
                x *= 3;
            }
            else
            {
                x /= 3;
            }
            if (to == 4)
            {
                x *= 7;
            }
            if (to == 3 && from < 3)
            {
                x *= 2;
            }
            if (level < 0.3 && from == 0 && to == 1)
            {
                x *= 10;
            }
            if (level < 0.3 && from == 1 && to == 2)
            {
                x *= 10;
            }
            if (level > 0.3 && utiltiy.toss())
            {
                x /= 5;
            }
            if (level > 0.5 && utiltiy.toss())
            {
                x /= 5;
            }
        } while (x != Mathf.Round(x) && !currDecimal);
        question.Add(questionBankObj.convertingTime(from, to, utiltiy.roundError(x)));
    }
    public static void gradientTwoPointsLadder()
    {
        float x1, x2, y1, y2, temp;
        do
        {
            x1 = utiltiy.getRandom(1, 10 + level * 20);
            x2 = x1 + utiltiy.getRandom(1, 10 + level * 20);
            y1 = utiltiy.getRandom(1, 10 + level * 20);
            y2 = y1 + utiltiy.getRandom(1, 10 + level * 20);
            temp = 0;
            if (level > 0.2)
            {
                temp = x1;
                x1 = x2;
                x2 = temp;
            }
            if (level > 0.3)
            {
                temp = y1;
                y1 = y2;
                y2 = temp;
            }
            if (level > 0.4 & utiltiy.toss())
            {
                y1 *= -1;
            }
            if (level > 0.5 & utiltiy.toss())
            {
                x1 *= -1;
            }
            if (level > 0.6 & utiltiy.toss())
            {
                y2 *= -1;
            }
            if (level > 0.7 & utiltiy.toss())
            {
                x2 *= -1;
            }
            if (level > 0.8)
            {
                if (utiltiy.toss())
                {
                    x1 /= 2;
                }
                if (utiltiy.toss())
                {
                    x2 /= 2;
                }
                if (utiltiy.toss())
                {
                    y1 /= 2;
                }
                if (utiltiy.toss())
                {
                    y2 /= 2;
                }
            }
        } while (y1 == y2);
        question.Add(questionBankObj.gradientFromTwoPoints(x1, y1, x2, y2));
    }
    public static void midpointTwoPointsLadder()
    {

        float temp = 0, x1, x2, y1, y2;
        do
        {
            x1 = utiltiy.getRandom(1, 10 + level * 20);
            x2 = x1 + utiltiy.getRandom(1, 10 + level * 20);
            y1 = utiltiy.getRandom(1, 10 + level * 20);
            y2 = y1 + utiltiy.getRandom(1, 10 + level * 20);
            if (level < 0.3 && (x1 + x2) % 2 != 0)
            {
                x2++;
            }
            if (level < 0.3 && (y1 + y2) % 2 != 0)
            {
                y2++;
            }

            if (level > 0.2)
            {
                temp = x1;
                x1 = x2;
                x2 = temp;
            }
            if (level > 0.3)
            {
                temp = y1;
                y1 = y2;
                y2 = temp;
            }
            if (level > 0.4 & utiltiy.toss())
            {
                y1 *= -1;
            }
            if (level > 0.5 & utiltiy.toss())
            {
                x1 *= -1;
            }
            if (level > 0.6 & utiltiy.toss())
            {
                y2 *= -1;
            }
            if (level > 0.7 & utiltiy.toss())
            {
                x2 *= -1;
            }
            if (level > 0.8)
            {
                if (utiltiy.toss())
                {
                    x1 /= 2;
                }
                if (utiltiy.toss())
                {
                    x2 /= 2;
                }
                if (utiltiy.toss())
                {
                    y1 /= 2;
                }
                if (utiltiy.toss())
                {
                    y2 /= 2;
                }
            }
            if (level > 0.9)
            {
                if (utiltiy.toss())
                {
                    x1 /= 2;
                }
                if (utiltiy.toss())
                {
                    x2 /= 2;
                }
                if (utiltiy.toss())
                {
                    y1 /= 2;
                }
                if (utiltiy.toss())
                {
                    y2 /= 2;
                }
            }
        } while (y1 == y2);
        question.Add(questionBankObj.midpointFromTwoPoints(x1, y1, x2, y2));
    }
    public static void completingSquareLadder()
    {
        float a = 1;
        float b = 2 * utiltiy.getRandom(1, 10 + level * 10);
        float c = 0;
        if (level > 0)
        {
            c = 2 * utiltiy.getRandom(1, 10 + level * 20);
        }
        if (level > 0.2f && utiltiy.toss())
        {
            c *= -1;
        }
        if (level > 0.4)
        {
            b = utiltiy.getRandom(1, 10 + level * 20);
        }
        if (level > 0.5f && utiltiy.toss())
        {
            b *= -1;
        }
        if (level > 0.8)
        {
            a = 2 * utiltiy.getRandom(1, 2);
            b *= 2;
        }
        if (level > 0.9 && utiltiy.toss())
        {
            a = -a;
        }
        question.Add(questionBankObj.completingSquare(a, b, c));
    }
    public static void turningPointToQuadraticLadder()
    {
        float x = utiltiy.getRandom(1, 10 + level * 10);
        float y = utiltiy.getRandom(1, 10 + level * 10);
        bool Min = true;
        if (level > 0.1 && utiltiy.toss())
        {
            y *= -1;
        }
        if (level > 0.3 && utiltiy.toss())
        {
            x *= -1;
        }
        if (level > 0.5 && utiltiy.toss())
        {
            Min = false;
        }
        if (level > 0.7)
        {
            Min = false;
        }
        question.Add(questionBankObj.turningPointToQuadratic(x, y, Min));
    }
    public static void factoriseExpandQuadraticsLadder(bool expanding)
    {
        float a = 1;
        float b = utiltiy.getRandom(1, 5 + level * 10);
        float c = 1;
        float d = utiltiy.getRandom(1, 5 + level * 10);

        if (level > 0.2f && utiltiy.toss())
        {
            b = -b;
        }
        if (level > 0.4f && utiltiy.toss())
        {
            d = -d;
        }
        if (level > 0.6)
        {
            a = utiltiy.getRandom(1, 3);
        }
        if (level > 0.7 && utiltiy.toss())
        {
            a = -a;
        }
        if (level > 0.8)
        {
            c = utiltiy.getRandom(1, 3);
        }
        if (level > 0.9 && utiltiy.toss() && a > 0)
        {
            c = -c;
        }
        question.Add(questionBankObj.factoriseExpandQuadratics(a, b, c, d, expanding));
    }
    public static void indexLawMultiplyLadder()
    {
        //  float currBase = utiltiy.letterPicker(-1, false); //not sure what this is for ???
        float currBase = utiltiy.getRandom(2, 4 + level * 2);
        float ex1, ex2, shift1, shift2;
        do
        {
            ex1 = utiltiy.getRandom(1, 5 + level * 2);
            ex2 = utiltiy.getRandom(1, 5 + level * 2);
            shift1 = 1;
            shift2 = 1;
            if (level > 0.1 && utiltiy.toss())
            {
                ex1 = -ex1;
            }
            if (level > 0.3 && utiltiy.toss())
            {
                ex2 = -ex2;
            }
            if (level > 0.5 && utiltiy.toss())
            {
                shift1 = utiltiy.getRandom(2, 4);
                ex1 = Mathf.Abs(ex1);
            }
            if (level > 0.7)
            {
                shift2 = utiltiy.getRandom(2, 4);
                ex2 = Mathf.Abs(ex2);
            }
            if (level > 0.8 && utiltiy.toss())
            {
                ex1 = -ex1;
            }
            if (level > 0.9 && utiltiy.toss())
            {
                ex2 = -ex2;
            }
        } while (ex1 == -ex2);

        question.Add(questionBankObj.indexLawMultiply(currBase, ex1, shift1, ex2, shift2));
    }
    public static void indexLawDivideLadder()
    {
        //   float currBase = utiltiy.letterPicker(); //again, whats the point of defining something only to redefine it again right below it ?
        float currBase = utiltiy.getRandom(2, 4 + level * 2);
        float ex1, ex2, shift1, shift2;
        do
        {
            ex1 = utiltiy.getRandom(1, 5 + level * 2);
            ex2 = utiltiy.getRandom(1, 5 + level * 2);
            shift1 = 1;
            shift2 = 1;
            if (level > 0.1 && utiltiy.toss())
            {
                ex1 = -ex1;
            }
            if (level > 0.3 && utiltiy.toss())
            {
                ex2 = -ex2;
            }
            if (level > 0.5 && utiltiy.toss())
            {
                shift1 = utiltiy.getRandom(2, 4);
                ex1 = Mathf.Abs(ex1);
            }
            if (level > 0.7)
            {
                shift2 = utiltiy.getRandom(2, 4);
                ex2 = Mathf.Abs(ex2);
            }
            if (level > 0.8 && utiltiy.toss())
            {
                ex1 = -ex1;
            }
            if (level > 0.9 && utiltiy.toss())
            {
                ex2 = -ex2;
            }
        } while (ex1 == -ex2);
        question.Add(questionBankObj.indexLawDivide(currBase, ex1, shift1, ex2, shift2));
    }
    public static void indexLawPowerOfPowerLadder()
    {
        // float currBase = utiltiy.letterPicker(); //wtf is thsi shit lamo
        float currBase = utiltiy.getRandom(2, 4 + level * 2);
        float ex1 = utiltiy.getRandom(1, 5 + level * 5);
        float ex2 = utiltiy.getRandom(1, 5 + level * 10);
        float shift1 = 1;
        if (level > 0.2 && utiltiy.toss())
        {
            ex1 = -ex1;
        }
        if (level > 0.4 && utiltiy.toss())
        {
            ex1 = Mathf.Abs(ex1);
            if (currBase < 4)
            {
                shift1 = utiltiy.getRandom(2, 3 + level * 3);
            }
            else
            {
                shift1 = utiltiy.getRandom(2, 3);
            }
        }
        if (level > 0.6 && utiltiy.toss())
        {
            ex1 = -ex1;
        }
        if (level > 0.7 && shift1 < 2)
        {
            shift1++;
        }
        if (level > 0.8)
        {
            ex1 = -Mathf.Abs(ex1);
        }
        question.Add(questionBankObj.indexLawPowerOfPower(currBase, ex1, shift1, ex2));
    }
    public static void indexLawMixedLadder()
    {
        switch (utiltiy.getRandom(0, 2))
        {
            case 0:
                indexLawMultiplyLadder();
                break;
            case 1:
                indexLawDivideLadder();
                break;
            case 2:
                indexLawPowerOfPowerLadder();
                break;
        }
    }
    public static void ratioMixedLadder()
    {
        switch (utiltiy.getRandom(0, 4))
        {
            case 0:
                ratioShareLadder();
                break;
            case 1:
                ratioReverseLadder();
                break;
            case 2:
                simplifyingRatiosLadder();
                break;
            case 3:
                combiningRatiosLadder();
                break;
            case 4:
                ratioDifferenceLadder();
                break;
        }
    }
    public static void combiningRatiosLadder()
    {
        int Max = (int)utiltiy.getRandom(3, 5 + level * 35);
        question.Add(questionBankObj.combiningRatios(Max));
    }


    public static void exponentsExpandedForm()
    {
        int currBase = utiltiy.getRandom(1, 12);
        int exp = utiltiy.getRandom(2, 5);
        question.Add(questionBankObj.exponentsExpandedForm(currBase, exp));
    }


}


