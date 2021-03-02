using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourTravels.Entities.Models
{
    public class utblMstTerm
    {
        [Key]

        public long TermID { get; set; }
        public string TermName { get; set; }
    }
}
