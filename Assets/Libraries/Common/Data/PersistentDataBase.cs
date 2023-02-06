namespace _Game.Data
{
    public abstract class PersistentDataBase
    {
        public bool IsDirty { get; set; }
        public abstract string DataId { get; }
    }
}