namespace _Game.Data
{
    public interface IUnitMovementSpeedProvider
    {
        public float GetSpeed(UnitType t, int level);
    }
}