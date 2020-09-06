using UnityEngine;

public class PickUpWeapon : Interaction
{
    public GameObject weapon;

    public PickUpWeapon(GameObject weapon)
    {
        this.weapon = weapon;
    }

    public override void OnInteraction(InteractableActor actor)
    {
        weapon.GetComponent<WeaponController>().Wield(actor);
    }
    
}