using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MDL_BASE.Models.IDENTITY;
using XLIB_COMMON.Repo.IDENTITY;
using MDL_BASE.ViewModel;
using XLIB_COMMON.Model;
using _LABELINSP_APPWEB.App_Start;
using MDL_BASE.Models.MasterData;
using MDL_BASE.Interfaces;
using XLIB_COMMON.Interface;
using _LABELINSP_APPWEB.Areas.IDENTITY.ViewModels;
using MDLX_MASTERDATA.Repos;
using Microsoft.Owin.Security.DataProtection;
using System;

namespace _LABELINSP_APPWEB.Areas.IDENTITY.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IDbContextCore db;  
        private readonly UserRepo UserManager;
        private readonly RoleRepo RoleManager;
        private readonly ApplicationSignInManager SignInManager;
        private readonly IAuthenticationManager AuthenticationManager;

        private string operatorTitle = "Operator";

        //public AccountController(IUserStore<User, string> userStore)
        //{

        //}

        public AccountController(
            IRoleStore<ApplicationRole, string> roleStore,
            IAuthenticationManager authenticationManager,
            IDbContextCore db,
            ApplicationSignInManager signInManager, //???
            IUserStore<User, string> userStore
            )
        {
            this.db = db;
            UserManager = new UserRepo(userStore, db);
            SignInManager = signInManager;
            RoleManager = new RoleRepo(roleStore, db);
            AuthenticationManager = authenticationManager;
        }

        public JsonResult Test()
        {
            RoleManager.AddRole(DefRoles.ILOGIS_VIEWER);
            UserManager.AddUser("wh_user", DefRoles.ILOGIS_VIEWER);
            
            return Json(0);
        }

        [HttpGet]
        public JsonResult AddUsers()
        {
            //Printer printer = new Printer();
            //FileStream fs = System.IO.File.Open(@"c:\inetpub\label.pdf", FileMode.Open);
            //printer.PrintPDF("Brother DCP-1610W series", fs, "label.pdf", false);
            //RoleManager.AddRole(DefRoles.ILOGIS_OPERATOR_FEEDING);
            //RoleManager.AddRole(DefRoles.ILOGIS_OPERATOR_INCOMING);
            //RoleManager.AddRole(DefRoles.ILOGIS_OPERATOR_PICKING);

            //UserManager.AddUser("_barszewa", DefRoles.ILOGIS_OPERATOR, "97813769");
            //UserManager.AddUser("_borowraf", DefRoles.ILOGIS_OPERATOR, "45964621");
            //UserManager.AddUser("_czechpaw", DefRoles.ILOGIS_OPERATOR, "78124028");
            //UserManager.AddUser("_dabroark", DefRoles.ILOGIS_OPERATOR, "42948801");
            //UserManager.AddUser("_delakmal", DefRoles.ILOGIS_OPERATOR, "83021640");
            //UserManager.AddUser("_dudekadr", DefRoles.ILOGIS_OPERATOR, "95058998");
            //UserManager.AddUser("_fignadar", DefRoles.ILOGIS_OPERATOR, "25071764");
            //UserManager.AddUser("_gontaada", DefRoles.ILOGIS_OPERATOR, "73196172");
            //UserManager.AddUser("_gutanmar", DefRoles.ILOGIS_OPERATOR, "37343193");
            //UserManager.AddUser("_maciedam", DefRoles.ILOGIS_OPERATOR, "47009796");
            //UserManager.AddUser("_malecwio", DefRoles.ILOGIS_OPERATOR, "46473312");
            //UserManager.AddUser("_mikomari", DefRoles.ILOGIS_OPERATOR, "88827701");
            //UserManager.AddUser("_mularpio", DefRoles.ILOGIS_OPERATOR, "37580798");
            //UserManager.AddUser("_nelecdam", DefRoles.ILOGIS_OPERATOR, "32216789");
            //UserManager.AddUser("_piejktad", DefRoles.ILOGIS_OPERATOR, "23217625");
            //UserManager.AddUser("_pudlopaw", DefRoles.ILOGIS_OPERATOR, "87607175");
            //UserManager.AddUser("_rogozdar", DefRoles.ILOGIS_OPERATOR, "31438607");
            //UserManager.AddUser("_skowrjak", DefRoles.ILOGIS_OPERATOR, "95928411");
            //UserManager.AddUser("_skowrmat", DefRoles.ILOGIS_OPERATOR, "15485663");
            //UserManager.AddUser("_sosnimar", DefRoles.ILOGIS_OPERATOR, "26462821");
            //UserManager.AddUser("_stawitom", DefRoles.ILOGIS_OPERATOR, "22346766");
            //UserManager.AddUser("_strakmar", DefRoles.ILOGIS_OPERATOR, "42902572");
            //UserManager.AddUser("_swedrpio", DefRoles.ILOGIS_OPERATOR, "16253915");
            //UserManager.AddUser("_tatarvik", DefRoles.ILOGIS_OPERATOR, "61299084");
            //UserManager.AddUser("_wajdaadr", DefRoles.ILOGIS_OPERATOR, "53373459");
            //UserManager.AddUser("_wozniann", DefRoles.ILOGIS_OPERATOR, "26649324");
            //UserManager.AddUser("_zajadawi", DefRoles.ILOGIS_OPERATOR, "82276845");
            //UserManager.AddUser("_zegarpio", DefRoles.ILOGIS_OPERATOR, "54391608");
            //UserManager.AddUser("_zurekkar", DefRoles.ILOGIS_OPERATOR, "28418926");

            //UserManager.AddUserNameToRole("_barszewa", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_borowraf", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_czechpaw", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_delakmal", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_dudekadr", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_fignadar", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_gontaada", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_gutanmar", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_maciedam", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_malecwio", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_mikomari", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_mularpio", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_piejktad", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_pudlopaw", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_rogozdar", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_skowrjak", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_skowrmat", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_sosnimar", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_stawitom", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_strakmar", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_swedrpio", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_tatarvik", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_wajdaadr", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_wozniann", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_zegarpio", DefRoles.ILOGIS_OPERATOR_PICKING);
            //UserManager.AddUserNameToRole("_barszewa", DefRoles.ILOGIS_OPERATOR_INCOMING);
            //UserManager.AddUserNameToRole("_czechpaw", DefRoles.ILOGIS_OPERATOR_INCOMING);
            //UserManager.AddUserNameToRole("_delakmal", DefRoles.ILOGIS_OPERATOR_INCOMING);
            //UserManager.AddUserNameToRole("_fignadar", DefRoles.ILOGIS_OPERATOR_INCOMING);
            //UserManager.AddUserNameToRole("_gontaada", DefRoles.ILOGIS_OPERATOR_INCOMING);
            //UserManager.AddUserNameToRole("_mikomari", DefRoles.ILOGIS_OPERATOR_INCOMING);
            //UserManager.AddUserNameToRole("_mularpio", DefRoles.ILOGIS_OPERATOR_INCOMING);
            //UserManager.AddUserNameToRole("_piejktad", DefRoles.ILOGIS_OPERATOR_INCOMING);
            //UserManager.AddUserNameToRole("_pudlopaw", DefRoles.ILOGIS_OPERATOR_INCOMING);
            //UserManager.AddUserNameToRole("_rogozdar", DefRoles.ILOGIS_OPERATOR_INCOMING);
            //UserManager.AddUserNameToRole("_skowrjak", DefRoles.ILOGIS_OPERATOR_INCOMING);
            //UserManager.AddUserNameToRole("_stawitom", DefRoles.ILOGIS_OPERATOR_INCOMING);
            //UserManager.AddUserNameToRole("_swedrpio", DefRoles.ILOGIS_OPERATOR_INCOMING);
            //UserManager.AddUserNameToRole("_tatarvik", DefRoles.ILOGIS_OPERATOR_INCOMING);
            //UserManager.AddUserNameToRole("_wozniann", DefRoles.ILOGIS_OPERATOR_INCOMING);
            //UserManager.AddUserNameToRole("_zegarpio", DefRoles.ILOGIS_OPERATOR_INCOMING);
            //UserManager.AddUserNameToRole("_barszewa", DefRoles.ILOGIS_OPERATOR_FEEDING);
            //UserManager.AddUserNameToRole("_borowraf", DefRoles.ILOGIS_OPERATOR_FEEDING);
            //UserManager.AddUserNameToRole("_czechpaw", DefRoles.ILOGIS_OPERATOR_FEEDING);
            //UserManager.AddUserNameToRole("_dabroark", DefRoles.ILOGIS_OPERATOR_FEEDING);
            //UserManager.AddUserNameToRole("_dudekadr", DefRoles.ILOGIS_OPERATOR_FEEDING);
            //UserManager.AddUserNameToRole("_fignadar", DefRoles.ILOGIS_OPERATOR_FEEDING);
            //UserManager.AddUserNameToRole("_gutanmar", DefRoles.ILOGIS_OPERATOR_FEEDING);
            //UserManager.AddUserNameToRole("_malecwio", DefRoles.ILOGIS_OPERATOR_FEEDING);
            //UserManager.AddUserNameToRole("_nelecdam", DefRoles.ILOGIS_OPERATOR_FEEDING);
            //UserManager.AddUserNameToRole("_piejktad", DefRoles.ILOGIS_OPERATOR_FEEDING);
            //UserManager.AddUserNameToRole("_pudlopaw", DefRoles.ILOGIS_OPERATOR_FEEDING);
            //UserManager.AddUserNameToRole("_skowrjak", DefRoles.ILOGIS_OPERATOR_FEEDING);
            //UserManager.AddUserNameToRole("_skowrmat", DefRoles.ILOGIS_OPERATOR_FEEDING);
            //UserManager.AddUserNameToRole("_stawitom", DefRoles.ILOGIS_OPERATOR_FEEDING);
            //UserManager.AddUserNameToRole("_swedrpio", DefRoles.ILOGIS_OPERATOR_FEEDING);
            //UserManager.AddUserNameToRole("_tatarvik", DefRoles.ILOGIS_OPERATOR_FEEDING);
            //UserManager.AddUserNameToRole("_wajdaadr", DefRoles.ILOGIS_OPERATOR_FEEDING);
            //UserManager.AddUserNameToRole("_wozniann", DefRoles.ILOGIS_OPERATOR_FEEDING);
            //UserManager.AddUserNameToRole("_zajadawi", DefRoles.ILOGIS_OPERATOR_FEEDING);
            //UserManager.AddUserNameToRole("_zegarpio", DefRoles.ILOGIS_OPERATOR_FEEDING);
            //UserManager.AddUserNameToRole("_zurekkar", DefRoles.ILOGIS_OPERATOR_FEEDING);


            return Json(0);
        }

        public ActionResult Browse()
        {
            IQueryable<User> users = UserManager.GetUsers().OrderBy(x => x.Name);

            List<ApplicationRole> roles = RoleManager.GetRoles().ToList();
            var list = roles.Select(rr =>
                new SelectListItem { Value = rr.Id.ToString(), Text = rr.Name }).ToList();

            ViewBag.Users = users;
            ViewBag.Roles = list;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Browse(string userId, string userNAme, string firstNAme, string lastNAme, string roleId, bool showOperators = false)
        {
            IQueryable<User> users = UserManager.Users.Where(u =>
                    (u.Id == userId || userId == "") &&
                    (u.UserName.Contains(userNAme) || userNAme == "") &&
                    (u.FirstName.Contains(firstNAme) || firstNAme == "") &&
                    (u.LastName.Contains(lastNAme) || lastNAme == "") &&
                    (u.Roles.Select(y => y.RoleId).Contains(roleId) || roleId == "") &&
                    ((showOperators == true && u.Title == operatorTitle) || (showOperators == false && u.Title != operatorTitle))
                ).OrderBy(u => u.UserName);

            List<ApplicationRole> roles = RoleManager.GetRoles().ToList();
            var list = roles.Select(rr =>
                new SelectListItem { Value = rr.Id.ToString(), Text = rr.Name }).ToList();

            ViewBag.Users = users;
            ViewBag.Roles = list;

            return View();
        }
        [HttpPost]
        public JsonResult UserGetList(UserGridViewModel filter)
        {
            IQueryable<User> users = UserManager.Users.Where(u =>
                    (u.Deleted == false) &&
                    (filter.Id == null || u.Id == filter.Id) &&
                    (filter.UserName == null || u.UserName.Contains(filter.UserName)) &&
                    (filter.FirstName == null || u.FirstName.Contains(filter.FirstName)) &&
                    (filter.LastName == null || u.LastName.Contains(filter.LastName)) &&
                    (filter.Title == null || u.Title.Contains(filter.Title)) &&
                    (filter.Factory == null || u.Factory.Contains(filter.Factory)) &&
                    (filter.DepartmentId == null || u.DepartmentId == filter.DepartmentId) &&
                    (filter.RoleId == null || u.Roles.Select(y => y.RoleId).Contains(filter.RoleId))
                ).OrderBy(u => u.UserName);

            List<UserGridViewModel> usersList = users.Select( x=> new UserGridViewModel {
                FirstName = x.FirstName,
                LastName = x.LastName,
                UserName = x.UserName,
                DepartmentId = x.DepartmentId,
                Email = x.Email,
                Factory = x.Factory,
                Id = x.Id,
                Title = x.Title
            }).ToList();

            return Json(usersList);
        }
         [HttpPost, AuthorizeRoles(DefRoles.ACCOUNT_ADMIN)]
        public JsonResult UserUpdate(UserGridViewModel item)
        {
            User user = UserManager.FindById(item.Id);

            if(user == null)
            {
                string userId = UserManager.AddUser(item.UserName);
                user = UserManager.FindById(userId);
                if(user == null) { return Json(0); }
            }

            user.FirstName = item.FirstName;
            user.LastName = item.LastName;
            user.DepartmentId = item.DepartmentId;
            user.Email = item.Email;
            user.Factory = item.Factory;
            user.Title = item.Title;

            UserManager.Update(user);

            return Json(user);
        }
        [HttpPost, AuthorizeRoles(DefRoles.ACCOUNT_ADMIN)]
        public JsonResult UserDelete(UserGridViewModel item)
        {
            User user = UserManager.FindById(item.Id);

            if (user != null)
            {
                //user.Deleted = true;
                //UserManager.Update(user);
                UserManager.SetDeleted(user.Id);
                return Json(1);
            }

            return Json(0);
        }

        [HttpPost, AuthorizeRoles(DefRoles.ACCOUNT_ADMIN)]
        public JsonResult UnLockUser(string Id)
        {
            User user = UserManager.FindById(Id);

            if (user != null)
            {
                user.LockoutEndDateUtc = DateTime.Now;
                UserManager.UpdateUser(user);
                return Json(1);
            }

            return Json(0);
        }

        public User GetUserById(string UserId)
        { 
            return UserManager.FindById(UserId); ;
        }

        public ActionResult Show(string id = null)
        {
            if (id == null)
            {
                id = User.Identity.GetUserId(); //UserManager.FindByName(User.Identity.Name)
            }

            EditViewModel vm = new EditViewModel();
            vm.User = UserManager.FindById(id);

            if (vm.User != null)
            {
                vm.UserRoles = (List<ApplicationRole>)RoleManager.GetRoles(id, true).ToList();
                return View(vm);
            }
            else
            {
                return RedirectToAction("Browse");
            }
        }

        public ActionResult Edit(string id)
        {
            int departmentId;
            if (id != null)
            {
                EditViewModel vm = new EditViewModel();
                //vm.EditUserRoleMode = true; //DefRoles.CheckAccessEditUserRoles(User);
                vm.User = UserManager.FindById(id);
                vm.EditUserMode = UserManager.IsInRole(User.Identity.GetUserId(), DefRoles.ACCOUNT_ADMIN);

                if (vm.EditUserMode && vm.User != null)
                {
                    IEnumerable<User> mgrList = UserManager.GetManagers();
                    IEnumerable<ApplicationRole> roles = RoleManager.GetRoles().ToList();
                    IEnumerable<Department> departments = new RepoDepartment(db).GetList();
                    vm.EditUserRoleMode = true;
                    vm.Managers = new SelectList(mgrList, "Id", "FullName"); //vm.User.SuperVisorUserID);
                    vm.Roles = new SelectList(roles, "Id", "Name");
                    if (vm.User.Department != null)
                    {
                        departmentId = vm.User.Department.Id;
                        vm.SelectedDepartmentId = departmentId;
                        vm.Departments = new SelectList(departments, "Id", "Name", vm.SelectedDepartmentId);
                    }
                    else
                    {
                        vm.Departments = new SelectList(departments, "Id", "Name");
                    }
                    vm.UserRoles = RoleManager.GetRoles(id, true).ToList(); //(List<IdentityRole>)UserManager.GetRoles(id, true);
                    vm.DefaultRoleId = string.Empty; //UserManager.GetDefaultRoleId(id);
                    vm.UserId = id;
                    //if (DefRoles.HasUserRole(User, DefRoles.Managers))
                    //{
                    //    vm.Subordinates = UserManager.GetSubordinates(vm.UserId);
                    //}

                    return View(vm);
                }
                else
                {
                    return RedirectToAction("Show", new { id = id });
                }
            }
            else
            {
                return RedirectToAction("Browse");
            }
        }
        [HttpPost, ValidateAntiForgeryToken, AuthorizeRoles(DefRoles.ACCOUNT_ADMIN)]
        public ActionResult Edit(EditViewModel vm)
        {

            if (ModelState.IsValid)
            {
                UserManager.SetDepartment(vm.UserId, vm.SelectedDepartmentId);
                UserManager.SetEmail(vm.UserId, vm.User.Email);
                UserManager.SetFirstName(vm.UserId, vm.User.FirstName);
                UserManager.SetLastName(vm.UserId, vm.User.LastName);
                UserManager.SetSupervisor(vm.UserId, vm.User.SuperVisorUserId);
                //UserManager.SetPosition(vm.UserId, vm.User.Position);
                UserManager.SetPhoneNumber(vm.UserId, vm.User.PhoneNumber);
                UserManager.SetDeputy(vm.UserId, vm.DeputyId);

            }

            return RedirectToAction("Edit", new { id = vm.UserId });
        }

        public JsonResult DeputiesAutoComplete(string prefix)
        {
            List<UserLight> ac = new List<UserLight>();
            var queue = (from t in UserManager.Users
                         where t.LastName.StartsWith(prefix)
                         select new UserLight()
                         {
                             FullName = t.LastName + " " + t.FirstName,
                             Id = t.Id
                         }).Take(5);
            ac.AddRange(queue.ToList());

            queue = (from t in UserManager.Users
                     where t.FirstName.StartsWith(prefix)
                     select new UserLight()
                     {
                         FullName = t.LastName + " " + t.FirstName,
                         Id = t.Id
                     }).Take(5);
            ac.AddRange(queue.ToList());


            return Json(ac, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = DefRoles.ACCOUNT_ADMIN)]
        public ActionResult RoleAddToUser(EditViewModel vm)
        {
            User user = UserManager.FindById(vm.UserId); //context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            //var account = new AccountController();

            if (!string.IsNullOrEmpty(vm.SelectedRoleId))
            {
                UserManager.AddToRole(user.Id, vm.SelectedRoleId, true);
                ViewBag.ResultMessage = "Role created successfully !";
            }

            // prepopulat roles for the view dropdown
            var list = RoleManager.GetRoles().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return RedirectToAction("Edit", new { id = vm.UserId });
        }

        [HttpGet]
        [Authorize(Roles = DefRoles.ACCOUNT_ADMIN)]
        public ActionResult DeleteRoleForUser(string userId, string RoleName)
        {
            //var account = new AccountController();

            if (UserManager.IsInRole(userId, RoleName))
            {
                UserManager.RemoveFromRole(userId, RoleName);
                ViewBag.ResultMessage = "Role removed from this user successfully !";
            }
            else
            {
                ViewBag.ResultMessage = "This user doesn't belong to selected role.";
            }
            // prepopulat roles for the view dropdown
            var list = RoleManager.GetRoles().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return RedirectToAction("Edit", new { id = userId });
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.OperatorMode = returnUrl.ToUpper().Contains("/MES/WORKPLACE");
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost, AllowAnonymous]
        public async Task<ActionResult> Login_old(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.OperatorMode = returnUrl.ToUpper().Contains("/MES/WORKPLACE");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            SignInStatus result = SignInStatus.RequiresVerification;
            if (IsAuthenticatedByLdap(model))
            {
                User user = UserManager.FindByName(model.UserName);
                if (user != null)
                    await UpdatePasswordIfNotMatch(user, model);
                else
                    await CreateUser(model);
            }
            result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: true);
            
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError(string.Empty, "Niepoprawne dane logowania.");
                    return View(model);
            }
        }

        private bool IsAuthenticatedByLdap(LoginViewModel model)
        {
            LDAP ldap = new LDAP();
            LDAP_result ldapResult = ldap.SearchUser(model.UserName, model.Password);

            return (ldapResult.Status == LDAP_ResponseStatus.Authenticated);
        }
        private async Task CreateUser(LoginViewModel model)
        {
            LDAP ldap = new LDAP();
            LDAP_result ldapResult = ldap.SearchUser(model.UserName, model.Password);
            LDAP_user ldapUser = ldap.GetUserData(ldapResult.Result);
            User user = new User() { 
                UserName = model.UserName, 
                FirstName = ldapUser.FirstName, 
                LastName = ldapUser.LastName, 
                Email = ldapUser.mail, 
                LockoutEnabled = false,
                PhoneNumber = ldapUser.mobile
            };
            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
                UserManager.AddToRole(user.Id, DefRoles.USER);
            else
                AddErrors(result);
        }
        private async Task<User> UpdatePasswordIfNotMatch(User user, LoginViewModel model)
        {
            User userReturn = await UserManager.FindAsync(model.UserName, model.Password);

            if (userReturn == null)
            {
                //zresetuj haslo do tego pasującego w AD
                UserManager.RemovePassword(user.Id);
                UserManager.AddPassword(user.Id, model.Password);
                userReturn = await UserManager.FindAsync(model.UserName, model.Password);
            }
            return userReturn;
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now.AddYears(-30);
                var user = new User { UserName = model.UserName, Email = model.Email, LastPasswordChangedDate = dt, LockoutEnabled = true };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Edit", "Account", new {id = user.Id });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult RegisterOperator()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterOperator(RegisterViewModel model)
        {
            model.Password = "Ytv53p6@";
            model.ConfirmPassword = "Ytv53p6@";

            if (model.UserName.Length > 0)
            {
                var user = new User { UserName = model.UserName, Email = model.UserName + "@operator.pl", Title = operatorTitle, LockoutEnabled = true };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddUserToRoleName(user.Id, DefRoles.ONEPROD_MES_OPERATOR);

                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Edit", "Account", new { id = user.Id });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            ICollection<SelectListItem> factorOptions = (ICollection<SelectListItem>)userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new User { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        public ActionResult Lockout()
        {
            return View();
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff(string redirectUrl = "")
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Abandon();
            if (redirectUrl.Length > 0)
                return RedirectToLocal(redirectUrl);
            else
                return RedirectToAction("Index", "Home", new { area = "" });
        }
        public ActionResult LogOffOneprodMes(int id = 0, string ip = "")
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Workplace", "MES", new { area = "ONEPROD", id=id, ip=ip });
        }


        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (UserManager != null)
                {
                    UserManager.Dispose();
                    //UserManager = null;
                }

                if (SignInManager != null)
                {
                    SignInManager.Dispose();
                    //SignInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}