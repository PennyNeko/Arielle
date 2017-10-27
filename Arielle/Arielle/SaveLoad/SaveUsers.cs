using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Arielle.SaveLoad
{
    class SaveUsers
    {
        string usersPath = @"..\..\JSON\Users.json";

        public SaveUsers(List<User> users)
        {
            string jsonUsers = JsonConvert.SerializeObject(users);
            File.WriteAllText(usersPath, jsonUsers);
        }
    }
}
