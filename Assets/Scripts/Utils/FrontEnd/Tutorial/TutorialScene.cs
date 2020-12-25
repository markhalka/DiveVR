using System.Collections.Generic;

public class TutorialScene
{

    public string sceneName;
    public int panelIndex;
    public List<TutorialPanel> panels;

    public TutorialScene(string name)
    {
        sceneName = name;
        panelIndex = 0;
        panels = new List<TutorialPanel>();
    }

    public class TutorialPanel
    {
        public int buttonIndex;
        public List<Model> information;


        public TutorialPanel()
        {
            information = new List<Model>();
            buttonIndex = 0;
        }



    }
}
