using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ClientWebAppMVC.Models
{
    public class StaffShiftModel
    {
        public int ShiftID { get; set; }
        [Required]
        public int StaffID { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime OverTimeDate { get; set; }
        [Required]
        public int ExtraHours { get; set; }
        [Required]
        public float OvertimeMultiplier { get; set; }
    }
}
