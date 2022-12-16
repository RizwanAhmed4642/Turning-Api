using AutoMapper;
using Meeting_App.Data.Database.Context;
using Meeting_App.Models;
using Meeting_App.Models.DTOs;
using Meeting_App.Service.Auth;
using Meeting_App.Service.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meeting_App.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        #region Fields
        private readonly UserManager<Applicationuser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;
        #endregion

        #region Constructor
        public AuthController(AuthService authService,UserManager<Applicationuser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _authService = authService;
        }
        #endregion

    


        #region Register

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO model)
        {
            try
            {
                var userExists = await userManager.FindByNameAsync(model.UserName);
                //Duplicate Check
                if (userExists != null)
                    return Ok(new { Status = "Warning", Message = "User Already Exists!" });


                Applicationuser user = new Applicationuser()
                {
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                    FullName=model.FullName,
                    Designation=model.Designation,
                    NP=model.Password
                    
                };

                ///
                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed! Please check user details and try again." });

                return Ok(new { Status = "Success", Message = "User created successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }

        #endregion

        #region Login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO model)
        {
            try
            {
                using (var db = new IDDbContext())
                {
                    var authClaims = new List<Claim>();
                     string Name = "";

                    Applicationuser user = await userManager.FindByNameAsync(model.UserName);

                     if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                    {
                        var userRoles = await userManager.GetRolesAsync(user);

                        ////Get User Roles Name with FormId
                        var userrole = (from role in db.AspNetRoles
                                        join userRole in db.AspNetUserRoles on role.Id equals userRole.RoleId
                                        where userRole.UserId == user.Id
                                        select new
                                        {
                                            role.Name
                                        }).ToList();

                        ////////////////////////////////////
                        ///

                        authClaims = new List<Claim>
                             {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                         
                             };



                        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                        var token = new JwtSecurityToken(
                            issuer: _configuration["JWT:ValidIssuer"],
                            audience: _configuration["JWT:ValidAudience"],
                            expires: DateTime.Now.AddDays(364),
                            claims: authClaims,
                            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo.AddDays(364),
                            user,
                            userrole,
                            Name=user.UserName
                        });
                    }

                    return Unauthorized();
                }
            }
            catch (Exception ex)
               {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }



        }

        #endregion

        #region GetUserList
        [AllowAnonymous]
        [HttpPost]
        [Route("GetUsers")]
        public IActionResult GetAllUsers([FromBody] PaginationViewModel model)
        {
            try
            {
                var UserList = _authService.GetUsers(model);

                return Ok(new { UserList.Data, UserList.RecordsTotal });
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion

        #region Create Role

        [AllowAnonymous]
        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole(string role)
        {
            try
            {
                bool x = await roleManager.RoleExistsAsync(role);
                if (!x)
                {
                    using var db = new IDDbContext();

                    var userRole = new ApplicationRole { Name = role };

                    var result = await roleManager.CreateAsync(userRole);

                    if (result.Succeeded)
                    {
                        return Ok(new
                        {
                            userroles = roleManager.Roles.OrderBy(x => x.Name).Select(y => y.Name).ToList()
                        });
                    }
                    else
                    {
                        return BadRequest(result.Errors);
                    }


                }
                return BadRequest("Role Already Exists");
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }

        }

        #endregion

        #region GetAllRoles
        [Authorize]
        [HttpGet]
        [Route("Roles")]
        public async Task<IActionResult> Roles()
        {
            try
            {
                return Ok(new { userroles = roleManager.Roles.OrderBy(x => x.Name).Select(y => y.Name).ToList() });
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }
        }

        #endregion

        #region Delete Role

        [Authorize]
        [HttpDelete]
        [Route("DeleteRole")]
        public async Task<IActionResult> DeleteRole(string role)
        {

            try
            {
                var userRole = await roleManager.FindByNameAsync(role);

                var result = await roleManager.DeleteAsync(userRole);

                if (result.Succeeded)
                {
                    return Ok(new
                    {
                        userroles = roleManager.Roles.OrderBy(x => x.Name).Select(y => y.Name).ToList()
                    });
                }

            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }

            return BadRequest("");

        }

        #endregion

        #region AssignUserRoles
        [AllowAnonymous]
        [HttpPost]
        [Route("AssignUserRoles")]
        public async Task<IActionResult> AssignUserRoles([FromBody] UserRole userroles)
        {
            List<string> deleteRoleIds = new List<string>();
            List<string> addRoleIds = new List<string>();

            try
            {
                Applicationuser user = await userManager.FindByIdAsync(userroles.UserId);

                if (user != null)
                {
                    foreach (string role in userroles.DeleteRoleId)
                    {
                        if (!string.IsNullOrEmpty(role))
                        {
                            if (await userManager.IsInRoleAsync(user, role))
                            {
                                deleteRoleIds.Add(role);
                            }
                        }
                       
                    }

                    foreach (string role in userroles.AddRoleId)
                    {
                        //Debug.WriteLine(role);
                        if (!await userManager.IsInRoleAsync(user, role))
                        {
                            addRoleIds.Add(role);
                        }
                    }
                    await userManager.RemoveFromRolesAsync(user, deleteRoleIds);
                    await userManager.AddToRolesAsync(user, addRoleIds);
                }

                return Ok(new { Status = "Succeed", Message = "Roles Assigned Successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }

        }

        #endregion

        #region GetUserRoles

        [Authorize]
        [HttpGet]
        [Route("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            try
            {

                UserRolesResult userRolesResult = await _authService.GetUserRolesAsync(userId);

                return Ok(new { allRoles = userRolesResult.AllRoles, userRoles = userRolesResult.UserRoles });
            }
            catch (Exception ex)
            {
                return Ok(UtilService.GetExResponse<Exception>(ex));
            }

        }
        #endregion



        #region ChangePassword
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> Changepassword(ChangePassword model)
        {
            try
            {
                


                bool x = await _authService.ChangePassword(model);
                if (x)
                {
                    return Ok(new { Status = "Succeed", Message = "Password Successfully Changed" });
                }
                else
                {
                    return Ok(new { Status = "Failed", Message = "Password Mismatch" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(UtilService.GetExResponse<Exception>(ex));
            }
        }
        #endregion
    }
}
