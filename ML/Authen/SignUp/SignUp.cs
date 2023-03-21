namespace ML.Authen.SignUp
{
    public class SignUp
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}