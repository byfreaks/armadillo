[System.Flags]
public enum DamageTypes
{
  //Player damage types  
  PLY_BULLET = 1 << 1,
  PLY_MELEE = 1 << 2,

  //Enemy damage types
  ENM_BULLET = 1 << 3,
  ENM_MELEE = 1 << 4 ,
  
  //Global damage types
  PLAYER_DAMAGE = PLY_BULLET | PLY_MELEE,
  ENM_DAMAGE = ENM_BULLET | ENM_MELEE,
  GLOBAL_DAMAGE = ENM_DAMAGE | PLAYER_DAMAGE
}
