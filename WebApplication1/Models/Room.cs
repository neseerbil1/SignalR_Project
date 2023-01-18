using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class Room
    {
        public Room()
        {
            Users= new List<User>();    
        }

        public int RoomID { get; set; }
        public int RoomName { get; set; }
        public List<User> Users { get; set; }
    }
}
