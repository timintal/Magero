using System.Collections.Generic;
using VContainer.Unity;

namespace _Game.Data
{
    public class DataManager: IStartable, ITickable
    {
        private readonly IEnumerable<PersistentDataBase> _datas;
        private readonly IPersistentDataHandler _dataHandler;

        public DataManager(IEnumerable<PersistentDataBase> datas, IPersistentDataHandler dataHandler)
        {
            _datas = datas;
            _dataHandler = dataHandler;
        }

        public void Start()
        {
            foreach (var dataBase in _datas)
            {
                _dataHandler.Load(dataBase);
            }
        }

        public void Tick()
        {
            foreach (var dataBase in _datas)
            {
                if (dataBase.IsDirty)
                {
                    _dataHandler.Save(dataBase);
                }
            }
        }
    }
}