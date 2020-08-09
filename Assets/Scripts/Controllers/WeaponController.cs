using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    enum aimStyle{
        hold,
        point
    }

    public enum weaponType{
        melee,
        ranged
    }

    public enum wielder{
        enemy,
        player
    }

    //GENERAL CONFIG
    public int damage;
    public int spriteOffset = 90;
    public DamageTypes DamageType { get => GetDamageType(); }
    public wielder Wielder = wielder.player;
    [SerializeField] private aimStyle WeaponAimStyle = aimStyle.hold;
    [HideInInspector] public WeaponCommands currentCommand;
    [HideInInspector] public Transform wielderTransform;
    private GameObject currentAttack;

    [HideInInspector] public weaponType WeaponType = weaponType.melee;

    //MELEE CONFIG
    [HideInInspector] public GameObject hitbox;
    //RANGED CONFIG
    private Transform gunpoint = null;
    [HideInInspector] public GameObject projectile = null;
    [HideInInspector] public float fireRate = 100f; //Test field, not in use

    //Components
    private SpriteRenderer sr;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        gunpoint = transform.Find("gunpoint");

    }

    public void Set(WeaponCommands command){

        if(!WeaponCommands.aim.HasFlag(command) && currentCommand == command) return;

        switch(command){
            case WeaponCommands.hold:
                if(WeaponAimStyle == aimStyle.hold)
                    AimWeapon(wielderTransform.position);
                else 
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
#region Attack
    public GameObject Attack(Vector2 direction){
        //TODO: handle ranged weapons

        switch(WeaponType){
            case weaponType.melee:
                return AttackMelee(direction) as GameObject;

            case weaponType.ranged:
                return AttackRanged(direction) as GameObject;

            default: throw new System.Exception("Weapon type not valid");

        }
    }

    private object AttackRanged(Vector2 dir)
    {
        //TODO: [zch] implement shooting rate/cooldown

        var origin = gunpoint.position;
        var atk = Instantiate(projectile);
        atk.AddComponent<Damage>().setDamage(DamageType, damage);
        atk.GetComponent<ProjectileController>().Setup( dir, 19.8f  );
        atk.transform.position = this.transform.position;  
        atk.transform.rotation = Quaternion.LookRotation(Vector3.forward, CursorDirection(origin));
        return currentAttack = atk;
    }

    private object AttackMelee(Vector2 dir)
    {
        if(currentAttack != null) return currentAttack;
        var origin = wielderTransform.transform.position;
        var atk = Instantiate(hitbox);
        atk.AddComponent<Damage>().setDamage(DamageType, damage);
        atk.transform.position = this.transform.position;  
        atk.transform.rotation = Quaternion.LookRotation(Vector3.forward, CursorDirection(origin));
        atk.transform.rotation *= Quaternion.Euler(0,0,90);
        return currentAttack = atk;
    }

    private DamageTypes GetDamageType(){
        switch (WeaponType)
        {
            case weaponType.ranged: return Wielder==wielder.player?DamageTypes.PLY_BULLET:DamageTypes.ENM_BULLET;
            case weaponType.melee: return Wielder==wielder.enemy?DamageTypes.ENM_BULLET:DamageTypes.ENM_MELEE;
            default: throw new System.Exception("Invalid Damage Type");
        }
    }

#endregion

#region status
    private void AimWeapon(Vector2 origin){
        this.transform.rotation = Quaternion.Euler(0,0,0);
        this.transform.localPosition = CursorDirection(origin);
        currentCommand = WeaponCommands.hold;
    }

    private void PointWeapon(Vector2 origin){
        var rot = this.transform.rotation = Quaternion.LookRotation(Vector3.forward, CursorDirection(origin));
        var plyPos = GameHelper.Player.transform.position;
        // print($"PLYPOS={plyPos.x}  WEAPONPOS={transform.position.x}");
        if(plyPos.x>transform.position.x){
            this.transform.rotation *= Quaternion.Euler(0,0, -spriteOffset);
            sr.flipX = true;
        } else {
            this.transform.rotation *= Quaternion.Euler(0,0, spriteOffset);
            sr.flipY = sr.flipX = false;
            // this.transform.rotation *= Quaternion.Euler(0,0, -spriteOffset);
            
        }
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
