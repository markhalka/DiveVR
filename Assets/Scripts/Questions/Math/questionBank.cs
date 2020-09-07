using System;
using System.Collections.Generic;
using UnityEngine;

public static class MyExtensionMethods
{

    public static string toFixed(this float number, float decimals)
    {
        return number.ToString("N" + decimals);
    }

    public static float toPrecision(this float number, float accuracy) //<----fix that 
    {
        return number;//toPrecision(number, accuracy).toString();
    }

    public static string slice(this float number, int start, int end) //<-- fix
    {
        string output = slice(number.ToString(), start, end);
        return output;

    }

    public static string slice(this string number, int start, int end) //<-- fix
    {

        return number.Substring(start, number.Length + end);

    }


}

public class questionBank
{


    //------------- NEED TO BE TESTED --------------------

    public utilities.Question arabicToRoman(int number)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "What is " + number + " in roman numerals?";
        problem.stringAnswer = utiltiy.ArabicToRoman(number);
        problem.isRomam = true;
        return problem;

    }

    public utilities.Question romanToArabic(string roman)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "What is " + roman + " in Arabic numerals?";
        problem.stringAnswer = utiltiy.RomanToArabic(roman).ToString();
        //   problem.isRomam = true;

        return problem;

    }




    //------------- NEED TO BE TESTED --------------------




    public utilities.Question fourOps(List<float> numbers, string type)
    {
        utilities.Question problem = new utilities.Question();

        problem.answer = numbers[0];
        problem.question = numbers[0].ToString();
        for (int i = 1; i < numbers.Count; i++)
        {
            problem.question += " " + type + " " + numbers[i];

            switch (type)
            {
                case "+":
                    problem.answer += numbers[i];
                    break;
                case "-":
                    problem.answer -= numbers[i];
                    break;
                case "*":
                    problem.answer *= numbers[i];
                    break;
                case "/":
                    problem.answer /= numbers[i];
                    break;
            }

        }


        problem.stringAnswer = problem.answer.ToString();

        return problem;
    }

    public utilities.Question fourOpsDecimal(float x, float y, string type)
    {
        utilities.Question problem = new utilities.Question();
        switch (type)
        {
            case "+":
                problem.question = x + " + " + y;
                problem.answer = x + y;
                break;
            case "-":
                problem.question = x + " - " + y;
                problem.answer = x - y;
                break;
            case "*":
                problem.question = x + " * " + y;
                problem.answer = x * y;
                break;
            case "/":
                problem.question = x + " / " + y;
                problem.answer = x / y;
                break;
        }
        problem.answer = utiltiy.roundError(problem.answer);
        return problem;
    }
    public utilities.Question fractionOfAmount(float num, float den, float amount)
    {
        utilities.Question problem = new utilities.Question();
        problem.answer = utiltiy.roundError(amount * num / den);
        var whole = Mathf.Floor(num / den);
        if (whole < 0)
        {
            whole++;
            if (whole != 0)
            {
                num = Mathf.Abs(num);
            }
        }
        num = num % den;
        var hcf = utiltiy.HCF(num, den);
        num /= hcf;
        den /= hcf;
        var fraction = "";
        if (whole != 0)
        {
            fraction += whole;
        }
        fraction += +num + "/" + den;
        problem.question = fraction + " of " + amount;
        return problem;
    }
    public utilities.Question fractionalChange(float num, float den, float amount, bool decrease)
    {
        utilities.Question problem = new utilities.Question();
        if (decrease)
        {
            problem.answer = amount - utiltiy.roundError(amount * num / den);
        }
        else
        {
            problem.answer = amount + utiltiy.roundError(amount * num / den);
        }
        var whole = Mathf.Floor(num / den);
        if (whole < 0)
        {
            whole++;
            if (whole != 0)
            {
                num = Mathf.Abs(num);
            }
        }
        num = num % den;
        var hcf = utiltiy.HCF(num, den);
        num /= hcf;
        den /= hcf;
        var fraction = "";
        if (whole != 0)
        {
            fraction += whole;
        }
        fraction += +num + "/" + den;
        if (decrease)
        {
            problem.question = "Decrease " + amount + " by " + fraction;
        }
        else
        {
            problem.question = "Increase " + amount + " by " + fraction;
        }
        return problem;
    }
    public utilities.Question percentageOfAmount(float percentage, float amount)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = percentage + "% of " + amount;
        problem.answer = utiltiy.roundError(percentage * amount / 100);
        return problem;
    }
    public utilities.Question percentageIncreaseDecrease(float percentage, float amount, bool increase, bool reverse)
    {
        utilities.Question problem = new utilities.Question();
        float newAmount = 0;
        if (increase)
        {
            newAmount = utiltiy.roundError(amount + percentage * amount / 100);
        }
        else
        {
            newAmount = utiltiy.roundError(amount - percentage * amount / 100);
        }
        if (!reverse)
        {
            if (increase)
            {
                problem.question = "Increase " + amount + " by " + percentage + "%";
            }
            else
            {
                problem.question = "Decrease " + amount + " by " + percentage + "%";
            }
            problem.answer = newAmount;
        }
        else
        {
            problem.question = "An amount was ";
            if (increase)
            {
                problem.question += "increased";

            }
            else
            {
                problem.question += "decreased";
            }
            problem.question += " by " + percentage + "% to " + newAmount + ".";
            problem.question += "What was the original amount?";
            problem.answer = amount;
        }
        return problem;
    }
    public utilities.Question percentageMultipliers(float percentage, float type)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "";
        switch (type)
        {
            case 0:
                problem.question += "What would you multiply by to find " + percentage + "% of an amount?";
                problem.answer = utiltiy.roundError(percentage / 100);
                break;
            case 1:
                problem.question += "What would you multiply by to increase an amount by " + percentage + "%?";
                problem.answer = utiltiy.roundError((100 + percentage) / 100);
                break;
            case 2:
                problem.question += "What would you multiply by to decrease an amount by " + percentage + "%?";
                problem.answer = utiltiy.roundError((100 - percentage) / 100);
                break;
        }
        problem.question += "";
        return problem;
    }
    public utilities.Question percentageChange(float oldAmount, float newAmount)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "";
        if (oldAmount > newAmount)
        {
            problem.question += "An amount was decreased from " + oldAmount + " to " + newAmount + ".";
            problem.question += "Work out the percentage decrease.";
        }
        else
        {
            problem.question += "An amount was increased from " + oldAmount + " to " + newAmount + ".";
            problem.question += "Work out the percentage increase.";
        }
        problem.stringAnswer = utiltiy.roundError(100 * Mathf.Abs(oldAmount - newAmount) / oldAmount).ToString();
        problem.after = "%";
        problem.question += "";
        return problem;
    }
    public utilities.Question repeatedPercentageChange(float originalAmount, float percentage, float iterations, bool increase)
    {
        utilities.Question problem = new utilities.Question();
        string[] units = new string[] { "second", "minute", "hour", "day", "week", "year" };
        var timeUnit = units[utiltiy.getRandom(0, units.Length - 1)];
        float newAmount = 0;
        problem.question = "";
        if (increase)
        {
            problem.question += "An amount of " + originalAmount + " is increased by " + percentage + "% every " + timeUnit + ".";
            problem.question += "How much will it be worth after " + iterations + " " + timeUnit + "s?";
            newAmount = originalAmount * Mathf.Pow(1 + percentage / 100, iterations);
        }
        else
        {
            problem.question += "An amount of " + originalAmount + " is decreased by " + percentage + "% every " + timeUnit + ".";
            problem.question += "How much will it be worth after " + iterations + " " + timeUnit + "s?";
            newAmount = originalAmount * Mathf.Pow(1 - percentage / 100, iterations);
        }
        problem.question += "";
        problem.answer = Mathf.Round(100 * newAmount) / 100;
        return problem;
    }
    //x is the number, i is the index

    public utilities.Question classifyNumbers(int level)
    {
        utilities.Question problem = new utilities.Question();
        int type = utiltiy.getRandom(0, level);
        problem.question = "What is the most specific number system for: " + getNumberType(type);
        string[] numberSystem = new string[] { "natural", "integers", "rational", "irrational", "complex" };

        List<int> included = new List<int>();
        included.Add(type);
        problem.choices = new List<string>();
        problem.choices.Add(numberSystem[type]);

        while (problem.choices.Count < 3)
        {
            int next = utiltiy.getRandom(0, numberSystem.Length - 1);
            while (included.Contains(next))
            {
                next = utiltiy.getRandom(0, numberSystem.Length - 1);
            }
            included.Add(next);
            problem.choices.Add(numberSystem[next]);
        }
        problem.multipleChoice = true;
        problem.stringAnswer = numberSystem[type];
        problem.choices.Shuffle();

        return problem;

    }

    public string getNumberType(int type)
    {
        string num = "";

        string[] real = new string[] { "e", "pi" };

        switch (type)
        {
            case 0: //natural numb, 
                num = utiltiy.getRandom(1, 10).ToString();
                break;
            case 1://integers,
                num = utiltiy.getRandom(-10, 0).ToString();
                break;
            case 2: // rational numbers
                int n = utiltiy.getRandom(5, 10);
                int d = utiltiy.getRandom(1, 4);
                num = (n / d) + "";
                break;
            case 3://irrational
                num = real[utiltiy.getRandom(0, real.Length - 1)];
                break;
            case 4:  //+ complex 
                num = utiltiy.getRandom(1, 10) + "i " + utiltiy.getRandom(-10, 20);
                break;

        }

        return num;
    }




    //-----------------------------------ADDED,NEED TESTING-------------------------------------------
    //for these ones you cant do enter value
    public utilities.Question identifyPlaceValue(float x, int i)
    {
        utilities.Question problem = new utilities.Question();
        char numberAt = x.ToString()[i];
        problem.question = "What is the value of the " + numberAt + " in " + x;
        int answer = x.ToString().Length - i - 1;
        List<string> included = new List<string>();
        included.Add(utiltiy.placeValuePicker(answer));
        for (int j = 0; j < 3; j++)
        {
            int next = utiltiy.getRandom(0, x.ToString().Length);
            while (included.Contains(utiltiy.placeValuePicker(next)))
            {
                next = utiltiy.getRandom(0, x.ToString().Length);
            }
            included.Add(utiltiy.placeValuePicker(next));
        }

        problem.stringAnswer = included[0];
        included.Shuffle();
        problem.choices = included;
        problem.multipleChoice = true;


        //generate the choices here, then set multiple choice to true, int math layout check if choices already exist, if not then you can create defualt ones 
        //in utilities replace it with try parse 

        return problem;
    }

    //the user has to say what 3000 + 2000 + 20 + 1 is 
    //make this also work for deciamls 
    public utilities.Question expandedForm(int x)
    {
        utilities.Question problem = new utilities.Question();
        List<int> decomp = new List<int>();
        int temp = x;
        while (x > 0)
        {
            decomp.Add(x % 10);
            x /= 10;
        }
        string output = "";
        if (x.ToString().Contains("."))
        {
            //in this case it would e decomp[i] * " 1/ however many zeros 
        }
        else
        {
            for (int i = decomp.Count - 1; i >= 0; i--)
            {
                output += decomp[i].ToString();
                if (i != 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        output += "0";
                    }
                    output += " + ";
                }
            }
        }

        problem.question = "What is the value of: " + output;
        problem.stringAnswer = temp.ToString();
        return problem;
    }

    //in this case co is the coefficent, a has to be bigger than b
    //e.g 7 hunderds is how many tens
    public utilities.Question convertingBetweenPlaceValue(int co, int a, int b)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = co + " " + utiltiy.placeValuePicker(a) + " is how many " + utiltiy.placeValuePicker(b) + "'s";
        int amount = co * (int)Mathf.Pow(10, a - b);

        problem.stringAnswer = amount.ToString();
        return problem;
    }

    //just two numbers 10 16
    //ok, so here maybe show 4 numbers and get them to click on the largest / smallest one
    public utilities.Question comparingNumbers(float a, float b) //this is a special question
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "select the sign that makes this true: " + a + "..." + b;
        string answer;
        if (a == b)
        {
            answer = "=";
        }
        else if (a < b)
        {
            answer = "<";
        }
        else
        {
            answer = ">";
        }
        problem.choices = new List<string>(new string[] { ">", "=", "<" });
        problem.stringAnswer = answer;
        problem.multipleChoice = true;
        return problem;
    }

    public utilities.Question choosingNumbers() //this is a special question
    {

        List<int> numbers = new List<int>();

        for (int i = 0; i < 4; i++)
        {
            numbers.Add(utiltiy.getRandom(50, 1000));
        }

        bool isSmallest = utiltiy.toss();


        utilities.Question problem = new utilities.Question();
        problem.question = "select the largest number";
        if (isSmallest)
        {
            problem.question = "select the smallest number";
        }

        numbers.Sort();

        string answer = numbers[0].ToString();
        if (!isSmallest)
        {
            answer = numbers[numbers.Count - 1].ToString();
        }

        numbers.Shuffle();
        problem.choices = new List<string>();
        foreach (var i in numbers)
        {
            problem.choices.Add(i.ToString());
        }

        problem.stringAnswer = answer;
        problem.multipleChoice = true;
        return problem;
    }

    public utilities.Question comparingNumbers(int n1, int d1, int n2, int d2) //this is a special question
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "select the sign that makes this true: " + n1 + "/" + d1 + " ... " + n2 + "/" + d2;
        float bigger = n1 / d1 > n2 / d2 ? n1 / d1 : n2 / d2;
        problem.stringAnswer = bigger.ToString();
        return problem;
    }


    public utilities.Question evenAndOdd(char sign, int a, int b) //this is a special question
    {
        utilities.Question problem = new utilities.Question();
        problem.question = a + " " + sign + " " + b + " is the answer even or odd?";
        int output = 0;
        switch (sign)
        {
            case '+':
                output = a + b;
                break;
            case '-':
                output = a - b;
                break;
            case '*':
                output = a * b;
                break;
            case '/':
                output = a / b;
                break;

        }
        string answer = output % 2 == 0 ? "Even" : "Odd";
        problem.stringAnswer = answer;
        problem.choices = new List<string>(new string[] { "Even", "Odd" });
        problem.multipleChoice = true;
        return problem;
    }


    public utilities.Question currentEvenAndOdd() //this is a special question
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "is the number even or odd?";
        int output = utiltiy.getRandom(10, 40);

        string answer = output % 2 == 0 ? "Even" : "Odd";
        problem.choices = new List<string>(new string[] { "Even", "Odd" });
        problem.stringAnswer = answer;
        problem.multipleChoice = true;
        return problem;
    }



    public utilities.Question lawsQuestion(string question, string answer, List<string> choices) //this is a special question
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "what law is shown here: " + question;
        problem.stringAnswer = answer;
        problem.choices = choices;
        problem.multipleChoice = true;

        return problem;
    }

    public utilities.Question termonlogigyQuestion(string question, string numbers, List<string> choices) //this is a special question
    {
        utilities.Question problem = new utilities.Question();
        int n = utiltiy.getRandom(0, 3);
        string curr = numbers.Split(',')[n];
        problem.question = "In the expression: " + question + "what is the number " + numbers[n] + " called"; //that should prob work
        problem.stringAnswer = choices[n];
        problem.choices = choices;
        problem.multipleChoice = true;

        return problem;
    }




    public utilities.Question estimating(char sign, int a, int b, int placeValue)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "estimate " + a + " " + sign + " " + b + " by rounding to the nearest " + utiltiy.placeValuePicker(placeValue);
        int output = 0;
        a = (int)Mathf.Round(a / Mathf.Pow(10, placeValue)) * (int)Mathf.Pow(10, placeValue);
        b = (int)Mathf.Round(b / Mathf.Pow(10, placeValue)) * (int)Mathf.Pow(10, placeValue);

        switch (sign)
        {
            case '+':
                output = a + b;
                break;
            case '-':
                output = a - b;
                break;
            case '*':
                output = a * b;
                break;
            case '/':
                output = a / b;
                break;
        }
        problem.stringAnswer = output.ToString();
        return problem;
    }


    public utilities.Question multipleTerms(List<int> terms)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "what is the product of: ";
        int output = 1;
        for (int i = 0; i < terms.Count; i++)
        {
            problem.question += terms[i];
            if (i != terms.Count - 1)
            {
                problem.question += "* ";
            }
            output *= terms[i];
        }
        problem.stringAnswer = output.ToString();
        return problem;
    }


    public utilities.Question primeComposite(int a) //this is a special question
    {
        utilities.Question problem = new utilities.Question();
        problem.question = " is " + a + " prime or composite";
        problem.stringAnswer = utiltiy.isPrime(a) ? "Prime" : "Composite";
        problem.choices = new List<string>(new string[] { "Prime", "Composite" });
        problem.multipleChoice = true;
        return problem;
    }



    public utilities.Question primeFactorization(int a) //<----------- this one is kind of fucked 
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "What is the prime factorization of " + a + " from greatest to least?";
        int[] factors = utiltiy.GetPrimeFactors(a);

        for (int i = 0; i < factors.Length; i++)
        {

            if (factors[i] == 0)
                break;
            problem.stringAnswer += factors[i].ToString();
            if (i != factors.Length - 1)
            {
                problem.stringAnswer += ",";
            }

        }
        if (problem.stringAnswer[problem.stringAnswer.Length - 1] == ',')
        {
            problem.stringAnswer = problem.stringAnswer.Substring(0, problem.stringAnswer.Length - 1);
        }

        problem.isOrdering = true;



        return problem;
    }

    // two hundreds, 3 tenths ....
    public utilities.Question writtenToNumber(string a, int value) //its easier to do this in diff
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "how do you write " + a + " as a number?";
        //for this one, just go throught t
        problem.stringAnswer = value.ToString();
        return problem;
    }



    public utilities.Question bedmas(string expresion)
    {

        //ok so for this one, just keep doing four ops, and get rid of the right hand side, done 
        //theres a tone of order of operations calculators online

        utilities.Question problem = new utilities.Question();
        problem.question = "solve using BEDMAS, " + expresion;
        problem.stringAnswer = utiltiy.Calculate(expresion).ToString();
        problem.isExpression = true;

        return problem;
    }


    //converts to 24 and 12 and vice versa
    //add the am shit
    public utilities.Question twelveTo24hour(int hours, int minutes, bool am, bool is24)
    {
        utilities.Question problem = new utilities.Question();
        string sm1 = minutes.ToString();

        if (minutes == 0)
        {
            sm1 = "00";
        }
        else if (minutes < 10)
        {
            sm1 = "0" + minutes;
        }

        string ending = is24 ? " in 12 hour format" : " in 24 hour format";
        problem.question = "what is " + hours + ":" + sm1 + ending;
        if (is24)
        {
            //conver to 12
        }
        else
        {

        }
        //here just convert between the two
        // return dt.ToString("yyyy/MM/dd, HH:mm:ss"); //24 hour
        //   return dt.ToString("yyyy/MM/dd, hh:mm:ss tt"); // 12 hour
        string amPm = am ? "AM" : "PM";
        string thing = hours + ":" + sm1 + " " + amPm;
        Debug.Log(thing);
        DateTime curr = DateTime.Parse(thing);
        string temp = "";
        if (is24)
        {
            temp = curr.ToString("hh:mm tt"); //was curr.ToString("hh:mm tt")
        }
        else
        {
            temp = curr.ToString("HH:mm tt");
        }
        if (temp[0] == '0')
        {
            temp = temp.Substring(1); //get rid of the leading 0 for the hours 
        }
        problem.stringAnswer = temp.Replace(':', ',');
        problem.inBetween = ":";
        problem.isOrdering = true;
        problem.isTime = true;
        return problem;
    }

    //the elapsed time between two times
    public utilities.Question elapsedTime(int h1, int m1, bool am1, int h2, int m2, bool am2) //<-- this one is also special
    {
        utilities.Question problem = new utilities.Question();
        string sam1 = am1 ? "AM" : "PM";
        string sam2 = am2 ? "AM" : "PM";
        string sm1 = m1.ToString();
        string sm2 = m2.ToString();

        if (m1 == 0)
        {
            sm1 = "00";
        }
        else if (m1 < 10)
        {
            sm1 = "0" + m1;
        }

        if (m2 == 0)
        {
            sm2 = "00";
        }
        else if (m2 < 10)
        {
            sm2 = "0" + m2;
        }



        string startTime = h1 + ":" + m1 + " " + sam1;
        string endTime = h2 + ":" + m2 + " " + sam2;

        TimeSpan durationreamin = DateTime.Parse(endTime).Subtract(DateTime.Parse(startTime));
        problem.question = "How much time passed between " + h1 + ":" + sm1 + " " + sam1 + " and " + h2 + ":" + sm2 + " " + sam2;
        if (durationreamin.Minutes < 0 || durationreamin.Hours < 0)
        {
            durationreamin = DateTime.Parse(startTime).Subtract(DateTime.Parse(endTime));
            problem.question = "How much time passed between " + h2 + ":" + sm2 + " " + sam2 + " and " + h1 + ":" + sm1 + " " + sam1;
        }


        //if its negative, then just flip it around 
        problem.stringAnswer = durationreamin.Hours + "," + durationreamin.Minutes;
        problem.inBetween = ":";
        problem.isOrdering = true;
        return problem;
    }

    //what time is it in passed amount of time 
    public utilities.Question timePassed(int h1, int m1, int passedH1, int passedM1)
    {
        string ps1 = m1.ToString();
        if (m1 == 0)
        {
            ps1 = "00";
        }
        else if (m1 < 10)
        {
            ps1 = "0" + m1;
        }
        utilities.Question problem = new utilities.Question();
        problem.question = "What time is it " + passedH1 + " hours and " + passedM1 + " minutes after " + h1 + ":" + ps1 + " A.M";

        DateTime curr = DateTime.Parse(h1 + ":" + m1).AddHours(passedH1).AddMinutes(passedM1);
        string temp = curr.ToString("h:mm tt");// + ":" + curr.Minute;
        problem.stringAnswer = temp.Replace(':', ',');
        problem.inBetween = ":";
        problem.isOrdering = true;
        problem.isTime = true;
        return problem;
    }

    //convert from an imporper fraction to a mixed number
    /*   public utilities.Question improperToMixed(int n1, int d1) //<-- you need to make sure the fraction is in lowest terms 
       {
           utilities.Question problem = new utilities.Question();
           problem.question = "Convert " + n1 + "/" + d1 + " to a mixed number";
           int whole = n1 / d1;
           int remainder = n1 - whole * d1;
           problem.stringAnswer = whole + " " + remainder + "/" + d1;
           return problem;
       }

       //conver t from a mixed number to an improper fraction
       public utilities.Question mixedToImproper(int w, int n1, int d1) //<-- you also need to make sure this is in lowest terms 
       {
           utilities.Question problem = new utilities.Question();
           problem.question = "convert " + w + " " + n1 + "/" + d1 + " to an improper fraction";
           int top = w * d1 + n1;
           problem.stringAnswer = top + "/" + d1;
           return problem;
       }
       */ //already have these apparently

    public utilities.Question properties(string left, string right, string property) //<------ this one is special, you cant do multiple choice with your current code
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "what property is shown: " + left + " = " + right;
        problem.stringAnswer = property;
        return problem;
    }



    public utilities.Question basic()
    {
        utilities.Question problem = new utilities.Question();

        return problem;
    }



    //-------------------------------------------------------------------------------------

    public utilities.Question halving(float x)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "Half of " + x;
        problem.stringAnswer = utiltiy.roundError(x / 2).ToString();
        return problem;
    }
    public utilities.Question doubling(float x)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "Double " + x;
        problem.stringAnswer = (x * 2).ToString();
        return problem;
    }



    public utilities.Question rounding(float x, float accuracy)
    {
        utilities.Question problem = new utilities.Question();
        string suffix = accuracy.ToString();
        if (accuracy == 1)
        {
            suffix = "the nearest whole number";
        }
        if (accuracy > 1)
        {
            suffix = "the nearest " + accuracy;
        }
        if (accuracy < 1)
        {
            suffix = -Mathf.Round(Mathf.Log(accuracy) / Mathf.Log(10)) + " decimal points";
        }
        problem.question = "Round " + x + " to " + suffix + "";
        problem.answer = utiltiy.roundError(Mathf.Round(x / accuracy) * accuracy);
        if (accuracy < 1)
        {
            problem.stringAnswer = problem.answer.toFixed(-Mathf.Round(Mathf.Log(accuracy) / Mathf.Log(10)));
        }
        return problem;
    }
    public utilities.Question sigFigs(float x, float accuracy)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "Round " + x + " to " + accuracy + " s.f";
        problem.answer = x.toPrecision(accuracy) - 0; //wtf
        return problem;
    }
    public utilities.Question ratioShare(float amount, float[] ratio)
    {
        utilities.Question problem = new utilities.Question();
        float parts = 0;
        problem.question = "Share " + amount + " in the ratio ";
        for (int i = 0; i < ratio.Length; i++)
        {
            parts += ratio[i];
            problem.question += ratio[i] + ":";
        }
        problem.question = problem.question.slice(0, -1);
        float amountPerPart = amount / parts;
        for (int i = 0; i < ratio.Length; i++)
        {
            problem.stringAnswer += ratio[i] * amountPerPart + ","; //putting a comma here
        }
        //  Debug.Log(amount + " " + problem.stringAnswer);
        problem.stringAnswer = problem.stringAnswer.slice(0, -1); //this is the sketch part 
                                                                  //  problem.isRatio = true;
        problem.isOrdering = true; //the order matters 
        problem.inBetween = ":";
        return problem;
    }
    public utilities.Question ratioReverse(float amount, float[] ratio)
    {
        utilities.Question problem = new utilities.Question();
        problem.stringAnswer = "";
        float parts = 0;
        problem.question = "An amount was shared in the ratio ";
        for (int i = 0; i < ratio.Length; i++)
        {
            parts += ratio[i];
            problem.question += ratio[i] + ":";
        }
        problem.question = problem.question.slice(0, -1);
        var amountPerPart = amount / parts;
        switch (utiltiy.getRandom(0, 3))
        {
            case 0:
                problem.question += ". The largest share was " + Mathf.Max(ratio) * amountPerPart + ".";
                problem.question += "What was the smallest share?";
                problem.answer = Mathf.Min(ratio) * amountPerPart;
                break;
            case 1:
                problem.question += ". The smallest share was " + Mathf.Min(ratio) * amountPerPart + ".";
                problem.question += "What was the largest share?";
                problem.answer = Mathf.Max(ratio) * amountPerPart;
                break;
            case 2:
                problem.question += ". The largest share was " + Mathf.Max(ratio) * amountPerPart + ".";
                problem.question += "What was the total amount shared?";
                problem.answer = amount;
                break;
            case 3:
                problem.question += ". The smallest share was " + Mathf.Min(ratio) * amountPerPart + ".";
                problem.question += "What was the total amount shared?";
                problem.answer = amount;
                break;
        }

        return problem;
    }

    utilities utiltiy = new utilities();
    public utilities.Question ratioDifference(float amount, float[] ratio)
    {
        utilities.Question problem = new utilities.Question();
        problem.stringAnswer = "";
        float parts = 0;
        List<utilities.Person> person = new List<utilities.Person>();
        var ratioDisp = "";
        var seed = utiltiy.getRandom(0, 20);
        for (int i = 0; i < ratio.Length; i++)
        {
            parts += ratio[i];
            ratioDisp += ratio[i] + ":";
            person.Add(utiltiy.namePicker(i + seed));
        }
        ratioDisp = ratioDisp.slice(0, -1);
        var amountPerPart = amount / parts;
        if (ratio.Length < 3)
        {
            problem.question = "" + person[0].name + " and " + person[1].name + " shared some money in the ratio " + ratioDisp;
        }
        else
        {
            problem.question = "" + person[0].name + ", " + person[1].name + " and " + person[2].name + " shared some money in the ratio " + ratioDisp;
        }
        int c1;
        int c2;
        int c3;
        do
        {

            c1 = utiltiy.getRandom(0, ratio.Length - 1);
            c2 = utiltiy.getRandom(0, ratio.Length - 1);
        } while (c1 == c2);
        c3 = utiltiy.getRandom(0, ratio.Length - 1);
        var c1Amount = ratio[c1] * amountPerPart;
        var c2Amount = ratio[c2] * amountPerPart;
        var diff = c2Amount - c1Amount;
        string adj;
        if (diff < 0)
        {
            adj = "more than";
        }
        else if (diff > 0)
        {
            adj = "less than";
        }
        else
        {
            adj = "the same as";
        }
        problem.question += ".";
        problem.question += person[c1].name + " gets $" + Mathf.Abs(diff) + " " + adj + " " + person[c2].name + ".";
        problem.question += "How much does " + person[c3].name + " receive?";
        problem.stringAnswer = ratio[c3] * amountPerPart + "";
        problem.before = "$";
        return problem;
    }

    public utilities.Question convertFDP(string type, float num, float den)
    {
        utilities.Question problem = new utilities.Question();
        float currDecimal = utiltiy.roundError((float)num / den);

        float percentage = utiltiy.roundError(currDecimal * 100);

        num = utiltiy.roundError(num);
        den = utiltiy.roundError(den);

        var whole = Mathf.Floor(num / den);
        if (whole < 0)
        {
            whole++;
            if (whole != 0)
            {
                num = Mathf.Abs(num);
            }
        }
        num = num % den;
        var hcf = utiltiy.HCF(num, den);
        num /= hcf;
        den /= hcf;
        var fraction = "";
        var typedFraction = "";
        if (whole != 0)
        {
            fraction += whole + " ";
            typedFraction += whole;
        }
        if (whole != 0 && num != 0)
        {
            typedFraction += " ";
        }
        if (num != 0)
        {
            fraction += num + "/" + den;
            typedFraction += num + "/" + den;
        }
        if (whole == 0 && num == 0)
        {
            fraction += "0";
            typedFraction = "0";
        }

        problem.question = "";
        switch (type)
        {
            case "PD":
                problem.question += percentage + "% as a decimal";
                problem.stringAnswer = currDecimal.ToString();
                break;
            case "DP":
                problem.question += currDecimal + " as a percentage";
                problem.stringAnswer = percentage.ToString();
                problem.after = "%";
                break;
            case "DF":
                problem.question += currDecimal + " as a fraction";
                problem.stringAnswer = fraction;
                problem.isFraction = true;

                break;
            case "PF":
                problem.question += percentage + "% as a fraction";
                problem.stringAnswer = fraction;
                problem.isFraction = true;

                break;
            case "FD":
                problem.question += fraction + " as a decimal";
                problem.stringAnswer = currDecimal.ToString();
                break;
            case "FP":
                problem.question += fraction + " as a percentage";
                problem.stringAnswer = percentage.ToString();
                problem.after = "%";
                break;
        }
        return problem;
    }
    public utilities.Question collectingTerms(string[] letters, string[] variables, float[] coeff)
    {
        utilities.Question problem = new utilities.Question();
        var totalTerms = coeff.Length;
        problem.question = "Simplify: ";
        for (int i = 0; i < totalTerms; i++)
        {
            if (coeff[i] > 0 && i > 0)
            {
                problem.question += " + ";
            }
            if (coeff[i] < 0)
            {
                problem.question += " - ";
            }
            if (Mathf.Abs(coeff[i]) > 1)
            {
                problem.question += Mathf.Abs(coeff[i]);
            }
            if (coeff[i] != 0)
            {
                problem.question += variables[i];
            }
        }
        List<float> collected = new List<float>();
        for (int i = 0; i < letters.Length; i++)
        {
            float count = 0;
            for (var j = 0; j < totalTerms; j++)
            {
                if (variables[j] == letters[i])
                {
                    count += coeff[j];
                }
            }
            collected.Add(count);
        }
        var answer = "";
        for (int i = 0; i < letters.Length; i++)
        {
            if (collected[i] > 0 && i > 0)
            {
                answer += "+";
            }
            if (collected[i] < 0)
            {
                answer += "-";
            }
            if (Mathf.Abs(collected[i]) > 1)
            {
                answer += Mathf.Abs(collected[i]);
            }
            if (collected[i] != 0)
            {
                answer += letters[i];
            }
        }
        if (answer == "")
        {
            answer = "0";
        }
        problem.isExpression = true;
        Debug.Log(answer + " answer");
        problem.stringAnswer = answer;
        return problem;
    }
    public utilities.Question multiplyingTerms(int type, bool negatives)
    {
        utilities.Question problem = new utilities.Question();
        string[] l = new string[4];
        var choice = utiltiy.getRandom(0, 20);
        for (int i = 0; i < 4; i++)
        {
            l[i] = (utiltiy.letterPicker(choice + i));
        }
        int[] c = new int[4];
        for (int i = 0; i < 4; i++)
        {
            c[i] = (utiltiy.getRandom(2, 8 + i));
            if (negatives && toss())
            {
                c[i] = -c[i];
            }
        }
        problem.question = "Simplify: ";
        if (type > 5)
        {
            type = 5;
        }
        switch (type)
        {
            case 0:
                if (toss())
                {
                    problem.question += c[0] + l[0] + " * " + c[1];
                }
                else
                {
                    problem.question += c[0] + " * " + c[1] + l[0];
                }
                problem.stringAnswer = (c[0] * c[1]) + l[0];
                break;
            case 1:
                if (toss())
                {
                    problem.question += l[0] + " * " + l[0];
                    problem.stringAnswer = l[0] + "^2";

                }
                else if (toss())
                {
                    problem.question += l[0] + " * " + l[0] + " * " + l[0];
                    problem.stringAnswer = l[0] + "^3";

                }
                else
                {
                    problem.question += l[0] + " * " + l[0] + " * " + l[0] + " * " + l[0];
                    problem.stringAnswer = l[0] + "^4";

                }
                break;
            case 2:
                if (toss())
                {
                    problem.question += l[0] + " * " + l[1];
                    problem.stringAnswer = l[0] + l[1];
                }
                else
                {
                    problem.question += l[0] + " * " + l[1] + " * " + l[2];
                    problem.stringAnswer = l[0] + l[1] + l[2];
                }
                break;
            case 3:
                if (toss())
                {
                    problem.question += c[0] + l[0] + " * " + c[1] + l[1];
                    problem.stringAnswer = (c[0] * c[1]) + l[0] + l[1];
                }
                else
                {
                    problem.question += c[0] + l[0] + " * " + c[1] + l[0];
                    problem.stringAnswer = (c[0] * c[1]) + l[0] + "^2";

                }
                break;
            case 4:
                if (toss())
                {
                    problem.question += c[0] + l[0] + l[1] + " * " + c[1] + l[0];
                    problem.stringAnswer = (c[0] * c[1]) + l[0] + "^2" + l[1];
                }
                else
                {
                    problem.question += c[0] + l[0] + l[1] + " * " + c[1] + l[0] + l[1];
                    problem.stringAnswer = (c[0] * c[1]) + l[0] + "^2" + l[1] + "^2";
                }
                break;
            case 5:
                if (toss())
                {
                    problem.question += c[0] + l[0] + "^2" + l[1] + " * " + c[1] + l[0] + " * " + l[1] + l[2];
                    problem.stringAnswer = (c[0] * c[1]) + l[0] + "^3" + l[1] + "^2" + l[2];
                }
                else
                {
                    problem.question += c[0] + l[1] + l[2] + " * " + c[1] + l[1] + " * " + c[2] + l[0] + l[2];
                    problem.stringAnswer = (c[0] * c[1] * c[2]) + l[0] + l[1] + "^2" + l[2] + "^2";
                }
                break;
        }
        problem.isExpression = true;
        return problem;
    }

    System.Random random = new System.Random();

    public utilities.Question factors(int maxFactors, int minNumber, int maxNumber)
    {
        utilities.Question problem = new utilities.Question();
        int totalFactors = maxFactors + 1;
        int x = 0;
        string answer = "1";
        while (totalFactors > maxFactors)
        {
            totalFactors = 1;

            x = utiltiy.getRandom(minNumber, maxNumber);
            if (x % 2 == 1 && random.NextDouble() < 0.5 && x < maxNumber)
            {
                x++;
            }
            for (int i = 2; i <= x; i++)
            {
                if (x % i == 0)
                {
                    answer += "," + i;
                    totalFactors++;
                    if (totalFactors > 4)
                    {
                        break;
                    }
                }
            }
        }
        if (totalFactors > 5)
        {
            problem.question = "What are the first five factors of " + x + " from least to greatest?";
        }
        else
        {
            problem.question = "What are the factors of " + x + " from least to greatest?";
        }


        problem.stringAnswer = answer;
        problem.isOrdering = true;
        return problem;
    }
    public utilities.Question multiples(int multiple, float x)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = multiple + utiltiy.ordinal(multiple) + " multiple of " + x;
        problem.answer = x * multiple;
        return problem;
    }


    public utilities.Question hcf(float x, float y, float z)
    {
        utilities.Question problem = new utilities.Question();
        if (z > 0)
        {
            problem.question = "HCF of " + x + ", " + y + " and " + z;
            problem.stringAnswer = utiltiy.HCF(utiltiy.HCF(x, y), z).ToString();
        }
        else
        {
            problem.question = "HCF of " + x + " and " + y;
            problem.stringAnswer = utiltiy.HCF(x, y).ToString();
        }
        return problem;
    }
    public utilities.Question lcm(float x, float y, float z)
    {
        utilities.Question problem = new utilities.Question();
        if (z > 0)
        { //not sure if that right
            var temp = x * y / (utiltiy.HCF(x, y));
            problem.question = "LCM of " + x + ", " + y + " and " + z;
            problem.answer = temp * z / utiltiy.HCF(temp, z);
        }
        else
        {
            problem.question = "LCM of " + x + " and " + y;
            problem.answer = x * y / (utiltiy.HCF(x, y));
        }
        return problem;
    }


    public utilities.Question simplifyingRatios(int terms, int maxPrime)
    {
        utilities.Question problem = new utilities.Question();
        int[] simplifiedRatio = new int[terms]; //(terms);
        int[] ratio = new int[terms];
        var multiplier = utiltiy.getRandom(2, maxPrime);
        int i = 0;
        for (i = 0; i < ratio.Length; i++)
        {
            simplifiedRatio[i] = utiltiy.getRandom(1, maxPrime);
            while (!utiltiy.isPrime(simplifiedRatio[i]))
            {
                simplifiedRatio[i] = utiltiy.getRandom(1, maxPrime);
            }
            ratio[i] = simplifiedRatio[i] * multiplier;
        }
        while (simplifiedRatio[0] == simplifiedRatio[1])
        {
            simplifiedRatio[1] = utiltiy.getRandom(1, maxPrime);
            while (!utiltiy.isPrime(simplifiedRatio[1]))
            {
                simplifiedRatio[1] = utiltiy.getRandom(1, maxPrime);
            }
            ratio[1] = simplifiedRatio[1] * multiplier;
        }
        problem.question = "Simplify ";
        problem.stringAnswer = "";

        for (i = 0; i < ratio.Length - 1; i++)
        {
            problem.question += ratio[i] + ":";
            problem.stringAnswer += simplifiedRatio[i] + ","; //made that a comma
        }
        problem.question += ratio[i]; //wtf is going on here
        problem.stringAnswer += simplifiedRatio[i];
        problem.inBetween = ":";
        problem.isOrdering = true; //the order matters 
        return problem;
    }
    public utilities.Question simplifyingFractions(int maxPrime)
    {
        utilities.Question problem = new utilities.Question();
        var numerator = utiltiy.getRandom(1, maxPrime / 2);
        var denominator = utiltiy.getRandom(2, maxPrime);
        while (denominator <= numerator || utiltiy.HCF(numerator, denominator) != 1)
        {
            denominator = utiltiy.getRandom(1, maxPrime);
        }
        var multiplier = utiltiy.getRandom(maxPrime / 4, maxPrime);
        problem.question = "Simplify " + numerator * multiplier + "/" + denominator * multiplier;
        problem.stringAnswer = numerator + "/" + denominator;
        problem.isFraction = true;
        problem.isOrdering = true; //the order matters, num then denom
        return problem;
    }
    public utilities.Question nthTermFinding(int a, int b, int c)
    {
        utilities.Question problem = new utilities.Question();
        List<float> terms = new List<float>();
        for (int i = 1; i < 5; i++)
        {
            terms.Add((a * i * i) + (b * i) + (c));
        }
        problem.question = "Find the nth term of";
        for (int i = 0; i < terms.Count; i++)
        {
            problem.question += terms[i] + ", ";
        }
        problem.question += "...";
        problem.stringAnswer = "";

        var firstTerm = true;
        problem.stringAnswer += utiltiy.fixTerm(a, "n^2", firstTerm);
        if (a != 0)
        {
            firstTerm = false;
        }
        problem.stringAnswer += utiltiy.fixTerm(b, "n", firstTerm);
        if (b != 0)
        {
            firstTerm = false;
        }
        problem.stringAnswer += utiltiy.fixTerm(c, "", firstTerm);
        problem.isExpression = true;
        return problem;
    }


    public utilities.Question sequencesNextTerm(float a, float b, float c, bool fraction) //ya use toFraction
    {
        utilities.Question problem = new utilities.Question();
        List<float> terms = new List<float>();
        var sequence = "";
        for (int i = 1; i < 5; i++)
        {
            terms.Add(utiltiy.roundError((a * i * i) + (b * i) + (c)));
            if (fraction)
            {
                sequence += utiltiy.toFraction(terms[i - 1]) + ", ";
            }
            else
            {
                sequence += terms[i - 1] + ", ";
            }

        }
        sequence += " ?";
        problem.question = sequence;
        if (fraction)
        {
            problem.stringAnswer = utiltiy.toFraction((a * 5 * 5) + (b * 5) + (c));
            problem.isFraction = true;
        }
        else
        {
            problem.answer = (a * 5 * 5) + (b * 5) + (c);
        }

        return problem;
    }


    public utilities.Question nthTermGenerating(int a, int b, int c)
    {
        utilities.Question problem = new utilities.Question();
        var term = utiltiy.getRandom(1, 10);
        var nthTerm = "";
        var firstTerm = true;
        nthTerm += utiltiy.fixTerm(a, "n^2", firstTerm);
        if (a != 0)
        {
            firstTerm = false;
        }
        nthTerm += utiltiy.fixTerm(b, "n", firstTerm);
        if (b != 0)
        {
            firstTerm = false;
        }
        nthTerm += utiltiy.fixTerm(c, "", firstTerm);
        problem.question = " Find the " + term + utiltiy.ordinal(term) + " term given:nth term =  " + nthTerm + "";
        problem.answer = (a * term * term) + (b * term) + c;
        return problem;
    }
    public utilities.Question addingCoins(int coins)
    {
        utilities.Question problem = new utilities.Question();
        string[] coin = new string[] { "1c", "5c", "10c", "25c", "$1", "$2" };
        int[] value = new int[] { 1, 5, 10, 25, 100, 200 };
        int[] quantity = new int[] { 0, 0, 0, 0, 0, 0 };
        var total = 0;
        var coinsUsed = 0;
        var plural = "";
        for (int i = 0; i < coins; i++)
        {
            quantity[(int)Mathf.Floor((float)(random.NextDouble() * coin.Length))] += 1;
        }
        problem.question = "Add together: ";
        var singleValue = true;
        for (int i = 0; i < coin.Length; i++)
        {
            if (quantity[i] > 0)
            {
                if (quantity[i] > 1)
                {
                    plural = "'s";
                }
                else
                {
                    plural = "";
                }
                coinsUsed += quantity[i];
                if (coinsUsed == coins && !singleValue)
                {
                    problem.question += "and " + utiltiy.wordedNumber(quantity[i]) + " " + coin[i] + plural + ". ";
                }
                else
                {
                    problem.question += utiltiy.wordedNumber(quantity[i]) + " " + coin[i] + plural + ", ";
                }
                singleValue = false;
            }
            total += value[i] * quantity[i];
        }
        problem.question = problem.question.slice(0, -2);
        problem.question += ".";
        problem.stringAnswer = ((float)total / 100).toFixed(2);//utiltiy.toPounds(total);
        problem.before = "$"; //not sure if that will work

        return problem;
    }
    public utilities.Question countingCoins(int quantity)
    {
        utilities.Question problem = new utilities.Question();
        string[] coin = new string[] { "1c", "5c", "10c", "25c", "$1", "$2" };
        int[] value = new int[] { 1, 5, 10, 25, 100, 200 };
        int currentCoin = (int)Mathf.Floor((float)(random.NextDouble() * coin.Length));
        var total = quantity * value[currentCoin];
        problem.question = "How many " + coin[currentCoin] + " coins are in " + utiltiy.toPounds(total) + "?";
        problem.answer = quantity;
        return problem;
    }
    public utilities.Question speedDistTime(float speed, float time, int type)
    {
        utilities.Question problem = new utilities.Question();
        string distanceUnit = "m";
        string timeUnit = "s";
        if (random.NextDouble() < 0.5)
        {
            distanceUnit = "km";
            timeUnit = "h";
        }
        var speedUnit = distanceUnit + "/" + timeUnit;
        float distance = speed * time;
        problem.question = "";
        switch (type)
        {
            case 0:
                problem.question += "An object travels at " + speed + " " + speedUnit + " for " + time + " " + timeUnit + ".How far does it travel?";
                problem.stringAnswer = distance + "";
                problem.after = distanceUnit;
                break;
            case 1:
                problem.question += "An object travels at " + speed + " " + speedUnit + " for " + distance + " " + distanceUnit + ".How long did it take?";
                problem.stringAnswer = time + "";
                problem.after = timeUnit;
                break;
            case 2:
                problem.question += "An object travels " + distance + " " + distanceUnit + " in " + time + " " + timeUnit + ".What speed was it travelling?";
                problem.stringAnswer = speed + "";
                problem.after = speedUnit;
                break;
        }

        problem.question += "";
        return problem;
    }
    public utilities.Question powersAndRoots(float x, float a, float b)
    {
        utilities.Question problem = new utilities.Question();
        if (a == 0)
        {
            problem.question = x + a + "";
        }
        else if (b == 2 && a == 1)
        {
            problem.question = "sqrt: " + x;
        }
        else if (b == 3 && a == 1)
        {
            problem.question = "cbrt: " + x;
        }
        else if (b != 1)
        {
            problem.question = x + a + "/" + b + "";
        }
        else
        {
            problem.question = x + a + "";
        }
        if (a < 0 && x != 1)
        {
            x = utiltiy.roundError(Mathf.Pow(x, Mathf.Abs(a) / b));
            problem.stringAnswer = "1/" + x;

        }
        else
        {
            x = utiltiy.roundError(Mathf.Pow(x, a / b));
            problem.stringAnswer = x + "";
        }
        return problem;
    }


    public utilities.Question ordering(int length, bool currDecimal, bool negative, bool descending, float range)
    {
        var list = new float[length];
        utilities.Question problem = new utilities.Question();
        var sequence = "";
        int i = 0;
        for (i = 0; i < list.Length; i++)
        {
            list[i] = Mathf.Floor((float)(random.NextDouble() * range));
            if (currDecimal)
            {
                list[i] /= Mathf.Pow(10, utiltiy.getRandom(0, 2));
            }
            if (negative)
            {
                list[i] = -list[i];
            }
        }
        problem.question = "Write in ";
        if (descending)
        {
            problem.question += "descending";
        }
        else
        {
            problem.question += "ascending";
        }
        problem.question += " order: ";

        for (i = 0; i < list.Length - 1; i++)
        {
            problem.question += list[i] + ", ";
        }
        problem.question += list[i];
        if (descending)
        {
            Array.Sort(list);
            Array.Reverse(list);  //descending
        }
        else
        {
            Array.Sort(list); //ascending 
        }

        for (i = 0; i < list.Length - 1; i++)
        {
            sequence += list[i] + ", ";
        }
        sequence += list[i];
        problem.question += ".";
        problem.stringAnswer = "";
        for (i = 0; i < list.Length; i++)
        {
            problem.stringAnswer += list[i];
            if (i != list.Length - 1)
            {
                problem.stringAnswer += ",";
            }
        }
        problem.isOrdering = true;
        problem.isDad = true;
        return problem;
    }
    public utilities.Question oneStepEquations(int type, float x, float answer, bool inequality)
    {
        utilities.Question problem = new utilities.Question();
        var letter = utiltiy.letterPicker(-1, false); //i think thats right?
        string side1 = "";
        float side2 = 0;
        var symbol = "=";
        if (inequality)
        {
            switch (utiltiy.getRandom(0, 3))
            {
                case 0:
                    symbol = "<";
                    break;
                case 1:
                    symbol = "<=";
                    break;
                case 2:
                    symbol = ">";
                    break;
                case 3:
                    symbol = ">=";
                    break;
            }
            if (type == 7)
            {
                type = utiltiy.getRandom(0, 4);
            }
        }
        switch (type)
        {
            case 0:
                side1 = letter + " + " + x;
                side2 = x + answer;
                break;
            case 1:
                side1 = x + " + " + letter;
                side2 = x + answer;
                break;
            case 2:
                side1 = x + letter;
                side2 = x * answer;
                break;
            case 3:
                side1 = letter + " - " + x;
                side2 = answer - x;
                break;
            case 4:
                side1 = x + " - " + letter;
                side2 = x - answer;
                break;
            case 5:
                side1 = letter + "/" + x;
                side2 = answer / x;
                break;
            case 6:
                side1 = x + "/" + letter;
                side2 = x / answer;
                break;
            case 7:
                side1 = letter + x + "";
                side2 = Mathf.Pow(answer, x);
                break;
        }
        side2 = utiltiy.roundError(side2);
        problem.question = "Solve: ";
        if (random.NextDouble() < 0.5 || inequality)
        {
            problem.question += side1 + " " + symbol + " " + side2;
        }
        else
        {
            problem.question += side2 + " " + symbol + " " + side1;
        }
        answer = (int)utiltiy.roundError(answer);
        /*   if (!inequality)
           {
               problem.stringAnswer = letter + " " + symbol + " " + answer;
           }
           else
           {
               problem.answer = answer;
           } */
        problem.stringAnswer = "" + answer;
        problem.before = letter + " " + symbol;
        return problem;
    }
    public utilities.Question twoStepEquations(int type, float x, float y, float answer, bool inequality)
    {
        utilities.Question problem = new utilities.Question();
        var letter = utiltiy.letterPicker(-1, false); //again not sure
        string side1 = "";
        float side2 = 0;
        var symbol = "=";
        if (inequality)
        {
            switch (utiltiy.getRandom(0, 3))
            {
                case 0:
                    symbol = "<";
                    break;
                case 1:
                    symbol = "<=";
                    break;
                case 2:
                    symbol = ">";
                    break;
                case 3:
                    symbol = ">=";
                    break;
            }
            if (type > 6)
            {
                type = utiltiy.getRandom(0, 6);
            }
        }
        switch (type)
        {
            case 0:
                side1 = x + letter + " + " + y;
                side2 = answer * x + y;
                break;
            case 1:
                side1 = x + letter + " - " + y;
                side2 = answer * x - y;
                break;
            case 2:
                side1 = y + " + " + x + letter;
                side2 = answer * x + y;
                break;
            case 3:
                side1 = letter + "/" + x + " + " + y;
                side2 = answer / x + y;
                break;
            case 4:
                side1 = letter + "/" + x + " - " + y;
                side2 = answer / x - y;
                break;
            case 5:
                side1 = y + " + " + letter + "/" + x;
                side2 = answer / x + y;
                break;
            case 6:
                side1 = y + " - " + x + letter;
                side2 = -(answer * x - y);
                break;
            case 7:
                side1 = letter + x + " + " + y;
                side2 = (int)Mathf.Pow(answer, x) + y;
                break;
            case 8:
                side1 = letter + x + " - " + y;
                side2 = (int)Mathf.Pow(answer, x) - y;
                break;
        }
        side2 = (int)utiltiy.roundError(side2);
        problem.question = "Solve: ";
        if (random.NextDouble() < 0.5 || inequality)
        {
            problem.question += side1 + " " + symbol + " " + side2;
        }
        else
        {
            problem.question += side2 + " " + symbol + " " + side1;
        }
        answer = (int)utiltiy.roundError(answer);
        if (!inequality)
        {
            problem.stringAnswer = answer + "";
            problem.before = letter + " " + symbol;
        }
        else
        {
            problem.stringAnswer = answer.ToString();
        }
        return problem;
    }
    public utilities.Question threeStepEquations(float x, float y, float z, float answer, bool reversable, bool inequality)
    {
        utilities.Question problem = new utilities.Question();
        var letter = utiltiy.letterPicker(-1, false); //again not sure
        string side1 = "";
        string side2 = "";
        var symbol = "=";
        if (inequality)
        {
            switch (utiltiy.getRandom(0, 3))
            {
                case 0:
                    symbol = "<";
                    break;
                case 1:
                    symbol = "<=";
                    break;
                case 2:
                    symbol = ">";
                    break;
                case 3:
                    symbol = ">=";
                    break;
            }
        }
        if (reversable && random.NextDouble() < 0.5)
        {
            side1 = utiltiy.fixTerm(z, "", true) + utiltiy.fixTerm((x + y), letter, false);
        }
        else
        {
            side1 = utiltiy.fixTerm((x + y), letter, true) + utiltiy.fixTerm(z, "", false);
        }
        if (reversable && random.NextDouble() < 0.5)
        {
            side2 = utiltiy.fixTerm(utiltiy.roundError(x * answer + z), "", true) + utiltiy.fixTerm(y, letter, false);
        }
        else
        {
            side2 = utiltiy.fixTerm(y, letter, true) + utiltiy.fixTerm(utiltiy.roundError(x * answer + z), "", false);
        }
        problem.question = "Solve: ";
        if ((reversable && random.NextDouble() < 0.5) || inequality)
        {
            problem.question += side1 + " " + symbol + " " + side2;
        }
        else
        {
            problem.question += side2 + " " + symbol + " " + side1;
        }
        answer = (int)utiltiy.roundError(answer);
        if (!inequality)
        {
            problem.stringAnswer = answer + "";
            problem.before = letter + " " + symbol;
        }
        else
        {
            problem.stringAnswer = answer.ToString();
        }
        return problem;
    }
    public utilities.Question equationsWithBrackets(float x, float y, float z, float answer, bool reversable)
    {
        utilities.Question problem = new utilities.Question();
        var letter = utiltiy.letterPicker(-1, false); //again not sure
        string side1 = "";
        float side2 = 0;
        if (reversable)
        {
            side1 = utiltiy.fixTerm(x, "", true) + "(" + utiltiy.fixTerm(z, "", true) + utiltiy.fixTerm(y, letter, false) + ")";
        }
        else
        {
            side1 = utiltiy.fixTerm(x, "", true) + "(" + utiltiy.fixTerm(y, letter, true) + utiltiy.fixTerm(z, "", false) + ")";
        }
        side2 = utiltiy.roundError(x * (y * answer + z));
        problem.question = "Solve: ";
        if (random.NextDouble() < 0.5 && reversable)
        {
            problem.question += side2 + " = " + side1;
        }
        else
        {
            problem.question += side1 + " = " + side2;
        }
        answer = utiltiy.roundError(answer);
        problem.stringAnswer = answer + "";
        problem.before = letter + " = ";
        return problem;
    }
    public utilities.Question equationsWithBracketsBoth(float a, float b, float c, float d, float e, float f, float answer, bool reversable)
    {
        utilities.Question problem = new utilities.Question();
        var letter = utiltiy.letterPicker(-1, false);
        string side1 = "";
        string side2 = "";
        if (reversable)
        {
            side1 = utiltiy.fixTerm(a, "", true) + "(" + utiltiy.fixTerm(c, "", true) + utiltiy.fixTerm(b, letter, false) + ")";
        }
        else
        {
            side1 = utiltiy.fixTerm(a, "", true) + "(" + utiltiy.fixTerm(b, letter, true) + utiltiy.fixTerm(c, "", false) + ")";
        }
        side2 = utiltiy.fixTerm(d, "", true) + "(" + utiltiy.fixTerm(e, letter, true) + utiltiy.fixTerm(f, "", false) + ")";
        problem.question = "Solve: ";
        if (random.NextDouble() < 0.5 && reversable)
        {
            problem.question += side2 + " = " + side1;
        }
        else
        {
            problem.question += side1 + " = " + side2;
        }
        answer = utiltiy.roundError(answer);
        problem.stringAnswer = answer.ToString();
        problem.before = letter + " = ";
        return problem;
    }
    public utilities.Question numberBonds(int type, float bond, float x)
    {
        utilities.Question problem = new utilities.Question();
        switch (type)
        {
            case 0:
                problem.question = x + " + xxx = " + bond;
                break;
            case 1:
                problem.question = "xxx + " + x + " = " + bond;
                break;
            case 2:
                problem.question = bond + " - " + " xxx = " + x;
                break;
            case 3:
                problem.question = bond + " - " + x + " = xxx";
                break;
            case 4:
                problem.question = bond + "+" + x + "+ xxx = " + bond + "+" + bond;
                break;
            case 5:
                problem.question = bond + "+" + "xxx+" + x + " = " + bond + "+" + bond;
                break;
            case 6:
                problem.question = bond + "+" + bond + "-" + "xxx = " + bond + "+" + x;
                break;
            case 7:
                problem.question = bond + "+" + bond + "-" + x + " = " + bond + "+xxx";
                break;
        }
        problem.stringAnswer = utiltiy.roundError(bond - x).ToString();
        return problem;
    }
    public utilities.Question fourOpsFractions(string w1, float n1, float d1, string w2, float n2, float d2, string w3, float n3, float d3, string o1, string o2)
    {
        // w = whole, n = numerator, d = denominator, o = operation, a = answer
        utilities.Question problem = new utilities.Question();

        var f1 = w1 + " " + n1 + "" + "/" + d1;
        var f2 = w2 + " " + n2 + "" + "/" + d2;
        var f3 = w3 + " " + n3 + "" + "/" + d3;


        if (w1 == "0")
        {
            // n1 = d1 + n1;
        }
        else
        {
            n1 = int.Parse(w1) * d1 + n1;
        }
        if (w2 == "0")
        {
            //  n2 = d2 + n2;
        }
        else
        {
            n2 = int.Parse(w2) * d2 + n2;
        }
        if (w3 == "0")
        {
            //   n3 = d3 + n3;
        }
        else
        {
            n3 = int.Parse(w3) * d3 + n3;
        }


        float num = 0;
        float den = 0;
        if (o1 == "+")
        {
            num = n1 * d2 + n2 * d1;
            den = d1 * d2;
        }
        else if (o1 == "-")
        {
            num = n1 * d2 - n2 * d1;
            den = d1 * d2;
        }
        else if (o1 == "*")
        {
            num = n1 * n2;
            den = d1 * d2;
        }
        else if (o1 == "/")
        {
            num = n1 * d2;
            den = d1 * n2;
        }
        if (o2 == "+")
        {
            num = num * d3 + n3 * den;
            den *= d3;
        }
        else if (o2 == "-")
        {
            num = num * d3 - n3 * den;
            den *= d3;
        }
        else if (o2 == "*")
        {
            num = num * n3;
            den = den * d3;
        }
        else if (o2 == "/")
        {
            num = num * d3;
            den = den * n3;
        }
        problem.question = f1 + " " + o1 + " " + f2;
        if (o2 != "")
        { //not sure about that one cheif
            problem.question += " " + o2 + " " + f3;
        }
        var whole = Mathf.Floor(num / den);
        if (whole < 0)
        {
            whole++;
            if (whole != 0)
            {
                num = Mathf.Abs(num);
            }
        }
        num = num % den;
        var hcf = utiltiy.HCF(num, den);
        num /= hcf;
        den /= hcf;
        problem.stringAnswer = "";
        if (whole != 0)
        {

            problem.stringAnswer += whole;
        }
        if (whole != 0 && num != 0)
        {
            problem.isFraction = true;
            problem.isMixed = true;
            problem.stringAnswer += " ";
        }
        if (num != 0)
        {
            problem.isFraction = true;
            problem.stringAnswer += num + "/" + den;
        }
        if (whole == 0 && num == 0)
        {
            problem.stringAnswer += "0";
        }


        problem.isOrdering = true;
        return problem;
    }
    public utilities.Question mean(float[] data)
    {
        utilities.Question problem = new utilities.Question();
        float total = 0;
        for (int i = 0; i < data.Length; i++)
        {
            total += data[i];
        }
        var mean = total / data.Length;
        problem.question = "Find the mean of: ";

        for (int i = 0; i < data.Length; i++)
        {
            problem.question += data[i] + ", ";
        }
        problem.question = problem.question.slice(0, -2);
        problem.question += "";
        problem.answer = mean;
        return problem;
    }
    public utilities.Question median(float[] data)
    {
        utilities.Question problem = new utilities.Question();
        float median;
        problem.question = "Find the median of: ";
        for (int i = 0; i < data.Length; i++)
        {
            problem.question += data[i] + ", ";
        }
        problem.question = problem.question.slice(0, -2);
        problem.question += "";
        Array.Sort(data); //ascending 
        if (data.Length % 2 == 1)
        {
            median = data[(data.Length + 1) / 2 - 1];
        }
        else
        {
            median = utiltiy.roundError((data[data.Length / 2] + data[data.Length / 2 - 1]) / 2);
        }
        problem.answer = median;
        return problem;
    }
    public utilities.Question range(float[] data)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "Find the range of: ";
        for (int i = 0; i < data.Length; i++)
        {
            problem.question += data[i] + ", ";
        }
        problem.question = problem.question.slice(0, -2);
        problem.question += "";
        Array.Sort(data); //ascending
        var range = data[data.Length - 1] - data[0];
        problem.answer = range;
        return problem;
    }
    public utilities.Question mode(float[] data)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "Find the mode of: ";
        for (int i = 0; i < data.Length; i++)
        {
            problem.question += data[i] + ", ";
        }
        problem.question = problem.question.slice(0, -2);
        problem.question += "";
        List<float> mode = new List<float>();
        List<float> count = new List<float>();
        var maxFrequency = 1;
        var current = 0;
        for (int i = 0; i < data.Length; i++)
        {
            for (var j = 0; j < data.Length; j++)
            {
                if (data[i] == data[j])
                {
                    current++;
                }
            }
            count.Add(current);
            if (current > maxFrequency)
            {
                maxFrequency = current;
            }
            current = 0;
        }
        for (int i = 0; i < data.Length; i++)
        {
            if (count[i] == maxFrequency && count[i] > 1)
            {
                mode.Add(data[i]);
            }
        }
        if ((mode[0]) != 0)
        { //here it was NaN, not sure what to do here
            mode.Sort(); //ascending
        }
        else
        {
            mode.Sort(); //descending 
        }
        if (mode.Count == 0)
        {
            problem.stringAnswer = "no mode";
        }
        else
        {
            float currentMode = 0;
            problem.stringAnswer = "";
            for (int i = 0; i < mode.Count; i++)
            {
                if (mode[i] != currentMode)
                {
                    problem.stringAnswer += mode[i] + ",";
                    currentMode = mode[i];
                }
            }
            problem.stringAnswer = problem.stringAnswer.slice(0, -2); //this isnt right i dont think???
        }
        return problem;
    }

    public utilities.Question standardForm(utilities.Term x, utilities.Term y, string op)
    {
        utilities.Question problem = new utilities.Question();
        utilities.Term a = new utilities.Term();
        switch (op)
        {
            case "*":
                op = "*";
                a.co = x.co * y.co;
                a.Pow = x.Pow + y.Pow;
                break;
            case "/":
                op = "/";
                a.co = x.co / y.co;
                a.Pow = x.Pow - y.Pow;
                break;
            case "+":
                op = "+";
                a.co = x.co * Mathf.Pow(10, x.Pow) + y.co * Mathf.Pow(10, y.Pow);
                a.Pow = 0;
                break;
            case "-":
                op = "-";
                a.co = x.co * Mathf.Pow(10, x.Pow) - y.co * Mathf.Pow(10, y.Pow);
                a.Pow = 0;
                break;
        }
        problem.question = "(" + x.co + " * 10^" + x.Pow + ") " + op + " (" + y.co + " * 10^" + y.Pow + ")";
        while (a.co >= 10)
        {
            a.co /= 10;
            a.Pow++;
        }
        while (a.co < 1 && a.co > 0)
        {
            a.co *= 10;
            a.Pow--;
        }
        a.co = utiltiy.roundError(a.co);
        //problem.stringAnswer = (int)a.co + " * 10^" + (int)a.Pow + "";
        problem.stringAnswer = (int)a.co + "^" + (int)a.Pow;
        problem.isExponent = true;
        problem.isOrdering = true; //order matters 
        return problem;
    }
    public utilities.Question convertingStandardForm(utilities.Term x, int type)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "";
        switch (type)
        {
            case 0:
                problem.question += "Write " + utiltiy.roundError(x.co * Mathf.Pow(10, x.Pow)) + " in standard form.";
                checkCo(x);
                //  problem.stringAnswer = utiltiy.roundError(x.co) + " * 10^" + x.Pow + "";

                problem.inBetween = "* 10 ^";
                problem.isOrdering = true;

                problem.stringAnswer = utiltiy.roundError(x.co) + "," + x.Pow;
                break;
            case 1:
                problem.question += "Write " + utiltiy.roundError(x.co) + " * 10^" + x.Pow + " as an ordinary number.";
                checkCo(x);
                problem.stringAnswer = utiltiy.roundError(x.co * Mathf.Pow(10, x.Pow)).ToString();
                break;
            case 2:
                problem.question += "Write " + utiltiy.roundError(x.co) + " * 10^" + x.Pow + " in standard form.";
                checkCo(x);
                //problem.stringAnswer = utiltiy.roundError(x.co) + " * 10^" + x.Pow + "";
                problem.inBetween = "* 10 ^";
                problem.isOrdering = true;

                problem.stringAnswer = utiltiy.roundError(x.co) + "," + x.Pow;
                break;
        }
        problem.stringAnswer += "";
        return problem;
    }


    public utilities.Term checkCo(utilities.Term term)
    {
        while (term.co >= 10)
        {
            term.co /= 10;
            term.Pow++;
        }
        while (term.co < 1 && term.co > 0)
        {
            term.co *= 10;
            term.Pow--;
        }
        utiltiy.roundError(term.co);
        return term;
    }


    public utilities.Question convertingFractions(float num, float den, bool toMixed)
    {
        utilities.Question problem = new utilities.Question();
        var hcf = utiltiy.HCF(num, den);
        num /= hcf;
        den /= hcf;
        var improper = num + "/" + den;
        var mixed = Mathf.Floor(num / den) + " " + (num % den) + "/" + den;
        problem.isFraction = true;
        problem.isOrdering = true;

        if (toMixed)
        {
            problem.question = improper + " as a mixed number";
            problem.stringAnswer = mixed;
            problem.isMixed = true;
        }
        else
        {
            problem.question = mixed + " as an improper fraction";
            problem.stringAnswer = improper;
        }
        return problem;
    }
    public utilities.Question convertingMetricLength(float m, int from, int to, int power)
    {
        //0, 1, 2, 3 -> mm, cm, m, km
        utilities.Question problem = new utilities.Question();
        string[] unit = new string[] { "mm", "cm", "m", "km" };
        float cm = utiltiy.roundError(m * Mathf.Pow(100, power));
        float mm = utiltiy.roundError(cm * Mathf.Pow(10, power));
        float km = utiltiy.roundError(m / Mathf.Pow(1000, power));
        float[] value = new float[] { mm, cm, m, km };
        if (power > 1)
        {
            problem.question = "Convert " + value[from] + " " + unit[from] + "^" + power + " to " + unit[to] + "^" + power;
        }
        else
        {


            problem.question = "Convert " + value[from] + " " + unit[from] + " to " + unit[to];
        }

        switch (to)
        {
            case 0:
                problem.stringAnswer = mm.ToString();
                break;
            case 1:
                problem.stringAnswer = cm.ToString();
                break;
            case 2:
                problem.stringAnswer = m.ToString();
                break;
            case 3:
                problem.stringAnswer = km.ToString();
                break;
        }

        //problem.stringAnswer += " " + unit[to];
        problem.after = unit[to];
        if (power > 1)
        {
            problem.after += "^" + power; //this one might be bad 
            //problem.isExponent = true; 
        }
        //    problem.after = unit[to];
        return problem;
    }
    public utilities.Question convertingMetricWeight(float kg, int from, int to)
    {
        //0, 1, 2, 3 -> mg, g, kg, tonnes
        utilities.Question problem = new utilities.Question();
        string[] unit = new string[] { "mg", "g", "kg", "tonnes" };
        float g = utiltiy.roundError(kg * 1000);
        float mg = utiltiy.roundError(g * 1000);
        float tonnes = utiltiy.roundError(kg / 1000);
        float[] value = new float[] { mg, g, kg, tonnes };
        problem.question = "Convert " + value[from] + " " + unit[from] + " to " + unit[to];
        switch (to)
        {
            case 0:
                problem.stringAnswer = mg.ToString();
                break;
            case 1:
                problem.stringAnswer = g.ToString();
                break;
            case 2:
                problem.stringAnswer = kg.ToString();
                break;
            case 3:
                problem.stringAnswer = tonnes.ToString();
                break;
        }
        problem.after = unit[to];
        return problem;
    }
    public utilities.Question convertingMetricVolume(float l, int from, int to)
    {
        //0, 1, 2 -> l, cl, l
        utilities.Question problem = new utilities.Question();
        string[] unit = new string[] { "ml", "cl", "litres" };
        float cl = utiltiy.roundError(l * 100);
        float ml = utiltiy.roundError(cl * 10);
        float[] value = new float[] { ml, cl, l };
        problem.question = "Convert " + value[from] + " " + unit[from] + " to " + unit[to];
        switch (to)
        {
            case 0:
                problem.stringAnswer = ml.ToString();
                break;
            case 1:
                problem.stringAnswer = cl.ToString();
                break;
            case 2:
                problem.stringAnswer = l.ToString();
                break;
        }
        problem.after = unit[to];
        return problem;
    }
    public utilities.Question basicProbability(int type)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "";
        var noun = "";
        var outcomes = 0;
        switch (type)
        {
            case 0:
                var side = "heads";
                if (random.NextDouble() < 0.5)
                {
                    side = "tails";
                }
                problem.question += "A fair coin is flipped. What is the probability of getting " + side + "?";
                problem.stringAnswer = "1/2";
                break;
            case 1:
                problem.question += "A fair six sided dice is rolled. What is the probability of rolling a " + utiltiy.getRandom(1, 6) + "?";
                problem.stringAnswer = "1/6";
                break;
            case 2:
                var sides = 2 * utiltiy.getRandom(2, 6);
                var number = utiltiy.getRandom(2, sides);
                noun = "less";
                outcomes = number - 1;
                if (random.NextDouble() < 0.5)
                {
                    noun = "more";
                    outcomes = sides - number;
                }
                problem.question += "A fair " + sides + " sided spinner labelled 1 to " + sides + " is spun. What is the probability of spinning a number " + noun + " than " + number + "?";
                problem.stringAnswer = outcomes + "/" + sides;
                break;
            case 3:
                switch (utiltiy.getRandom(0, 4))
                {
                    case 0:
                        noun = "two heads";
                        outcomes = 1;
                        break;
                    case 1:
                        noun = "two tails";
                        outcomes = 1;
                        break;
                    case 2:
                        noun = "at least one heads";
                        outcomes = 3;
                        break;
                    case 3:
                        noun = "at least one tails";
                        outcomes = 3;
                        break;
                    case 4:
                        noun = "exactly one head and tails";
                        outcomes = 2;
                        break;
                }
                problem.question += "A fair coin is flipped twice. What is the probability of getting " + noun + "?";
                problem.stringAnswer = outcomes + "/4";
                break;
        }
        problem.question += "";
        problem.isFraction = true;
        return problem;
    }
    public utilities.Question expectedFrequency(float trials)
    {
        utilities.Question problem = new utilities.Question();
        var number = utiltiy.getRandom(1, 6);
        problem.question = "";
        problem.question += "A fair six sided dice is rolled " + trials + " times. How many times would you expect to roll a " + number + "?";
        problem.answer = Mathf.Round(trials / 6);
        problem.question += "";
        return problem;
    }


    public utilities.Question substitution(int type, bool negatives, int v1, int v2)
    {
        utilities.Question problem = new utilities.Question();
        var choice = utiltiy.getRandom(0, 21);
        string l1 = utiltiy.letterPicker(choice);
        string l2 = utiltiy.letterPicker(choice + 1);
        var x = utiltiy.getRandom(2, 10);
        var y = utiltiy.getRandom(1, 10);
        var z = utiltiy.getRandom(2, 10);
        if (negatives && random.NextDouble() < 0.5)
        {
            x = -x;
        }
        if (negatives && random.NextDouble() < 0.5)
        {
            y = -y;
        }
        var answer = 0;
        problem.question = "If " + l1 + " = " + v1;
        if (type > 4)
        {
            problem.question += " and " + l2 + " = " + v2;
        }
        switch (utiltiy.getRandom(0, 2))
        {
            case 0:
                problem.question += ", work out: ";
                break;
            case 1:
                problem.question += ", evaluate: ";
                break;
            case 2:
                problem.question += ", calculate: ";
                break;
        }
        switch (type)
        {
            case 0:
                if (random.NextDouble() < 0.5)
                {
                    problem.question += utiltiy.fixTerm(1, l1, true) + utiltiy.fixTerm(x, "", false);
                }
                else
                {
                    problem.question += utiltiy.fixTerm(x, "", true) + utiltiy.fixTerm(1, l1, false);
                }
                answer = v1 + x;
                break;
            case 1:
                if (x < v1)
                {
                    problem.question += utiltiy.fixTerm(1, l1, true) + utiltiy.fixTerm(-x, "", false);
                    answer = v1 - x;
                }
                else
                {
                    problem.question += utiltiy.fixTerm(x, "", true) + utiltiy.fixTerm(-1, l1, false);
                    answer = x - v1;
                }
                break;
            case 2:
                problem.question += utiltiy.fixTerm(x, l1, true);
                answer = v1 * x;
                break;
            case 3:
                if (random.NextDouble() < 0.5)
                {
                    problem.question += utiltiy.fixTerm(y, "", true) + utiltiy.fixTerm(x, l1, false);
                }
                else
                {
                    problem.question += utiltiy.fixTerm(x, l1, true) + utiltiy.fixTerm(y, "", false);
                }
                answer = v1 * x + y;
                break;
            case 4:
                if (v1 % x == 0)
                {
                    problem.question += utiltiy.fixTerm(1, l1, true) + "/" + utiltiy.fixTerm(x, "", true);
                    answer = v1 / x;
                }
                else
                {
                    x *= v1;
                    problem.question += utiltiy.fixTerm(x, "", true) + "/" + utiltiy.fixTerm(1, l1, true);
                    answer = x / v1;
                }
                break;
            case 5:
                if (v1 * x < v2 * y)
                {
                    problem.question += utiltiy.fixTerm(x, l1, true) + utiltiy.fixTerm(y, l2, false);
                    answer = v1 * x + v2 * y;
                }
                else
                {
                    problem.question += utiltiy.fixTerm(x, l1, true) + utiltiy.fixTerm(-y, l2, false);
                    answer = v1 * x - v2 * y;
                }
                break;
            case 6:
                if (random.NextDouble() < 0.5)
                {
                    problem.question += utiltiy.fixTerm(1, l1, true) + "^2" + utiltiy.fixTerm(x, l2, false);
                    answer = v1 * v1 + x * v2;
                }
                else
                {
                    problem.question += utiltiy.fixTerm(1, l1 + l2, true) + "^2";
                    answer = v1 * v2 * v2;
                }
                break;
            case 7:
                if (random.NextDouble() < 0.5)
                {
                    problem.question += utiltiy.fixTerm(1, l1 + l2, true) + utiltiy.fixTerm(x, "", false);
                    answer = v1 * v2 + x;
                }
                else
                {
                    problem.question += utiltiy.fixTerm(1, x + l1 + l2, true);
                    answer = x * v1 * v2;
                }
                break;
            case 8:
                switch (utiltiy.getRandom(0, 2))
                {
                    case 0:
                        problem.question += z + "(" + utiltiy.fixTerm(x, l1, true) + utiltiy.fixTerm(y, l2, false) + ")";
                        answer = z * (v1 * x + v2 * y);
                        break;
                    case 1:
                        problem.question += z + "(" + utiltiy.fixTerm(1, l1, true) + "^2" + utiltiy.fixTerm(y, l2, false) + ")";
                        answer = z * (v1 * v1 + v2 * y);
                        break;
                    case 2:
                        problem.question += utiltiy.fixTerm(1, l1, true) + "(" + utiltiy.fixTerm(x, l1, true) + utiltiy.fixTerm(y, l2, false) + ")";
                        answer = v1 * (x * v1 + y * v2);
                        break;
                }
                break;
        }
        problem.answer = utiltiy.roundError(answer);
        return problem;
    }

    public class Noun
    {
        public string name;
        public float cost;
        public float newQuantity;
        public float quantity;
        public string total;
        public string newTotal;
    }
    public utilities.Question unitaryMethod(float cost, float quantity, float newQuantity)
    {
        utilities.Question problem = new utilities.Question();
        Noun noun = new Noun();
        noun.name = utiltiy.itemPicker("small", -1); //i think thats it 
        noun.cost = cost;
        noun.quantity = quantity;
        noun.newQuantity = newQuantity;
        noun.total = utiltiy.toPounds(noun.cost * noun.quantity);
        noun.newTotal = utiltiy.toPounds(noun.cost * noun.newQuantity);
        problem.question = "" + noun.quantity + " " + noun.name + "s costs " + noun.total + ".";
        problem.question += "How much would " + noun.newQuantity + " " + noun.name + "s cost?";
        problem.stringAnswer = ((noun.cost * noun.newQuantity) / 100).toFixed(2);//noun.newTotal;
        return problem;
    }
    public utilities.Question difference(float a, float b)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "Find the difference between " + a + " and " + b + ".";
        problem.answer = utiltiy.roundError(Mathf.Abs(a - b));
        return problem;
    }
    public utilities.Question changingTemperatures(float original, float change)
    {
        utilities.Question problem = new utilities.Question();
        var newTemp = utiltiy.roundError(original + change);
        var time = utiltiy.getRandom(1, 12);
        string verb = "";
        if (change > 0)
        {
            verb = "increased";
        }
        else
        {
            verb = "decreased";
        }
        problem.question = "At " + time + " o'clock the temperature was " + original + " degree C. ";
        problem.question += "The temperature " + verb + " by " + change + " degree C. What is the new temperature?";
        problem.stringAnswer = newTemp.ToString();
        problem.after = " degree C";
        return problem;
    }


    public utilities.Question polygonSides(int maxPol)
    {
        utilities.Question problem = new utilities.Question();
        List<utilities.Polygon> polygon = new List<utilities.Polygon>();
        polygon.Add(new utilities.Polygon(" name:a triangle, sides: 3"));
        polygon.Add(new utilities.Polygon(" name:an equilateral triangle, sides: 3"));
        polygon.Add(new utilities.Polygon(" name:an isosceles triangle, sides: 3"));
        polygon.Add(new utilities.Polygon(" name:a scalene triangle, sides: 3"));
        polygon.Add(new utilities.Polygon(" name:a quadrilateral, sides: 4"));
        polygon.Add(new utilities.Polygon(" name:a square, sides: 4"));
        polygon.Add(new utilities.Polygon(" name:a rectangle, sides: 4"));
        polygon.Add(new utilities.Polygon(" name:a parallelogram, sides: 4"));
        polygon.Add(new utilities.Polygon(" name:a rhombus, sides: 4"));
        polygon.Add(new utilities.Polygon(" name:a trapezium, sides: 4"));
        polygon.Add(new utilities.Polygon(" name:a kite, sides: 4"));
        polygon.Add(new utilities.Polygon(" name:a pentagon, sides: 5"));
        polygon.Add(new utilities.Polygon(" name:a hexagon, sides: 6"));
        polygon.Add(new utilities.Polygon(" name:a heptagon, sides: 7"));
        polygon.Add(new utilities.Polygon(" name:an octagon, sides: 8"));
        polygon.Add(new utilities.Polygon(" name:a nonagon, sides: 9"));
        polygon.Add(new utilities.Polygon(" name:a decagon, sides: 10"));
        polygon.Add(new utilities.Polygon(" name:a hendecagon, sides: 11"));
        polygon.Add(new utilities.Polygon(" name:a dodecagon, sides: 12"));
        polygon.Add(new utilities.Polygon(" name:a tridecagon, sides: 13"));
        polygon.Add(new utilities.Polygon(" name:a tetradecagon, sides: 14"));
        polygon.Add(new utilities.Polygon(" name:a pentadecagon, sides: 15"));
        polygon.Add(new utilities.Polygon(" name:a hexadecagon, sides: 16"));
        polygon.Add(new utilities.Polygon(" name:a heptadecagon, sides: 17"));
        polygon.Add(new utilities.Polygon(" name:a octadecagon, sides: 18"));
        polygon.Add(new utilities.Polygon(" name:a enneadecagon, sides: 19"));
        polygon.Add(new utilities.Polygon(" name:an icosagon, sides: 20"));
        polygon.Add(new utilities.Polygon(" name:a triacontagon, sides: 30"));
        polygon.Add(new utilities.Polygon(" name:a tetracontagon, sides: 40"));
        polygon.Add(new utilities.Polygon(" name:a pentacontagon, sides: 50"));
        polygon.Add(new utilities.Polygon(" name:a hexacontagon, sides: 60"));
        polygon.Add(new utilities.Polygon(" name:a heptacontagon, sides: 70"));
        polygon.Add(new utilities.Polygon(" name:an octacontagon, sides: 80"));
        polygon.Add(new utilities.Polygon(" name:an enneacontagon, sides: 90"));
        polygon.Add(new utilities.Polygon(" name:a hectogon, sides: 100"));
        polygon.Add(new utilities.Polygon(" name:a chiliagon, sides: 1000"));
        polygon.Add(new utilities.Polygon(" name:a myriagon, sides: 10000"));
        polygon.Add(new utilities.Polygon(" name:a megagon, sides: 1000000"));
        int temp = utiltiy.getRandom(0, maxPol);
        problem.question = "How many sides does " + polygon[temp].name + " have?";
        problem.answer = polygon[temp].sides;
        return problem;
    }
    public utilities.Question singleBrackets(int type, bool negatives)
    {
        utilities.Question problem = new utilities.Question();
        var choice = utiltiy.getRandom(0, 21);
        var x = utiltiy.letterPicker(choice);
        var y = utiltiy.letterPicker(choice + 1);
        var z = utiltiy.letterPicker(choice + 2);
        var a = utiltiy.getRandom(2, 10);
        var b = utiltiy.getRandom(2, 10);
        var c = utiltiy.getRandom(2, 10);
        var Pow = utiltiy.getRandom(2, 5);
        var Pow2 = utiltiy.getRandom(2, 5);
        var sign = " + ";
        var sign2 = " + ";
        if (toss())
        {
            sign = " - ";
        }
        if (toss())
        {
            sign2 = " - ";
        }
        var exp = "";
        var expansion = "";
        if (type > 5)
        {
            type = 5;
        }
        if (negatives)
        {
            a = -a;
        }
        switch (type)
        {
            case 0:
                if (toss())
                {
                    exp = a + "(" + x + sign + b + ")";
                    expansion = a + x + sign + (a * b);
                }
                else
                {
                    exp = a + "(" + b + sign + x + ")";
                    expansion = (a * b) + sign + a + x;
                }
                break;
            case 1:
                if (toss())
                {
                    exp = a + "(" + c + x + sign + b + ")";
                    expansion = (a * c) + x + sign + (a * b);
                }
                else
                {
                    exp = a + "(" + b + sign + c + x + ")";
                    expansion = (a * b) + sign + (a * c) + x;
                }
                break;
            case 2:
                exp = a + "(" + x + sign + y + sign2 + b + ")";
                expansion = a + x + sign + a + y + sign2 + (a * b);
                break;
            case 3:
                if (toss())
                {
                    exp = x + "(" + x + sign + a + ")";
                    expansion = x + "^2 " + sign + a + x;
                }
                else
                {
                    exp = x + "(" + a + sign + x + ")";
                    expansion = a + x + sign + x + "^2";
                }
                break;
            case 4:
                if (toss())
                {
                    exp = a + x + "(" + b + x + sign + c + y + ")";
                    expansion = (a * b) + x + "^2 " + sign + (a * c) + x + y;
                }
                else
                {
                    exp = a + x + "(" + c + y + sign + b + x + ")";
                    expansion = (a * c) + x + y + sign + (a * b) + x + "^2";
                }
                break;
            case 5:
                if (toss())
                {
                    exp = a + x + Pow + "" + "(" + b + x + Pow2 + "" + sign + c + y + z + ")";
                    expansion = (a * b) + x + (Pow + Pow2) + "" + sign + (a * c) + x + (Pow) + "" + y + z;
                }
                else
                {
                    exp = a + x + Pow + "" + "(" + c + y + z + sign + b + x + Pow2 + "" + ")";
                    expansion = (a * c) + x + (Pow) + "" + y + z + sign + (a * b) + x + (Pow + Pow2) + "";
                }
                break;
        }
        problem.question = "Expand:" + exp;
        problem.isExpression = true;
        problem.stringAnswer = expansion;
        return problem;
    }

    public bool toss()
    {
        return true;
    }



    public utilities.Question expandSimplifySingleBrackets(int type, int max)
    {
        utilities.Question problem = new utilities.Question();
        var choice = utiltiy.getRandom(0, 21);
        var x = utiltiy.letterPicker(choice);
        var y = utiltiy.letterPicker(choice + 1);
        if (toss())
        {
            y = "";
        }
        if (toss() && type > 2)
        {
            x += "^2";
        }
        if (toss() && y != "" && type > 3)
        {
            y += "^2";
        }
        var a = utiltiy.getRandom(2, max);
        var b = utiltiy.getRandom(2, max);
        var c = utiltiy.getRandom(2, max);
        var d = utiltiy.getRandom(2, max);
        var e = utiltiy.getRandom(2, max);
        var f = utiltiy.getRandom(2, max);
        var exp = "";
        var expansion = "";
        if (type > 6)
        {
            type = 6;
        }
        switch (type)
        {
            case 0:
                exp = a + "(" + x + " + " + b + ") + " + c + "(" + x + " + " + d + ")";
                expansion = (a + c) + x + " + " + (a * b + c * d);
                break;
            case 1:
                exp = a + "(" + b + x + " + " + c + ") + " + d + "(" + e + x + " + " + f + ")";
                expansion = (a * b + d * e) + x + " + " + (a * c + d * f);
                break;
            case 2:
                exp = a + "(" + x + " + " + b + y + ") + " + c + "(" + x + " - " + d + y + ")";
                if (a * b < c * d)
                {
                    expansion = (a + c) + x + " - " + Mathf.Abs(a * b - c * d) + y;
                }
                else
                {
                    expansion = (a + c) + x + " + " + (a * b - c * d) + y;
                }
                break;
            case 3:
                exp = a + "(" + x + " - " + b + y + ") + " + c + "(" + x + " - " + d + y + ")";
                expansion = (a + c) + x + " - " + (a * b + c * d) + y;
                break;
            case 4:
                while (a * b <= d * e)
                {
                    b++;
                }
                exp = a + "(" + b + x + " + " + c + y + ") - " + d + "(" + e + x + " + " + f + y + ")";
                if (a * c < d * f)
                {
                    expansion = (a * b - d * e) + x + " - " + Mathf.Abs(a * c - d * f) + y;
                }
                else
                {
                    expansion = (a * b - d * e) + x + " + " + (a * c - d * f) + y;
                }
                break;
            case 5:
                while (a * b <= d * e)
                {
                    b++;
                }
                exp = a + "(" + b + x + " + " + c + y + ") - " + d + "(" + e + x + " - " + f + y + ")";
                expansion = (a * b - d * e) + x + " + " + (a * c + d * f) + y;
                break;
            case 6:
                while (a * b <= d * e)
                {
                    b++;
                }
                exp = a + "(" + b + x + " - " + c + y + ") - " + d + "(" + e + x + " - " + f + y + ")";
                if (a * c < d * f)
                {
                    expansion = (a * b - d * e) + x + " + " + (d * f - a * c) + y;
                }
                else
                {
                    expansion = (a * b - d * e) + x + " - " + Mathf.Abs(d * f - a * c) + y;
                }
                break;
        }
        problem.question = "Expand and simplify:" + exp;
        problem.stringAnswer = expansion;
        problem.isExpression = true;
        return problem;
    }
    public utilities.Question interchangingFDP(float x, float y, float amount, string type)
    {
        utilities.Question problem = new utilities.Question();
        var givenAmount = utiltiy.roundError(x * amount);
        var newAmount = utiltiy.roundError(y * amount);
        problem.question = "";
        switch (type)
        {
            case "random":
                if (toss())
                {
                    problem.question += "If " + utiltiy.toFraction(x) + " of an amount is " + givenAmount + ",";
                    problem.question += " what is " + utiltiy.toPercentage(y) + "?";
                    problem.after = "%";
                }
                else
                {
                    problem.question += "If " + utiltiy.toPercentage(x) + " of an amount is " + givenAmount + ",";
                    problem.question += " what is " + utiltiy.toFraction(y) + "?";

                }
                break;
            case "fraction":
                problem.question += "If " + utiltiy.toFraction(x) + " of an amount is " + givenAmount + ",";
                problem.question += " what is " + utiltiy.toFraction(y) + "?";
                break;
            case "percentage":
                problem.question += "If " + utiltiy.toPercentage(x) + " of an amount is " + givenAmount + ",";
                problem.question += " what is " + utiltiy.toPercentage(y) + "?";
                break;
        }
        problem.question += "";
        problem.stringAnswer = newAmount.ToString();
        return problem;
    }
    public utilities.Question fibonacci(int f0, int f1, int given1, int given2, int find)
    {
        utilities.Question problem = new utilities.Question();
        List<int> s = new List<int>();
        // s[0] = f0;
        // s[1] = f1;
        s.Add(f0);
        s.Add(f1);

        for (int i = 2; i < Mathf.Max(given2, find) + 1; i++)
        {
            s.Add((int)utiltiy.roundError(s[i - 1] + s[i - 2]));
        }
        problem.question = "A fibonacci sequence begins: ";
        for (int i = 0; i < s.Count; i++)
        {
            if (i == given1 || i == given2)
            {
                problem.question += s[i] + ", ";
            }
            else
            {
                problem.question += "? ";
            }
        }
        problem.question += "...";
        problem.question += "Find the " + (find + 1) + utiltiy.ordinal(find + 1) + " term.";
        problem.stringAnswer = s[find] + "";
        return problem;
    }


    public utilities.Question geometricSequence(float a, float r, float given1, float given2, int find)
    {
        utilities.Question problem = new utilities.Question();
        List<float> s = new List<float>();
        s.Add(a);
        //  s[0] = a;
        for (int i = 1; i < Mathf.Max(given2, find) + 1; i++)
        {
            s.Add(utiltiy.roundError(s[i - 1] * r));
        }
        problem.question = "A geometric sequence begins: ";
        for (int i = 0; i < s.Count; i++)
        {
            if (i == given1 || i == given2)
            {
                problem.question += s[i] + ", ";
            }
            else
            {
                problem.question += "?, ";
            }
        }
        problem.question += "...";
        problem.question += "Find the " + (find + 1) + utiltiy.ordinal(find + 1) + " term.";
        problem.stringAnswer = s[find] + "";
        return problem;
    }
    public utilities.Question convertingTime(int from, int to, float x)
    {
        string[] units = new string[] { "seconds", "minutes", "hours", "days", "weeks" };
        int[] mutliplier = new int[] { 1, 60, 60, 24, 7 };
        utilities.Question problem = new utilities.Question();
        problem.question = "Convert " + x + " " + units[from] + " to " + units[to];
        if (to > from)
        {
            for (int i = from + 1; i <= to; i++)
            {
                x /= mutliplier[i];
            }
        }
        if (from > to)
        {
            for (int i = to + 1; i <= from; i++)
            {
                x *= mutliplier[i];
            }
        }
        problem.stringAnswer = utiltiy.roundError(x) + "";
        problem.after = "" + units[to];
        return problem;
    }
    public utilities.Question gradientFromTwoPoints(float x1, float y1, float x2, float y2)
    {
        utilities.Question problem = new utilities.Question();
        var num = y2 - y1;
        var den = x2 - x1;
        var gradient = num / den;
        string answer = gradient.ToString();
        if (gradient != Mathf.Round(gradient))
        {
            var hcf = utiltiy.HCF(num, den);
            num /= hcf;
            den /= hcf;
            if (num < 0 && den < 0)
            {
                num *= -1;
                den *= -1;
            }
            if (num > 0 && den < 0)
            {
                num *= -1;
                den *= -1;
            }
            answer = num + "/" + den;
        }
        problem.question = "A straight line passes through the points (" + x1 + ", " + y1 + ") and (" + x2 + ", " + y2 + ").";
        problem.question += "Find the gradient of the line.";
        problem.stringAnswer = answer;
        problem.isFraction = true;
        problem.isOrdering = true;
        return problem;
    }
    public utilities.Question midpointFromTwoPoints(float x1, float y1, float x2, float y2)
    {
        utilities.Question problem = new utilities.Question();
        var ym = utiltiy.roundError((y2 + y1) / 2);
        var xm = utiltiy.roundError((x2 + x1) / 2);
        problem.question = "A line segment passes from (" + x1 + ", " + y1 + ") to (" + x2 + ", " + y2 + ").";
        problem.question += "Find the midpoint of the line segment.";
        problem.stringAnswer = xm + "," + ym;

        problem.inBetween = ",";

        problem.isOrdering = true;
        return problem;
    }
    public utilities.Question completingSquare(float a, float b, float c)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "Complete the square for: ";
        if (a != 1)
        {
            problem.question += a;
        }
        problem.question += "x^2 ";
        if (b > 0)
        {
            problem.question += " + " + b;
        }
        else if (b != 0)
        {
            problem.question += " - " + Mathf.Abs(b);
        }
        problem.question += "x";
        if (c > 0)
        {
            problem.question += " + " + c;
        }
        else if (c != 0)
        {
            problem.question += " - " + Mathf.Abs(c);
        }
        b /= a;
        var xCo = b / 2;
        var constant = c - (((b / 2) * (b / 2)) * a);
        problem.stringAnswer = "";
        if (a != 1)
        {
            problem.stringAnswer = a + "(x";
        }
        else
        {
            problem.stringAnswer = "(x";
        }
        if (xCo > 0)
        {
            problem.stringAnswer += " + " + xCo + ")^2";
        }
        else
        {
            problem.stringAnswer += " - " + Mathf.Abs(xCo) + ")^2";
        }
        if (constant > 0)
        {
            problem.stringAnswer += " + " + constant;
        }
        else
        {
            problem.stringAnswer += " - " + Mathf.Abs(constant);
        }

        problem.isExpression = true;
        return problem;
    }
    public utilities.Question turningPointToQuadratic(float xTurn, float yTurn, bool min)
    {
        utilities.Question problem = new utilities.Question();
        var type = min ? "minimum" : "maximum";
        problem.question = "The " + type + " turning point of a quadratic curve is ";
        problem.question += "(" + xTurn + ", " + yTurn + ").";
        if (type == "minimum")
        {
            problem.question += "Write an equation for this in the form:y = x^2 + ax + b.";
        }
        else
        {
            problem.question += "Write an equation for this in the form:y = -x^2 + ax + b.";
        }
        var a = (type == "minimum") ? 1 : -1;
        var b = -2 * xTurn * a;
        var c = xTurn * xTurn * a + yTurn;
        problem.before = "y = ";

        if (a == -1)
        {
            problem.stringAnswer += "-";
        }
        problem.stringAnswer += "x^2 ";
        if (b > 0)
        {
            problem.stringAnswer += " + " + b;
        }
        else if (b != 0)
        {
            problem.stringAnswer += " - " + Mathf.Abs(b);
        }
        problem.stringAnswer += "x";
        if (c > 0)
        {
            problem.stringAnswer += " + " + c;
        }
        else if (c != 0)
        {
            problem.stringAnswer += " - " + Mathf.Abs(c);
        }
        problem.isExpression = true;
        return problem;
    }
    public utilities.Question factoriseExpandQuadratics(float a, float b, float c, float d, bool expanding)
    {
        utilities.Question problem = new utilities.Question();
        var let = utiltiy.letterPicker(-1, false);
        var exp = "";
        float[] co = new float[3]
; co[0] = a * c;
        co[1] = a * d + b * c;
        co[2] = b * d;
        if (Mathf.Abs(co[0]) > 1)
        {
            exp += co[0] + let + "^2";
        }
        else
        {
            if (co[0] < 0)
            {
                exp += "-";
            }
            exp += let + "^2";
        }
        if (co[1] < 0)
        {
            if (co[1] < -1)
            {
                exp += " - " + Mathf.Abs(co[1]) + let;
            }
            else
            {
                exp += " - " + let;
            }
        }
        else
        {
            if (co[1] > 1)
            {
                exp += " + " + co[1] + let;
            }
            else if (co[1] != 0)
            {
                exp += " + " + let;
            }
        }
        if (co[2] < 0)
        {
            exp += " - " + Mathf.Abs(co[2]);
        }
        else
        {
            exp += " + " + co[2];
        }

        var fact = "(";
        if (a < 0)
        {
            fact += "-";
        }
        if (Mathf.Abs(a) > 1)
        {
            fact += Mathf.Abs(a);
        }
        fact += let;
        if (b > 0)
        {
            fact += " + " + b;
        }
        else
        {
            fact += " - " + Mathf.Abs(b);
        }
        fact += ")(";
        if (c < 0)
        {
            fact += "-";
        }
        if (Mathf.Abs(c) > 1)
        {
            fact += Mathf.Abs(c);
        }
        fact += let;
        if (d > 0)
        {
            fact += " + " + d;
        }
        else
        {
            fact += " - " + Mathf.Abs(d);
        }
        fact += ")";
        if (!expanding)
        {
            problem.question = "Factorise:" + exp;
            problem.stringAnswer = fact;
        }
        else
        {
            problem.question = "Expand and simplify:" + fact;
            problem.stringAnswer = exp;
        }
        problem.isExpression = true;
        return problem;
    }
    public utilities.Question indexLawMultiply(float currBase, float ex1, float shift1, float ex2, float shift2)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "Simplify: ";
        var exp = Mathf.Pow(currBase, shift1) + "^" + (ex1) + "";
        exp += " * ";
        exp += Mathf.Pow(currBase, shift2) + "^" + (ex2) + "";
        var sol = currBase + "^" + ((ex1 * shift1) + (ex2 * shift2));
        problem.question += exp;
        problem.stringAnswer = sol.ToString();
        problem.isExponent = true;
        problem.isOrdering = true;
        return problem;
    }
    public utilities.Question indexLawDivide(float currBase, float ex1, float shift1, float ex2, float shift2)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "Simplify: ";
        var exp = Mathf.Pow(currBase, shift1) + "^" + (ex1) + "";
        exp += " / ";
        exp += Mathf.Pow(currBase, shift2) + "^" + (ex2) + "";
        var sol = currBase + "^" + ((ex1 * shift1) - (ex2 * shift2));
        problem.question += exp;
        problem.stringAnswer = sol.ToString();
        problem.isExponent = true;
        problem.isOrdering = true;
        return problem;
    }
    public utilities.Question indexLawPowerOfPower(float currBase, float ex1, float shift1, float ex2)
    {
        utilities.Question problem = new utilities.Question();
        problem.question = "Write ";
        var exp = "(" + Mathf.Pow(currBase, shift1) + "^" + (ex1) + ") ^" + ex2 + "";
        exp += " as a Power of " + currBase + ".";
        var sol = currBase + "^" + ((ex1 * shift1) * ex2);
        problem.question += exp;
        problem.stringAnswer = sol.ToString();
        problem.isExponent = true;
        problem.isOrdering = true;
        return problem;
    }


    public utilities.Question combiningRatios(int max)
    {
        utilities.Question problem = new utilities.Question();
        var seed = utiltiy.getRandom(0, 30);
        var x = utiltiy.letterPicker(seed);
        var y = utiltiy.letterPicker(seed + 1);
        var z = utiltiy.letterPicker(seed + 2);
        var a = utiltiy.getRandom(1, max);
        var c = utiltiy.getRandom(1, max);
        float d = 0;
        float b = 0;
        do
        {
            b = utiltiy.getRandom(1, max);
            d = utiltiy.getRandom(1, max);
        } while (a == b || c == d);
        problem.question = "The ratio of " + x + " to " + y + " is " + a + " : " + b + ".The ratio of " + y + " to " + z + " is " + c + " : " + d + ".";
        problem.question += "Find the ratio " + x + " : " + y + " : " + z + " in its simplest form.";
        var hcf = utiltiy.HCF(utiltiy.HCF(a * c, b * c), b * d);
        problem.stringAnswer = a * c / hcf + "," + b * c / hcf + "," + b * d / hcf;
        problem.isOrdering = true;
        problem.isRatio = true;
        problem.inBetween = ":";

        return problem;
    }

    public utilities.Question exponentsExpandedForm(int currBase, int exp)
    {
        utilities.Question problem = new utilities.Question();
        for (int i = 0; i < exp; i++)
        {
            problem.question += currBase;
            if (i != exp - 1)
            {
                problem.question += " * ";
            }
        }
        problem.stringAnswer = currBase + "^" + exp;
        problem.isExponent = true;
        problem.isOrdering = true;
        return problem;
    }


}

//these could all be added as problem solving and word problems, you need to get them from differentiator tho 
