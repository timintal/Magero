namespace _Game.Data
{
    public interface IWeaponUpgradePriceProvider
    {
        public int GetPrice(WeaponType t, int level);
    }
}