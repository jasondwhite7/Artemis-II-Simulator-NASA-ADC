 using UnityEngine;

public class BonusTimeSplice
{
    //Fields
    public float Time { get; set; }
    public float Px { get; set; }
    public float Py { get; set; }
    public float Pz { get; set; }
    public float Vx { get; set; }
    public float Vy { get; set; }
    public float Vz { get; set; }
    public double Mass { get; set; }
    public float MoonPx { get; set; }
    public float MoonPy { get; set; }
    public float MoonPz { get; set; }
    public float MoonVx { get; set; }
    public float MoonVy { get; set; }
    public float MoonVz { get; set; }
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
    public Vector3 MoonPositionVector { get; set; }
    public Vector3 MoonVelocityVector { get; set; }
    public Vector3 BonusPositionVector { get; set; }
    public Vector3 BonusVelocityVector { get; set; }
    public float MoonResultantVelocity { get; set; }
}
