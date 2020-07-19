public enum DamageTypes
{
  //Player damage types  
  PLY_BULLET,
  PLY_MELEE,

  //Enemy damage types
  ENM_BULLET,
  ENM_MELEE,
  
  //Global damage types
  PLAYER_DAMAGE = PLY_BULLET | PLY_MELEE,
  ENM_DAMAGE = ENM_BULLET | ENM_MELEE,
  GLOBAL_DAMAGE = PLY_BULLET | PLY_MELEE | ENM_BULLET | ENM_MELEE
}
