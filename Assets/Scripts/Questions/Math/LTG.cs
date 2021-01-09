using AwesomeCharts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//a lot of fucking work to be done here, just leave this for later 


static class MyExtensions
{


    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}


public class LTG : MonoBehaviour
{


    public GameObject textContainer;
    public Image gridImage;
    public Image pointImage;
    public Image mainImage;


    public Sprite[] shapes;
    public Sprite[] angles;
    public Sprite[] lines;
    public Sprite[] lineSystems;
    public Sprite[] triangleAngles;
    public Sprite[] triangleSides;
    public Sprite[] shapeNets;
    public Sprite[] shapes3D;
    public Sprite[] polygons;



    utilities utility;
    utilities.Question problem;

    List<Vector2> checkPoints;
    List<Vector2> currPoints;

    public bool isClickable;




    public void clearPrevious()
    {
        Debug.Log("clearing");
        mainImage.gameObject.SetActive(false);
        gridImage.gameObject.SetActive(false);
        positiveGrid.gameObject.SetActive(false);
        pointImage.gameObject.SetActive(false);

        isClickable = false;

        //you need to clearthe graphs 

        /* for(int i = 0; i < pointImage.transform.parent.childCount; i++)
         {
             Destroy(pointImage.transform.parent.GetChild(i));
         }*/

        /*    if(scatterObjects != null)
            {
                for(int i = scatterObjects.Count-1; i >= 0; i--)
                {
                    Destroy(scatterObjects[i]);
                }
            }*/
        bar.transform.parent.parent.parent.gameObject.SetActive(false);
        /*   for(int i = bar.transform.childCount-1; i >= 0; i--)
           {
               Destroy(bar.transform.GetChild(i));
           }*/

        wedge.transform.parent.gameObject.SetActive(false);
        tableRow.transform.parent.gameObject.SetActive(false);

    }
    //ok, so for this one, test all of ltg again, then build it and test it again 


    public GameObject layoutGameobject;
    //so what this will do is similart utility, it will get the question number, based on that it will generate the question and the answer, which will then be handled by mathlayout
    public void getShapeQuestion(string topic, float level)
    {
        problem = new utilities.Question();
        if (utility == null)
        {
            utility = new utilities();
        }
        clearPrevious();
        problem.multipleChoice = true;
        isClickable = false;
        switch (topic)
        {
            case "131":
                pointToCoords(level);//5 coords 6 coords 7 coords 8 coords 9 coords 10 coords  //not mc
                break;
            case "132":
                CoordsToPoint(level);//5 coords 6 coords 7 coords 8 coords 9 coords 10 coordspositionFromCoors //not mc
                isClickable = true;
                isLine = false;
                break;
            case "133":
                mixedBarQuestion(); //5 data 6 data 7 data 8 data 9 data 10 data
                break;
            case "134":
                mixedCircleQuestion(); //7 data 8 data 9 data 10 data
                break;
            case "135":
                mixedDoubleBarQuestion();//7 data 8 data 9 data 10 data   <---------------- not done
                break;
            case "136":
                mixedLineQuestion(); //7 data 8 data 9 data 10 data
                break;
            case "137":
                mixedScatterQuestion();//5 data 6 data 7 data 8 data 9 data 10 data
                break;
            case "138":
                mixedTableQuestion();//5 data 6 data 7 data 8 data 9 data 10 data
                break;
            case "139":
                mixedDoubleLineQuestion(); //8 data 9 data      <------------------------ not done
                break;
            case "140":
                typeAngleQuestion(); //5 angles 6 2d 7 2d 8 2d 9 2d
                break;
            case "141":
                typePolygonQuestion(); //5 2d 6 2d 7 2d 8 2d 9 2d
                break;
            case "142":
                quadQuestion();//5 2d 6 2d 7 2d 8 2d 9 2d
                break;
            case "143":
                typeTriangleAngle();//5 angles 6 2d 7 2d 8 2d 9 2d
                break;
            case "144":
                typeTriangleSide();//5 2d 6 2d 7 2d 8 2d 9 2d (polygon sies ladder is 62)
                break;
            case "145":
                typeLine(); //8 linear 9 linear 10 linear  //gradient and midpoint are 82 and 83 (slope and midpoint)
                break;
            case "146":
                drawLineEquation();//8 linear 9 linear 10 linear funcs  //not mc
                isClickable = true;
                isLine = true;
                break;
            case "147":
                drawLineTable();//8 linear 9 linear 10 linear funcs  //not mc
                break;
            case "148":
                interpretLine();//8 linear 9 linear 10 linear funcs
                break;
            case "149":
                shape3DProperties(); //5 3d shapes 6 3d shapes 7 3d shapes 8 3d shapes 9 3d shapes 
                break;
            case "150":
                shape3DNet(); //5 3d shapes 6 3d shapes 7 3d shapes 8 3d shapes 9 3d shapes 
                break;
            case "151":
                distanceQuestion(); //7 coords 8 coords 9 coords 10 coords 
                break;
            case "152":
                directionQuestion();//7 coords 8 coords 9 coords 10 coords    //not mc
                break;
            case "153":
                tallyQuestion();
                break;


        }
        //  Debug.Log(problem.question + " " + problem.stringAnswer);
        Debug.Log(problem.question + " question");
        //   title.text = problem.question;
        differentiator.question = new List<utilities.Question>();
        problem.isClickable = isClickable;
        problem.isShape = true;
        differentiator.question.Add(problem);

        layoutGameobject.SetActive(true);

    }

    //coord:
    //7,8,9 distance, translations
    //



    public void pointToCoords(float level) //this will show a point at some position, the use has to enter the coordinates
    {

        int x = 0, y = 0;
        switch (level)
        {
            case 0.1f:
            case 0.2f:
            case 0.3f:
            case 0.4f:
            case 0.5f:
                x = utility.getRandom(0, 3);
                y = utility.getRandom(0, 3);

                break;
            case 0.6f:
            case 0.7f:
            case 0.8f:
            case 0.9f:
            case 1f:
                x = utility.getRandom(-3, 3);
                y = utility.getRandom(-3, 3);
                break;
        }
        //make the grid show 
        gridImage.gameObject.SetActive(true);
        pointImage.gameObject.SetActive(true);
        Vector2 prePos = positionFromCoords(x, y);
        pointImage.transform.localPosition = new Vector2(prePos.x - transform.parent.localPosition.x, prePos.y - transform.parent.localPosition.y);// - transform.localPosition;
        problem.stringAnswer = x + "," + y;
        problem.question = "What are the coordinates of the point shown?";
        Debug.Log(problem.stringAnswer + " answer");

        problem.isOrdering = true;
        problem.multipleChoice = false;
        problem.inBetween = ",";

    }
    public Camera cam;
    //then on click, check the position of the cursor 
    //do this last 


