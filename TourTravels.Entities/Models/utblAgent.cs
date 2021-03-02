using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourTravels.Entities.Models
{
    public class utblAgent
    {
        [Key]
        public long AgentID { get; set; }
        [Required]
        public string AgentName { get; set; }
        [Required]
        public string AgentAddress { get; set; }
        [Required]
        public string AgentEmail { get; set; }
        public string AgentMobile { get; set; }
        public string AgentDocumentPath { get; set; }
        public string Status { get; set; }
    }
}
