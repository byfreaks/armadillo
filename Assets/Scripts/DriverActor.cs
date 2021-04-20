using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverActor : MonoBehaviour
{
    public bool isMounted {get; set; } = false;
    public TurretController ride; //Temporal

    public void Mount(TurretController turret) //TODO: Generalize to all mount types
    {
        this.isMounted = true;
        ride = turret;
    }

    public void UnMount()
    {
        this.isMounted = false;
        ride.unMount();
        ride = null;
    }
}