    public void CoordsToPoint(float level) //the user has to click on the right cooridnates 
    {
        int x = 0, y = 0;
        switch (level)
        {
            case 0.1f:
            case 0.2f:
            case 0.3f:
            case 0.4f:
            case 0.5f:
                x = utility.getRandom(0, 3);
                y = utility.getRandom(0, 3);

                break;
            case 0.6f:
            case 0.7f:
            case 0.8f:
            case 0.9f:
            case 1f:
                x = utility.getRandom(-3, 3);
                y = utility.getRandom(-3, 3);
                break;
        }
        //make the grid show 
        gridImage.gameObject.SetActive(true);
        // pointImage.gameObject.SetActive(true);
        //   pointImage.transform.localPosition = positionFromCoords(x, y);
        problem.question = "Click on the coordinates: " + "(" + x + "," + y + ")";
        Vector2 position = positionFromCoords(x, y);
        problem.stringAnswer = position.x + "," + position.y;
        problem.multipleChoice = false;
        problem.isOrdering = true;
        checkPoints = new List<Vector2>();
        checkPoints.Add(positionFromCoords(x, y));
        clickableCooldown = 40;
        currPoints = new List<Vector2>();
    }

    //ok, so for this one you need to init checkpoints and user points
    //for use checkpoints you need to get the position from the coordinates 

    public void mixedBarQuestion()
    {
        List<Data> data = getRandomData();
        generateRandomTGQuestion(data);
        loadBarGraph(data);

    }

    public void mixedCircleQuestion()
    {
        List<Data> data = getRandomData();
        generateRandomTGQuestion(data);

        loadPieGraph(data);

    }

    public void mixedTableQuestion()
    {
        List<Data> data = getRandomData();
        generateRandomTGQuestion(data);

        loadTable(data);
    }

    public void mixedScatterQuestion()
    {
        List<Data> data = getRandomData();

        for (int i = 0; i < data.Count; i++)
        {
            float value = data[i].value;
            if (value > maxLine)
            {
                data[i].value = utility.getRandom(0, maxLine);
            }
        }
        generateRandomTGQuestion(data);

        loadScatterGraph(data);
    }

    float maxLine = 10;
    public void mixedLineQuestion()
    {
        List<Data> data = getRandomData();
        //so cap data at 10, for both sides 

        for (int i = 0; i < data.Count; i++)
        {
            float value = data[i].value;
            if (value > maxLine)
            {
                data[i].value = utility.getRandom(0, maxLine);
            }
        }
        generateRandomTGQuestion(data);

        loadLineGraph(data);
    }

    public void mixedDoubleBarQuestion() //do the double ones later 
    {
        List<Data> data1 = getRandomData();
        List<Data> data2 = getRandomData();

        generateRandomTGQuestion(data1);
        loadDoubleBarGraph(data1, data2);


    }

    public void tallyQuestion()
    {
        List<Data> data = getRandomData();
        //for tally questions it will be differnet 


    }

    public void mixedDoubleLineQuestion()
    {
        List<Data> data1 = getRandomData();
        List<Data> data2 = getRandomData();

        generateRandomTGQuestion(data1);
        loadDoubleLineGraph(data1, data2);
    }

    public void typeAngleQuestion() //this will ask them what type of angle it is, just have a sprite 
    {
        //load the sprite sheet, this is pretty much just shapes  
        int n = utility.getRandom(0, angles.Length - 1);
        List<string> multipleChoice = new List<string>();
        List<int> inclucded = new List<int>();
        inclucded.Add(n);
        multipleChoice.Add(utility.AngleName(n));
        while (multipleChoice.Count < 3)
        {
            int curr = utility.getRandom(0, angles.Length - 1);
            while (inclucded.Contains(curr))
            {
                curr = utility.getRandom(0, angles.Length - 1);
            }
            multipleChoice.Add(utility.AngleName(curr));
            inclucded.Add(curr);
        }
        problem.question = "What is the type of angle shown below?";
        problem.stringAnswer = multipleChoice[0];
        mainImage.gameObject.SetActive(true);
        mainImage.sprite = angles[n];
        multipleChoice.Shuffle();
        problem.choices = multipleChoice;
    }

    public void typePolygonQuestion() //it will show some polygon, they need to find out what it is 
    {
        int n = utility.getRandom(0, polygons.Length - 1);
        problem.question = "What is this polygon called?";

        List<string> multipleChoice = new List<string>();
        List<int> inclucded = new List<int>();
        inclucded.Add(n);
        multipleChoice.Add(utility.polygonPicker(n).name);
        while (multipleChoice.Count < 3)
        {
            int curr = utility.getRandom(0, polygons.Length - 1);
            while (inclucded.Contains(curr))
            {
                curr = utility.getRandom(0, polygons.Length - 1);
            }
            multipleChoice.Add(utility.polygonPicker(curr).name);
            inclucded.Add(curr);
        }
        problem.question = "What is the name of the polygon shown below?";
        problem.stringAnswer = multipleChoice[0];
        mainImage.gameObject.SetActive(true);
        mainImage.sprite = polygons[n];
        multipleChoice.Shuffle();
        problem.choices = multipleChoice;

    }

    public void quadQuestion() //they need to identify the shape 
    {
        int n = utility.getRandom(0, shapes.Length - 1);
        List<string> multipleChoice = new List<string>();
        List<int> inclucded = new List<int>();
        inclucded.Add(n);
        multipleChoice.Add(utility.ShapeName(n));
        while (multipleChoice.Count < 3)
        {
            int curr = utility.getRandom(0, shapes.Length - 1);
            while (inclucded.Contains(curr))
            {
                curr = utility.getRandom(0, shapes.Length - 1);


            }
            multipleChoice.Add(utility.ShapeName(curr));
            inclucded.Add(curr);
        }
        problem.question = "What is the name of the shape shown below?";
        problem.stringAnswer = multipleChoice[0];
        mainImage.gameObject.SetActive(true);
        mainImage.sprite = shapes[n];
        multipleChoice.Shuffle();
        problem.choices = multipleChoice;
        //    createMultipleChoice(multipleChoice);
    }


    public void typeTriangleAngle() //type of triangle based on the angles
    {
        int n = utility.getRandom(0, triangleAngles.Length - 1);
        List<string> multipleChoice = new List<string>();
        List<int> inclucded = new List<int>();
        inclucded.Add(n);
        multipleChoice.Add(utility.triangleAngleName(n));
        while (multipleChoice.Count < 3)
        {
            int curr = utility.getRandom(0, triangleAngles.Length - 1);
            while (inclucded.Contains(curr))
            {
                curr = utility.getRandom(0, triangleAngles.Length - 1);
            }
            multipleChoice.Add(utility.triangleAngleName(curr));
            inclucded.Add(curr);
        }

        problem.question = "What is the name of this triangle based on its angles?";
        problem.stringAnswer = multipleChoice[0];

        mainImage.gameObject.SetActive(true);
        mainImage.sprite = triangleAngles[n];
        multipleChoice.Shuffle();
        problem.choices = multipleChoice;

    }

