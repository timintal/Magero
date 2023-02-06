using _Game.Data;

public class WeaponControlService 
{
    private readonly PlayerData _playerData;
    private readonly WeaponData _weaponData;

    public WeaponControlService(PlayerData playerData, WeaponData weaponData)
    {
        _playerData = playerData;
        _weaponData = weaponData;
    }

    public void EquipWeapon(WeaponType t)
    {
        if (_playerData.LeftHandWeapon == t)
            return;

        _playerData.RightHandWeapon = _playerData.LeftHandWeapon;
        _playerData.LeftHandWeapon = t;
    }
}
