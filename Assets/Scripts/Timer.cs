using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEditor.Rendering;
using System.Data.Common;

public class Timer : MonoBehaviour
{
   [Header("Component")]
   //main timer text
   public TextMeshProUGUI timerText;
   //initial timer speed
   public float timerSpeed = 1;
   //speed slider
   public Slider slider;
   public TextMeshProUGUI simSpeedText;
   //list of timesplices
   public List<TimeSplice> timeSplices;
   public List<BonusTimeSplice> bonusTimeSplices;

   [Header("Timer Settings")]
   //current time in seconds
   public float currentTime;
   //current and previous indexes
   private int currentIndex;
   private int previousIndex = 0;
   private int previousBonusIndex = 0;
   //if the timer is active or not
   public bool timerIsActive;
   //bools to only run the first index once
   bool firstIndexRan;
   bool firstBonusIndexRan;
   //time in minutes
   float realMinutes;
   //DO NOT ADJUST APPROXIMATION
   //public const double approximation = 0.001;
   //instantiate all the stuff, each of these are a dufferent script
   private DataStorer ds;
   private VelocityWidgetController widget;
   private MissionLine line;
   private MoonController moon;
   private CapsuleController capsule;
   private EarthController earth;
   private DisplayVelocityText velocityText;
   private TotalDistance totalDistance;
   private AnimationController animator;
   private AnimationToggle animationToggle;
   private MarioMode userMarioMode;
   private NotificationController notification;
   private ColorKey colorKey;
   private MoonPhase moonPhase;
   [Header("Game Objects")]
   //public gameobjects which have the scripts above 
   public GameObject Earth;
   public GameObject Moon;
   public GameObject Capsule;
   public GameObject CapsuleParent;
   public GameObject Velocities;
   public GameObject Distance;
   public GameObject AntennaDisplay;
   public GameObject VelocityWidget;
   public GameObject Lines;
   public GameObject MoonPhase;
   public GameObject AnimationButton;
   public GameObject MarioModeButton;
   public GameObject Notification;
   public GameObject ColorKey;
   public GameObject BulletBill;
   public GameObject BigBulletBill;
   public GameObject RedDot;
   //timer stuff by isaac
   private float timerOne;
   [Header("Timers")]
   public TextMeshProUGUI timerOneText;
   public TextMeshProUGUI countdownTimerText;
   public TextMeshProUGUI currentPhase;
   public GameObject PauseImage;
   public GameObject PlayImage;
   private bool cdtIsActive;
   private bool cdtIsPaused;
   private bool firstRunThrough;
   private float countdownTimer;
   private bool secondRunThrough;
   private bool thirdRunThrough;
   private float nextCountdown;
   //Bool to detect if user wants to slow down for animations or no
   private bool slowDown;
   //bool to detect if user has mario mode toggled on or off
   private bool marioModeToggle;
   //bool to detect if mario mode should be turned on or not and if it's on
   private bool shouldBeMarioMode;
   private bool isMarioMode;
   //bool to detect if they have discovered mario mode or not
   private bool hasDiscoveredMarioMode;
//DON'T CHANGE
   //runs on start
   public void Start()
   {
       //instantiates the datastore
       ds = new DataStorer();
       //Parses the data
       ds.ParseData();
       //gets the timesplices
       timeSplices = ds.timeSpliceList;
       bonusTimeSplices = ds.bonusTimeSpliceList;
       //debug to test if found all timeSplices
       Debug.Log($"Timer Prepped - {timeSplices.Count} timesplices found...");
       //start the timer!
       timerIsActive = true;
       //initialize all of the stuff, basically set them equal to a script which can be called later
       widget = VelocityWidget.GetComponent<VelocityWidgetController>();
       line = Lines.GetComponent<MissionLine>();
       capsule = CapsuleParent.GetComponent<CapsuleController>();
       earth = Earth.GetComponent<EarthController>();
       moon = Moon.GetComponent<MoonController>();
       moonPhase = MoonPhase.GetComponent<MoonPhase>();
       velocityText = Velocities.GetComponent<DisplayVelocityText>();
       totalDistance = Distance.GetComponent<TotalDistance>();
       animator = Capsule.GetComponent<AnimationController>();
       animationToggle = AnimationButton.GetComponent<AnimationToggle>();
       userMarioMode = MarioModeButton.GetComponent<MarioMode>();
       notification = Notification.GetComponent<NotificationController>();
       colorKey = ColorKey.GetComponent<ColorKey>();
       AntennaDisplay.SendMessage("CreateLists", timeSplices);
       AntennaDisplay.SendMessage("CreateOffNomLists", bonusTimeSplices);
       //draw the mission paths
       line.DrawMissionPaths(timeSplices, bonusTimeSplices);
       //rotate everything but the earth for dr harmon
       transform.Rotate(90, 0, 23.5f);
       //issac stuff
       cdtIsActive = true;
       firstRunThrough = true;
       secondRunThrough = false;
       thirdRunThrough = false;
       timerSpeed = 1;
       timerOne = 12347;
       nextCountdown = timeSplices[193].Time * 60;
       countdownTimer = nextCountdown;
       currentPhase.text = "Time Until Second Stage Separates";
       simSpeedText.text = $"Simulation Speed: {timerSpeed}";
       isMarioMode = false;
       hasDiscoveredMarioMode = false;
       marioModeToggle = true;
   }
   public void SliderChanged()
   {
        //when the slider is changed set sim speed to the value, and send it to all the elements that need it
        timerSpeed = slider.value;
        simSpeedText.text = $"Simulation Speed: {timerSpeed}";
        SendSimSpeed(timerSpeed);
   }
   private void TurnOnMarioMode()
   {
        BulletBill.SetActive(true);
        BigBulletBill.SetActive(true);
        Capsule.SetActive(false);
        RedDot.SetActive(false);
        isMarioMode = true;
   }
   private void TurnOffMarioMode()
   {
        BulletBill.SetActive(false);
        BigBulletBill.SetActive(false);
        Capsule.SetActive(true);
        RedDot.SetActive(true);
        isMarioMode = false;
   }
   public void ToggleMarioMode()
   {
        marioModeToggle = userMarioMode.ToggleMarioMode();
        if (marioModeToggle == false && isMarioMode == true)
        {
            TurnOffMarioMode();
        }
   }
   //called every frame
   private void Update()
   {
    if (marioModeToggle)
    {
        if (timerSpeed == slider.maxValue)
        {
            shouldBeMarioMode = colorKey.IsMarioMode();
            if (!isMarioMode && shouldBeMarioMode)
            {
                if (!hasDiscoveredMarioMode)
                {
                    hasDiscoveredMarioMode = true;
                    userMarioMode.DiscoverMarioMode();
                    notification.CueNotification();
                }
                TurnOnMarioMode();
            }
            if (isMarioMode && !shouldBeMarioMode)
            {
                TurnOffMarioMode();
            }
        }
        else 
        {
            TurnOffMarioMode();
        }
    }
    

    //DO NOT MESS WITH, This is the timer itself
    //var niceTime = displayTime(currentTime);
    /*int hours = Mathf.FloorToInt(realMinutes / 3600F);
    int minutes = Mathf.FloorToInt(realMinutes - hours * 3600);
    int seconds = Mathf.FloorToInt(realMinutes - (hours * 3600) - (minutes * 60));*/
    // Debug.Log(realMinutes);
    //string niceTime = currentTime.ToString(@"hh\:mm\:ss\.fff");
    //string niceTime = string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);
    //timerText.text = currentTime.ToString();
    //Debug.Log($"indexOfList = {indexOfList} --- timeSplices Count = {timeSplices.Count}");
    // var currentSplice = timeSplices[indexOfList];
    // var currentBonusSplice = bonusTimeSplices[bonusIndexOfList];
    // var nextTimeToFind = currentSplice.Time;
    // var bonusNextTimeToFind = currentBonusSplice.Time;
    //Debug.Log($"nextTimeToFind = {nextTimeToFind}");

    //isaac stuff
   if(previousIndex >= 193)
    {
        if(firstRunThrough)
        {
            firstRunThrough = false;
            secondRunThrough = true;
            nextCountdown = (timeSplices[12953].Time - timeSplices[previousIndex].Time) * 60;
            // Debug.Log("Number here: "+nextCountdown);
            countdownTimer = nextCountdown;
            currentPhase.text = "Time Until Service Module Separates";
            if (slowDown)
            {  
                if (!isMarioMode)
                {
                    animator.RunSecondStage();
                    slider.value = 420;
                    timerSpeed = 420;
                    simSpeedText.text = $"Simulation Speed: {timerSpeed}";
                    SendSimSpeed(timerSpeed);
                }
            }
            else
            {
                animator.HideSecondStage();
            }
        }
        // Debug.Log("timer is: " + cdtIsActive);
    }

    if(previousIndex > 12953)
    {
        if(secondRunThrough)
        {
            nextCountdown = (timeSplices[timeSplices.Count - 1].Time - timeSplices[previousIndex].Time) * 60;
            secondRunThrough = false;
            thirdRunThrough = true;
            countdownTimer = nextCountdown;
            currentPhase.text = "Time Until Splashdown";
            if (slowDown)
            {
                if (!isMarioMode)
                {
                    slider.value = 60;
                    timerSpeed = 60;
                    simSpeedText.text = $"Simulation Speed:\n{timerSpeed}";
                    SendSimSpeed(timerSpeed);
                    animator.RunServiceModule();
                }
            }
            else
            {
               
                animator.HideServiceModule();
            }
        }
        // Debug.Log("timer is: " + cdtIsActive);
    }
    if(previousIndex >= timeSplices.Count - 1)
    {
        if(thirdRunThrough)
        {
            thirdRunThrough = false;
            cdtIsActive = false;
            countdownTimer = 0;
            currentPhase.text = "Welcome Home";
            if (!isMarioMode)
            {
                animator.RunSplashDown();
            }
        }
        // Debug.Log("timer is: " + cdtIsActive);
    }
    if(cdtIsActive)
    {
        // Debug.Log("CountDown is at: "+countdownTimer);
        // Debug.Log("PreviousIndex: "+ previousIndex);
        countdownTimer = countdownTimer >= 0 ? countdownTimer - (Time.deltaTime * timerSpeed): 0;            
        countdownTimerText.text = DisplayTime(countdownTimer, true);

    }
    else if (cdtIsPaused)
    {
        //do nothing
    }
    else 
    {
        countdownTimerText.text = DisplayTime(0, true);
    }
    if(timerIsActive == true)
    {
        //increment time in seconds, calculate time in minutes
        currentTime =  currentTime + (Time.deltaTime * timerSpeed);
        realMinutes = currentTime / 60;

        //isaac stuff
        timerOne =  timerOne + (Time.deltaTime * timerSpeed);
        // Debug.Log("TimerOne = "+timerOne);

        timerText.text = "T+:"+DisplayTime(currentTime, true);
        timerOneText.text = DisplayTime(timerOne, false) + " UTC";
        // Debug.Log($"Previous Index Before Is: {previousIndex}");

        //call these every frame, they decide whether to update or not, it will always update the value
        //of previous index, but it may be the same number as it was before
        previousIndex = UpdateIfAppropiate(timeSplices, previousIndex, realMinutes);
        previousBonusIndex = UpdateBonusIfAppropiate(bonusTimeSplices, previousBonusIndex, realMinutes);


        // Debug.Log($"Previous Index After Is: {previousIndex}");


        // if(numberCloseEnough(nextTimeToFind, realMinutes))
        // {
        //     Debug.Log("numberCloseEnough Ran, time: " + indexOfList + "," + realMinutes);


        //     SendNormalUpdates(currentSplice);
            
        //     indexOfList++;
            
        //     Debug.Log($"indexOfList = {indexOfList}");
        // }
        // if(numberCloseEnough(bonusNextTimeToFind, realMinutes))
        // {
        //     SendBonusUpdates(currentBonusSplice);
        //     bonusIndexOfList++;
        // }
        // while((Mathf.Abs((float)nextTimeToFind - realMinutes) < (float)approximation) && Mathf.Abs((float)nextTimeToFind) > realMinutes)
        // {
        //     indexOfList++;
        // }
    }
    
    // Debug.Log(niceTime);
    //if numberCloseEnough resulted true, do whatever it needs to at that time
    
   }
   /*public string displayTime(float timeInSeconds)
   {
       int minutes = Mathf.FloorToInt(realMinutes / 60F);
       int seconds = Mathf.FloorToInt(realMinutes - minutes * 60);
       string niceTime = realMinutes.ToString();
       return niceTime;
   }*/
   /* Check's if the time in the list is close enough to the current time,
   results in true or false */
   // public bool numberCloseEnough(double nextTime , float realMinutes)
   // {
   //     // if next time subtracted by the current time is < 0.001 then its close enough to run
   //     if (Mathf.Abs((float)nextTime - realMinutes) < (float)approximation)
   //         {
   //             return true;
   //          // if it missed the the next time and the current time is actually greater than the time in the list
   //         }else if(realMinutes> (float)nextTime)
   //         {
   //             return true;
   //         }
   //         //not the right time, don't do anything
   //             return false;
          
