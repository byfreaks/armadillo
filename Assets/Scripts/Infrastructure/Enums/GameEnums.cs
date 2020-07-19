public enum GameState{
    //[PRE] Setup states
    PRE_Load,
    PRE_Setup,

    //[RUN] Run states
    RUN_Running,
    RUN_Enconunter,
    
    //[GOV] Game Over States
    GOV_DeadPlayer,
    GOV_VehicleDestroyed,

    //Overall states
    PREPARATION = PRE_Setup | PRE_Load,
    RUNNING = RUN_Running | RUN_Enconunter,
    GAMEOVER = GOV_DeadPlayer | GOV_VehicleDestroyed
}