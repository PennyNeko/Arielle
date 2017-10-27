using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Arielle
{
    class LoadUsers
    {
        List<User> users = new List<User>();
        public LoadUsers()
        {
            string usersFile = File.ReadAllText(@"..\..\JSON\Users.json");
            foreach (var user in JsonConvert.DeserializeObject<dynamic>(usersFile))
            {
                User newuser = new User((long)user.ID, (int)user.Points);
                users.Add(newuser);
            }
        }
        
        public List<User> GetUsers()
        {
            return users;
        }
    }
}
