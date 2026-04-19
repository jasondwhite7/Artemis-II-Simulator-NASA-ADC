//Data Storer
using System.Collections.Generic;
using UnityEngine;

public class DataStorer
{
    //Creating a list of TimeSplice objects
    public List<TimeSplice> timeSpliceList; 
    //same for bonus
    public List<BonusTimeSplice> bonusTimeSpliceList; 
    //Instantiating the DataParser class from the DataParser script
    public DataParser parser = new DataParser();
    //same for bonus
    public BonusDataParser bonusParser = new BonusDataParser();
    public void ParseData()
    {
        //Call the CreateList function from the DataParser class
        timeSpliceList = parser.CreateList();
        //Do the same for the bonus data
        bonusTimeSpliceList = bonusParser.CreateList();
    } 
}