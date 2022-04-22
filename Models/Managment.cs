using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace beltExam.Models
{
    public class Managment
    {
        [Key]
        public int ManagmentId {get;set;}
        public int UserId {get;set;}
        public int MeetUpId {get;set;}
        public User User {get;set;}
        public MeetUp MeetUp {get;set;}

    }
}