using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using farmLogin.Models;
using System.Web.Caching;

namespace farmLogin
{
    public class MyRoleProvider : RoleProvider
    {
        private int _cacheTimeoutInMinute = 20;
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }
            //Check cache
            var cacheKey = string.Format("{0}_role", username);
            if(HttpRuntime.Cache[cacheKey] != null)
            {
                return (string[])HttpRuntime.Cache[cacheKey];
            }
            string[] roles = new string[] { };
            using (FarmDbContext dc = new FarmDbContext())
            {
                //roles = (from a in dc.UserAccessLevels
                //            join b in dc.UserRoles on a.UserAccessLevelID equals b.UserAccesssLevelID
                //            join c in dc.Users on b.UserID equals c.UserID
                //            where c.UserEmailAddress.Equals(username)
                //            select a.UserAccessLevelDescr).ToArray<string>();
                roles = (from a in dc.UserAccessLevels
                         join b in dc.Users on a.UserAccessLevelID equals b.UserAccessLevelID
                         where b.UserEmailAddress.Equals(username)
                         select a.UserAccessLevelDescr).ToArray<string>();
                if (roles.Count() > 0)
                {
                    HttpRuntime.Cache.Insert(cacheKey, roles, null, DateTime.Now.AddMinutes(_cacheTimeoutInMinute), Cache.NoSlidingExpiration);
                }
            }
            return roles;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var userRoles = GetRolesForUser(username);
            return userRoles.Contains(roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}