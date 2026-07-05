using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using EcommerceDemo.Globals;
namespace EcommerceDemo.Models
{

    [Table("Users")]
    class User
    {


        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; } = 2000;

        public MoneyType BalanceType { get; set; } = MoneyType.Dollars;

        public bool IsAdmin { get; set; }

        public static implicit operator bool(User v)
        {
            throw new NotImplementedException();
        }
    }
}