    public void typeTriangleSide() //type of triangle based on side
    {

        int n = utility.getRandom(0, triangleSides.Length - 1);
        List<string> multipleChoice = new List<string>();
        List<int> inclucded = new List<int>();
        inclucded.Add(n);
        multipleChoice.Add(utility.triangleSideName(n));
        while (multipleChoice.Count < 3)
        {
            int curr = utility.getRandom(0, triangleSides.Length - 1);
            while (inclucded.Contains(curr))
            {
                curr = utility.getRandom(0, triangleSides.Length - 1);
            }
            multipleChoice.Add(utility.triangleSideName(curr));
            inclucded.Add(curr);
        }
        problem.question = "What is the name of this triangle based on its sides?";
        problem.stringAnswer = multipleChoice[0];
        mainImage.gameObject.SetActive(true);
        mainImage.sprite = triangleSides[n];
        multipleChoice.Shuffle();
        problem.choices = multipleChoice;
    }

    public void typeLine() //type of line based on intersection (parallel, perp, intersecting etc.)
    {
        int n = utility.getRandom(0, lines.Length - 1);
        List<string> multipleChoice = new List<string>();
        List<int> inclucded = new List<int>();
        inclucded.Add(n);
        multipleChoice.Add(utility.LineName(n));
        while (multipleChoice.Count < 3)
        {
            int curr = utility.getRandom(0, lines.Length - 1);
            while (inclucded.Contains(curr))
            {
                curr = utility.getRandom(0, lines.Length - 1);
            }
            multipleChoice.Add(utility.LineName(curr)); //problem here
            inclucded.Add(curr);
        }
        problem.question = "What are these angles called?";
        problem.stringAnswer = multipleChoice[0];
        mainImage.gameObject.SetActive(true);
        mainImage.sprite = lines[n];
        multipleChoice.Shuffle();
        problem.choices = multipleChoice;
    }

    public void typeLineSystem()
    {
        int n = utility.getRandom(0, lineSystems.Length - 1);
        List<string> multipleChoice = new List<string>();
        List<int> inclucded = new List<int>();
        inclucded.Add(n);
        multipleChoice.Add(utility.LineSystemName(n));
        while (multipleChoice.Count < 3)
        {
            int curr = utility.getRandom(0, lineSystems.Length - 1);
            while (inclucded.Contains(curr))
            {
                curr = utility.getRandom(0, lineSystems.Length - 1);
            }
            multipleChoice.Add(utility.LineSystemName(curr));
            inclucded.Add(curr);
        }
        problem.question = "What is this linear system called?";
        problem.stringAnswer = multipleChoice[0];
        mainImage.gameObject.SetActive(true);
        mainImage.sprite = lineSystems[n];
        multipleChoice.Shuffle();
        problem.choices = multipleChoice;
    }
    string tempAnswer = "";
    public void drawLineEquation() //draw a line based on equation
    {
        //to draw a line the user needs to click on two points, keep track of both points
        //when they click put a point sprite in that position 
        //when they click on the second point check if its right or not, if its not then get rid of it 
        problem.multipleChoice = false;
        problem.isClickable = true;
        gridImage.gameObject.SetActive(true);
        int slope = utility.getRandom(-3, 5);
        int yIntercept = utility.getRandom(-3, 3);
        char sign = yIntercept > 0 ? '+' : '-';
        problem.question = "Select two points that the line y=" + slope + "x " + sign + Mathf.Abs(yIntercept).ToString() + " passes throught";
        problem.stringAnswer = slope + "," + yIntercept;
        tempAnswer = slope + "," + yIntercept;
        Debug.Log(problem.stringAnswer + " answer");
        currPoints = new List<Vector2>();
        checkPoints = new List<Vector2>();


    }

    //for the higher levels make some fractions, or make it go up non linearly (change how much it goes up by each time)
    public void drawLineTable() //draw a line based on table
    {
        //make a line, generate table values based on those 
        //create the table, ask for the line 
        int slope = utility.getRandom(-3, 5);
        int yIntercept = utility.getRandom(-5, 5);

        int startX = utility.getRandom(-5, 5);
        int n = 4; //ill put 4 values in the table
        List<Data> tableData = new List<Data>();
        for (int i = 0; i < n; i++)
        {
            Data curr = new Data();
            curr.name = (startX + i).ToString();
            curr.value = ((startX + i) * slope + yIntercept);
            tableData.Add(curr);
        }
        loadTable(tableData);
        problem.question = "What is the equation of the line shown in the table below?";
        problem.stringAnswer = slope + "," + yIntercept;
        problem.multipleChoice = false;
        problem.isOrdering = true;
        problem.before = "y=";
        problem.inBetween = "x +";

    }

    public void interpretLine() //asks questions about the slope and y intercept of a line, word problems ish
    {

    }

    public void shape3DProperties() //ask about number of sides, number verticies etc.
    {
        Debug.Log("here at shapes ");
        int n = utility.getRandom(0, shapes3D.Length - 1);
        List<string> multipleChoice = new List<string>();
        List<int> inclucded = new List<int>();
        inclucded.Add(n);
        multipleChoice.Add(utility.shapePicker3D(n));
        while (multipleChoice.Count < 3)
        {
            int curr = utility.getRandom(0, shapes3D.Length - 1);
            if (!inclucded.Contains(curr))
            {
                multipleChoice.Add(utility.shapePicker3D(curr));
                inclucded.Add(curr);
            }
        }
        //  problem.question = "What is this linear system called?";
        problem.stringAnswer = multipleChoice[0];
        mainImage.gameObject.SetActive(true);
        mainImage.sprite = shapes3D[n];
        multipleChoice.Shuffle();
        problem.choices = multipleChoice;
    }

    public void shape3DNet() //what shape based on the net 
    {
        int n = utility.getRandom(0, shapeNets.Length - 1);
        List<string> multipleChoice = new List<string>();
        List<int> inclucded = new List<int>();
        inclucded.Add(n);
        multipleChoice.Add(utility.shapeNetPicker(n));
        while (multipleChoice.Count < 3)
        {
            int curr = utility.getRandom(0, shapeNets.Length - 1);
            if (!inclucded.Contains(curr))
            {
                multipleChoice.Add(utility.shapeNetPicker(curr));
                inclucded.Add(curr);
            }
        }
        problem.question = "Which 3d shapes net is shown below?";
        problem.stringAnswer = multipleChoice[0];
        mainImage.gameObject.SetActive(true);
        mainImage.sprite = shapeNets[n];
        multipleChoice.Shuffle();
        problem.choices = multipleChoice;
    }

