using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace ChristmasJoy.Models.Entities
{
    public class User: TableEntity
    {
        public User()
        {

        }

        public string UserPartitionKey { get; set; }
        
        public string Email { get; set; }

        public string UserName { get; set; }

        public bool IsAdmin { get; set; }

        public string HashedPassword { get; set; }
    }
}
