using Entitas;

[Game, Input]
public class DestroyedComponent : IComponent
{
    
}

public interface IDestroyableEntity : IEntity, IDestroyedEntity { }

public partial class GameEntity : IDestroyableEntity { }
public partial class InputEntity : IDestroyableEntity { }