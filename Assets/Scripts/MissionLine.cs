using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class MissionLine : MonoBehaviour
{
    //Create colors
    Color orange = new Color(1, .647f, 0);
    Color purple = new Color(0.424f, 0, 1);
    Color pink = new Color(1, 0, 0.651f);
    Color green = new Color(0.341f, 0.823f, 0.046f);
    Color blue = new Color(0.1098f, 0.4745f, 0.792f);
    Color lilac = new Color(0.6941f, 0.4862f, 0.9882f);
    //transform for the parents which will hold every line
    public GameObject Antenna;
    public GameObject Velocity;
    public GameObject OffNominal;
    public GameObject Mission;
    public GameObject Dynamic;
    public GameObject Trail;
    public GameObject BigAntenna;
    public GameObject BigVelocity;
    public GameObject BigOffNominal;
    public GameObject BigMission;
    public GameObject BigDynamic;
    public GameObject BigTrail;
    public GameObject PP;
    private List<LineRenderer> DynamicList = new List<LineRenderer>();
    private List<LineRenderer> BigDynamicList = new List<LineRenderer>();
    private List<GameObject> TrailList = new List<GameObject>();
    private List<GameObject> BigTrailList = new List<GameObject>();
      
    public void DrawAntennaLine(List<TimeSplice> timeSplices, int i, GameObject parent, int width, string layer)
    {
        //Create line game object
        GameObject antennaLine = new GameObject(parent.name + " Line " + i);
        //Make new line into a child of lines
        antennaLine.transform.parent = parent.transform;
        //Add the line renderer component into the line
        LineRenderer alr = antennaLine.AddComponent<LineRenderer>();
        //Make new material of a basic line material
        Material alrMaterial = new Material(Shader.Find("Sprites/Default"));
        //Find the color based on the availability
        Color color= AntennaColor(timeSplices[i].WPSA, timeSplices[i].DS54, timeSplices[i].DS24, timeSplices[i].DS34);
        //Set the color
        alrMaterial.color = color;
        //Set material to the material made above
        alr.material = alrMaterial; 
        //Setstart width and ending width
        alr.startWidth = width;
        alr.endWidth = width;
        //Set the starting position to the first vector3 and the ending to the next vector3
        alr.SetPosition(0, timeSplices[i].PositionVector3);
        alr.SetPosition(1, timeSplices[i + 1].PositionVector3);
        //this is so the lines rotate
        alr.useWorldSpace = false;
        //change the big lines layer so each camera sees their respective lines
        antennaLine.layer = LayerMask.NameToLayer(layer);
    } 
    public void DrawVelocityLine(List<TimeSplice> timeSplices, List<(Color color, float position)> colorKeys, int i, GameObject parent, int width, string layer)
    {
        //Create line game object
        GameObject velocityLine = new GameObject(parent.name + " Line " + i);
        //Make new line into a child of lines
        velocityLine.transform.parent = parent.transform;
        //Add the line renderer component into the line
        LineRenderer vlr = velocityLine.AddComponent<LineRenderer>();
        //Make new material of a basic line material
        Material vlrMaterial = new Material(Shader.Find("Sprites/Default"));
        //Find how far along the mission the current iteration is, will be from 0-1
        float percentOfLine = (float)i / (timeSplices.Count - 1);
        //Find the color based on the color keys and the percent along the line
        Color color = InterpolateColors(colorKeys, percentOfLine);
        //Set the color
        vlrMaterial.color = color;
        //Set the material to the one made above
        vlr.material = vlrMaterial;
        //Set start and end width
        vlr.startWidth = width;
        vlr.endWidth = width;
        //Set the starting position to the first vector3 and the ending to the next vector3
        vlr.SetPosition(0, timeSplices[i].PositionVector3);
        vlr.SetPosition(1, timeSplices[i + 1].PositionVector3); 
        //this is so the lines rotate
        vlr.useWorldSpace = false;
        //change the big lines layer so each camera sees their respective lines
        velocityLine.layer = LayerMask.NameToLayer(layer);
    }  
    public void DrawOffNominalLine(List<BonusTimeSplice> timeSplices, int i, GameObject parent, int width, string layer)
    {
        //Create line game object
        GameObject offNominalLine = new GameObject(parent.name + " Line " + i);
        //Make new line into a child of lines
        offNominalLine.transform.parent = parent.transform;
        //Add the line renderer component into the line
        LineRenderer olr = offNominalLine.AddComponent<LineRenderer>();
        //Make new material of a basic line material
        Material olrMaterial = new Material(Shader.Find("Sprites/Default"));
        //Set color to white (can be changed to whatever we want)
        olrMaterial.color = AntennaColor(timeSplices[i].WPSA, timeSplices[i].DS54, timeSplices[i].DS24, timeSplices[i].DS34);
        //Set material to the material made above
        olr.material = olrMaterial; 
        //Set the start width and ending width
        olr.startWidth = width;
        olr.endWidth = width;
        //Set the starting position to the first vector3 and the ending to the next vector3
        olr.SetPosition(0, timeSplices[i].BonusPositionVector);
        olr.SetPosition(1, timeSplices[i + 1].BonusPositionVector);
        //this is so the lines rotate
        olr.useWorldSpace = false;
        //change the big lines layer so each camera sees their respective lines
        offNominalLine.layer = LayerMask.NameToLayer(layer);
    }
    public void DrawMissionLine(List<TimeSplice> timeSplices, int i, GameObject parent, int width, string layer)
    {
        //Create line game object
        GameObject missionLine = new GameObject(parent.name + " Line " + i);
        //Make new line into a child of lines
        missionLine.transform.parent = parent.transform;
        //Add the line renderer component into the line
        LineRenderer mlr = missionLine.AddComponent<LineRenderer>();
        //Make new material of a basic line material
        Material mlrMaterial = new Material(Shader.Find("Sprites/Default"));
        //Set color to white (can be changed to whatever we want)
        Color color = MissionColor(timeSplices[i].Time);
        mlrMaterial.color = color;
        //Set material to the material made above
        mlr.material = mlrMaterial; 
        //Set the start width and ending width
        mlr.startWidth = width;
        mlr.endWidth = width;
        //Set the starting position to the first vector3 and the ending to the next vector3
        mlr.SetPosition(0, timeSplices[i].PositionVector3);
        mlr.SetPosition(1, timeSplices[i + 1].PositionVector3);
        //this is so the lines rotate
        mlr.useWorldSpace = false;
        //change the big lines layer so each camera sees their respective lines
        missionLine.layer = LayerMask.NameToLayer(layer);
    }
    public void DrawDynamicLine(List<TimeSplice> timeSplices, int i, GameObject parent, int width, string layer, List<LineRenderer> list)
    {
        //Create line game object
        GameObject dynamicLine = new GameObject(parent.name + " Line " + i);
        //Make new line into a child of lines
        dynamicLine.transform.parent = parent.transform;
        //Add the line renderer component into the line
        LineRenderer dlr = dynamicLine.AddComponent<LineRenderer>();
        //Make new material of a basic line material
        Material dlrMaterial = new Material(Shader.Find("Sprites/Default"));
        //Set color to blue (can be changed to whatever we want)
        dlrMaterial.color = purple;
        //Set material to the material made above
        dlr.material = dlrMaterial; 
        //Set the start width and ending width
        dlr.startWidth = width;
        dlr.endWidth = width;
        //Set the starting position to the first vector3 and the ending to the next vector3
        dlr.SetPosition(0, timeSplices[i].PositionVector3);
        dlr.SetPosition(1, timeSplices[i + 1].PositionVector3);
        //this is so the lines rotate
        dlr.useWorldSpace = false;
        //change the big lines layer so each camera sees their respective lines
        dynamicLine.layer = LayerMask.NameToLayer(layer);
        //add each line renderer to the list of line renderers to be manipulated later
        list.Add(dlr);
    }
    public void DrawTrailLine(List<TimeSplice> timeSplices, int i, GameObject parent, int width, string layer, List<GameObject> list)
    {
        //Create line game object
        GameObject trailLine = new GameObject(parent.name + " Line " + i);
        //Make new line into a child of lines
        trailLine.transform.parent = parent.transform;
        //Add the line renderer component into the line
        LineRenderer tlr = trailLine.AddComponent<LineRenderer>();
        //Make new material of a basic line material
        Material tlrMaterial = new Material(Shader.Find("Sprites/Default"));
        //Set color to blue (can be changed to whatever we want)
        tlrMaterial.color = blue;
        //Set material to the material made above
        tlr.material = tlrMaterial; 
        //Set the start width and ending width
        tlr.startWidth = width;
        tlr.endWidth = width;
        //Set the starting position to the first vector3 and the ending to the next vector3
        tlr.SetPosition(0, timeSplices[i].PositionVector3);
        tlr.SetPosition(1, timeSplices[i + 1].PositionVector3);
        //this is so the lines rotate
        tlr.useWorldSpace = false;
        //change the big lines layer so each camera sees their respective lines
        trailLine.layer = LayerMask.NameToLayer(layer);
        trailLine.SetActive(false);
        //add each line object to the list of objects to be turned on/off later
        list.Add(trailLine);
    }
    public void TurnOffLines()
    {
        //turn all lines except mission lines off since those are the default
        Antenna.SetActive(false);
        Velocity.SetActive(false);
        OffNominal.SetActive(false);
        Dynamic.SetActive(false);
        Trail.SetActive(false);
        BigAntenna.SetActive(false);
        BigVelocity.SetActive(false);
        BigOffNominal.SetActive(false);
        BigDynamic.SetActive(false);
        BigTrail.SetActive(false);
    }
    public Color AntennaColor(float WPSA, float DS54, float DS24, float DS34) 
    {
        float antenna;
        //Returns the sum of the availabilities which are all either 0 or 1
        antenna = WPSA + DS54 + DS24 + DS34;
        //Simple if else to determine the color based on the availability
        if (antenna == 0)
        {
            return Color.red;
        }
        else if (antenna == 1)
        {
            return orange;
        }
        else if (antenna == 2)
        {
            return Color.yellow;
        }
        else if (antenna == 3)
        {
            return Color.green;
        }
        else 
        {
            //Return white if something went wrong
            return Color.white;
        }
    } 
    public Color MissionColor(float time)
    {
        if (time < 1494)
        {
            return purple;
        }
        else if (time >= 1494 && time < 7080)
        {
            return pink;
        }
        else if (time >=7080 && time <12960)
        {
            return green;
        }
        else
        {
            return blue;
        }      
    }

    public List<(Color color, float position)> CreateColorKeys(List<TimeSplice> timeSplices)
    {
        //Color keys are made when the velocity crosses these numbers
        int[] velocityThresholds = {1, 2, 3, 4, 5, 6, 7, 8};
        //Initial list of the times when the velocity crosses thresholds
        List<float> timeChanges = new List<float>();
        //Initial list of which velocity threshold was crossed
        List<int> velocityThresholdChanges = new List<int>();
        //Start at 1 to prevent an out of index error
        for (int i = 1; i < timeSplices.Count; i++)
        {
            //Find previous and current velocity at the specific iteration
            float previousVelocity = timeSplices[i-1].ResultantVelocity;
            float currentVelocity = timeSplices[i].ResultantVelocity;
             
             //Foreach loop to go through every threshold
            foreach (int velocityThreshold in velocityThresholds)
            {
                //If the difference is greater than 1 then it is what happens from 0 - 8.23 minutes
                //and without this it would make 8 color keys at 0 minutes, very bad
                if ((currentVelocity - previousVelocity) > 1)
                {
                   // Do Nothing
                }
                else
                {
                //Check if previous and current velocities are on different sides of the threshold such as 0.99 and 1.01
                if ((previousVelocity < velocityThreshold && currentVelocity >= velocityThreshold) ||
                    (previousVelocity >= velocityThreshold && currentVelocity < velocityThreshold))
                    {
                        //Add the time it was crossed and which threshold
                        timeChanges.Add(timeSplices[i].Time);
                        velocityThresholdChanges.Add(velocityThreshold);
                    }
                }   
            }
        }
        //Initialize list of color keys with a color and respective position
        List<(Color color, float position)> colorKeys = new List<(Color, float)>();
        //Create the list of colors, slowest to fastest
        Color[] thresholdColors = {Color.blue, Color.cyan, Color.green, Color.yellow, orange, Color.red, Color.magenta, Color.white};
        //Find the max time of the whole mission
        float maxTime = timeSplices.Last().Time;
        //Initalize list of times
        List<float> normalizedTimes = new List<float>();
        foreach (float time in timeChanges)
        {
            //Create standardized time for 0-1 for every time change
            normalizedTimes.Add(time / maxTime);
        }
        //Add the initial color key at the start of the mission
        colorKeys.Add((Color.white, 0));
        for (int i = 0; i < normalizedTimes.Count; i++)
        {
            //Add a color key with the color that corresponds to the threshold crossed and the position 0-1 of the mission
            colorKeys.Add((thresholdColors[velocityThresholdChanges[i] - 1], normalizedTimes[i]));
        }
        //Add the last color key at the end of the mission
        colorKeys.Add((Color.white, 1));

        return colorKeys;
    }
    public Color InterpolateColors(List<(Color color, float position)> colorKeys, float percentOfLine)
    {
        for (int i = 0; i < colorKeys.Count - 1; i++)
        {
            //Checks if the percent of line sent in is between 2 color keys
            if (percentOfLine >= colorKeys[i].position && percentOfLine <= colorKeys[i + 1].position)
            {
                //Calculate the distance between them by doing part / whole
                float distanceBetweenColorKeys = (percentOfLine - colorKeys[i].position) / (colorKeys[i + 1].position - colorKeys[i].position);
                //Creates a intermediate color using interpolation
                return Color.Lerp(colorKeys[i].color, colorKeys[i + 1].color, distanceBetweenColorKeys);
            }
        }
        //return black if something is wrong
        return Color.black;
    }
    public void DrawMissionPaths(List<TimeSplice> timeSplices, List<BonusTimeSplice> bonusTimeSplices)
   {
       //Create the color keys
       List<(Color color, float position)> colorKeys = CreateColorKeys(timeSplices);
       for (int i = 0; i < timeSplices.Count - 1; i++)
       {
           DrawVelocityLine(timeSplices, colorKeys, i, Velocity, 25, "Main Camera");
           DrawAntennaLine(timeSplices, i, Antenna, 25, "Main Camera"); 
           DrawMissionLine(timeSplices, i, Mission, 25, "Main Camera");
           DrawDynamicLine(timeSplices, i, Dynamic, 25, "Main Camera", DynamicList);
           DrawTrailLine(timeSplices, i, Trail, 25, "Main Camera", TrailList);
           DrawVelocityLine(timeSplices, colorKeys, i, BigVelocity, 1500, "Mini Camera");
           DrawAntennaLine(timeSplices, i, BigAntenna, 1500, "Mini Camera"); 
           DrawMissionLine(timeSplices, i, BigMission, 1500, "Mini Camera");
           DrawDynamicLine(timeSplices, i, BigDynamic, 1500, "Mini Camera", BigDynamicList); 
           DrawTrailLine(timeSplices, i, BigTrail, 1500, "Mini Camera", BigTrailList); 
       }
       for (int i = 0; i < bonusTimeSplices.Count - 1; i++)
       {
            DrawOffNominalLine(bonusTimeSplices, i, OffNominal, 25, "Main Camera");
            DrawOffNominalLine(bonusTimeSplices, i, BigOffNominal, 1500, "Mini Camera");
       }
       TurnOffLines();
   } 
   private int j; //int to be used down below so the function remembers which lines were previously affected
   public void ChangeDynamicColor(int currentIndex)
    {
        //starting at the last line affected, up to the current index
        for(int i = j; i < currentIndex; i++)
        {
            //get the line renderers from the lists populated at run time
            LineRenderer dlr = DynamicList[i];
            LineRenderer bdlr = BigDynamicList[i];
            //make new material for line renderers
            Material dlrMaterial = new Material(Shader.Find("Sprites/Default"));
            //set the color of the new material to lilac
            dlrMaterial.color = lilac;
            //set the line renderers material to new lilac material
            dlr.material = dlrMaterial;
            bdlr.material = dlrMaterial;
            //turn on each individual line object that needs to be turned on these will only be visible 
            // if the user selects the trail option, which turns on the parent object
            TrailList[i].SetActive(true);
            BigTrailList[i].SetActive(true);
        }
        //set j to current index so next time this function is ran it will know where to start
        j = currentIndex;
    }
    public void RestartDynamicTrail(int currentIndex)
    {
        //start at first line and go up to current index so only lines that have been affected get reset
        for(int i = 0; i < currentIndex; i++)
        {
            //get the line renderers from the lists populated at run time
            LineRenderer dlr = DynamicList[i];
            LineRenderer bdlr = BigDynamicList[i];
            //make new material for line renderers
            Material dlrMaterial = new Material(Shader.Find("Sprites/Default"));
            //set the color of the new material to purple
            dlrMaterial.color = purple;
            //set the line renderers material to new purple material
            dlr.material = dlrMaterial;
            bdlr.material = dlrMaterial;
            //turn off all the trail lines that were turned on
            TrailList[i].SetActive(false);
            BigTrailList[i].SetActive(false);
        }
    }
}


