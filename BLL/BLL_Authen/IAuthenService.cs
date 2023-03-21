using ML.APIResult;
using ML.Authen.SignIn;
using ML.Authen.SignUp;

namespace BLL.BLL_Authen
{
    public interface IAuthenService
    {
        public Task<APIResult<SignInResult>> SignIn(SignIn signIn);
        public Task<APIResultBase> SignUp(SignUp signUp);
    }
}