    float sigma = 0.01f;
    public void distanceQuestion() //distance between 2 points
    {
        //choose two points, ask what the distance is between them
        //make sure its a perfect square 
        //first choose one point, then choose a length, then find the other points 

        Vector2 startPoint = new Vector2(utility.getRandom(-10, 10), utility.getRandom(-10, 10));
        int length = utility.getRandom(3, 5);
        //now its a radius with length, centered at start point, find all other integer coordinates from there
        List<Vector2> points = new List<Vector2>();
        //this will search above the y
        for (int i = (int)startPoint.x - length; i < startPoint.x + length; i++)
        {
            float deltaY = Mathf.Sqrt(Mathf.Pow(length, 2) - Mathf.Pow(i, 2));
            if (float.Parse(deltaY.toFixed(2)) - deltaY < sigma)
            {
                //then its an int 
                points.Add(new Vector2(i, deltaY + startPoint.y));
                points.Add(new Vector2(i, startPoint.y - deltaY));

            }
        }

        Vector2 endPoint = points[utility.getRandom(0, points.Count)];
        problem.question = "What is the distance between (" + startPoint.x + "," + startPoint.y + ") and (" + endPoint.x + "," + endPoint.y + ") ?";
        problem.stringAnswer = length.ToString();
        problem.choices = new List<string>(utility.multipleChoiceNumber(problem.stringAnswer));


    }

    public void directionQuestion() //coord plane directions
    {
        //choose a point, choose direction, they need to figure out where it ends up
        Vector2 startPoint = new Vector2(utility.getRandom(-10, 10), utility.getRandom(-10, 10));
        Vector2 directions = new Vector2(utility.getRandom(-10, 10), utility.getRandom(-10, 10));
        Vector2 endPoint = new Vector2(startPoint.x + directions.x, startPoint.y + directions.y);
        problem.question = "If you start at (" + startPoint.x + "," + startPoint.y + ") and move " + directions.x + " to the right and " + directions.y + " up, where will you end up?";
        problem.stringAnswer = endPoint.x + "," + endPoint.y;
        problem.multipleChoice = false;
        problem.isOrdering = true;
        //    string[] xChoices = utility.multipleChoiceNumber(endPoint.x.ToString());
        //  string[] yChoices = utility.multipleChoiceNumber(endPoint.y.ToString());
        //ok so now you need to find the right x and y 


    }




    //---------------------TG UTILS -------------------
    #region utils


    public List<Data> getRandomData()
    {
        int curr = utility.getRandom(0, 7);

        List<Data> output = new List<Data>();
        switch (curr)
        {
            case 0:
                output = storePicker(utility.getRandom(3, 7));
                break;
            case 1:
                output = tempraturePickerWeek(utility.getRandom(3, 7));
                break;
            case 2:
                output = tempraturePickerMonth(utility.getRandom(3, 7));
                break;
            case 3:
                output = farmPicker(utility.getRandom(3, 7));
                break;
            case 4:
                output = studentPicker(utility.getRandom(3, 7));
                break;
            case 5:
                output = foodPicker(utility.getRandom(3, 7));
                break;
            case 6:
                output = zooPickerDiffZoo(utility.getRandom(3, 7));
                break;
            case 7:
                output = zooPickerDiffAnimal(utility.getRandom(3, 7));
                break;

        }
        return output;
    }

    //you need to add name to these guys 
    public void generateRandomTGQuestion(List<Data> data)
    {
        int curr = utility.getRandom(0, 8);

        switch (curr)
        {
            case 0:
                largestTG(data, true);
                //handeld in class 
                break;
            case 1:
                largestTG(data, false);
                problem.choices = new List<string>(utility.multipleChoiceNumber(problem.stringAnswer));
                break;
            case 2:
                smallestTG(data, true);
                //handeled in class 
                break;
            case 3:
                smallestTG(data, false);
                problem.choices = new List<string>(utility.multipleChoiceNumber(problem.stringAnswer));
                break;
            case 4:
                totalTG(data);
                problem.choices = new List<string>(utility.multipleChoiceNumber(problem.stringAnswer));
                break;
            case 5:
                nameToValueTG(data);
                problem.choices = new List<string>(utility.multipleChoiceNumber(problem.stringAnswer));
                break;
            case 6:
                valueToNamesTG(data);
                //handeled in class
                break;
            case 7:
                atLeastValuesTG(data);
                problem.choices = new List<string>(utility.multipleChoiceNumber(problem.stringAnswer));
                break;
            case 8:
                togetherTG(data);
                problem.choices = new List<string>(utility.multipleChoiceNumber(problem.stringAnswer));
                break;


        }
        foreach (var d in data)
        {
            Debug.Log(d.name + " " + d.value);
        }
        //  Debug.Log(problem.question + " " + problem.stringAnswer);
    }
    #region dataGen
    public List<Data> storePicker(int n) //it sold x amount of an object on that day 
    {
        List<Data> data = new List<Data>();
        if (n > 7)
        {
            n = 7; //make it only one week, shouldn't be more than 7
        }
        string storeObject = utility.itemPicker("large");
        //Below is the graph of the number of item sold by a store over the last week 

        for (int i = 0; i < n; i++)
        {
            Data curr = new Data();
            float value = utility.getRandom(0, 10);
            string day = utility.dayPicker(i);
            curr.name = day;
            curr.value = value;
            curr.nameType = storeObject;
            curr.questionEnding = "amount of " + storeObject + "'s sold?";
            data.Add(curr);
        }

        return data;
    }

    public List<Data> tempraturePickerWeek(int n) //it sold x amount of an object on that day 
    {
        List<Data> data = new List<Data>();
        if (n > 7)
        {
            n = 7; //make it only one week, shouldn't be more than 7
        }
        //   string storeObject = utility.itemPicker("large");
        //Below is the graph of the number of item sold by a store over the last week 

        for (int i = 0; i < n; i++)
        {
            Data curr = new Data();
            float value = utility.getRandom(5, 30);
            string day = utility.dayPicker(i);
            curr.name = day;
            curr.value = value;
            //    curr.nameType = storeObject;
            curr.nameType = "degree";
            curr.questionEnding = " temprature?";
            data.Add(curr);
        }
        return data;

    }

    public List<Data> tempraturePickerMonth(int n) //it sold x amount of an object on that day 
    {
        List<Data> data = new List<Data>();
        int start = utility.getRandom(0, 11);

        //   string storeObject = utility.itemPicker("large");
        //Below is the graph of the number of item sold by a store over the last week 

        for (int i = 0; i < n; i++)
        {
            Data curr = new Data();
            float value = utility.getRandom(5, 30);
            string month = utility.monthPicker(i + start);
            curr.name = month;
            curr.value = value;
            //    curr.nameType = storeObject;
            curr.nameType = "degree";
            curr.questionEnding = " temprature?";
            data.Add(curr);

        }
        return data;
    }


