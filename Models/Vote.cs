using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessageBoard.Models
{
    [Table("votes")]
    public class Vote
    {
        [Key]
        public int VoteId {get;set;}
        public int UserId {get;set;}
        public int PostId {get;set;}
        public bool IsUpvote {get;set;}

        // NAV PROPS
        public User Voter {get;set;}
        public Post Voted {get;set;}
        
    }
}