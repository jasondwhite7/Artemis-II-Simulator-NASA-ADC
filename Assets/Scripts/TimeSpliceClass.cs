using Unity.VisualScripting;
using UnityEngine;

public class TimeSplice
{
    //Fields
    public float Time { get; set; }
    public float Px { get; set; }
    public float Py { get; set; }
    public float Pz { get; set; }
    public float Vx { get; set; }
    public float Vy { get; set; }
    public float Vz { get; set; }
    public float Mass { get; set; }
    public float WPSA { get; set; }
    public float WPSArange { get; set; }
    public float DS54 { get; set; }
    public float DS54range { get; set; }
    public float DS24 { get; set; }
    public float DS24range { get; set; }
    public float DS34 { get; set; }
    public float DS34range { get; set; }
    public float LinkBudgetWPSA { get; set; }
    public float LinkBudgetDS54 { get; set; }
    public float LinkBudgetDS24 { get; set; }
    public float LinkBudgetDS34 { get; set; }
    public float ResultantVelocity { get; set; }
    public Vector3 PositionVector3 { get; set; }
    public Vector3 VelocityVector3 { get; set; }
    public Vector3 CalculatedPositionVector3 { get; set; }
    public float SmoothX { get; set; }
    public float SmoothY { get; set; }
    public float SmoothZ { get; set; }
    public Vector3 SmoothPositionVector3 { get; set; }
    // public float SmoothVx { get; set; }
    // public float SmoothVy { get; set; }
    // public float SmoothVz { get; set; }
    // public Vector3 SmoothVelocityVector3 { get; set; }
    public float DataTotalDistance { get; set; }
    // public float SmoothedTotalDistance { get; set; }
    public float RoundedResultantVelocity { get; set; }
    public float RoundedVx { get; set; }
    public float RoundedVy { get; set; }
    public float RoundedVz { get; set; }
    public float NewVx { get; set; }
    public float NewVy { get; set; }
    public float NewVz { get; set; }
    public Vector3 NewVelocityVector { get; set; }
}
