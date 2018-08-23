using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BabyOnBoard.Models
{
    public partial class ChecklistMetadata
    {
        public int ChecklistId { get; set; }
        [Display(Name = "Picture URL")]
        public string Picture { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        public string Brand { get; set; }
        [Required]
        public string Condition { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
    }
}