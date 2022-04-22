using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace beltExam.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}
        [Required]
        [MinLength(2)]
        public string FirstName {get;set;}
        [Required]
        [MinLength(2)]
        public string LastName {get;set;}
        [Required]
        [EmailAddress]
        public string Email {get;set;}
        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$")]
        [DataType(DataType.Password)]
        public string Password {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public List<Managment> GuestList {get;set;}

        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword {get;set;}

    }
}