//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiInterfaceGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial interface IMaxHealthEntity {

    MaxHealthComponent maxHealth { get; }
    bool hasMaxHealth { get; }

    void AddMaxHealth(float newValue);
    void ReplaceMaxHealth(float newValue);
    void RemoveMaxHealth();
}
