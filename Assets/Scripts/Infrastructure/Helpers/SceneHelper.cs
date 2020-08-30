using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneHelper
{
    public static void LoadScene(GameScenes scene){
        SceneManager.LoadScene((int)scene);
    }
}
