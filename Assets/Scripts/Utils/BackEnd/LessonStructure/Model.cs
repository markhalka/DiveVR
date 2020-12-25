using System.Collections.Generic;

public class Model
{

    public List<string> simpleInfo;
    public List<string> advancedInfo;
    public List<string> questions;
    public int section;
    public bool wasShown;

    public Model()
    {

        simpleInfo = new List<string>();
        advancedInfo = new List<string>();
        questions = new List<string>();
        section = 0;
        wasShown = false;


    }

}
