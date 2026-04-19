 //Data Parser and Calculator
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;
using CsvHelper;
using System;
using System.Linq;
using UnityEngine.UIElements;
using JetBrains.Annotations;
using Unity.VisualScripting;

//How to install CSVHelper into a Unity Script:
//Go to https://www.nuget.org/packages/CsvHelper and hit download package on the right
//In your downloads, replace .nupkg with .zip and then unzip it
//Go into the the csvhelper.33.0.1 folder, go into the lib folder, then netstandard2.1
//Find the CsvHelper.dll file
//Put it in your Plugins folder for your project, if there is not one,
//Make a folder called Plugins in your Assets folder and drop it there


public class DataParser
{
    public CramersRule cramer = new CramersRule();
    public List<TimeSplice> CreateList()
    {
        List<TimeSplice> timeSpliceList = new List<TimeSplice>(); //Create list of TimeSplice objects
        using (var reader = new StreamReader("Assets/Data/Updated NASA Data.csv")) //Path to csv file
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) //CSV Helper stuff
        {   
            csv.Read(); //CSV Helper method to read file
            csv.ReadHeader(); //CSV Helper method to read file headers
            while (csv.Read()) //While loop to read through the whole file
            {
                var record = new TimeSplice //Create a new record every loop
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
                    WPSA = csv.GetField<float>("WPSA"),
                    WPSArange = csv.GetField<float>("WPSArange"),
                    DS54 = csv.GetField<float>("DS54"),
                    DS54range = csv.GetField<float>("DS54range"),
                    DS24 = csv.GetField<float>("DS24"),
                    DS24range = csv.GetField<float>("DS24range"),
                    DS34 = csv.GetField<float>("DS34"),
                    DS34range = csv.GetField<float>("DS34range")
                };
                timeSpliceList.Add(record); //Add the record to the list of time splices
            }       
        }
        SmoothXData(timeSpliceList);
        SmoothYData(timeSpliceList);
        SmoothZData(timeSpliceList);
        // SmoothVXData(timeSpliceList);
        // SmoothVYData(timeSpliceList);
        // SmoothVZData(timeSpliceList);    
        CalculatePositonVector(timeSpliceList);
        for (int i = 0; i < timeSpliceList.Count - 1; i++)
        {
            //make new velocities with position values
            timeSpliceList[i].NewVx = MakeNewVelocity(timeSpliceList[i+1].Px, timeSpliceList[i].Px, timeSpliceList[i+1].Time, timeSpliceList[i].Time);
            timeSpliceList[i].NewVy = MakeNewVelocity(timeSpliceList[i+1].Py, timeSpliceList[i].Py, timeSpliceList[i+1].Time, timeSpliceList[i].Time);
            timeSpliceList[i].NewVz = MakeNewVelocity(timeSpliceList[i+1].Pz, timeSpliceList[i].Pz, timeSpliceList[i+1].Time, timeSpliceList[i].Time);
        }
        foreach (var item in timeSpliceList) //Loop through time spice list
        {
            //WPSA Link Budget Calculation
            item.LinkBudgetWPSA = CalculateLinkBudget(item.WPSA, item.WPSArange, 12);
            //DS54 Link Budget Calculation
            item.LinkBudgetDS54 = CalculateLinkBudget(item.DS54, item.DS54range, 34);
            //DS24 Link Budget Calculation
            item.LinkBudgetDS24 = CalculateLinkBudget(item.DS24, item.DS24range, 34);
            //DS34 Link Budget Calculation
            item.LinkBudgetDS34 = CalculateLinkBudget(item.DS34, item.DS34range, 34);
            //Resultant Velocity Calculation
            item.ResultantVelocity = CalculateResulantVelocity(item.Vx, item.Vy, item.Vz);
            //Create Position Vectors
            item.PositionVector3 = CreateVector(item.Px, item.Py, item.Pz);
            //Create Velocity Vectors
            item.VelocityVector3 = CreateVector(item.Vx, item.Vy, item.Vz);
            
            item.SmoothPositionVector3 = CreateVector(item.SmoothX, item.SmoothY, item.SmoothZ);
            //Create new velocity vector
            item.NewVelocityVector = CreateVector(item.NewVx, item.NewVy, item.NewVz);
            //Round numbers to however many sig figs is in the second parameter
            item.RoundedResultantVelocity = (float)RoundNumber(item.ResultantVelocity, 5);
            item.RoundedVx = (float)RoundNumber(item.Vx, 4);
            item.RoundedVy = (float)RoundNumber(item.Vy, 4);
            item.RoundedVz = (float)RoundNumber(item.Vz, 4);
        }
        //Do this after since theres a for loop built into the function
        CalculateTotalDistance(timeSpliceList);
        //return the list yay!
        return timeSpliceList;
    }
    
    //Calculate link budget based on availability, slant range, and diameter of the antenna
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
    //Calculate resultant velocity based on velocity components
    private float CalculateResulantVelocity(float Vx, float Vy, float Vz)
    {
        return Mathf.Sqrt((Vx * Vx) + (Vy * Vy) + (Vz * Vz)); //Return calculated value
    }
    //Create Vector3s
    private Vector3 CreateVector(float x, float y, float z)
    {
        return new Vector3(x, y, z); 
    }
    //Caluclate the total distance traveled at each time splice
    private void CalculateTotalDistance(List<TimeSplice> timeSplices)
    {
        float DataDistanceTraveled = 0;
        // float SmoothedDistanceTraveled = 0;
        for (int i = 0; i < timeSplices.Count; i++)
        {
            if (i == 0)
            {
                //cheese it at zero!
                timeSplices[i].DataTotalDistance = DataDistanceTraveled;
                // timeSplices[i].SmoothedTotalDistance = SmoothedDistanceTraveled;
            }
            else
            {
                //find the distance between the last point and the current point and add it to the total distance
                DataDistanceTraveled = DataDistanceTraveled + Vector3.Distance(timeSplices[i - 1].PositionVector3, timeSplices[i].PositionVector3);
                // SmoothedDistanceTraveled = SmoothedDistanceTraveled + Vector3.Distance(timeSplices[i - 1].CalculatedPositionVector3, timeSplices[i].CalculatedPositionVector3);
                //set the value at each timesplice to the number above
                timeSplices[i].DataTotalDistance = DataDistanceTraveled;
                // timeSplices[i].SmoothedTotalDistance = SmoothedDistanceTraveled;
            }
        }  
    }
    // Create new position vectors using the inital position and velocities
    private void CalculatePositonVector(List<TimeSplice> timeSplices)
    {
        //Set variables equal to 0 to not cause a scope error
        float currentX = 0;
        float currentY = 0;
        float currentZ = 0;
        for (int i = 0; i < timeSplices.Count; i++)
        {
            if (i == 0 || i == 1)
            {
                //Create the initial and second vector3 since the data is wonky so we cheesed the second vector3
                currentX = timeSplices[i].Px;
                currentY = timeSplices[i].Py;
                currentZ = timeSplices[i].Pz;
                //Create the inital vector3
                timeSplices[i].CalculatedPositionVector3 = new Vector3(currentX, currentY, currentZ);
            }
            else if (i < timeSplices.Count - 1)
            {
                //Find the difference between times in minutes
                float distanceBetweenTimes = timeSplices[i].Time - timeSplices[i - 1].Time;
                //Convert to seconds since velocities are in km/s
                float distanceInSeconds = distanceBetweenTimes * 60;

                //Find the distance traveled based on the average of the previous and current velocity
                float XTraveled = (timeSplices[i - 1].Vx + timeSplices[i].Vx) / 2  * distanceInSeconds;
                float YTraveled = (timeSplices[i - 1].Vy + timeSplices[i].Vy) / 2  * distanceInSeconds;
                float ZTraveled = (timeSplices[i - 1].Vz + timeSplices[i].Vz) / 2  * distanceInSeconds;

                //Add the prev
                currentX = currentX + XTraveled;
                currentY = currentY + YTraveled;
                currentZ = currentZ + ZTraveled;

                timeSplices[i].CalculatedPositionVector3 = new Vector3(currentX, currentY, currentZ);
            }
            else 
            {
                //Create the last vector3 since the data is wonky and we cheesed it
                currentX = timeSplices[i].Px;
                currentY = timeSplices[i].Py;
                currentZ = timeSplices[i].Pz;
                timeSplices[i].CalculatedPositionVector3 = new Vector3(currentX, currentY, currentZ);
            }
        }  
    }

    //rounding number function i found!!
    // C# program to round-off a number 
