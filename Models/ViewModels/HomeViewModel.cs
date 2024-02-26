using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationPortal.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Jobs> Jobs { get; set; }
        public IEnumerable<Rank> Ranks { get; set; }
    }
}