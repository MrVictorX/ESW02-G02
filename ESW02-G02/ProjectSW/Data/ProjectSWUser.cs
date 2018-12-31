using System;
using System.Collections.Generic;
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
        public string Name { get; set; }

        [PersonalData]
        public string Address { get; set; }

        [PersonalData]
        public DateTime DateOfBirth { get; set; }

        [PersonalData]
        public string UserType { get; set; }

        [PersonalData]
        public byte[] FotoFile { get; set; }
    }
}
