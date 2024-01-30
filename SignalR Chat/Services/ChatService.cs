namespace SignalR_Chat.Services
{
    public class ChatService
    {
        private static readonly Dictionary<string, string> _users = new();

        public bool AddUser(string userToAdd)
        {
            lock(_users)
            {
                foreach(var user in _users)
                {
                    if(user.Key.Equals(userToAdd, StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }
            }

            _users.Add(userToAdd, null);
            return true;
        }

        public void AddUserConnectionId(string user, string connectionId)
        {
            lock(_users)
            {
                if(_users.ContainsKey(user))
                {
                    _users[user] = connectionId;
                }
            }
        }

        public string GetUserByConnectionId(string connectionId)
        {
            lock(_users)
            {
                return _users.Where(x => x.Value == connectionId).Select(x => x.Key).FirstOrDefault();
            }
        }

        public void RemoveUser(string userToRemove)
        {
            lock(_users)
            {
                if (_users.ContainsKey(userToRemove))
                    _users.Remove(userToRemove);
            }
        }

        public string[] GetOnlineUsers()
        {
            lock (_users)
            {
                return _users.OrderBy(x => x.Key).Select(x => x.Key).ToArray();
            }
        }

        public string GetConnectionIdByUser(string user)
        {
            lock(_users)
            {
                if(_users.ContainsKey(user))
                {
                    return _users[user];
                }
            }

            return null;
        }
    }
}
