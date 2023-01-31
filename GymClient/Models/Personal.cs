using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymClient.Models
{
    public class Personal
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDay { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Gender { get; set; }

        public int? RoleId { get; set; } = 0;

        public string IMGPath { get; set; }

        public string Password { get; set; }


	


	}
}
