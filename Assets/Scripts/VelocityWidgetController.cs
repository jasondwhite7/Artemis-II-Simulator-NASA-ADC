//Velocity Widget
using UnityEngine;
public class VelocityWidgetController : MonoBehaviour
{
    //Declaring master object (Controller), which all others should be children of
    //Declaring transform components and the target to look at
    public GameObject Controller;
    public Transform CylinderX;
    // public GameObject ConeX;
    public Transform CylinderY;
    // public GameObject ConeY;
    public Transform CylinderZ;
    // public GameObject ConeZ;
    //The base of Cylinder R must be set at 0, 0, 0 of the pivot
    public Transform CylinderR;
    // public GameObject ConeR;
    //Pivot must be set at the middle of where we want the widget to be
    public Transform Pivot;
    private Vector3 target;


    // public void InitializeVelocityWidget()
    // {
    //     //Find main object with the name VelocityWidget
    //     //Orange Text match the name of the GameObject in Unity
    //     Controller = GameObject.Find("Velocity Widget");

    //     //Find the children using transform.Find of the master object
    //     CylinderX = Controller.transform.Find("Cylinder X");
    //     CylinderY = Controller.transform.Find("Cylinder Y");
    //     CylinderZ = Controller.transform.Find("Cylinder Z");
    //     Pivot = Controller.transform.Find("Pivot");

    //     //Since Cylinder R is a child of Pivot, it must use Pivot.transform instead of Controller.transform
    //     CylinderR = Pivot.transform.Find("Cylinder R");
    // }
    
    //Function to be called in the timer with the velocity parameters
    public void VelocityWidget(float Vx, float Vy, float Vz, float Vr)
    { 
        //Creating a target for the LookAt method to look at, add values to make it work no matter where the widget is
        target = new Vector3(Vx + Controller.transform.position.x, Vy + 
        Controller.transform.position.y, Vz + Controller.transform.position.z);

        //Set the position of each cylinder based on the respective velocities
        CylinderX.transform.localPosition = new Vector3(Vx, 0, 0);
        CylinderY.transform.localPosition = new Vector3(0, Vy, 0);
        CylinderZ.transform.localPosition = new Vector3(0, 0, Vz);

        //By scaling the objects at the same magnitude as the position, it should 
        //scale it in one direction.
        //All the 1's will be changed eventually to a better number to fit our canvas better
        CylinderX.transform.localScale = new Vector3(10f, Vx, 10f);
        CylinderY.transform.localScale = new Vector3(10f, Vy, 10f);
        CylinderZ.transform.localScale = new Vector3(10f, Vz, 10f);
        
        //Same thing as the other cylinders
        CylinderR.transform.localPosition = new Vector3(0, Vr, 0);
        //Same thing as the other cylinders
        CylinderR.transform.localScale = new Vector3(10f, Vr, 10f);

        //Rotate the cylinder to make it point in the direction of the resultant velocity
        Pivot.transform.LookAt(target);
        
        //Rotate to make the Y - Axis to point at the target not the Z - Axis
        Pivot.transform.Rotate(90, 0, 0);
    }
}
