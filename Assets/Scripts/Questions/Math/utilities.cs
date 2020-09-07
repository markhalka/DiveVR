using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class utilities
{
    public class Person
    {
        public string gender;
        public string name;
        public string subject;
        public string personObject;
        public string owner;

    }

    public class Polygon
    {
        public string name;
        public int sides;

        public Polygon(string toParse)
        {
            int first = toParse.IndexOf(":");
            string n = toParse.Substring(first + 1, toParse.IndexOf(",") - first - 1);
            Debug.Log(n + " name");

            string secondPart = toParse.Substring(toParse.IndexOf(","));
            string s = secondPart.Substring(secondPart.IndexOf(":") + 1);
            s.Trim();
            Debug.Log(s + " sides");

            name = n;
            sides = int.Parse(s);
        }

        public Polygon()
        {

        }
    }


    public string[] multipleChoiceNumber(string answer)
    {
        float currAnswer = 0;

        if (!float.TryParse(answer, out currAnswer))
        {
            Debug.LogError("could not parse");
            return new string[] { "error, please report the question" };
        }

        string[] other = new string[] { getRandom(-currAnswer, currAnswer - 1).ToString(), getCloseNumber(answer), getCloseNumber(answer) };



        int answerIndex = getRandom(0, 3);
        string[] output = new string[4];
        float[] included = new float[] { 0, 0, 0 };
        for (int i = 0; i < 4; i++)
        {
            if (i == answerIndex)
            {
                output[i] = answer;
            }
            else
            {
                int index = getRandom(0, 2);
                while (included[index] != 0)
                {
                    index = getRandom(0, 2);
                }
                included[index] = 1;
                output[i] = other[index];
            }
        }
        return output;
    }

    public string[] multipleChoiceFraction(string answer)
    {


        string[] other = new string[3];

        for (int i = 0; i < 3; i++)
        {
            string curr = getCloseFraction(answer);
            while (other.Contains(curr))
            {
                curr = getCloseFraction(answer);
            }
            other[i] = curr;
        }



        int answerIndex = getRandom(0, 3);
        string[] output = new string[4];
        float[] included = new float[] { 0, 0, 0 };
        for (int i = 0; i < 4; i++)
        {
            if (i == answerIndex)
            {
                output[i] = answer;
            }
            else
            {
                int index = getRandom(0, 2);
                while (included[index] != 0)
                {
                    index = getRandom(0, 2);
                }
                included[index] = 1;
                output[i] = other[index];
            }
        }
        return output;
    }

    public string getCloseFraction(string answer)
    {
        int index = answer.IndexOf('/');
        int num = int.Parse(answer.Substring(0, index));
        int denom = int.Parse(answer.Substring(index + 1));
        string output = "";
        bool negatvie = answer.Contains('-');
        switch (getRandom(0, 4))
        {
            case 0: //change denom, get a close fraction in num
                int newDenum = getRandom(1, 3);
                denom *= newDenum;
                int newNum = getRandom(-5, 5);
                if (newNum == 0)
                {
                    newNum = 1;
                }
                num += newNum;
                return num + "/" + denom;

                break;
            case 1: //have differnet denom
                string denum2 = getCloseNumber(denom.ToString());
                return num + "/" + denum2;
                break;
            case 2: //have differnt num
                string num2 = getCloseNumber(num.ToString());
                return num2 + "/" + denom;
                break;
            case 3: //have totatly random fraction
                int a = getRandom(-20, 20);
                int b = getRandom(-50, 50);

                string temp = a + "/" + b;
                if (temp == answer)
                {
                    a++;
                    temp = a + "/" + b;

                }
                return temp;
                break;
            case 4: //change the sing 
                if (negatvie)
                {
                    output = output.Substring(1); //just get rid of the negative
                }
                else
                {
                    output.Insert(0, "-"); //just put a negatvie sign infront of it 

                }
                return output;
                break;
        }
        return output;
    }

    public string[] multipleChoiceTime(string answer)
    {
        //so you can change the am or pm
        //you can generate a random time
        //you can have a close hour and a close minute 
        string[] output = new string[4];
        for (int i = 0; i < 3; i++)
        {
            string curr = getCloseTime(answer);


            while (output.Contains(curr))
            {


                curr = getCloseTime(answer);

            }
            output[i] = curr;
        }

        output[3] = answer;
        output.Shuffle();
        return output;

    }
    string getCloseTime(string time)
    {
        bool am = time.Contains("AM");
        DateTime currTime = DateTime.Parse(time);
        int min = currTime.Minute;
        int hour = currTime.Hour;
        string output = "";
        int temp = getRandom(0, 2);

        switch (temp)
        {
            case 0://change the am and pm

                if (am)
                {
                    output = time.Replace("AM", "PM");
                }
                else
                {
                    output = time.Replace("PM", "AM");
                }
                Debug.Log("0 " + output);
                break;

            case 1: //have a close minute

                int change = getRandom(-10, 10);
                currTime = currTime.AddMinutes(change);

                output = currTime.ToString("hh:mm tt");

                break;
            case 2: //get a close hour 

                change = getRandom(-10, 10);
                currTime = currTime.AddHours(change);
                output = currTime.ToString("hh:mm tt");

                break;

        }

        return output;
    }



    public string getCloseNumber(string number)
    {
        string output = "";
        //first close number
        float currNumber = float.Parse(number);
        float multiplier = 0;

        System.Text.StringBuilder numberBuilder = new System.Text.StringBuilder(number);

        if (toss())
        {
            if (Mathf.Abs(currNumber) < 10)
            {
                multiplier = currNumber * 2;
            }
            else if (Mathf.Abs(currNumber) < 20)
            {
                multiplier = currNumber * 0.5f;
            }
            else
            {
                multiplier = currNumber * 0.1f;
            }
            //its going to be close in value


            float nextNumber = 0;
            do
            {
                nextNumber = getRandom(currNumber - multiplier, currNumber + multiplier);
            } while (nextNumber == currNumber);

            return nextNumber.ToString();
        }
        else
        {
            //close in apperance
            //you can either: switch 2 numbers, change one number
            //if there is a decimal you can move the decimal
            bool hasLength = false;
            bool hasDeciaml = false;
            //    bool wasChanged = false;

            if (number.Contains("."))
            {
                hasDeciaml = true;
            }
            if (number.Length > 2)
            {
                hasLength = true;
            }

            while (true)
            {
                int curr = getRandom(0, 3);
                switch (curr)
                {
                    case 1: //swtich
                        if (!hasLength)
                            continue;
                        int pos1 = getRandom(0, number.Length - 1);
                        int pos2 = getRandom(0, number.Length - 2);
                        if (pos1 == pos2)
                            pos2++;

                        string possString = SwapCharacters(number, pos1, pos2);
                        if (possString == number || possString[0] == '0')
                        {
                            continue;
                        }
                        else
                        {
                            return possString;
                        }
                    case 2: //replace
                        int replace = getRandom(0, 9);
                        int replaceIndex = getRandom(0, number.Length - 1);
                        if (replaceIndex == 0 && number.Length > 1)
                        {
                            if (replace == 0)
                                replace++;
                        }

                        numberBuilder[replaceIndex] = char.Parse(replace.ToString());
                        if (numberBuilder.ToString() == number)
                            continue;
                        //remove all leading zeros
                        int leadingZeroCount = 0;
                        for (int i = 0; i < numberBuilder.Length; i++)
                        {
                            if (numberBuilder[i] == '0')
                            {
                                leadingZeroCount++;
                            }
                        }
                        string numberBuilderString = numberBuilder.ToString().Substring(leadingZeroCount);
                        return numberBuilderString;

                    case 3: //move decimal
                        if (!hasDeciaml)
                            continue;
                        int decimalIndex = getRandom(1, number.Length - 1);

                        int decimalArrIndex = number.IndexOf('.');
                        if (decimalIndex == decimalArrIndex)
                        {
                            if (decimalIndex == number.Length - 1)
                            {
                                decimalIndex--;
                            }
                            else
                            {
                                decimalIndex++;
                            }
                        }

                        return SwapCharacters(number, decimalArrIndex, decimalIndex);


                }

            }

        }
    }

    string SwapCharacters(string value, int position1, int position2)
    {
        //
        // Swaps characters in a string.
        //

        char[] array = value.ToCharArray(); // Get characters
        char temp = array[position1]; // Get temporary copy of character
        array[position1] = array[position2]; // Assign element
        array[position2] = temp; // Assign element
        return new string(array); // Return string
    }





    public class Question
    {
        public int topicIndex;
        public string question;
        public float answer;
        public string stringAnswer;
        public string typedAnswer;
        public int layout;
        public bool isOrdering;
        public bool multipleChoice;
        public bool isFraction;
        public bool isExponent;
        public bool isExpression;
        public bool isRatio;
        public bool isMixed;
        public bool isRomam;
        public bool isDad;
        public bool isShape;
        public bool isTime;
        public bool isClickable;

        public string before;
        public string after;
        public string inBetween;

        public List<string> choices;

        public Question()
        {
            topicIndex = 0;
            question = "";
            stringAnswer = "";
            before = "";
            after = "";
            inBetween = "";

            isClickable = false;
            isOrdering = false;
            multipleChoice = false;
            isFraction = false;
            isExponent = false;
            isExpression = false;
            isRatio = false;
            isMixed = false;
            isRomam = false;
            isDad = false;
            isShape = false;
            isTime = false;

            choices = new List<string>();


            layout = 0;

        }

        private float roundError(float answer)
        {
            float newAnswer = Mathf.Round(answer * 100000) / 100000; ;
            // Debug.Log(answer + " " + newAnswer + " round error");

            return newAnswer;
        }




        public void setStringAnswer() //here, find all numbers and user roundError on them to make them nice 
        {

            if (isOrdering)
                return;
            if (answer.ToString() != "" && stringAnswer == "")
            {
                stringAnswer = answer.ToString();

            }
            else if (stringAnswer != "")
            {
                string currNumber = "";
                string output = "";
                int lastIndex = 0;
                for (int i = 0; i < stringAnswer.Length; i++)
                {
                    if (char.IsNumber(stringAnswer[i]) || stringAnswer[i] == '.')
                    {
                        currNumber += stringAnswer[i];

                    }
                    else
                    {
                        //found the longest number, parse it 

                        if (currNumber != "")
                        {
                            output += roundError(float.Parse(currNumber));
                            currNumber = "";
                        }



                        output += stringAnswer[i];

                        //     lastIndex = i;
                    }
                }
                if (currNumber != "")
                    output += roundError(float.Parse(currNumber));
                //  output += stringAnswer.Substring(lastIndex);
                stringAnswer = output;

            }


        }
    }


    public class Term
    {
        public float co;
        public float Pow;

    }
    //for these guys have overloaded functions 

    private Dictionary<char, int> CharValues = null;

    // Convert Roman numerals to an integer.
    public int RomanToArabic(string roman)
    {
        // Initialize the letter map.
        if (CharValues == null)
        {
            CharValues = new Dictionary<char, int>();
            CharValues.Add('I', 1);
            CharValues.Add('V', 5);
            CharValues.Add('X', 10);
            CharValues.Add('L', 50);
            CharValues.Add('C', 100);
            CharValues.Add('D', 500);
            CharValues.Add('M', 1000);
        }

        if (roman.Length == 0) return 0;
        roman = roman.ToUpper();

        // See if the number begins with (.
        if (roman[0] == '(')
        {
            // Find the closing parenthesis.
            int pos = roman.LastIndexOf(')');

            // Get the value inside the parentheses.
            string part1 = roman.Substring(1, pos - 1);
            string part2 = roman.Substring(pos + 1);
            return 1000 * RomanToArabic(part1) + RomanToArabic(part2);
        }

        // The number doesn't begin with (.
        // Convert the letters' values.
        int total = 0;
        int last_value = 0;
        for (int i = roman.Length - 1; i >= 0; i--)
        {
            int new_value = CharValues[roman[i]];

            // See if we should add or subtract.
            if (new_value < last_value)
                total -= new_value;
            else
            {
                total += new_value;
                last_value = new_value;
            }
        }

        // Return the result.
        return total;
    }



    public string getNumberWithLength(int length, int decimalLength)
    {
        string num = "";
        for (int j = 0; j < length; j++)
        {
            string next = getRandom(0, 9).ToString();
            while (num.Contains(next))
            {
                next = getRandom(0, 9).ToString();
            }
            if (j == 0 && next == "0")
            {
                next = "1";
            }
            num += next;
        }
        Debug.LogError("parsing: " + num);
        float tempNum = float.Parse(num) / Mathf.Pow(10, getRandom(0, decimalLength));
        num = tempNum.ToString();

        Debug.LogError("returning: " + num);
        return num;
    }

    private string[] ThouLetters = { "", "M", "MM", "MMM" };
    private string[] HundLetters =
        { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
    private string[] TensLetters =
        { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
    private string[] OnesLetters =
        { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };

    // Convert Roman numerals to an integer.
    public string ArabicToRoman(int arabic)
    {
        // See if it's >= 4000.
        if (arabic >= 4000)
        {
            // Use parentheses.
            int thou = arabic / 1000;
            arabic %= 1000;
            return "(" + ArabicToRoman(thou) + ")" +
                ArabicToRoman(arabic);
        }

        // Otherwise process the letters.
        string result = "";

        // Pull out thousands.
        int num;
        num = arabic / 1000;
        result += ThouLetters[num];
        arabic %= 1000;

        // Handle hundreds.
        num = arabic / 100;
        result += HundLetters[num];
        arabic %= 100;

        // Handle tens.
        num = arabic / 10;
        result += TensLetters[num];
        arabic %= 10;

        // Handle ones.
        result += OnesLetters[arabic];

        return result;
    }


    #region calculatorShit
    public double Calculate(string input)
    {
        try { return double.Parse(GetExpression(input)); }
        catch (Exception) { return Counting(GetExpression(input)); }

    }

    private string GetExpression(string input)
    {
        string output = string.Empty;
        string fun = string.Empty;
        Stack<char> operStack = new Stack<char>();
        char k = ' '; string p = "";
        for (int i = 0; i < input.Length; i++)
        {
            if (IsOperator(input[i]) || Char.IsDigit(input[i]))
            {
                if (k == ' ')
                    k = input[i];
                else
                    if (input[i] == '-' && !Char.IsDigit(k))
                    p += " 0 ";
                k = input[i];
            }
            p += input[i];
        }
        input = p;
        for (int i = 0; i < input.Length; i++)
        {
            if (IsDelimeter(input[i]))
                continue;
            if (Char.IsDigit(input[i]))
            {
                while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                {
                    output += input[i];
                    i++;
                    if (i == input.Length) break;
                }
                output += " ";
                i--;
            }
            else
                if (IsOperator(input[i]))
            {
                if (input[i] == '(')
                    operStack.Push(input[i]);
                else if (input[i] == ')')
                {
                    char s = operStack.Pop();
                    while (s != '(')
                    {
                        output += s.ToString() + ' ';
                        s = operStack.Pop();
                    }
                }
                else
                {
                    if (operStack.Count > 0)
                        if (GetPriority(input[i]) <= GetPriority(operStack.Peek()))
                            output += operStack.Pop().ToString() + " ";

                    operStack.Push(char.Parse(input[i].ToString()));

                }
            }
            else if (input[i] == '\u03C0')
                output += " \u03C0 ";
            else if (input[i] == 'e')
                output += " e ";
            else
            {
                fun = String.Empty;
                while (input[i] != '(')
                {
                    fun += input[i];
                    i++;
                    if (i == input.Length) break;
                }
                i++;
                if (IsFunction(fun))
                {
                    string param = string.Empty;
                    while (input[i] != ')')
                    {
                        param += input[i];
                        i++;
                        if (i == input.Length) break;
                    }
                    float d;
                    try { d = float.Parse(param); }
                    catch (Exception) { d = Counting(GetExpression(param)); }
                    output += doFunc(fun, d);
                }
            }
        }
        while (operStack.Count > 0)
            output += operStack.Pop() + " ";

        return output;
    }

    private float Counting(string input)
    {
        float result = 0;
        float b = 0;
        Stack<float> temp = new Stack<float>();
        try { return float.Parse(input); }
        catch (Exception)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsDigit(input[i]))
                {
                    string a = string.Empty;

                    while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                    {
                        a += input[i];
                        i++;
                        if (i == input.Length) break;
                    }
                    temp.Push(float.Parse(a));
                    i--;
                }
                else if (input[i] == '\u03C0')
                    temp.Push(Mathf.PI);
                else if (input[i] == 'e')
                    temp.Push(Mathf.Exp(1));
                else if (IsOperator(input[i]))
                {
                    float a = temp.Pop();
                    try
                    { b = temp.Pop(); }
                    catch (Exception) { b = 0; }

                    switch (input[i])
                    {
                        case '!': result = factorial((int)a); break;
                        case 'P': result = factorial((int)b) / factorial((int)(b - a)); break;
                        case 'C': result = factorial((int)b) / (factorial((int)a) * factorial((int)(b - a))); break;
                        case '^': result = Mathf.Pow((float)b, (float)a); break;
                        case '%': result = b % a; break;
                        case '+': result = b + a; break;
                        case '-': result = b - a; break;
                        case '*': result = b * a; break;
                        case '/': if (a == 0) throw new DividedByZeroException(); else result = b / a; break;
                    }
                    temp.Push(result);
                }
            }
            try { return temp.Peek(); }
            catch (Exception) { throw new SyntaxException(); }

        }

    }
    static private bool IsDelimeter(char c)
    {
        if ((" =".IndexOf(c) != -1))
            return true;
        return false;
    }
    static private bool IsOperator(char с)
    {
        if (("+-/*^()PC!%".IndexOf(с) != -1))
            return true;
        return false;
    }
    static private bool IsFunction(string s)
    {
        string[] func = { "sin", "cos", "tg", "asin", "acos", "atg", "sqrt", "ln", "lg" };
        if (Array.Exists(func, e => e == s))
            return true;
        return false;
    }
    private string doFunc(string fun, float param)
    {
        switch (fun)
        {
            case "cos": return Mathf.Cos(param).ToString();
            case "sin": return Mathf.Sin(param).ToString();
            case "tg": if (Mathf.Abs(param % (2 * Mathf.PI)) == (Mathf.PI / 2)) throw new TgException(param); else return Mathf.Tan(param).ToString();
            case "asin": if (param < -1 || param > 1) throw new ArcSinCosException(param); else return Mathf.Asin(param).ToString();
            case "acos": if (param < -1 || param > 1) throw new ArcSinCosException(param); else return Mathf.Acos(param).ToString();
            case "atg": return Mathf.Atan(param).ToString();
            case "sqrt": if (param < 0) throw new SqrtException(param); else return Mathf.Sqrt(param).ToString();
            case "ln": if (param <= 0) throw new LogException(param); else return Mathf.Log(param).ToString();
            case "lg": if (param <= 0) throw new LogException(param); else return Mathf.Log10(param).ToString();
            default: return "";
        }
    }
    private byte GetPriority(char s)
    {
        switch (s)
        {
            case '(': return 0;
            case ')': return 1;
            case '+': return 2;
            case '-': return 3;
            case '!': return 4;
            case '%': return 4;
            case '*': return 4;
            case '/': return 4;
            case '^': return 5;
            default: return 4;
        }
    }


    public class MyException : Exception
    {
        public string type;
    }
    public class NegativeFactorialException : MyException
    {
        public NegativeFactorialException(int x)
        {
            this.type = "Math error";
            Debug.Log("Factorial(" + x + ") does not exsists");

        }
    }
    public class TgException : MyException
    {
        public TgException(double x)
        {
            this.type = "Math error";
            Debug.Log("Tg(" + x + ") does not exsists");

        }
    }
    public class SqrtException : MyException
    {
        public SqrtException(double x)
        {
            this.type = "Math error";
            Debug.Log("Sqrt(" + x + ") does not exsists");

        }
    }
    public class DividedByZeroException : MyException
    {
        public DividedByZeroException()
        {
            this.type = "Math error";
            Debug.Log("Division by zero is impossible");

        }
    }
    public class LogException : MyException
    {
        public LogException(double x)
        {
            this.type = "Math error";
            Debug.Log("Log(" + x + ") does not exsists");

        }
    }
    public class SyntaxException : MyException
    {
        public SyntaxException()
        {
            this.type = "Syntax error";
            Debug.Log("You made a mistake");
        }
    }
    public class ArcSinCosException : MyException
    {
        public ArcSinCosException(double x)
        {
            this.type = "Math error";
            Debug.Log("Acos(or Asin) (" + x + ") does not exsists");
        }
    }
    #endregion
    //calculator shit  -----------------------------------------







    public string wordedNumber(int n)
    {
        string[] wordedNumber = new string[]{"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten",
                "eleven", "twelve", "thirteen", "forteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen", "twenty" };
        return wordedNumber[n];
    }

    public string townPicker(int n)
    {
        string[] town = new string[]{"Appleby", "Barcombe", "Bromwich", "Cullfield", "Faversham", "Gillamoor", "Glossop",
                "Gramsby", "Helmfirth", "Holbeck", "Ironhaven", "Kirkwall", "Langdale", "Monmouth",
                "Murkwell", "Northbury", "Perlshaw", "Westray", "Westray", "Woodpine" };
        if (n < 0)
        {
            n = getRandom(0, town.Length - 1);
        }
        if (n >= town.Length)
        {
            n %= town.Length;
        }
        return town[n];
    }
    public Person namePicker(int n)
    {
        string[] name = new string[]{"Jonny", "Onene", "Marcia", "Becky", "Nick", "Phoebe", "Aleema", "Raheem", "Noor", "David",
                "Amanda", "Nicola", "Marek", "Maral", "Rajuia", "Donatella", "Annasara", "Sky", "Natalia", "Heben", "Sara",
                "Solomon", "Ebenezer", "Robinder", "Zofia", "Kelly", "Wisal", "Ferial", "Connor", "Dean", "Creflo",
                "Raheem", "Sultan", "Paulina", "Boguslawa", "Michael", "Hanadi", "Fiza", "Arron", "Umar", "Alixe",
                "Musab", "Safia", "Ivanilsa", "Ionut", "Simon", "Shanzah", "Raphael", "Zulqarnain", "Kieren", "Shareen",
                "Mustafa", "Yad", "Rishikesh", "Adeeba", "Frank", "Maria", "Dawid", "Dominik", "Sulaimaan", "Ghadi", "Ayoub" };
        string[] gender = new string[]{"M", "M", "F", "F", "M", "F", "F", "M", "F", "M", "F", "F", "M", "F", "F", "F", "F", "F",
                "F", "F", "F", "M", "M", "M", "F", "F", "F", "F", "M", "M", "M", "M", "M", "F", "F", "M", "F", "F", "M", "M", "F",
                "M", "F", "F", "M", "M", "F", "M", "M", "M", "F", "M", "M", "M", "F", "M", "F", "M", "M", "M", "F", "M" };
        if (n < 0)
        {
            n = getRandom(0, name.Length - 1);
        }
        if (n >= name.Length)
        {
            n %= name.Length;
        }
        Person person = new Person();
        person.name = name[n];
        person.gender = gender[n];
        if (person.gender == "M")
        {
            person.subject = "he";
            person.personObject = "him";
            person.owner = "his";
        }
        else
        {
            person.subject = "she";
            person.personObject = "her";
            person.owner = "her";
        }
        return person;
    }

    public Person namePicker()
    {
        string[] name = new string[]{"Jonny", "Onene", "Marcia", "Becky", "Nick", "Phoebe", "Aleema", "Raheem", "Noor", "David",
                "Amanda", "Nicola", "Marek", "Maral", "Rajuia", "Donatella", "Annasara", "Sky", "Natalia", "Heben", "Sara",
                "Solomon", "Ebenezer", "Robinder", "Zofia", "Kelly", "Wisal", "Ferial", "Connor", "Dean", "Creflo",
                "Raheem", "Sultan", "Paulina", "Boguslawa", "Michael", "Hanadi", "Fiza", "Arron", "Umar", "Alixe",
                "Musab", "Safia", "Ivanilsa", "Ionut", "Simon", "Shanzah", "Raphael", "Zulqarnain", "Kieren", "Shareen",
                "Mustafa", "Yad", "Rishikesh", "Adeeba", "Frank", "Maria", "Dawid", "Dominik", "Sulaimaan", "Ghadi", "Ayoub" };
        string[] gender = new string[]{"M", "M", "F", "F", "M", "F", "F", "M", "F", "M", "F", "F", "M", "F", "F", "F", "F", "F",
                "F", "F", "F", "M", "M", "M", "F", "F", "F", "F", "M", "M", "M", "M", "M", "F", "F", "M", "F", "F", "M", "M", "F",
                "M", "F", "F", "M", "M", "F", "M", "M", "M", "F", "M", "M", "M", "F", "M", "F", "M", "M", "M", "F", "M" };

        int n = getRandom(0, name.Length - 1);

        if (n >= name.Length)
        {
            n %= name.Length;
        }
        Person person = new Person();
        person.name = name[n];
        person.gender = gender[n];
        if (person.gender == "M")
        {
            person.subject = "he";
            person.personObject = "him";
            person.owner = "his";
        }
        else
        {
            person.subject = "she";
            person.personObject = "her";
            person.owner = "her";
        }
        return person;
    }


    public string colourPicker(int n)
    {
        var colour = new string[] { "red", "blue", "yellow", "green", "orange", "purple", "pink", "black", "white", "brown" };
        if (n < 0)
        {
            n = getRandom(0, colour.Length - 1);
        }
        if (n >= colour.Length)
        {
            n %= colour.Length;
        }
        return colour[n];
    }
    public string colourPicker()
    {
        var colour = new string[] { "red", "blue", "yellow", "green", "orange", "purple", "pink", "black", "white", "brown" };


        int n = getRandom(0, colour.Length - 1);

        if (n >= colour.Length)
        {
            n %= colour.Length;
        }
        return colour[n];
    }
    public string dayPicker(int n)
    {
        var day = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        if (n < 0)
        {
            n = getRandom(0, day.Length - 1);
        }
        if (n >= day.Length)
        {
            n %= day.Length;
        }
        return day[n];
    }

    public string monthPicker(int n)
    {
        string[] month = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        if (n < 0)
        {
            n = getRandom(0, month.Length - 1);
        }
        if (n >= month.Length)
        {
            n %= month.Length;
        }
        return month[n];
    }

    public string animalPicker(int n)
    {
        var animals = new string[] { "sheep", "goat", "cow", "chicken", "rooster", "horse" };
        if (n < 0)
        {
            n = getRandom(0, animals.Length - 1);
        }
        if (n >= animals.Length)
        {
            n %= animals.Length;
        }
        return animals[n];
    }

    public string zooAnimalPicker(int n)
    {
        var animals = new string[] { "tiger", "monkey", "sloth", "bear", "elephant", "zebra", "penguin" };
        if (n < 0)
        {
            n = getRandom(0, animals.Length - 1);
        }
        if (n >= animals.Length)
        {
            n %= animals.Length;
        }
        return animals[n];
    }

    public string fruitPicker(int n)
    {
        var fruit = new string[] { "apple", "pear", "banana", "plum", "orange", "lemon", "lime", "nectarine", "melon" };
        if (n < 0)
        {
            n = getRandom(0, fruit.Length - 1);
        }
        if (n >= fruit.Length)
        {
            n %= fruit.Length;
        }
        return fruit[n];
    }


    public string fruitPicker()
    {
        var fruit = new string[] { "apple", "pear", "banana", "plum", "orange", "lemon", "lime", "nectarine", "melon" };


        int n = getRandom(0, fruit.Length - 1);

        if (n >= fruit.Length)
        {
            n %= fruit.Length;
        }
        return fruit[n];
    }

    public string itemPicker(string cost, int n)
    {
        string[] item = new string[0];
        switch (cost)
        {
            case "large":
                item = new string[] { "cooker", "television", "fridge", "computer", "mobile phone", "laptop", "dishwasher", "washing machine" };
                break;
            case "small":
                item = new string[] { "pencil", "ruler", "pen", "rubber", "chocolate bar", "sweet" };
                break;
        }
        if (n < 0)
        {
            n = getRandom(0, item.Length - 1);
        }
        if (n >= item.Length)
        {
            n %= item.Length;
        }
        return item[n];
    }

    public string itemPicker(string cost)
    {
        string[] item = new string[0];
        //just make it random 

        switch (cost)
        {
            case "large":
                item = new string[] { "cooker", "television", "fridge", "computer", "mobile phone", "laptop", "dishwasher", "washing machine" };
                break;
            case "small":
                item = new string[] { "pencil", "ruler", "pen", "rubber", "chocolate bar", "sweet" };
                break;
        }


        int n = getRandom(0, item.Length - 1);

        if (n >= item.Length)
        {
            n %= item.Length;
        }
        return item[n];
    }

    public string shapePicker3D(int n)
    {
        string[] sides = new string[] { "cube", "sphere", "square based pyramid", "cone", "cylinder", "hexagonal based pyramid", "pyramid", "pentagaonal prism", "hexagonal prism", "prism", "pentagonal based pyramid" }; //add the rest 
        return sides[n];
    }

    public string shapeNetPicker(int n)
    {
        string[] nets = new string[] { "cone", "cube", "cylinder", "square based pyramid", "pyramid", "triangular based pyramid" };
        return nets[n];
    }

    public string triangleSideName(int n)
    {
        string[] sides = new string[] { "Equilateral", "Isosceles", "Right", "Isosceles", "Righ isosceles", "Equilateral" };
        return sides[n];
    }


    public string triangleAngleName(int n)
    {
        string[] angles = new string[] { "Right", "Acute", "Obtuse", };
        return angles[n];
    }


    public string LineSystemName(int n)
    {
        string[] lines = new string[] { "Intersecting", "Parallel", "Perpendicular" };
        return lines[n];
    }

    public string LineName(int n)
    {
        string[] angles = new string[] { "Complimentary", "Adjacent", "Suplimentary", "Vertical", "Alternate interior", "Alternate exterior", "Corresponding" };
        return angles[n];
    }

    public string AngleName(int n)
    {
        string[] angles = new string[] { "Acute", "Right", "Obtuse", "Straight line", "Relfex" };
        return angles[n];
    }

    public string ShapeName(int n)
    {
        string[] shapes = new string[] { "square", "rhombus", "triangle", "rectangle", "trapezoid", "right angled triangle", "rhombus", "kite", "circle" };
        return shapes[n];
    }

    public string letterPicker(int n, bool capitalLetter)
    {
        string[] capital = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "M", "N", "P", "Q", "R", "T", "U", "V", "W", "X", "Y" };
        string[] lowercase = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "j", "k", "m", "n", "p", "q", "r", "t", "u", "v", "w", "x", "y" };
        if (n < 0)
        {
            n = getRandom(0, capital.Length - 1);

        }
        if (n >= capital.Length)
        {
            n %= capital.Length;
        }
        if (capitalLetter)
        {
            return capital[n];
        }
        else
        {
            return lowercase[n];
        }
    }

    public string letterPicker()
    {
        bool capitalLetter = false;
        int n = 0;
        string[] capital = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "M", "N", "P", "Q", "R", "T", "U", "V", "W", "X", "Y" };
        string[] lowercase = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "j", "k", "m", "n", "p", "q", "r", "t", "u", "v", "w", "x", "y" };
        if (n < 0)
        {
            n = getRandom(0, capital.Length - 1);

        }
        if (n >= capital.Length)
        {
            n %= capital.Length;
        }
        if (capitalLetter)
        {
            return capital[n];
        }
        else
        {
            return lowercase[n];
        }
    }

    public string letterPicker(int n)
    {
        string[] lowercase = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "j", "k", "m", "n", "p", "q", "r", "t", "u", "v", "w", "x", "y" };
        if (n < 0)
        {
            n = getRandom(0, lowercase.Length - 1);

        }
        if (n >= lowercase.Length)
        {
            n %= lowercase.Length;
        }

        return lowercase[n];
    }

    public string unitPicker(string type)
    {
        string[] unit = new string[0];
        switch (type)
        {
            case "Length":
                unit = new string[] { "mm", "cm", "m", "km" };
                break;
            case "weight":
                unit = new string[] { "mg", "g", "kg" };
                break;
            case "volume":
                unit = new string[] { "ml", "l" };
                break;
        }
        return unit[getRandom(0, unit.Length - 1)];
    }

    public Polygon polygonPicker(int sides)
    {
        var name = new string[] { "triangle", "quadrilateral", "pentagon", "hexagon", "heptagon", "octagon", "nonagon", "decagon" };
        if (sides < 0)
        {
            sides = getRandom(0, name.Length - 1);
        }
        if (sides >= name.Length)
        {
            sides %= name.Length;
        }
        var polygon = new Polygon();

        polygon.name = name[sides];
        polygon.sides = sides + 3;
        return polygon;
    }


    public string placeValuePicker(int x)
    {
        var positivePlaceValue = new string[] { "one", "ten", "hundred", "thousand", "ten thousand", "hundred thousand", "million", "ten million", "hundred million", "billion" };

        var negativePlaceValue = new string[] { "tenths", "hundreths", "thousandths", "ten thousandths", "hundred thousandths", "millionths" };


        if (x < 0)
        {
            return negativePlaceValue[(int)Mathf.Abs(x)];
        }
        else
        {
            return positivePlaceValue[x];
        }
    }

    /*    public string showDate() {
            var weekDays = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            var months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            DateTime date = DateTime.Today;
            var text = date.getDate() + ordinal(date.getDate()) + " " + months[date.getMonth()] + " " + date.getFullYear();
            return text;
        } */
    public string ordinal(int n)
    {
        var x = "";
        if (n % 10 == 1 && n != 11)
        {
            x += "st";
        }
        else if (n % 10 == 2 && n != 12)
        {
            x += "nd";
        }
        else if (n % 10 == 3 && n != 13)
        {
            x += "rd";
        }
        else
        {
            x += "th";
        }
        return x;
    }
    public string capitalFirst(string str)
    {
        //return str[0].toUpperCase() + str.slice(1);
        return str;
    }

    public float toDegrees(float angle)
    {
        return angle * (180 / Mathf.PI);
    }
    public float toRadians(float angle)
    {
        return angle * (Mathf.PI / 180);
    }
    public string toFraction(float n)
    {
        float num = n * 1000000;
        float den = 1000000;
        float hcf = HCF(num, den);
        return (num / hcf).ToString() + "/" + (den / hcf).ToString();
    }
    public string toPercentage(float n)
    {
        return roundError(n * 100) + "%";
    }
    public string toPounds(float n)
    {
        var pounds = Mathf.Floor(n / 100);
        var pence = (n % 100);
        /* if (pence < 10) {
             pence = "0" + pence;
         }*/
        return "$" + pounds + "." + pence;
    }
    System.Random random = new System.Random();

    public int getRandom(int min, int max)
    {
        return (int)Mathf.Floor((float)(random.NextDouble() * (max - min + 1) + min));
    }

    public float getRandom(float min, float max)
    {
        return (int)Mathf.Floor((float)(random.NextDouble() * (max - min + 1) + min));
    }

    public string getRandomMember(string[] array)
    {
        return array[getRandom(0, array.Length - 1)];
    }
    public string stripAnswer(string answer)
    {
        // return answer.replace(/ / g, "").toLowerCase();
        return answer;
    }
    public float roundError(float answer)
    {
        return Mathf.Round(answer * 100000) / 100000;
    }

    public string fixTerm(float coefficient, string variable, bool firstTerm)
    {
        var term = "+" + coefficient + variable;
        if (coefficient < 0)
        {
            term = "-" + Mathf.Abs(coefficient) + variable;
        }
        switch (coefficient)
        {
            case 0:
                term = "";
                break;
            case 1:
                if (variable != "")
                {
                    term = "+" + variable;
                }
                break;
            case -1:
                if (variable != "")
                {
                    term = "-" + variable;
                }
                break;
        }
        if (firstTerm && coefficient > 0)
        {
            term = term.Substring(1);
        }
        return term;
    }
    /*
       public string fixTerm(float coefficient, string variable, bool firstTerm) {
        var term = "+" + coefficient + variable;
        if (coefficient < 0) {
            term = "-" + Mathf.Abs(coefficient) + variable;
        }
        switch (coefficient) {
            case 0:
                term = "";
                break;
            case 1:
                if (variable != "") {
                    term = "+" + variable;
                }
                break;
            case -1:
                if (variable != "") {
                    term = "-" + variable;
                }
                break;
        }
        if (firstTerm  && coefficient > 0) {
            term = term.Substring(1);
        }
        return term;
    }*/ //need to overload this 
    public float HCF(float x, float y)
    {
        // Returns the highest common factor of x and y.
        float temp = 0;
        if (x < 0)
        {
            x *= -1;
        }
        if (y < 0)
        {
            y *= -1;
        }
        if (x == y)
        {
            return x;
        }
        while (x != 0)
        {
            y = y % x;
            temp = x;
            x = y;
            y = temp;
        }
        return y;
    }

    public int[] GetPrimeFactors(int num)
    {
        int count = 0;
        int[] arr = new int[100];
        int[] arrResult = new int[100];
        int i = 0;
        int idx = 0;
        for (i = 2; i <= num; i++)
        {
            if (isPrime(i) == true)

                arr[count++] = i;
        }
        while (true)
        {
            if (isPrime(num) == true)
            {
                arrResult[idx++] = num;
                break;
            }
            for (i = count - 1; i >= 0; i--)
            {
                if ((num % arr[i]) == 0)
                {
                    arrResult[idx++] = arr[i];
                    num = num / arr[i];
                    break;
                }
            }
        }
        return arrResult;
    }

    public bool isPrime(int n)
    {
        // Returns true if n is prime.
        var isPrime = true;
        if (n < 2)
        {
            isPrime = false;
        }
        for (var i = 2; i <= Mathf.Sqrt(n); i++)
        {
            if ((n % i) == 0)
            {
                isPrime = false;
            }
        }
        return isPrime;
    }
    public int factorial(int n)
    {
        int fact = 1;
        for (var i = 2; i <= n; i++)
        {
            fact *= i;
        }
        return fact;
    }
    public float combin(int n, int k)
    {
        return Mathf.Round(n / (factorial(k) * factorial(n - k)));
    }
    public bool toss()
    {
        if (random.NextDouble() < 0.5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /*  public changeBG(colour) {
          var questions = document.getElementsByClassName("questionBox"};
          for (i = 0; i < questions.Length; i++) {
              questions[i].style.backgroundColor = colour;
          }
      } */
    /* function goFull() {
         if (controls.style.display !== "none") {
             controls.style.display = "none";
             content.style.height = "100vh";
             footer.style.display = "none";
         } else {
             controls.style.display = "block";
             content.style.height = "100%";
             footer.style.display = "block";
         }
         try {
             if (theCanvas) {
                 context = theCanvas.getContext("2d"};
                 context.canvas.width = 0.95 * window.innerWidth;
                 context.canvas.height = 0.95 * window.innerHeight;
                 drawScreen();
             }
         } catch (e) {

         }
     }*/
    //so to use this, just call getQuestion, and give it the number
    public void getQuestion(string currentTopic, float level)
    {
        differentiator.level = level;

        switch (currentTopic)
        {
            case "0":
                differentiator.placeValueLadder();
                break;
            case "1":
                differentiator.additionLadder(false); //5, +-, 6 +-
                break;
            case "2":
                differentiator.subtractionLadder(false);//5 +-, 6 +-
                break;
            case "3":
                differentiator.multiplicationLadder(false); //5 mult, */ 6 */
                break;
            case "4":
                differentiator.divisionLadder(); // 5 / , 6*/
                break;
            case "5":
                differentiator.halvingLadder(); //5 division 6 */
                break;
            case "6":
                differentiator.level = 0.5f; //<----------------------------------------- you can add decimals to doubling and halving, just change the level
                differentiator.doublingLadder(); //5 * 6*
                break;
            case "7":
                differentiator.fractionOfAmountLadder(); //5 frac and mixed num, 6 frac and mixed num, 7 frac and mixed numbers, 8 ops with fractiosn
                break;
            case "8":
                differentiator.percentageOfAmountLadder(); // 6 percents 7 percents, 8 percents 9 percents
                break;
            case "9":
                differentiator.roundingLadder(false);  //5 place value 6 whole numbers 
                break;
            case "10":
                differentiator.negativeLadder(); //5 number theory, 6 whole numbers 7 integers
                break;
            case "11":
                differentiator.powersOfTenLadder(); //6 */, exp 7 exp and square roots 
                break;
            case "12":
                differentiator.ratioShareLadder();//6 ratios and rates, 7 ratios rates and proportions, 8 ratios rates and proportions, 9 ratios rates and proportions
                break;
            case "13":
                differentiator.convertingFDPLadder("random");//6 percents, 7 percents, 8 percents, 9 percents
                break;
            case "14":
                differentiator.collectingTermsLadder(); //7 expresions and properties, 8 exp and properites
                break;
            case "15":
                differentiator.factorLadder(); //5 number theory, 6 number theory, 7 integers?, 8 integers, 9 numbers, 
                break;
            case "16":
                differentiator.multipleLadder(); //5 number theory, 6 number thoery, 7 integers, 8 num theory, integers 
                break;
            case "17":
                differentiator.hcfLadder();//5 num t, 6 num t, 7 numt, integers 8 num t, integers
                break;
            case "18":
                differentiator.lcmLadder(); //5 num t, 6 num t, 7 num t  integers 8 num t, integers
                break;
            case "19":
                differentiator.simplifyingRatiosLadder(); //6 ratios rates, 7 ratios rates , 8 ratios rates 
                break;
            case "20":
                differentiator.simplifyingFractionsLadder(); //5 fractiosn and equiv ordering, 6 frac and mixed numb, 7 frac and mixed num, 8 op with fractions, 
                break;
            case "21":
                differentiator.nthTermLinearLadder();//5 pattersn seq, 7 number seq, 8 numbe seq 9 number seq
                break;
            case "22":
                differentiator.nthTermGeneratingLadder();//7 num seq, 8 num seq, 9 num seq
                break;
            case "23":
                differentiator.fourOpsLadder(false); //5 placement, 7 integers,
                break;
            case "24":
                differentiator.addingCoinsLadder(); //5 money, 6 money 
                break;
            case "25":
                differentiator.countingCoinsLadder(); //5 money 6 money
                break;
            case "26":
                differentiator.speedDistTimeLadder(); //5 time 6 time
                break;
            case "27":
                differentiator.powersAndRootsLadder();//6 exp 7 exp 8 exp 9 exp 10 exp
                break;
            case "28":
                differentiator.orderingLadder();//5 frac and equi ord, 6 rational, 7 rational
                break;
            case "29":
                differentiator.oneStepEquationLadder(false, false); //5 variable exp, 6 1 var equa, expressoins and prop, 7 exp and prop, 1 variable, 8 exp and prop, 1 variable
                break;
            case "30":
                differentiator.numberBondsLadder(); //5 var and exp, 6 exp and prop
                break;
            case "31":
                differentiator.addSubtractFractionsLadder();//5 +- frac, 6 +- frac, 7 +- frac, 
                break;
            case "32":
                differentiator.multiplyDivideFractionsLadder(); //5 */ frac, 6*/ frac 7 */ frac, 
                break;
            case "33":
                differentiator.meanLadder(); //5 prop and stat, 6 pop stat 7 prob stat 8 prop stat
                break;
            case "34":
                differentiator.medianLadder();//5 prop and stat, 6 pop stat 7 prob stat 8 prop stat
                break;
            case "35":
                differentiator.rangeLadder();//5 prop and stat, 6 pop stat 7 prob stat 8 prop stat
                break;
            case "36":
                differentiator.modeLadder();//5 prop and stat, 6 pop stat 7 prob stat 8 prop stat
                break;
            case "37":
                differentiator.averagesLadder();// 6 pop stat 7 prob stat 8 prop stat
                break;
            case "38":
                differentiator.twoStepEquationLadder(false, false); //6 1 var, 7 1 var 8 1 var 9 var exp
                break;
            case "39":
                differentiator.multiplyDivideStandardFormLadder("*"); //8 scientific notation, 9 scientific notation, 10 scientific notation
                break;
            case "40":
                differentiator.addSubtractStandardFormLadder("+"); //8 scientific notation, 9 scientific notation, 10 scientific notation
                break;
            case "41":
                differentiator.convertingToStandardFormLadder();  //8 scientific notation, 9 scientific notation, 10 scientific notation
                break;
            case "42":
                differentiator.convertingFractionsLadder(); //5 frac and equiv, 6 frac and mixed num, 7 frac and mixed 8 frac and mixed
                break;
            case "43":
                differentiator.threeStepEquationLadder(false, false); //8 one var equ, 9 var exp and equ 10 solving equ
                break;
            case "44":
                differentiator.equationsMixedLadder(false, false); //6 one var equ, 7 one var equ, 8 one var equ 
                break;
            case "45":
                differentiator.convertingMeticLengthLadder(false); //5 units, 6 units 7 units 8 units
                break;
            case "46":
                differentiator.equationsWithBracketsLadder(); //7 one var equ, 8 one var equ
                break;
            case "47":
                differentiator.sequencesNextTermLadder(false); //5 pat, 7 num seq, 8 num seq, 9 num seq 
                break;
            case "48":
                differentiator.equationsWithBracketsBothLadder(); //9 var exp and ladders, 8 one var 
                break;
            case "49":
                differentiator.percentageIncreaseDecreaseLadder(); //6 percents, 7 percents, 8 percents, 9 percents 
                break;
            case "50":
                differentiator.reversePercentageLadder();//6 percents, 7 percents, 8 percents, 9 percents 
                break;
            case "51":
                differentiator.substitutionLadder();// 5 var exp, 6 exp and prop, 1 var exp, 7 exp and prop, 1 var equ, 8 exp and prop, 1 var equ 
                break;
            case "52":
                differentiator.ratioReverseLadder(); //6 ratios rates, 7 ratios rates , 8 ratios rates 9 ratios rates
                break;
            case "53":
                differentiator.unitaryMethodLadder();//5 money 6 money
                break;
            case "54":
                differentiator.fractionsFourOpsLadder(); // 8 ops with frac 9 operations
                break;
            case "55":
                differentiator.convertingMeticWeightLadder(); //5 units, 6 units 7 units 8 units
                break;
            case "56":
                differentiator.convertingMeticVolumeLadder(); //5 units, 6 units 7 units 8 units
                break;
            case "57":
                differentiator.convertingMeticMixedLadder(); //5 units, 6 units 7 units 8 units
                break;
            case "58":
                differentiator.probabilityBasicLadder(); //5 prob stat, 6 prob stat 7 prob stat 8 prob stat 9 prob stat
                break;
            case "59":
                differentiator.fractionalChangeLadder(); //5 frac and mixed 6 frac and mixed
                break;
            case "60":
                differentiator.differenceLadder(); //7 ints, 9 ints
                break;
            case "61":
                differentiator.changingTemperaturesLadder(); //5 units of meas, 6 units of meas
                break;
            case "62":
                differentiator.polygonSidesLadder(); //5 2d figures 6 2d figures 7 2d figures
                break;
            case "63":
                differentiator.expectedFrequencyLadder(); //5 prob stat, 6 prob stat 7 prob stat 8 prob stat 9 prob stat
                break;
            case "64":
                differentiator.multiplyingTermsLadder(); //5 var exp, 6 exp and prop, 1 var exp, 7 exp and prop, 1 var exp, 8 exp and prop, 1 var exp 
                break;
            case "65":
                differentiator.singleBracketsLadder(); //5 var exp, 6 exp and prop, 1 var exp, 7 exp and prop, 1 var exp, 8 exp and prop, 1 var exp 
                break;
            case "66":
                differentiator.expandSimplifySingleBracketsLadder(); //6 one var equ, 7 one var equ, 8 one var equ
                break;
            case "67":
                differentiator.interchangingFDPLadder("random");//7 percent,s 8 percents, 9 percents
                break;
            case "68":
                differentiator.oneStepEquationLadder(true, false); //5 var exp, 6 exp and prop, 1 var equ, 7 exp and prop, 1 var exp, 8 exp and prop 1 var equ
                break;
            case "69":
                differentiator.twoStepEquationLadder(true, false); //9 single var ineq, 10 single var ineq
                break;
            case "70":
                differentiator.threeStepEquationLadder(true, false);//9 single var ineq, 10 single var ineq
                break;
            case "71":
                differentiator.fibonacciLadder(); //7 num seq, 8 num seq, 9 num seq
                break;
            case "72":
                differentiator.geometricSequenceLadder(); //7 num seq, 8 num seq, 9 num seq
                break;
            case "73":
                differentiator.nthTermQuadraticLadder(); //10 relations and functions
                break;
            case "74":
                differentiator.convertingTimeLadder(); //5 time, units of measurment, 6 units of measu, time, 7 untis of measur, 8 units of measur
                break;
            case "75":
                differentiator.addSubtractStandardFormLadder("-"); //8 scientific notation, 9 sci notation 10 sci notation
                break;
            case "76":
                differentiator.multiplyDivideStandardFormLadder("/");//8 scientific notation, 9 sci notation 10 sci notation
                break;
            case "77":
                differentiator.interchangingFDPLadder("fraction"); //6 fractions, percents, decimals 7 f p d 8 f pd 
                break;
            case "78":
                differentiator.interchangingFDPLadder("percentage");//6 fractions, percents, decimals 7 f p d 8 f pd 
                break;
            case "79":
                differentiator.convertingFDPLadder("decimal");//6 fractions, percents, decimals 7 f p d 8 f pd 
                break;
            case "80":
                differentiator.convertingFDPLadder("fraction");//6 fractions, percents, decimals 7 f p d 8 f pd 
                break;
            case "81":
                differentiator.convertingFDPLadder("percentage");//6 fractions, percents, decimals 7 f p d 8 f pd 
                break;
            case "82":
                differentiator.gradientTwoPointsLadder(); //8 linear funcs, 9 linear funcs, 10 linear funcs
                break;
            case "83":
                differentiator.midpointTwoPointsLadder();//8 linear funcs, 9 linear funcs, 10 linear funcs
                break;
            case "84":
                differentiator.completingSquareLadder(); //10 quad
                break;
            case "85":
                differentiator.turningPointToQuadraticLadder(); //10 quad
                break;
            case "86":
                differentiator.factoriseExpandQuadraticsLadder(false); //10 quad 10 funcs
                break;
            case "87":
                differentiator.factoriseExpandQuadraticsLadder(true); //10 funcs
                break;
            case "88":
                differentiator.indexLawMultiplyLadder(); //8 exp and roots 9 exp and roots
                break;
            case "89":
                differentiator.indexLawDivideLadder(); //8 exp and roots 9 exp and roots
                break;
            case "90":
                differentiator.indexLawPowerOfPowerLadder(); //9 exp and sqaure roots  10 exp
                break;
            case "91":
                differentiator.indexLawMixedLadder();  //9 exp and sqaure roots  10 exp
                break;
            case "92":
                differentiator.ratioMixedLadder(); //6 ratios, rates 7 ratios rates 8 ratios rates 9 ratios rates  
                break;
            case "93":
                differentiator.combiningRatiosLadder();//6 ratios, rates 7 ratios rates 8 ratios rates 9 ratios rates  
                break;
            case "94":
                differentiator.ratioDifferenceLadder();//6 ratios, rates 7 ratios rates 8 ratios rates 9 ratios rates  
                break;
            case "95":
                differentiator.percentageMultiplierLadder();//6 percents 7 percents, 8 percents, 9 percents  //use this for lower grades 
                break;
            case "96":
                differentiator.percentageChangeLadder();//6 percents 7 percents, 8 percents, 9 percents  
                break;
            case "97":
                differentiator.repeatedPercentageChangeLadder(); // 8 percents, 9 percents 
                break;
            case "98":
                differentiator.addingNegativesLadder(); //7 integers, 8 integers, 9 numbers
                break;
            case "99":
                differentiator.subtractingNegativesLadder();// integers, 8 integers, 9 numbers
                break;
            case "100":
                differentiator.multiplyingDividingNegativesLadder("*");  //6 */ 7 ints 8 ints
                break;
            case "101":
                differentiator.multiplyingDividingNegativesLadder("/"); //6 */ 7 ints 8 ints
                break;
            case "102":
                differentiator.convertingFromStandardFormLadder();  //8 sci notation, 9 sci notation, 10 sci notation
                break;

            //to be tested
            case "103":
                differentiator.placeValueLadder(); //5 place value 6 place value
                break;
            case "104":
                differentiator.identifyPlaceValueLadder();//5 place value 6 place value
                break;
            case "105":
                differentiator.convertingBetweenPlaceValueLadder();//5 place value 6 place value
                break;
            case "106":
                differentiator.expandedFormLadder(); //5 plcae value 6 whole numbers
                break;
            case "107":
                differentiator.writtenToNumberLadder(); //5 place value 6 whole numbers
                break;
            case "108":
                differentiator.comparingNumersLadder(); //5 number theory 6 whole numbers
                break;
            case "109":
                differentiator.estimatingLadder(); //6 problem solving estimating, 7 problem solving and estimating 
                break;
            case "110":
                differentiator.elapsedTimeLadder();//5 time 6 time
                break;
            case "111":
                differentiator.timeProblemLadder();//5 time 6 time
                break;
            case "112":
                differentiator.convertingTimeLadder24();//5 time, units, 6 time, units, 7 units 8 units
                break;
            case "113":
                differentiator.bedmasLadder(); //not done
                break;
            case "114":
                differentiator.primeCompositeLadder(); //5 num theory, 6 num theory 7 num theory, 8 num theory
                break;
            case "115":
                differentiator.primeFactorizationLadder(); //5 num theory, 6 num theory 7 num theory, 8 num theory
                break;
            case "116":
                differentiator.classifyNumbers(); //5 num theory
                break;
            case "117":
                differentiator.fourOpsLadder(true); //8 ops, 9 numbers
                break;
            case "118":
                differentiator.roundingLadder(true); //6 deciamls, 7 decimals
                break;
            case "119":
                differentiator.sequencesNextTermLadder(true);//6 seq, 7 seq
                break;
            case "120":
                differentiator.exponentsExpandedForm(); //6 exp, 7 exp, 8 exp
                break;
            case "121":
                differentiator.level = 0.9f;
                differentiator.fourOpsLadder(true);//7 decimals, 8 decimals   //<----------------------------------- change level to get decimal
                differentiator.level = 0.5f;
                break;
            case "122":
                differentiator.convertingMeticLengthLadder(true);//7 units 8 units
                break;
            case "123":
                differentiator.equationsMixedLadder(true, true); //9 single var ineq 10 single var ineq
                break;
            case "124":
                differentiator.equationsMixedLadder(false, true); //7 1 var equ 8 1 var equ
                break;
            case "125":
                differentiator.level = 0.9f;
                differentiator.additionLadder(false); //5 deciamls, 6 decimals  //<----------------------------------- change level to get decimal
                differentiator.level = 0.5f;
                break;
            case "126":
                differentiator.level = 0.9f;
                differentiator.additionLadder(true); //7 decimals, 8 decimals  //<----------------------------------- change level to get decimal
                differentiator.level = 0.5f;
                break;
            case "127":
                differentiator.level = 0.9f;
                differentiator.fourOpsLadder(false); //    //<----------------------------------- change level to get decimal
                differentiator.level = 0.5f;
                break;
            case "128":
                differentiator.equationsMixedLadder(true, false);
                break;
            case "129":
                differentiator.romanToArabicLadder();
                break;
            case "130":
                differentiator.arabicToRomanLadder();
                break;
            case "131":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "132":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "133":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "134":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "135":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "136":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "137":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "138":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "139":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "140":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "141":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "142":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "143":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "144":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "145":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "146":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "147":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "148":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "149":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "150":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "151":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "152":
                differentiator.shapeQuestion(currentTopic);
                break;
            case "153":
                differentiator.getOperationTerminology();
                break;
            case "154":
                differentiator.getLaws();
                break;
            case "155":
                differentiator.evenAndOdd();
                break;



        }
    }
}

/*   public void getGcseQuestion(float currentTopic) {
       var totalQuestions = 110;

       utilities.Question question;
       switch (currentTopic) {
           case 1:
                differentiator.missingValuesUsingTheMean();
               break;
           case 2:
                differentiator.repeatedPercentageChange();
               break;
           case 3:
         //       differentiator.tangentsToCircles(); *
               break;
           case 4:
                differentiator.bestValue(); 
               break;
           case 5:
                differentiator.sumProductDifference();
               break;
           case 6:
                differentiator.factorSumProblem(); *
               break;
           case 7:
                differentiator.percentageDecrease();
               break;
           case 8:
                differentiator.buyingCheese();
               break;
           case 9:
                differentiator.LengthOfStick();
               break;
           case 10:
                differentiator.squareRectanglePerimeters();
               break;
           case 11:
             //   differentiator.findOriginalGivenHcfLcm(); *
               break;
           case 12:
                differentiator.readingFuel();
               break;
           case 13:
                differentiator.gcse13();
               break;
           case 14:
                differentiator.basicPythagoras();
               break;
           case 15:
                differentiator.gcse15();
               break;
           case 16:
                differentiator.directInverseProportion();
               break;
           case 17:
                differentiator.speedDistanceTime();
               break;
           case 18:
                differentiator.sharingRatioWithFDP();
               break;
           case 19:
                differentiator.linearInequalities();
               break;
           case 20:
                differentiator.savingPercentageOfWages();
               break;
           case 21:
                differentiator.circleWithinSemicircle(); *
               break;
           case 22:
                differentiator.cafeMenuChangeProblem();
               break;
           case 23:
                differentiator.thinkOfANumber();
               break;
           case 24:
                differentiator.holidayLoan();
               break;
           case 25:
                differentiator.equationOfPerpendiculars();
               break;
           case 26:
                differentiator.anglesInTetrahedron(); *
               break;
           case 27:
                differentiator.angleAndAreaOfTriangles();
               break;
           case 28:
                differentiator.sideLengthOfEquilateral();
               break;
           case 29:
                differentiator.mixingDensities();
               break;
           case 30:
                differentiator.estimatingPopulations(); *
               break;
           case 31:
                differentiator.expandingCubics(); *
               break;
           case 32:
                differentiator.nthTermOfQuadratic(); *
               break;
           case 33:
                differentiator.proofs(); *
               break;
           case 34:
                differentiator.errorIntervals(); *
               break;
           case 35:
                differentiator.pressureForceArea();
               break;
           case 36:
                differentiator.boxOfPens();
               break;
           case 37:
                differentiator.convertingSpeeds();
               break;
           case 38:
                differentiator.comparingPuzzleTimes(); *
               break;
           case 39:
                differentiator.exactTrigValues(); *
               break;
           case 40:
                differentiator.fruitProblem();
               break;
           case 41:
                differentiator.proportionalDivision();
               break;
           case 42:
                differentiator.quadraticInequalities(); *
               break;
           case 43:
                differentiator.cardCombinations();
               break;
           case 44:
                differentiator.turningPoints(); *
               break;
           case 45:
                differentiator.boyGirlCombinations();
               break;
           case 46:
                differentiator.gardenSlugs();
               break;
           case 47:
                differentiator.reverseProbabilityWithRatio();
               break;
           case 48:
                differentiator.railcardDiscounts();
               break;
           case 49:
                differentiator.orderingFDPCalc();
               break;
           case 50:
                differentiator.exchangeRates();
               break;
           case 51:
                differentiator.sowingSeeds(); *
               break;
           case 52:
                differentiator.fibonacciAlgebra(); *
               break;
           case 53:
                differentiator.probabilityPercentages();
               break;
           case 54:
                differentiator.concreteRatio(); *
               break;
           case 55:
                differentiator.sharingRatioWithPercentages();
               break;
           case 56:
                differentiator.proportionalRelationships();
               break;
           case 57:
                differentiator.combiningRatios(); *
               break;
           case 58:
                differentiator.dimensionalScaleFactors();  *
               break;
           case 59:
                differentiator.factorisingDiffOfTwoSquares(); *
               break;
           case 60:
                differentiator.maxItemsSameQuantity();
               break;
           case 61:
                differentiator.algebraPolygonPerimeter();
               break;
           case 62:
                differentiator.oddEvenAlgebra(); 
               break;
           case 63:
                differentiator.linearSequences();
               break;
           case 64:
                differentiator.isoscelesAlgebra();
               break;
           case 65:
                differentiator.algebraicTaxis(); 
               break;
           case 66:
                differentiator.numbersFromCards();
               break;
           case 67:
                differentiator.ratioDonatingShares();
               break;
           case 68:
                differentiator.functionSubAndSolve();
               break;
           case 69:
                differentiator.compositeFunctions(); *
               break;
           case 70:
                differentiator.inverseFunctions();
               break;
           case 71:
                differentiator.missingCardValuesUingMean();
               break;
           case 72:
                differentiator.equationOfPerpendicularsWithRatio(); *
               break;
           case 73:
                differentiator.findingMidpointsWithRatio(); *
               break;
           case 74:
                differentiator.repeatedPercentageChangeInReverse();
               break;
           case 75:
                differentiator.deliveringGoods();
               break;
           case 76:
                differentiator.profitOnGoods();
               break;
           case 77:
                differentiator.changingRatios();
               break;
           case 78:
                differentiator.estimatedProfit();
               break;
           case 79:
                differentiator.testingPythagoras();
               break;
           case 80:
                differentiator.multipleRatiosAndPercentages();
               break;
           case 81:
                differentiator.nonCalcReversePercentage();
               break;
           case 82:
                differentiator.productOfPrimes();
               break;
           case 83:
                differentiator.multiplyingDecimals();
               break;
           case 84:
                differentiator.areaOfSquareExpressions();
               break;
           case 85:
                differentiator.framingMetal();
               break;
           case 86:
                differentiator.showingIfParallel();
               break;
           case 87:
                differentiator.vectorsInParallelograms();
               break;
           case 88:
                differentiator.expectedFrequency();
               break;
           case 89:
                differentiator.theatreSeats89();
               break;
           case 90:
                differentiator.combinedAverageSpeed90();
               break;
           case 91:
                differentiator.similarTriangles91();
               break;
           case 92:
                differentiator.squareInACircle92();
               break;
           case 93:
                differentiator.reversePercentages93();
               break;
           case 94:
                differentiator.sandEquations94();
               break;
           case 95:
                differentiator.ratiosOnALine95();
               break;
           case 96:
                differentiator.surfaceAreaVolumeCube96();
               break;
           case 97:
                differentiator.percentageProfit97();
               break;
           case 98:
                differentiator.sameDistanceDifferentTime98();
               break;
           case 99:
                differentiator.proportionalWages99();
               break;
           case 100:
                differentiator.ageEquationsWithRatio100();
               break;
           case 101:
                differentiator.missingConstantsInFunctions101();
               break;
           case 102:
                differentiator.transfromingTrigValues102();
               break;
           case 103:
                differentiator.cuttingWire103();
               break;
           case 104:
                differentiator.evenOddMultiples104();
               break;
           case 105:
                differentiator.countersInABag105();
               break;
           case 106:
                differentiator.wordedProbabilityScale106();
               break;
           case 107:
                differentiator.squaresOnAnAxes107();
               break;
           case 108:
                differentiator.interiorExteriorAngles108();
               break;
           case 109:
                differentiator.percentageWageBonus109();
               break;
           case 110:
                differentiator.estimatingWithSpeedOfPlane110();
               break;
       }
       return question;
   }

   */
