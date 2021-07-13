using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StaffShiftService.Database.Entites
{
    public class StaffShift
    {
        [Key]
        public int ShiftID { get; set; }
        [Required(ErrorMessage ="Please enter an staff ID to fulfill the shift.")]
        public int StaffID { get; set; }
        [Required(ErrorMessage = "Must enter a date for overtime")]
        [DataType(DataType.Date)]
        public DateTime OverTimeDate { get; set; }
        [Required(ErrorMessage = "Must enter how many extra hours the emplye will do")]
        public int ExtraHours { get; set; }

        public float OvertimeMultiplier { get; set; }
    }
}
