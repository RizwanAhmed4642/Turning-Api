using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Meeting_App.Data.Database.Tables;
using Meeting_App.Models;
using Meeting_App.Models.DTOs;
using Meeting_App.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Service.Auth
{
    public class AuthService
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly UserManager<Applicationuser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserRolesResult _userRolesResult;

        #endregion

        #region Constructors
        public AuthService(IMapper mapper, UserManager<Applicationuser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._userRolesResult = new UserRolesResult();
            _mapper = mapper;
        }
        #endregion

        #region GetUsersList
        public PaginationResult<AspNetUsers> GetUsers(PaginationViewModel model)
        {
            try
            {
                using (var db = new IDDbContext())
                {
                    List<AspNetUsers> userlist = db.AspNetUsers.OrderBy(x => x.UserName).ToList();

                    var TotalCount = userlist.Count();

                    //userlist = userlist.OrderBy(x => x.Id).Skip((model.Page - 1) * (model.PageSize)).Take(model.PageSize).ToList();

                    var res = this._mapper.Map<List<AspNetUsers>>(userlist.ToList());

                    return new PaginationResult<AspNetUsers> { Data = res, RecordsTotal = TotalCount };
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region GetUserRoles
        public async Task<UserRolesResult> GetUserRolesAsync(string userId)
        {
            try
            {
                Applicationuser applicationUser = await this.userManager.FindByIdAsync(userId);

                _userRolesResult.UserRoles = await this.userManager.GetRolesAsync(applicationUser);

                _userRolesResult.AllRoles = roleManager.Roles.OrderBy(x => x.Name).Select(y => y.Name).ToList();

                return _userRolesResult;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region ChangePassword
        public async Task<bool> ChangePassword(ChangePassword model)
        {
            IdentityResult result;
            try
            {
                Applicationuser applicationUser = await this.userManager.FindByIdAsync(model.userid.ToString());



                if (applicationUser != null)
                {
                    applicationUser.NP = model.Newpassword;
                }
                result = await userManager.ChangePasswordAsync(applicationUser, model.CurrentPassword, model.Newpassword);

                if (result.Succeeded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }

        }
        #endregion

    }

}