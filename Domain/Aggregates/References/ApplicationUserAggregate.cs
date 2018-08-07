namespace Domain.Aggregates.References
{
    public class ApplicationUserAggregate
    {
        public ApplicationUserAggregate(string email)
        {
            UserName = email;
            Email = email;
        }

        public string Id { get; private set; }

        public string Email { get; private set; }

        public string UserName { get; private set; }
        public string PasswordHash { get; private set; }

        public void SetPassword(string passwrod)
        {
        }
    }
}