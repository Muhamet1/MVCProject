using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Project.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ProjectUser class
public class ProjectUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public byte[]? ProfilePicture { get; set; }
}