    public List<Data> farmPicker(int n) //have different towns and different number of animals in that town
    {
        List<Data> data = new List<Data>();


        string animal = utility.animalPicker(-1);
        //below is a list of farms and the number of + animal + that they have 
        List<string> repeatcheck = new List<string>();
        for (int i = 0; i < n; i++)
        {
            Data curr = new Data();
            float value = utility.getRandom(0, 10);
            string townName = utility.townPicker(-1);
            while (repeatcheck.Contains(townName))
            {
                townName = utility.townPicker(-1);
            }
            repeatcheck.Add(townName);
            curr.name = townName;
            curr.value = value;
            curr.nameType = animal;
            curr.questionEnding = "amount of " + animal + "'s?";
            data.Add(curr);
        }
        return data;
    }

    public List<Data> studentPicker(int n) //have a bunch of studnets and small items
    {
        List<Data> data = new List<Data>();

        string studentObject = utility.itemPicker("small");

        //below is a list of studnets and the amount of + item + that they have 
        List<string> repeatcheck = new List<string>();
        for (int i = 0; i < n; i++)
        {
            Data curr = new Data();
            float value = utility.getRandom(0, 10);
            string name = utility.namePicker(-1).name; //check if names is already contained, if it is pick a different one 
            while (repeatcheck.Contains(name))
            {
                name = utility.townPicker(-1);
            }
            repeatcheck.Add(name);
            curr.name = name;
            curr.value = value;
            curr.nameType = studentObject;
            curr.questionEnding = "amount of " + studentObject + "'s?";
            data.Add(curr);
        }
        return data;

    }

    public List<Data> foodPicker(int n) //have a bunch of fruits  
    {
        List<Data> data = new List<Data>();

        List<string> repeatcheck = new List<string>();
        for (int i = 0; i < n; i++)
        {
            Data curr = new Data();
            float value = utility.getRandom(10, 50);

            curr.value = value;
            string fruit = utility.fruitPicker(-1);
            while (repeatcheck.Contains(fruit))
            {
                fruit = utility.townPicker(-1);
            }
            repeatcheck.Add(fruit);
            curr.name = fruit;
            curr.nameType = "fruit";
            curr.questionEnding = "amount of fruits's?";
            data.Add(curr);
        }
        return data;
    }

    public List<Data> zooPickerDiffZoo(int n) //different zoos but the same animal
    {
        List<Data> data = new List<Data>();

        string animalName = utility.zooAnimalPicker(-1);

        //below is a list of studnets and the amount of + item + that they have 

        List<string> repeatcheck = new List<string>();
        for (int i = 0; i < n; i++)
        {
            Data curr = new Data();
            float value = utility.getRandom(0, 5);
            string name = utility.townPicker(-1); //check if names is already contained, if it is pick a different one 
            while (repeatcheck.Contains(name))
            {
                name = utility.townPicker(-1);
            }
            repeatcheck.Add(name);
            curr.name = name;
            curr.nameType = animalName;
            curr.value = value;
            curr.questionEnding = "amount of " + animalName + "'s?";
            data.Add(curr);
        }
        return data;
    }

    public List<Data> zooPickerDiffAnimal(int n) //differnt animals in the same zoo
    {
        List<Data> data = new List<Data>();

        string zooName = utility.townPicker(-1);
        //below is a list of studnets and the amount of + item + that they have 

        List<string> repeatcheck = new List<string>();
        for (int i = 0; i < n; i++)
        {
            Data curr = new Data();
            float value = utility.getRandom(0, 5);
            string animal = utility.animalPicker(-1);
            while (repeatcheck.Contains(name))
            {
                name = utility.townPicker(-1);
            }
            repeatcheck.Add(name);
            curr.value = value;
            curr.name = animal;
            curr.nameType = zooName;
            curr.questionEnding = "amount of animal's";
            data.Add(curr);

        }
        return data;
    }


    #endregion

    //for these functions, you should generate all the random values in the function, then set the answe for problem

    //for largest and smalletst you can either have the name or the value 
    public void largestTG(List<Data> data, bool name) //what is the largest in the data (already implemented)
    {
        float largest = data[0].value;
        List<Data> same = new List<Data>();
        foreach (var d in data)
        {
            if (d.value > largest)
            {
                largest = d.value;
                same = new List<Data>();
                same.Add(d);
            }
            else if (d.value == largest)
            {
                same.Add(d);
            }
        }
        if (name)
        {
            problem.question = "What has the largest " + data[0].questionEnding;
            problem.stringAnswer = same[0].name;
            //ok, here you need to generate multiple choice answers 
            generateRandomNameAnswers(data);
        }
        else
        {
            problem.question = "What is the largest " + data[0].questionEnding; //amount of " + data[0].nameType  +data[0].questionEnding;
            problem.stringAnswer = largest.ToString();
        }

    }

    void generateRandomNameAnswers(List<Data> data)
    {
        List<string> included = new List<string>();

        included.Add(problem.stringAnswer);
        for (int i = 0; i < Mathf.Min(data.Count, 3); i++)
        {
            string curr = data[utility.getRandom(0, data.Count - 1)].name;
            while (included.Contains(curr))
            {
                curr = data[utility.getRandom(0, data.Count - 1)].name;
            }
        }
        included.Shuffle();
        problem.choices = included;
        foreach (var a in included)
        {
            Debug.LogError("the current choices are: " + a);
        }

        problem.multipleChoice = true;
    }

    void generateRandomNameAnswersMin(List<Data> data)
    {
        List<string> included = new List<string>();
        //so string answer has all the good ones,
        //check if its in string answer, if not add it
        //then pick a random one from string answer, and that will be the final answer
        //if there is not enought then just use as much as you can 

        // included.Add(problem.stringAnswer);
        for (int i = 0; i < Mathf.Min(data.Count, 3); i++)
        {
            string curr = data[utility.getRandom(0, data.Count - 1)].name;
            int errorCheck = 0;
            while (included.Contains(curr) || problem.stringAnswer.Contains(curr))
            {
                curr = data[utility.getRandom(0, data.Count - 1)].name;
                errorCheck++;
                if (errorCheck > 10)
                {
                    //then just leave it at that?
                    break;
                }
            }
        }


        string[] newChoices = problem.stringAnswer.Split(',');
        string finalChoice = newChoices[utility.getRandom(0, newChoices.Length - 1)]; //that should work 
        problem.stringAnswer = finalChoice;
        included.Add(finalChoice);

        included.Shuffle();
        problem.choices = included;
        foreach (var a in included)
        {
            Debug.LogError("the current choices are: " + a);
        }

        problem.multipleChoice = true;


    }

