using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arielle
{
    class User
    {
        public User(ulong id, int points)
        {
            ID = id;
            Points = points;
        }

        public ulong ID { get; set; }
        public int Points { get; set; }
    }
}
