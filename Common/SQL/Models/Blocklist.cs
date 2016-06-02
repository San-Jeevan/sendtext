using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Common.SQL.Models
{
    class Blocklist
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed, Unique]
        public string Phonenumber { get; set; }
        public int ContactId { get; set; }
        public string Username { get; set; }
    }
}
