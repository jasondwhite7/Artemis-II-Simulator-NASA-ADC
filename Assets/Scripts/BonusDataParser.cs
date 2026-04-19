using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using System.IO;
using System.Globalization;

public class BonusDataParser
{
    public BonusTimeSplice BonusTimeSplice = new BonusTimeSplice();
    public List<BonusTimeSplice> CreateList()
    {
        List<BonusTimeSplice> bonusTimeSpliceList = new List<BonusTimeSplice>(); //Create list of TimeSplice objects
        using (var reader = new StreamReader("Assets/Data/Bonus NASA Data.csv")) //Path to csv file
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) //CSV Helper stuff
        {   
            csv.Read(); //CSV Helper method to read file
            csv.ReadHeader(); //CSV Helper method to read file headers
            while (csv.Read()) //While loop to read through the whole file
            {
                var record = new BonusTimeSplice //Create a new record every loop
                {
                    //Field Name = Get the float value from the field with the header name "Orange Text"
                    Time = csv.GetField<float>("Time"), 
                    Px = csv.GetField<float>("Px"),
                    Py = csv.GetField<float>("Py"),
                    Pz = csv.GetField<float>("Pz"),
                    Vx = csv.GetField<float>("Vx"),
                    Vy = csv.GetField<float>("Vy"),
                    Vz = csv.GetField<float>("Vz"),
                    Mass = csv.GetField<float>("Mass"),
                    MoonPx = csv.GetField<float>("MoonPx"),
                    MoonPy = csv.GetField<float>("MoonPy"),
                    MoonPz = csv.GetField<float>("MoonPz"),
                    MoonVx = csv.GetField<float>("MoonVx"),
                    MoonVy = csv.GetField<float>("MoonVy"),
                    MoonVz = csv.GetField<float>("MoonVz"),
                    WPSA = csv.GetField<float>("WPSA"),
                    WPSArange = csv.GetField<float>("WPSArange"),
                    DS54 = csv.GetField<float>("DS54"),
                    DS54range = csv.GetField<float>("DS54range"),
                    DS24 = csv.GetField<float>("DS24"),
                    DS24range = csv.GetField<float>("DS24range"),
                    DS34 = csv.GetField<float>("DS34"),
                    DS34range = csv.GetField<float>("DS34range")
                };
                bonusTimeSpliceList.Add(record); //Add the record to the list of time splices
            } 
        }
        foreach (var item in bonusTimeSpliceList)
        {
            //WPSA Link Budget Calculation
            item.LinkBudgetWPSA = CalculateLinkBudget(item.WPSA, item.WPSArange, 12);
            //DS54 Link Budget Calculation
            item.LinkBudgetDS54 = CalculateLinkBudget(item.DS54, item.DS54range, 34);
            //DS24 Link Budget Calculation
            item.LinkBudgetDS24 = CalculateLinkBudget(item.DS24, item.DS24range, 34);
            //DS34 Link Budget Calculation
            item.LinkBudgetDS34 = CalculateLinkBudget(item.DS34, item.DS34range, 34);
            //Create vectors for the moon position and the capsule position
            item.MoonPositionVector = CreateVector(item.MoonPx, item.MoonPy, item.MoonPz);
            item.MoonVelocityVector = CreateVector(item.MoonVx, item.MoonVy, item.MoonVz);
            item.BonusPositionVector = CreateVector(item.Px, item.Py, item.Pz);
            item.BonusVelocityVector = CreateVector(item.Vx, item.Vy, item.Vz);
            item.MoonResultantVelocity = CalculateResulantVelocity(item.MoonVx, item.MoonVy, item.MoonVz);
        }
        return bonusTimeSpliceList;
    }
    //Create vector3s
    private Vector3 CreateVector(float x, float y, float z)
    {
        return new Vector3(x, y, z);
    }
    private float CalculateResulantVelocity(float Vx, float Vy, float Vz)
    {
        return Mathf.Sqrt((Vx * Vx) + (Vy * Vy) + (Vz * Vz)); //Return calculated value
    }
    private float CalculateLinkBudget(float item, float range, float diameter)
    {
        //Constants
        float pi = Mathf.PI;
        float lambda = 0.136363636f; 
        float antenna = 0; //Link budget is set to 0 if availability is 0
        if (item != 0) //If availability is not 0, calculate link budget
        {
            antenna = Mathf.Pow(10, 0.10f * (-0.43f + 10 * Mathf.Log10(0.55f * Mathf.Pow((pi*diameter)/(lambda), 2)) - 20 * Mathf.Log10((4000 * pi * range)/(lambda)) + 228.6f - (10 * Mathf.Log10(222)))) / 1000;
        }
        antenna = antenna < 10000 ? antenna : 10000;
        return antenna; //Return link budget, either 0 or calculated value
    }
}
