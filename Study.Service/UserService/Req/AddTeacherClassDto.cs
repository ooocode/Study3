using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Services.UserService.Req
{
    public class AddTeacherClassDto
    {
        [Required]
        public string TeacherId  { get; set; }


        [Required]
        public string ClassId { get; set; }
    }
}
