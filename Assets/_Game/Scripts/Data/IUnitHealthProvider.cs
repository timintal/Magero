namespace _Game.Data
{
    public interface IUnitHealthProvider
    {
        public float GetHealth(UnitType t, int level);
    }
}