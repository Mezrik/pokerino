using System;
namespace Pokerino.Server.Helpers
{
    public class ConnectionMapping<T>
    {
        private readonly Dictionary<string, T> _connections =
            new Dictionary<string, T>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void Add(string connectionId, T data)
        {
            lock (_connections)
            {
                _connections.Add(connectionId, data);
            }
        }

        public T? GetConnectionData(string connectionId)
        {
            T data;
            if (_connections.TryGetValue(connectionId, out data))
            {
                return data;
            }

            return default(T);
        }

        public void Remove(string connectionId)
        {
            lock (_connections)
            {
                _connections.Remove(connectionId);
            }
        }
    }
}

