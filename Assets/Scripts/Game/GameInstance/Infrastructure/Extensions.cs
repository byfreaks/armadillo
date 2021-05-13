using UnityEngine;

namespace Armadillo.Game.GameInstance.Infrastructure
{
    public static class Extensions
    {
        public static GameObject ClearName(this GameObject go, string match = "(Clone)")
        {
            if(go.name.Contains(match))
            {
                go.name = go.name.Replace(match, "");
                go.transform.name = go.transform.name.Replace(match, "");
            }
            return go;
        }

        public static GameObject Pos(this GameObject go, Vector3 pos)
        {
            go.transform.position = pos;
            return go;
        }
    }
}