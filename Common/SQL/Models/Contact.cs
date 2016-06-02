using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Common.SQL.Models
{
    class Contact
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Indexed]
        public string Number { get; set; }
        public bool AppInstalled { get; set; }
        public DateTime LastChecked { get; set; }

    }
}
