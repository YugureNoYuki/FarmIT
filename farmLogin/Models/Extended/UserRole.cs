using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace farmLogin.Models
{
    [MetadataType(typeof(UserRoleMetaData))]
    public partial class UserRole
    {
    }
    public class UserRoleMetaData
    {
        [Key]
        public int UserRoleID { get; set; }
    }
}