namespace _Game.Data
{
    public interface IWeaponDamageProvider
    {
        public float GetDamage(WeaponType t, int level);
    }
}