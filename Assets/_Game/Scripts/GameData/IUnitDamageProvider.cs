namespace _Game.Data
{
    
    
    public interface IUnitDamageProvider
    {
        public float GetDamage(UnitType t, int level);
    }
}