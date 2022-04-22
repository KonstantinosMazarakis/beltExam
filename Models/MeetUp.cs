using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace beltExam.Models
{
    public class MeetUp
    {
        [Key]
        public int MeetUpId {get;set;}
        [Required]
        public string Title {get;set;}
        [Required]
        public DateTime DateOfMeetUp {get;set;}
        [Required]
        public int Duration {get;set;}
        [Required]
        public string DurationHoursDays {get;set;}
        [Required]
        public string Description {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public List<Managment> GuestList {get;set;}
        public int UserId {get;set;}
        public User Host {get;set;}
    }
}
