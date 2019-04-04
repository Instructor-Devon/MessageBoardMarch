using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessageBoard.Models
{
    [Table("posts")]
    public class Post   
    {
        [Key]
        public int PostId {get;set;}
        [Required]
        public int UserId {get;set;}
        [Required]
        public string Topic {get;set;}
        [Required]
        [MinLength(5, ErrorMessage="Message Too $hort")]
        public string Content {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        
        // NAVIGATION PROPERTY
        public User Creator {get;set;}
        public List<Vote> VotesGiven {get;set;}
        // public Post()
        // {
        //     CreatedAt = DateTime.Now;
        //     UpdatedAt = DateTime.Now;
        // }
    }
}