    public void smallestTG(List<Data> data, bool name) //what is the smallest 
    {
        float smallest = data[0].value;
        List<Data> same = new List<Data>();
        foreach (var d in data)
        {
            if (d.value < smallest)
            {
                smallest = d.value;
                same = new List<Data>();
            }
            else if (d.value == smallest)
            {
                same.Add(d);
            }
        }

        problem.question = "What is the smallest " + data[0].questionEnding; //amount of " + data[0].nameType + data[0].questionEnding;
        problem.stringAnswer = smallest.ToString();

    }

    public void totalTG(List<Data> data)
    {
        float total = 0;
        foreach (var d in data)
        {
            total += d.value;
        }
        problem.question = "What is the total " + data[0].questionEnding;// amount of " + data[0].nameType + "?";
        problem.stringAnswer = total.ToString();
    }

    public void nameToValueTG(List<Data> data)
    {

        int index = utility.getRandom(0, data.Count - 1);
        float value = data[index].value;

        problem.question = "What is the amount of " + data[0].nameType + "'s for " + data[index].name + "?";
        problem.stringAnswer = data[index].value.ToString();
    }


    public void valueToNamesTG(List<Data> data)
    {
        List<string> names = new List<string>();

        int index = utility.getRandom(0, data.Count - 1);
        float value = data[index].value;
        foreach (var d in data)
        {
            if (d.value == value)
            {
                names.Add(d.name);

            }
        }
        if (names.Count == 1)
        {
            problem.question = "What has the value of " + value + "?";
            problem.stringAnswer = data[index].name;

            generateRandomNameAnswers(data);
        }
        else
        {
            //if there are mulitple 
            problem.question = "How many have " + value + " " + data[0].questionEnding + "'s ?";
            problem.stringAnswer = names.Count.ToString();
        }

    }

    public void atLeastValuesTG(List<Data> data) //(e.g how many shops sold atleast 100 units)
    {
        List<string> names = new List<string>();
        int index = utility.getRandom(0, data.Count - 1);
        float min = data[index].value;
        foreach (var d in data)
        {
            if (d.value >= min)
            {
                names.Add(d.name);
            }
        }
        problem.question = "What has at least " + min + " " + data[0].nameType + "'s ?";
        for (int i = 0; i < names.Count; i++)
        {
            problem.stringAnswer += names[i];
            if (i != names.Count - 1)
            {
                problem.stringAnswer += ",";
            }
        }
        names.Shuffle(); //only show one of the correct answers 
                         //then you need you're own function to deal with taht 
        generateRandomNameAnswersMin(data); //not sure if that will work but oh well 

        // problem.choices = names;
        //this way you can get the count, and which ones 
        //for this make usre ordering is false 
    }

    public void togetherTG(List<Data> data) //(e.g how many units did a and b sell together)
    {
        int count = utility.getRandom(0, data.Count / 3);
        if (count <= 1)
        {
            count = 2;
        }
        List<int> together = new List<int>();
        for (int i = 0; i < count; i++)
        {
            int curr = utility.getRandom(0, data.Count - 1);
            while (together.Contains(curr))
            {
                curr = utility.getRandom(0, data.Count - 1);
            }
            together.Add(curr);
        }

        float total = 0;
        foreach (int i in together)
        {
            total += data[i].value;
        }
        problem.question = "How many " + data[0].nameType + "'s does ";
        for (int i = 0; i < together.Count; i++)
        {
            problem.question += data[together[i]].name;
            if (i < together.Count - 2)
            {
                problem.question += ", ";
            }
            else if (i == together.Count - 2)
            {
                problem.question += " and ";
            }
        }
        problem.question += " have all together?";

        problem.stringAnswer = total.ToString();

    }




    #endregion
    //----------TG UTILS----------------------


    Vector2 getClickPoint()
    {
        return new Vector2(0, 0);
    }
    //60 88 116 144
    // 28  28 
    Vector2 origin = new Vector2(-271, 43);//new Vector2(-4, 60); //you need to change this... (actually for both of them)


    float gridLengthX = 32;//35;
    float gridLengthY = 23;//28;
    //this will give the correct x and y so that it matches up with the grid 
    Vector2 positionFromCoords(float x, float y)
    {
        return new Vector2(origin.x + x * gridLengthX, origin.y + y * gridLengthY);
    }

    Vector2 CoordsFromPosition(float x, float y)
    {
        return new Vector2((x - origin.x) / gridLengthX, (y - origin.y) / gridLengthY);
    }

    Vector2 originPos = new Vector2(-370.2f, 43);
    float gridLengthXPos = 20.4f;
    float gridLengthYPos = 13.9f;

    Vector2 positionFromCoordsPos(int x, int y)
    {
        //  x++;
        // y++;
        return new Vector2(originPos.x + x * gridLengthXPos, originPos.y + y * gridLengthYPos);
    }

    Vector2 CoordsFromPositionPos(float x, float y)
    {
        return new Vector2((x - originPos.x) / gridLengthXPos, (y - originPos.y) / gridLengthYPos);
    }





    //----------- GRAPH STUFF -------------

    //ok so for each data, it has a value, 
    public class Data
    {

        bool isLine;

        //for lines, scatter graph and line graph
        public Vector2 points;
        public string label;

        //for bar and circle graph and table
        public string name;
        public float value;

        public string nameType; //earasers, people etc., used to display it 

        public string questionEnding = "";

    }
    //    public void createGraph(string type, List<>)

    //make bar graphs from data 
    //ok, so make sure that the graphs are cleared after each use 
    GameObject scrollObject = null;
    public GameObject bar;
    public Text title;


    //ok, so right here, just iterate throught the data, put the name, and put the tally, put it in like a table 
    public GameObject tallyTable;
    public Sprite[] tallyies;
    void loadTallyQuestion(List<Data> data)
    {
        var cell = tallyTable.transform.GetChild(0);
        GameObject currentCell = Instantiate(cell.gameObject, cell, true);
        currentCell.transform.SetParent(currentCell.transform.parent.parent);

        for (int i = 0; i < data.Count; i++)
        {
            int num5 = (int)data[i].value / 5;
            int leftOver = (int)(data[i].value % 5);

            for (int j = 0; j < num5; j++)
            {
                GameObject curr5 = Instantiate(cell.transform.GetChild(0).gameObject, cell.transform.GetChild(0), true);
                curr5.GetComponent<Image>().sprite = tallyies[4];
                curr5.transform.SetParent(curr5.transform.parent.parent);
            }

            GameObject leftSprite = Instantiate(cell.transform.GetChild(0).gameObject, cell.transform.GetChild(0), true);
            leftSprite.GetComponent<Image>().sprite = tallyies[leftOver - 1];
            leftSprite.transform.SetParent(leftSprite.transform.parent.parent);

            //ok and that should work

            //ok, now just instantiate it, and it will have a horizontal layout group
            //just show the fives first 

        }
    }

