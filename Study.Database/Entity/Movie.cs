using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Study.Database.Entity
{
    public class Movie
    {
        [Key]
        public string Id { get; set; }

        public string Title  { get; set; }

        public string Url  { get; set; }
    }
}
