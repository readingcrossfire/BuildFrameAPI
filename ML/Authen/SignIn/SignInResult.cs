namespace ML.Authen.SignIn
{
    public class SignInResult
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateTimeOffset ExpiredDate { get; set; }
        public Int64 ExpiredUnix { get; set; }
    }
}