// to given no. of significant digits 
	static double RoundNumber(double N, double n) 
	{ 
		int h; 
		double b, d, e, i, j, m, f; 
		b = N; 
		// c = Math.Floor(N); 

		// Counting the no. of digits to the 
		// left of decimal point in the given no. 
		for (i = 0; b >= 1; ++i) 
			b = b / 10; 

		d = n - i; 
		b = N; 
		b = b * Math.Pow(10, d); 
		e = b + 0.5; 
		if ((float)e == (float)Math.Ceiling(b)) { 
			f = (Math.Ceiling(b)); 
			h = (int)(f - 2); 
			if (h % 2 != 0) { 
				e = e - 1; 
			} 
		} 
		j = Math.Floor(e); 
		m = Math.Pow(10, d); 
		j = j / m; 
		Console.WriteLine("The number after " + 
					"rounding-off is " + j); 
        return j;
        // This code is contributed by vt_m.
	} 

    public float MakeNewVelocity(float nextPosition, float currentPosition, float nextTime, float currentTime)
    {
        return (nextPosition - currentPosition) / ((nextTime - currentTime) * 60);
    }
    private void SmoothXData(List<TimeSplice> timeSplices)
    {
        List<double> numbers = new List<double>();
        List<double> unalteredPositions = new List<double>();
        List<double> unalteredTimes = new List<double>();
        double a;
        double b;
        double c;
        double newPosition;
        double previousX;
        double currentX;
        double nextX;
        double previousTime;
        double currentTime;
        double nextTime;
        double middleTime;
        for (int i = 1; i < timeSplices.Count; i++)
        {
            
            previousX = (double)timeSplices[i-1].Px;
            currentX = (double)timeSplices[i].Px;
            currentTime = (double)timeSplices[i].Time;

            if (previousX != currentX)
            {
                timeSplices[i].SmoothX = (float)currentX;
                unalteredPositions.Add(currentX);
                unalteredTimes.Add(currentTime);
            }
            else
            {
                previousX = unalteredPositions[unalteredPositions.Count - 2];
                previousTime = unalteredTimes[unalteredTimes.Count - 2];
                currentX = unalteredPositions[unalteredPositions.Count - 1];
                middleTime = unalteredTimes[unalteredTimes.Count - 1];
                int j = i;
                while(timeSplices[j].Px == timeSplices[j+1].Px)
                {
                    j++;
                }
                nextX = timeSplices[j + 1].Px;
                nextTime = timeSplices[j + 1].Time;
                numbers.Clear();
                numbers.Add(previousTime * previousTime);
                numbers.Add(previousTime);
                numbers.Add(1);
                numbers.Add(previousX);
                numbers.Add(middleTime * middleTime);
                numbers.Add(middleTime);
                numbers.Add(1);
                numbers.Add(currentX);
                numbers.Add(nextTime * nextTime);
                numbers.Add(nextTime);
                numbers.Add(1);
                numbers.Add(nextX);

                List<double> coeffs = cramer.Main(numbers);
            
                a = coeffs[0];
                b = coeffs[1];
                c = coeffs[2];

                newPosition = (a * currentTime * currentTime) + (b * currentTime) + c;
                timeSplices[i].SmoothX = (float)newPosition;
            }
        }
    }
    private void SmoothYData(List<TimeSplice> timeSplices)
    {
        List<double> numbers = new List<double>();
        List<double> unalteredPositions = new List<double>();
        List<double> unalteredTimes = new List<double>();
        double a;
        double b;
        double c;
        double newPosition;
        double previousY;
        double currentY;
        double nextY;
        double previousTime;
        double currentTime;
        double nextTime;
        double middleTime;
        for (int i = 1; i < timeSplices.Count; i++)
        {
            
            previousY = (double)timeSplices[i-1].Py;
            currentY = (double)timeSplices[i].Py;
            currentTime = (double)timeSplices[i].Time;

            if (previousY != currentY)
            {
                timeSplices[i].SmoothY = (float)currentY;
                unalteredPositions.Add(currentY);
                unalteredTimes.Add(currentTime);
            }
            else
            {
                previousY = unalteredPositions[unalteredPositions.Count - 2];
                previousTime = unalteredTimes[unalteredTimes.Count - 2];
                currentY = unalteredPositions[unalteredPositions.Count - 1];
                middleTime = unalteredTimes[unalteredTimes.Count - 1];
                int j = i;
                while(timeSplices[j].Py == timeSplices[j+1].Py)
                {
                    j++;
                }
                nextY = timeSplices[j + 1].Py;
                nextTime = timeSplices[j + 1].Time;
                numbers.Clear();
                numbers.Add(previousTime * previousTime);
                numbers.Add(previousTime);
                numbers.Add(1);
                numbers.Add(previousY);
                numbers.Add(middleTime * middleTime);
                numbers.Add(middleTime);
                numbers.Add(1);
                numbers.Add(currentY);
                numbers.Add(nextTime * nextTime);
                numbers.Add(nextTime);
                numbers.Add(1);
                numbers.Add(nextY);

                List<double> coeffs = cramer.Main(numbers);
            
                a = coeffs[0];
                b = coeffs[1];
                c = coeffs[2];

                newPosition = (a * currentTime * currentTime) + (b * currentTime) + c;
                timeSplices[i].SmoothY = (float)newPosition;
            }
        }
    }
    private void SmoothZData(List<TimeSplice> timeSplices)
    {
        List<double> numbers = new List<double>();
        List<double> unalteredPositions = new List<double>();
        List<double> unalteredTimes = new List<double>();
        double a;
        double b;
        double c;
        double newPosition;
        double previousZ;
        double currentZ;
        double nextZ;
        double previousTime;
        double currentTime;
        double nextTime;
        double middleTime;
        for (int i = 1; i < timeSplices.Count; i++)
        {
            
            previousZ = (double)timeSplices[i-1].Pz;
            currentZ = (double)timeSplices[i].Pz;
            currentTime = (double)timeSplices[i].Time;

            if (previousZ != currentZ)
            {
                timeSplices[i].SmoothZ = (float)currentZ;
                unalteredPositions.Add(currentZ);
                unalteredTimes.Add(currentTime);
            }
            else
            {
                previousZ = unalteredPositions[unalteredPositions.Count - 2];
                previousTime = unalteredTimes[unalteredTimes.Count - 2];
                currentZ = unalteredPositions[unalteredPositions.Count - 1];
                middleTime = unalteredTimes[unalteredTimes.Count - 1];
                int j = i;
                while(timeSplices[j].Pz == timeSplices[j+1].Pz)
                {
                    j++;
                }
                nextZ = timeSplices[j + 1].Pz;
                nextTime = timeSplices[j + 1].Time;
                numbers.Clear();
                numbers.Add(previousTime * previousTime);
                numbers.Add(previousTime);
                numbers.Add(1);
                numbers.Add(previousZ);
                numbers.Add(middleTime * middleTime);
                numbers.Add(middleTime);
                numbers.Add(1);
                numbers.Add(currentZ);
                numbers.Add(nextTime * nextTime);
                numbers.Add(nextTime);
                numbers.Add(1);
                numbers.Add(nextZ);

                List<double> coeffs = cramer.Main(numbers);
            
                a = coeffs[0];
                b = coeffs[1];
                c = coeffs[2];

                newPosition = (a * currentTime * currentTime) + (b * currentTime) + c;
                timeSplices[i].SmoothZ = (float)newPosition;
            }
        }
    }  
}