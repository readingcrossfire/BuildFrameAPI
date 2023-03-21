using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ML.APIResult;
using ML.Authen.SignIn;
using ML.Authen.SignUp;
using ML.Entities;
using SHARED.LoadConfig;
using StackExchange.Redis;
using SignInResult = ML.Authen.SignIn.SignInResult;

namespace BLL.BLL_Authen
{
    public partial class BLL_Authen : IAuthenService
    {
        private readonly UserManager<NewsAppUser> _userManager;
        private readonly RoleManager<NewsAppRole> _roleManager;

        public BLL_Authen(UserManager<NewsAppUser> userManager, RoleManager<NewsAppRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        private JwtSecurityToken CreateToken(List<Claim> lstClaim)
        {
            SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(LoadConfig.Instance.GetValue<string>("JWT:Secret")));
            SigningCredentials signinCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: LoadConfig.Instance.GetValue<string>("JWT:ValidIssuer"),
                audience: LoadConfig.Instance.GetValue<string>("JWT:ValidAudience"),
                expires: DateTime.Now.AddHours(3),
                claims: lstClaim,
                signingCredentials: signinCredentials
            );

            return token;
        }

        public async Task<APIResult<SignInResult>> SignIn(SignIn objSignIn)
        {
            try
            {
                NewsAppUser objUser = await this._userManager.FindByNameAsync(objSignIn.UserName);
                if (objUser == null)
                {
                    return new APIResult<SignInResult>
                    {
                        IsError = true,
                        Message = "Tên đăng nhập hoặc mật khẩu không đúng"
                    };
                }

                bool signInResult = await this._userManager.CheckPasswordAsync(objUser, objSignIn.Password);
                if (signInResult)
                {
                    List<Role> lstRole = await this._userManager.GetRolesAsync(objUser) as List<Role> ?? new List<Role>();
                    List<Claim> lstClaim = new List<Claim>();

                    lstClaim.Add(new Claim(ClaimTypes.NameIdentifier, objUser.UserName));
                    lstClaim.Add(new Claim(ClaimTypes.Name, objUser.FullName));

                    if (lstRole != null && lstRole.Count > 0)
                    {
                        foreach (Role roleItem in lstRole)
                        {
                            lstClaim.Add(new Claim(ClaimTypes.Role, roleItem.Value.ToString()));
                        }
                    }

                    JwtSecurityToken jwtToken = this.CreateToken(lstClaim);

                    return new APIResult<SignInResult>
                    {
                        IsError = false,
                        Message = "Đăng nhập thành công",
                        ResultObject = new SignInResult
                        {
                            Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                            FullName = objUser.FullName,
                            UserName = objUser.UserName,
                            ExpiredDate = jwtToken.ValidTo,
                            ExpiredUnix = ((DateTimeOffset)jwtToken.ValidTo).ToUnixTimeSeconds()
                        }
                    };
                }
                else
                {
                    return new APIResult<SignInResult>
                    {
                        IsError = true,
                        Message = "Tên đăng nhập hoặc mật khẩu không đúng"
                    };
                }
            }
            catch (Exception objEx)
            {
                return new APIResult<SignInResult>
                {
                    IsError = true,
                    Message = objEx.ToString(),
                };
            }
        }

        public async Task<APIResultBase> SignUp(SignUp signUp)
        {
            try
            {
                if (signUp == null)
                {
                    return new APIResultBase
                    {
                        IsError = true,
                        Message = "Có lỗi xảy ra"
                    };
                }

                NewsAppUser objUser = await this._userManager.FindByNameAsync(signUp.UserName);
                if (objUser == null)
                {
                    NewsAppUser userNew = new NewsAppUser
                    {
                        UserName = signUp.UserName,
                        FullName = signUp.FullName,
                    };

                    IdentityResult createdUserResult = await this._userManager.CreateAsync(userNew, signUp.Password);
                    if (createdUserResult.Succeeded)
                    {
                        if (signUp.Roles.Count > 0)
                        {
                            foreach (string roleItem in signUp.Roles)
                            {
                                NewsAppRole objRole = await this._roleManager.FindByIdAsync(roleItem);
                                if (objRole == null)
                                {
                                    return new APIResultBase
                                    {
                                        IsError = true,
                                        Message = "Lỗi phân quyền người dùng"
                                    };
                                }
                                IdentityResult addRoleResult = await this._userManager.AddToRoleAsync(userNew, roleItem);
                                if (!addRoleResult.Succeeded)
                                {
                                    return new APIResultBase
                                    {
                                        IsError = true,
                                        Message = "Lỗi phân quyền người dùng"
                                    };
                                }
                            }
                        }

                        return new APIResultBase
                        {
                            IsError = false,
                            Message = "Tạo tài khoản thành công"
                        };
                    }

                    return new APIResultBase
                    {
                        IsError = true,
                        Message = "Đã tồn tại người dùng"
                    };
                }
                else
                {
                    return new APIResultBase
                    {
                        IsError = true,
                        Message = "Đã tồn tại người dùng"
                    };
                }
            }
            catch (Exception objEx)
            {
                return new APIResultBase
                {
                    IsError = true,
                    Message = objEx.ToString()
                };
            }
        }
    }
}