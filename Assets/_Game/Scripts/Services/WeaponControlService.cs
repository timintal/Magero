using _Game.Data;

public class WeaponControlService 
{
    private readonly PlayerData _playerData;

    public WeaponControlService(PlayerData playerData)
    {
        _playerData = playerData;
    }

    public void EquipWeapon(WeaponType t, bool isLeftHand)
    {
        if (isLeftHand)
        {
            _playerData.LeftHandWeapon = t;
            if (_playerData.RightHandWeapon == t)
            {
                _playerData.RightHandWeapon = WeaponType.None;
            }
        }
        else
        {
            _playerData.RightHandWeapon = t;
            if (_playerData.LeftHandWeapon == t)
            {
                _playerData.LeftHandWeapon = WeaponType.None;
            }
        }
    }
}
