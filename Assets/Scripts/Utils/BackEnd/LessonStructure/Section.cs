using System.Collections.Generic;


// Start is called before the first frame update
public class Section
{
    public List<Model> questions;
    public int section;
    public Section(int sec)
    {
        questions = new List<Model>();
        section = sec;
    }
}

