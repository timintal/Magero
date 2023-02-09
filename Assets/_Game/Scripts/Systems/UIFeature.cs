public class UIFeature : Feature
{
    public UIFeature(Contexts contexts, PoolService poolService) : base("UI Feature")
    {
        Add(new CollectDamageToShowInUI(contexts));
        Add(new CreateUIOverlaySystem(contexts, poolService));
        Add(new DamageForUICooldownSystem(contexts));
    }
}