   // }

   public void SendNormalUpdates(TimeSplice currentSplice, int indexOfList)
   {
       // Send Updates To  Elements
       
       if (indexOfList == 12977)
       {
        //cheese the last index by making a new last final position so the capsule has something to look at
           capsule.MoveCapsule(currentSplice.PositionVector3, new Vector3(6318.936f,1504.452f,-162.0853f));
       }
       else
       {
        //send the position to move to and the position to look at
        capsule.MoveCapsule(currentSplice.PositionVector3, timeSplices[indexOfList + 1].PositionVector3);
       }
       //send the velocity to move in if sim speed is low enough
       capsule.RecieveVelocityVector(currentSplice.NewVelocityVector);
       //change opacities but its too slow so its commented out
        //capsule.ChangeOpacities(indexOfList);

        //send the velocity data to the widget, multiplied by 50 so the cylinders arent tiny
       widget.VelocityWidget(currentSplice.Vx * 50, currentSplice.Vy * 50, currentSplice.Vz * 50, currentSplice.ResultantVelocity * 20);

       //for display velocities script
       velocityText.DisplayVelocity(currentSplice);

        //send the total distance so it can be displayed
       totalDistance.DisplayTotalDistance(currentSplice.DataTotalDistance);

        AntennaDisplay.SendMessage("PickAntenna", indexOfList);
       // for antenna display scripts
    //    AntennaDisplay.SendMessage("WPSADisplay", currentSplice.LinkBudgetWPSA);
    //    AntennaDisplay.SendMessage("DS54Display", currentSplice.LinkBudgetDS54);
    //    AntennaDisplay.SendMessage("DS24Display", currentSplice.LinkBudgetDS24);
    //    AntennaDisplay.SendMessage("DS34Display", currentSplice.LinkBudgetDS34);
       // Debug.Log($"Index of List is {indexOfList}");
       

        //send the time so the earth can rotate to where it should be
       earth.RotateEarth(currentSplice.Time);

        //send current index so trail and dynamic lines can be updated appropiately
       line.ChangeDynamicColor(indexOfList);
   }


