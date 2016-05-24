using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiAndWeb.Models;

namespace ApiAndWeb.Data.Tables
{
    public class Session
    {
        public int Id { get; set; }
        public string SessionId { get; set; }
        public string SessionShortName { get; set; }
        public List<ApplicationUser> Participants { get; set; }

    }
}
