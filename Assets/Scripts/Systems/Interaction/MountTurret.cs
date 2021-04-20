using UnityEngine;

public class MountTurret : Interaction
{
    public TurretController turret;

    public MountTurret(TurretController turret)
    {
        this.turret = turret;
    }

    public override void OnInteraction(InteractableActor actor)
    {
        var actorObject = actor.actorObject.gameObject;
        turret.mount(actorObject);
        if( actorObject.TryGetComponent<DriverActor>(out var driverComponent) ){
            Debug.Log(actorObject.name);
            driverComponent.Mount(turret);
        } else {
            throw new System.Exception($"Actor object {actor} does not have a Driver Actor Component");
        }

    }
    
}