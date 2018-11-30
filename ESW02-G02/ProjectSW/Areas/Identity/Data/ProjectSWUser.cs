using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ProjectSW.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ProjectSWUser class
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
        public string FotoFile { get; set; }
    }
}
