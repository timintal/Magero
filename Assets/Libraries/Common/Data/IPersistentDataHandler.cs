namespace _Game.Data
{
    public interface IPersistentDataHandler
    {
        public void Save(PersistentDataBase data);
        public void Load<T>(T data) where T : PersistentDataBase;
    }
}