using System;
using Entitas;
using UnityEngine;

namespace _Game.Data
{
    
    [CreateAssetMenu(menuName = "Magero/UnitStatsService")]
    public class UnitStatsService : ScriptableObject, IUnitDamageProvider, IUnitHealthProvider, IUnitMovementSpeedProvider
    {
        [SerializeField] UnitInitialParams[] UnitParams;
        
        [Serializable]
        public struct UnitInitialParams
        {
            public UnitType Type;
            public float BaseDamage;
            public float BaseHealth;
            public float BaseSpeed;
        }
        
        public float GetDamage(UnitType t, int level)
        {
            foreach (var unitParam in UnitParams)
            {
                if (unitParam.Type == t)
                {
                    return unitParam.BaseDamage * Mathf.Pow(1.1f, level - 1);
                }
            }
            
            return 0;
        }

        public float GetHealth(UnitType t, int level)
        {
            foreach (var unitParam in UnitParams)
            {
                if (unitParam.Type == t)
                {
                    return unitParam.BaseHealth * Mathf.Pow(1.1f, level - 1);
                }
            }
            
            return 0;
        }

        public float GetSpeed(UnitType t, int level)
        {
            foreach (var unitParam in UnitParams)
            {
                if (unitParam.Type == t)
                {
                    return unitParam.BaseSpeed * Mathf.Pow(1.1f, level - 1);
                }
            }
            
            return 0;
        }
    }
}