namespace Task_Tracking.Models
{
    public class User
    {
       
        public int Id { get; set; }

        
        public string Username { get; set; }

        
        public string FullName { get; set; }

        
        public string Email { get; set; }

        
        public Roles Role { get; set; }
    }
    public enum Roles
    {
        SoftwareDeveloper,
        SystemAdministrator,
        SoftwareTester,
        SupportTechnician
    }
}