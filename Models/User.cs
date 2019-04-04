using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessageBoard.Models
{
    [Table("users")]
    // Entity Models
    public class User
    {
        [Key]
        public int UserId {get;set;}
        [Required]
        public string FirstName {get;set;}
        [EmailAddress]
        [Required]
        public string Email {get;set;}
        public string Motto {get;set;}
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password {get;set;}
        [Compare("Password")]
        [NotMapped]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        // NAVIGATION PROPERTY
        public List<Post> Posts {get;set;}
        public List<Vote> VotesIssued {get;set;}
    }
    // Not Entity Model (only for validations)
    public class LoginUser
    {
        [EmailAddress]
        [Required]
        public string Email {get;set;}
        [DataType(DataType.Password)]
        public string Password {get;set;}
    }
}