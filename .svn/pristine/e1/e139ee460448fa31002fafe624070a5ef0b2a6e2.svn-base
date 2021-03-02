using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TourTravels.API.Models;
using TourTravels.Entities.Dal;
using TourTravels.Entities.Models;
using TourTravels.Entities.ViewModels;
namespace TourTravels.API.Areas.Admin.Controllers
{
    [Authorize]
    [RoutePrefix("api/Admin/agentconfig")]
    public class AgentController : ApiController
    {
        // GET: Admin/Agent

        private const string LocalLoginProvider = "Local";
        dalAgentConfig objDal = new dalAgentConfig();
        #region Initilize
        public AgentController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
            //var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("StatePortal");
            //UserManager.UserTokenProvider = new Microsoft.AspNet.Identity.Owin.DataProtectorTokenProvider<ApplicationUser>(
            //    provider.Create("ResetPassword"));
            var dataProtectionProvider = Startup.DataProtectionProvider;
            UserManager.UserTokenProvider =
            new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("tourtravels123"));
        }
        public AgentController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }


        #endregion


        #region Agent Registration

        [HttpGet]
        [Route("agentlist")]
        public async Task<AgentVM> AgentList(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetAgentListAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("saveagent")]
        public async Task<string> SaveAgent(utblAgent model)
        {
            if (ModelState.IsValid)
            {
                string result = "";
                string msg = objDal.SaveAgentAsync(model);
                if (!(msg.Contains("Agent Details Updated")))
                {
                    if (!(msg.Contains("Error")))
                    {
                        result = await Register(model.AgentEmail, model.AgentName, model.AgentMobile);
                    }

                    if (result != "User Registered")
                        await objDal.DeleteAgentAsync(Convert.ToInt64(msg));
                }
                else
                {
                    result = msg;
                }
                return result;
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("agentbyid")]
        public async Task<utblAgent> AgentByID(long AgentID)
        {
            return await objDal.GetAgentByIDAsync(AgentID);
        }
        [HttpDelete]
        [Route("deleteagent")]
        public async Task<string> DeleteAgent(long AgentID)
        {
            return await objDal.DeleteAgentAsync(AgentID);
        }

        public async Task<string> Register(string Email, string ProfileName, string MobileNo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return "Error: Missing required fields.";
                }

                ApplicationUser existingUser = await UserManager.FindByEmailAsync(Email);
                if (existingUser != null)
                {
                    return "Email already in use by other user, please choose another email";
                }

                ApplicationUser user = new ApplicationUser() { Email = Email, ProfileName = ProfileName, UserName = Email, PhoneNumber = MobileNo, IsActive = true };

                //string password = Membership.GeneratePassword(6, 1);

                IdentityResult result = await UserManager.CreateAsync(user, Email);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "Agent");

                    //send email..
                    //var code = await UserManager.GenerateUserTokenAsync("New", user.Id);
                    //var callbackUrl = domainUrl + "/Account/New?userId=" + user.Id + "&code=" + System.Web.HttpUtility.UrlEncode(code) + "&role=" + StringEncodeDecoder.Encrypt(model.RoleName, "userrole");
                    //string msg = "Please activate your " + model.RoleName + " account by clicking <a href=\"" + callbackUrl + "\">here</a>";
                    //await SendMail(model.Email, msg, "New");

                    return "User Registered";
                }

                return "Error: " + string.Join("; ", result.Errors
                                         .Select(x => x));

            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }


        #endregion




    }
}