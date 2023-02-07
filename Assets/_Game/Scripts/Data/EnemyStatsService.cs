using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Data
{
    [CreateAssetMenu(menuName = "Magero/Data/EnemyStats")]
    public class EnemyStatsService : ScriptableObject, IUnitDamageProvider, IUnitHealthProvider, IUnitMovementSpeedProvider
    {
        public List<float> MinionHP = new();
        public List<float> MinionSpeed = new();
        public List<float> MinionDamage = new();

        public List<float> WarriorHP = new();
        public List<float> WarriorSpeed = new();
        public List<float> WarriorDamage = new();

        public List<float> BruteHP = new();
        public List<float> BruteSpeed = new();
        public List<float> BruteDamage = new();

        [Button(ButtonSizes.Large)]
        public void ParseData(TextAsset file)
        {
            MinionHP.Clear();
            MinionSpeed.Clear();
            MinionDamage.Clear();
            
            WarriorHP.Clear();
            WarriorSpeed.Clear();
            WarriorDamage.Clear();

            BruteHP.Clear();
            BruteSpeed.Clear();
            BruteDamage.Clear();
            
            TextReader reader = new StringReader(file.text);
            var firstLine = reader.ReadLine();
            var headers = firstLine.Split(",");
            string str = string.Empty;
            while ((str = reader.ReadLine()) != null)
            {
                var fields = str.Split(',');
                
                MinionHP.Add(float.Parse(fields[1]));
                MinionSpeed.Add(float.Parse(fields[2]));
                MinionDamage.Add(float.Parse(fields[3]));
                WarriorHP.Add(float.Parse(fields[4]));
                WarriorSpeed.Add(float.Parse(fields[5]));
                WarriorDamage.Add(float.Parse(fields[6]));
                BruteHP.Add(float.Parse(fields[7]));
                BruteSpeed.Add(float.Parse(fields[8]));
                BruteDamage.Add(float.Parse(fields[9]));
            }
        }

        public float GetDamage(UnitType t, int level)
        {
            if (t == UnitType.Minion)
            {
                return MinionDamage[Mathf.Min(level - 1, MinionDamage.Count - 1)];
            }
            
            if (t == UnitType.Brute)
            {
                return BruteDamage[Mathf.Min(level - 1, BruteDamage.Count - 1)];
            }
            
            if (t == UnitType.Warrior)
            {
                return WarriorDamage[Mathf.Min(level - 1, WarriorDamage.Count - 1)];
            }

            return -1;
        }

        public float GetHealth(UnitType t, int level)
        {
            if (t == UnitType.Minion)
            {
                return MinionHP[Mathf.Min(level - 1, MinionHP.Count - 1)];
            }
            
            if (t == UnitType.Brute)
            {
                return BruteHP[Mathf.Min(level - 1, BruteHP.Count - 1)];
            }
            
            if (t == UnitType.Warrior)
            {
                return WarriorHP[Mathf.Min(level - 1, WarriorHP.Count - 1)];
            }

            return -1;        
        }

        public float GetSpeed(UnitType t, int level)
        {
            if (t == UnitType.Minion)
            {
                return MinionSpeed[Mathf.Min(level - 1, MinionSpeed.Count - 1)];
            }
            
            if (t == UnitType.Brute)
            {
                return BruteSpeed[Mathf.Min(level - 1, BruteSpeed.Count - 1)];
            }
            
            if (t == UnitType.Warrior)
            {
                return WarriorSpeed[Mathf.Min(level - 1, WarriorSpeed.Count - 1)];
            }

            return -1;        

        }
    }
}