   public void SendBonusUpdates(BonusTimeSplice currentSplice)
   {
        //move the moon, send its velocity which is needed in case sim speed is low, and send time to make sure its right
       moon.MoveMoon(currentSplice.MoonPositionVector);
       moon.RecieveVelocityVector(currentSplice.MoonVelocityVector);
       moonPhase.UpdateMoonPhase(currentSplice.Time);
   }


   public int UpdateIfAppropiate(List<TimeSplice> timeSplices, int previousIndex, float currentTime)
   {
        //int to be used and incremented
       int i = 1;
       //if current time is greater than the last time in the data
       if (currentTime >= timeSplices[timeSplices.Count - 1].Time)
       {
            currentIndex = timeSplices.Count - 1;
            //stop the flipping timer :()
           timerIsActive = false;
            //update elements at the last time splice
           SendNormalUpdates(timeSplices[timeSplices.Count - 1], timeSplices.Count - 1);  
           //return this, idk if you need this but its doing no harm
           return timeSplices.Count - 1;
       }
       //if the time at the next index is less than the current time, no need to display it since it 
        // probably got frame skipped. the while loop stops once it reaches a time value greater than the
        //current time, meaning weve reached a timesplice not yet accounted for
       while(timeSplices[previousIndex + i].Time <= currentTime)
       {
            //increment i when the condition above is met
           i++;
       }
       //if we are at the start and the first index has not been displayed yet, this is omega cheese
       if (previousIndex == 0 && firstIndexRan == false)
       {
            //update elements at the first index
           SendNormalUpdates(timeSplices[previousIndex], previousIndex);
           //set this to true so it doesnt update again
           firstIndexRan = true;
           //return 0
           return previousIndex;
       }
       if (i == 1)
       {
            //sim speed is slower than 1 timesplice/frame, so displaying another timesplice
            //would be big bad. basically the next time value in the data is greater than the current
            //time so we havent reached the time to display that time splice
           return previousIndex;
           //Do nothing
       }
       else
       {  
            //yay we get to display something! display the time splice at the last index that the while 
            //condition was met. this is the most appropiate timesplice to show this frame, since if the 
            //mimus 1 wasnt there, wed technically be displaying a timesplice we havent reached yet
           currentIndex = previousIndex + i - 1;
            //send the updates at this index
           SendNormalUpdates(timeSplices[currentIndex], currentIndex);
           //return it yay!
           return currentIndex;
       }      
   }
   public int UpdateBonusIfAppropiate(List<BonusTimeSplice> timeSplices, int previousIndex, float currentTime)
   {
        //literally copy and pasted from the one above, has to be a new function since time values
        //in the bonus data are different
       int i = 1;
       if (currentTime >= timeSplices[timeSplices.Count-1].Time)
       {
           SendBonusUpdates(timeSplices[timeSplices.Count - 1]);
           SendTimerStatus(timerIsActive);
           return timeSplices.Count - 1;
       }
       while(timeSplices[previousIndex + i].Time <= currentTime)
       {
           i++;
       }
       if (previousIndex == 0 && firstBonusIndexRan == false)
       {
           firstBonusIndexRan = true;
           SendTimerStatus(timerIsActive);
           SendBonusUpdates(timeSplices[previousBonusIndex]);
       }        
       if (i == 1)
       {
           return previousIndex;
           //Do nothing
       }
       else
       {
          
           currentIndex = previousIndex + i - 1;
           SendBonusUpdates(timeSplices[currentIndex]);
           return currentIndex;
       }      
   }
   public void SendSimSpeed(float speed)
   {
        //send the sim speed to everything that needs it
       earth.RecieveSimSpeed(speed);
       moon.RecieveSimSpeed(speed);
       capsule.RecieveSimSpeed(speed);
       moonPhase.RecieveSimSpeed(speed);
   }
   public void SendTimerStatus(bool timerIsActive)
   {
        //send the timer status to everything that needs it
       earth.StopTimer(timerIsActive);
       moon.StopTimer(timerIsActive);
       capsule.StopTimer(timerIsActive);
       moonPhase.StopTimer(timerIsActive);
   }
   public void Restart()
   {
        timerIsActive = true;
        cdtIsActive = true;
        currentTime = 0;
        previousIndex = 0;
        previousBonusIndex = 0;
        firstIndexRan = false;
        firstBonusIndexRan = false;
        firstRunThrough = true;
        secondRunThrough = false;  
        thirdRunThrough = false;
        currentPhase.text = "Time Until Second Stage Separates";
        nextCountdown = timeSplices[193].Time * 60;
        countdownTimer = nextCountdown;
        AntennaDisplay.SendMessage("ResetSwitches");
        animator.RestartCapsule();
        line.RestartDynamicTrail(currentIndex);
   }
   public void Pause()
   {
        if (timerIsActive)
        {
            timerIsActive = false;
            cdtIsActive = false;
            cdtIsPaused = true;
            SendTimerStatus(timerIsActive);
            PauseImage.SetActive(false);
            PlayImage.SetActive(true);
        }
        else
        {
            timerIsActive = true;
            cdtIsActive = true;
            cdtIsPaused = false;
            SendTimerStatus(timerIsActive);
            PauseImage.SetActive(true);
            PlayImage.SetActive(false);
        }
   }
   public void ToggleAnimations()
    {
        slowDown = animationToggle.ToggleAnimations();
    }
   //for the antenna toggles
   public void SwitchAlgorithm(string algorithm)
   {
        AntennaDisplay.SendMessage("SwitchAlgorithm", new object[] {algorithm, currentIndex});
   }
   public void SwitchNom(bool nom) {
        AntennaDisplay.SendMessage("SwitchNom", new object[] {nom, currentIndex});
   }
   //isaac stuff
   private String ZeroPadTo2Places(int theNumber){
       var theString = $"0{theNumber}";
       return theString.Substring(theString.Length - 2);
   }
   private String ZeroBeforeSeconds(float seconds)
   {
       if(seconds < 10)
       {
           return "0";
       }else
       {
           return null;
       }
   }
   public List<int> Times(float amountOfTime, bool needDays)
   {
       int days = (Mathf.FloorToInt(amountOfTime) /  86400);       
       int hours = ((Mathf.FloorToInt(amountOfTime) - days * 86400)) / 3600;
       int minutes = (Mathf.FloorToInt(amountOfTime) - days * 86400 - hours * 3600) / 60;
       float seconds = (Mathf.FloorToInt(amountOfTime) - days * 86400 - hours * 3600 - minutes * 60);
       if(needDays == true)
       {
           return new List<int>{days,hours,minutes,Mathf.FloorToInt(seconds)};
       }else{
           return new List<int>{hours,minutes,Mathf.FloorToInt(seconds)};
       }
   }
   public String DisplayTime(float amountOfTime, bool needDays)
   {
       if(needDays)
       {
           return ZeroPadTo2Places(Times(amountOfTime, true)[0])+":"+ZeroPadTo2Places(Times(amountOfTime, true)[1])+":"+ZeroPadTo2Places(Times(amountOfTime, true)[2])+":"+ZeroPadTo2Places(Times(amountOfTime, true)[3]);
       }else
       {
           return ZeroPadTo2Places(Times(amountOfTime, false)[0])+":"+ZeroPadTo2Places(Times(amountOfTime, false)[1])+":"+ZeroPadTo2Places(Times(amountOfTime, false)[2]);
       }
   }
}

