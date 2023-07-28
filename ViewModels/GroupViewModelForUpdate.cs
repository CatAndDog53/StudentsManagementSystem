using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class GroupViewModelForUpdate : GroupViewModel
    {
        [StringLength(30, ErrorMessage = "The maximum length for Name is 30 characters")]
        [Remote(action: "CheckIfEditedGroupWontBeDuplicate", controller: "Groups", AdditionalFields = "Name, GroupId")]
        public override string Name { get; set; }
    }
}