    #region graphs

    //that should work 
    public GameObject barGraph;

    void loadDoubleBarGraph(List<Data> data1, List<Data> data2)
    {
        AwesomeCharts.BarChart script = barGraph.GetComponent<AwesomeCharts.BarChart>();
        AwesomeCharts.BarData thing = new AwesomeCharts.BarData();

        AwesomeCharts.BarDataSet set1 = new AwesomeCharts.BarDataSet();
        AwesomeCharts.BarDataSet set2 = new AwesomeCharts.BarDataSet();

        int min = Mathf.Min(data1.Count, data2.Count);

        List<string> lables = new List<string>();

        int currColor = 0;

        for (int i = 0; i < min; i++)
        {

            lables.Add(data1[i].name);
            set1.AddEntry(new AwesomeCharts.BarEntry(i, data1[i].value));
            //  set1.BarColors.Add(Information.colors[i % Information.colors.Length]);
            set1.BarColors.Add(Color.red);

        }

        for (int i = 0; i < min; i++)
        {

            //  lables.Add(data2[i].name);
            set2.AddEntry(new AwesomeCharts.BarEntry(i, data2[i].value));
            //  set1.BarColors.Add(Information.colors[i % Information.colors.Length]);
            set2.BarColors.Add(Color.blue);
        }

        thing.CustomLabels = lables;

        thing.DataSets.Add(set1);
        thing.DataSets.Add(set2);

        script.data = thing;
        script.SetDirty();

        barGraph.gameObject.SetActive(true); //that should work 

    }

    void loadBarGraph(List<Data> data)
    {

        AwesomeCharts.BarChart script = barGraph.GetComponent<AwesomeCharts.BarChart>();
        AwesomeCharts.BarData thing = new AwesomeCharts.BarData();
        AwesomeCharts.BarDataSet set1 = new AwesomeCharts.BarDataSet();


        List<string> lables = new List<string>();


        int currColor = 0;

        for (int i = 0; i < data.Count; i++)
        {


            if (currColor > Information.colors.Length - 1)
            {
                currColor = 0;
            }

            //   float percentage = (float)fillerWordsCount[i] / (float)maxCount;
            // float height = maxLength * percentage;

            lables.Add(data[i].name);
            set1.AddEntry(new AwesomeCharts.BarEntry(i, data[i].value));
            set1.BarColors.Add(Information.colors[i % Information.colors.Length]);

        }
        thing.CustomLabels = lables;
        thing.DataSets.Add(set1);

        script.data = thing;
        script.SetDirty();

        barGraph.gameObject.SetActive(true); //that should work 

    }

    float largestData(List<Data> data)
    {
        float output = data[0].value;
        for (int i = 1; i < data.Count; i++)
        {
            if (output < data[i].value)
            {
                output = data[i].value;
            }
        }
        return output;
    }

    float sumData(List<Data> data)
    {
        float output = 0;
        foreach (var d in data)
        {
            output += d.value;
        }
        return output;
    }


