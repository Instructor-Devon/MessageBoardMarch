using System.Collections.Generic;

namespace MessageBoard.Models
{
    public class DashboardVM
    {
        public List<Post> AllPosts {get;set;}
        public Post NewPost {get;set;}
    }
}