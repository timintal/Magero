using UnityEngine;

namespace Game.Config.Model
{
    public abstract class Validator : ScriptableObject
    {
        public static readonly string EverythingIsOkMsg = "Everything is Ok!!";
        public abstract string ValidateModel(GameConfig config);
    }
}