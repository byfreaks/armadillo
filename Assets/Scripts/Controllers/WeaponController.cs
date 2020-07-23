using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    [Header("Config")]
    public int damage;
    public GameObject hitbox;

    [Header("Info")]
    public WeaponCommands currentCommand;
    public Transform wielderTransform;

    private GameObject currentAttack;

    public void Set(WeaponCommands command){

        if(!WeaponCommands.aim.HasFlag(command) && currentCommand == command) return;

        switch(command){
            case WeaponCommands.hold:
                AimWeapon(wielderTransform.position);
                break;

            case WeaponCommands.point:
                PointWeapon(wielderTransform.position);
                break;

            case WeaponCommands.sheath:
                SheathWeapon();
                break;

            case WeaponCommands.store:
                StoreWeapon();
                break;

        }
    }

    public GameObject Attack(DamageTypes damageType){
        //TODO: handle ranged weapons
        if(currentAttack != null) return currentAttack;
        var origin = wielderTransform.transform.position;
        var atk = Instantiate(hitbox);
        atk.AddComponent<Damage>().setDamage(DamageTypes.PLY_MELEE, damage);
        atk.transform.position = this.transform.position;  
        atk.transform.rotation = Quaternion.LookRotation(Vector3.forward, CursorDirection(origin));
        atk.transform.rotation *= Quaternion.Euler(0,0,90);
        return currentAttack = atk;
    }

    #region status
    private void AimWeapon(Vector2 origin){
        this.transform.rotation = Quaternion.Euler(0,0,0);
        this.transform.localPosition = CursorDirection(origin);
        currentCommand = WeaponCommands.hold;
    }

    private void PointWeapon(Vector2 origin){
        this.transform.rotation = Quaternion.LookRotation(Vector3.forward, CursorDirection(origin));
        this.transform.localPosition = CursorDirection(origin);
        currentCommand = WeaponCommands.point;
    }

    private void SheathWeapon()
    {
        this.transform.rotation = Quaternion.Euler(0,0,-120);
        this.transform.localPosition = new Vector3(0,-0.25f,-1);
        currentCommand = WeaponCommands.sheath;
    }

    private void StoreWeapon()
    {
        this.transform.rotation = Quaternion.Euler(0,0,190);
        this.transform.localPosition = new Vector3(-0.3f,0,1);
        currentCommand = WeaponCommands.store;
    }

    private void DropWeapon(){
        throw new System.NotImplementedException();
    }

    private void ThrowWeapon(){
        throw new System.NotImplementedException();
    }

    //TODO: replace with inputcontroller method
    private Vector3 CursorDirection(Vector2 origin){
        var dir = Camera.main.ScreenToWorldPoint(InputController.MousePosition()) - (Vector3)origin;
        var distance = 1.5f;
        dir += new Vector3(0,0,-1);
        return dir.normalized * distance;
    }
#endregion

}