    public GameObject wedge;
    public GameObject legend;
    public GameObject wedgeText;
    //ok, so instantiate the whole container 
    void loadPieGraph(List<Data> data)
    {
        wedge.transform.parent.gameObject.SetActive(true);
        float rotationAmount = 0;
        float maxCount = 0;
        int currColor = 0;

        Vector2 legendOffset = new Vector2(0, 20);
        Vector3 currOffset = new Vector3(0, -0);

        for (int i = 0; i < data.Count; i++)
        {
            maxCount += data[i].value;
        }

        for (int i = 0; i < data.Count; i++)
        {
            GameObject currWedge = Instantiate(wedge.gameObject, wedge.transform, true);
            currWedge.transform.SetParent(currWedge.transform.parent.parent);

            GameObject currLegend = Instantiate(legend.gameObject, legend.transform, false);
            currLegend.transform.SetParent(currLegend.transform.parent.parent);

            GameObject currText = Instantiate(wedgeText.gameObject, wedgeText.transform.parent, true);
            //  currText.transform.SetParent(currText.transform.parent.);


            currLegend.transform.GetChild(0).GetComponent<Image>().color = Information.colors[currColor];
            currLegend.transform.GetChild(1).GetComponent<Text>().text = data[i].name;

            currWedge.GetComponent<Image>().color = Information.colors[currColor++];
            if (data[i].value != 0)
                currText.GetComponentInChildren<Text>().text = data[i].value.ToString();

            currLegend.transform.localPosition += currOffset;
            currLegend.SetActive(true);
            currOffset.y -= legendOffset.y;

            if (currColor > Information.colors.Length - 1)
            {
                currColor = 0;
            }
            currWedge.SetActive(true);

            float percentage = (float)data[i].value / (float)maxCount;
            currWedge.GetComponent<Image>().fillAmount = percentage;
            float deltaRotation = percentage * 360.0f;



            rotationAmount += deltaRotation;

            if (rotationAmount <= 180)
            {
                currText.transform.GetChild(0).Rotate(new Vector3(0, 0, 180));
                currText.transform.GetChild(0).GetComponent<Text>().alignment = TextAnchor.MiddleRight;
            }

            if (rotationAmount > 0)
                currWedge.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, rotationAmount)); //rotate z negative
            currText.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, rotationAmount - deltaRotation / 2)); //rotate z negative

        }

    }


    float xOffset = 10;
    public GameObject scatterImage;

    void loadScatterGraph(List<Data> data)
    {
        AwesomeCharts.LineChart script = lineChart.GetComponent<AwesomeCharts.LineChart>();
        //   script.Config = new
        AwesomeCharts.LineData thing = new AwesomeCharts.LineData();
        script.XAxis.LinesCount = data.Count;



        //   set1.LineColor = new Color32(54, 105, 126, 255);
        //  set1.FillColor = new Color(0.4f, 1, 0.6313726f, 1);
        List<string> lables = new List<string>();

        //   List<AwesomeCharts.LineDataSet> dataSets = new List<AwesomeCharts.LineDataSet>();


        for (int i = 0; i < data.Count; i++)
        {
            string label = data[i].name;
            float score = data[i].value;
            lables.Add(label);
            LineDataSet set1 = new LineDataSet();
            set1.AddEntry(new LineEntry(i, score));
            set1.AddEntry(new LineEntry(i + 0.01f, score + 0.01f));
            //dataSets.Add(set1);
            thing.DataSets.Add(set1);

        }
        //66B0FF

        //66FFA1
        thing.CustomLabels = lables;


        script.data = thing;
        script.SetDirty();
        lineChart.gameObject.SetActive(true); //that should work 
    }

    //just draw a bunch of lines between all the points 
    public GameObject lineContainer;
    public GameObject positiveGrid;

    //that should also work
    public GameObject lineChart;

    void loadDoubleLineGraph(List<Data> data1, List<Data> data2)
    {
        AwesomeCharts.LineChart script = lineChart.GetComponent<AwesomeCharts.LineChart>();
        //   script.Config = new
        AwesomeCharts.LineData thing = new AwesomeCharts.LineData();
        int min = Mathf.Min(data1.Count, data2.Count);
        script.XAxis.LinesCount = min;
        LineDataSet set1 = new LineDataSet();
        LineDataSet set2 = new LineDataSet();


        set1.LineColor = new Color32(54, 105, 126, 255);
        //    set1.FillColor = new Color(0.4f, 1, 0.6313726f, 1);//Color32(54, 105, 126, 110);

        set2.LineColor = new Color32(130, 14, 232, 255);
        //   set2.FillColor = new Color32(175, 114, 237, 255);//Color32(54, 105, 126, 110); //you dont need the fill colors 
        List<string> lables = new List<string>();
        //      List<AwesomeCharts.LineEntry> dataSets = new List<AwesomeCharts.LineEntry>();
        for (int i = 0; i < min; i++)
        {
            string label = data1[i].name;
            float score = data1[i].value;
            lables.Add(label);
            set1.AddEntry(new LineEntry(i, score));
            //  dataSets.Add(score);

        }

        for (int i = 0; i < min; i++)
        {
            //    string label = data2[i].name;
            float score = data2[i].value;
            //   lables.Add(label);
            set2.AddEntry(new LineEntry(i, score));
            //  dataSets.Add(score);

        }


        thing.CustomLabels = lables;
        thing.DataSets.Add(set1);
        thing.DataSets.Add(set2);


        script.data = thing;
        script.SetDirty();
        lineChart.gameObject.SetActive(true); //that should work 
    }

    void loadLineGraph(List<Data> data)
    {



        AwesomeCharts.LineChart script = lineChart.GetComponent<AwesomeCharts.LineChart>();
        //   script.Config = new
        AwesomeCharts.LineData thing = new AwesomeCharts.LineData();
        script.XAxis.LinesCount = data.Count;
        LineDataSet set1 = new LineDataSet();

        set1.LineColor = new Color32(54, 105, 126, 255);
        set1.FillColor = new Color(0.4f, 1, 0.6313726f, 1);//Color32(54, 105, 126, 110);
        List<string> lables = new List<string>();
        //      List<AwesomeCharts.LineEntry> dataSets = new List<AwesomeCharts.LineEntry>();
        for (int i = 0; i < data.Count; i++)
        {
            string label = data[i].name;
            float score = data[i].value;
            lables.Add(label);
            set1.AddEntry(new LineEntry(i, score));
            //  dataSets.Add(score);

        }

        thing.CustomLabels = lables;
        thing.DataSets.Add(set1);

        script.data = thing;
        script.SetDirty();
        lineChart.gameObject.SetActive(true); //that should work 
    }



    //ok here just instantiate that shit 
    public GameObject tableRow;
    void loadTable(List<Data> data)
    {
        Vector2 offset = new Vector2(0, -15.6f);
        for (int i = 0; i < data.Count; i++)
        {

            GameObject curr = Instantiate(tableRow.gameObject, tableRow.transform, true);
            curr.transform.SetParent(curr.transform.parent.parent);
            curr.transform.Translate(offset);
            offset.y -= 10;
            curr.transform.GetChild(0).GetComponent<Text>().text = data[i].name;
            curr.transform.GetChild(1).GetComponent<Text>().text = data[i].value.ToString();

            curr.gameObject.SetActive(true);
        }
        tableRow.transform.parent.gameObject.SetActive(true);
    }
    #endregion
    //----------- GRAPH STUFF -------------

    bool isLine;
    int clickableCooldown = 40;
    void Update()
    {/*
        if (clickableCooldown > 0)
        {
            clickableCooldown--;
            Information.currPosition = Vector3.zero;
            return;
        }
        if (isClickable && Input.GetMouseButtonDown(0))
        {

            if (Information.currPosition != Vector3.zero)
            {
                //then that means it was clicked, so get the position 
                //if its the point, then check 
                if (isLine)
                {
                    if (currPoints.Count >= 2)
                    {
                        currPoints = new List<Vector2>();
                    }
                }
                currPoints.Add(Information.currPosition);
                if (isLine)
                {
                    checkLine();
                }
                else
                {
                    checkPointLists();
                }


                Information.currPosition = Vector3.zero;
            }
        }
        */
    }

    float maxDiff = 10; //the maximum error for each point 
    void checkPointLists()
    {
        if (currPoints.Count != checkPoints.Count)
        {
            currPoints = new List<Vector2>();
            return;
        }
        bool[] incl = new bool[currPoints.Count];
        for (int i = 0; i < incl.Length; i++)
        {
            incl[i] = false;
        }
        for (int i = 0; i < checkPoints.Count; i++)
        {
            Debug.Log(checkPoints[i].x + " " + checkPoints[i].y + " " + currPoints[i].x + " " + currPoints[i].y);
            int j = 0;
            for (j = 0; j < currPoints.Count; j++)
            {
                if (Vector2.Distance(checkPoints[i], currPoints[j]) < maxDiff && incl[j] == false)
                {
                    incl[j] = true;
                    break;
                }
            }

            if (j == checkPoints.Count)
            {//then its wrong 
                Information.isCorrect = false;
                Information.isIncorrect = true;
                currPoints = new List<Vector2>();
                return;
            }

        }
        Information.isCorrect = true;

    }
    float lineDelta = 0.5f;
    void checkLine()
    {
        //you need to convert the position points into coordinates 
        //so now, take 
        if (currPoints.Count <= 1)
        {
            return;
        }
        currPoints[0] = CoordsFromPosition(currPoints[0].x, currPoints[0].y);
        currPoints[1] = CoordsFromPosition(currPoints[1].x, currPoints[1].y);



        float slope = (currPoints[0].y - currPoints[1].y) / (currPoints[0].x - currPoints[1].x);
        float yIntercept = currPoints[0].y - slope * currPoints[0].x;
        string[] temp = tempAnswer.Split(',');

        float goodSlope = float.Parse(temp[0]);
        float goodIntercept = float.Parse(temp[1]);
        Debug.Log(slope + " " + goodSlope + " " + yIntercept + " " + goodIntercept);
        if (Mathf.Abs(slope - goodSlope) > lineDelta || Mathf.Abs(yIntercept - goodIntercept) > lineDelta)
        {
            Information.isCorrect = false;
            Information.isIncorrect = true;
            return;
        }
        Information.isCorrect = true;

    }
}
