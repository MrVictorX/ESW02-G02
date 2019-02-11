using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ProjectSW.Data
{
    // Add profile data for application users by adding properties to the ProjectSWUser class

    /// <summary> Representa o modelo do utilizador, usada em conjunto com a classe IdentityUser para representar toda a informação dos utilizadores</summary>
    public class ProjectSWUser : IdentityUser
    {
        [PersonalData]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [PersonalData]
        [Display(Name = "Morada")]
        public string Address { get; set; }

        [PersonalData]
        [Display(Name = "Data de nascimento")]
        public DateTime DateOfBirth { get; set; }

        [PersonalData]
        [Display(Name = "Cargo")]
        public string UserType { get; set; }

        [PersonalData]
        public byte[] FotoFile { get; set; }
    }
}
