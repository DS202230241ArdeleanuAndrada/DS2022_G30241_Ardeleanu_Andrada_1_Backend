using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.Entities
{
    public class DeviceWithUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string MaxConsumption { get; set; }
        public string NameOfUser { get; set; }
        public string Username { get; set; }
    }
}
