using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationPortal.Models
{
    public class Jobs
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Rank")]
        [Required(ErrorMessage = "Please Select a Rank")]
        public int RankId { get; set; }

        [Required]
        [Display(Name = "Job Name")]
        public string JobName { get; set; }

        [Required]
        public string Description { get; set; }

        [ForeignKey("RankId")]
        public virtual Rank Rank{ get; set; }
    }
}