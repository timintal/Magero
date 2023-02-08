using Game.Config.Model;

namespace _Game.Data
{
    public class EnemyStatsConfig : IUnitDamageProvider, IUnitHealthProvider
    {
        private readonly GameConfig _gameConfig;

        public EnemyStatsConfig(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
        }

        public float GetDamage(UnitType t, int level)
        {
            var enemyUpgrades = _gameConfig.GetConfigModel<EnemyUpgradesModel>();
            var key = level.ToString();
            if (enemyUpgrades.ContainsKey(key))
            {
                if (t == UnitType.Minion)
                {
                    return enemyUpgrades[key].MinionDamage;
                }
                else if (t == UnitType.Brute)
                {
                    return enemyUpgrades[key].BruteDamage;
                }
                else if (t == UnitType.Warrior)
                {
                    return enemyUpgrades[key].WarriorDamage;
                }
            }


            return -1;
        }

        public float GetHealth(UnitType t, int level)
        {
            var enemyUpgrades = _gameConfig.GetConfigModel<EnemyUpgradesModel>();
            var key = level.ToString();
            if (enemyUpgrades.ContainsKey(key))
            {
                if (t == UnitType.Minion)
                {
                    return enemyUpgrades[key].MinionHealth;
                }
                else if (t == UnitType.Brute)
                {
                    return enemyUpgrades[key].BruteHealth;
                }
                else if (t == UnitType.Warrior)
                {
                    return enemyUpgrades[key].WarriorHealth;
                }
            }


            return -1;
        }
    }
}