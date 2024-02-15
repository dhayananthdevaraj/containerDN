using System ;

namespace dotnetapp.Models{
    
    public class Issue
{
    public long IssueId { get; set; }
    public string Description { get; set; }
    public string Severity { get; set; }
    public long UserId { get; set; }

     public long AssignmentId { get; set; } // Add AssignmentId property

    public User User { get; set; } // Relationship with the User model
    public Assignment Assignment { get; set; }

}


}