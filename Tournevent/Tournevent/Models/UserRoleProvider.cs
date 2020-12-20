using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Tournevent.Models
{
    public class UserRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            using (Entities _Context = new Entities())
            {
                foreach(string username in usernames)
                {
                    foreach(string roleName in roleNames)
                    {
                        int userId = (from user in _Context.Benutzer
                                      where user.Email == username
                                      select user.Id).FirstOrDefault();
                        int rolesId = (from role in _Context.Rollen
                                       where role.Rolle == roleName
                                      select role.Id).FirstOrDefault();
                        BenutzerRollen userRolesMapping = new BenutzerRollen();
                        userRolesMapping.BenutzerId = userId;
                        userRolesMapping.RollenId = rolesId;
                        _Context.BenutzerRollen.Add(userRolesMapping);
                        _Context.SaveChanges();
                    }
                }
                
            }
        }
        public  void AddUserToRole(string username, string roleName)
        {
            using (Entities _Context = new Entities())
            {
                int userId = (from user in _Context.Benutzer
                              where user.Email == username
                                                select user.Id).FirstOrDefault();
                int rolesId = (from role in _Context.Rollen
                               where role.Rolle == roleName
                                                select role.Id).FirstOrDefault();
                BenutzerRollen userRolesMapping = new BenutzerRollen();
                userRolesMapping.BenutzerId = userId;
                userRolesMapping.RollenId = rolesId;
                _Context.BenutzerRollen.Add(userRolesMapping);
                _Context.SaveChanges();
            }
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
            using (Entities _Context = new Entities())
            {
                var userRoles = (from user in _Context.Benutzer
                                 join roleMapping in _Context.BenutzerRollen
                                 on user.Id equals roleMapping.BenutzerId
                                 join role in _Context.Rollen
                                 on roleMapping.RollenId equals role.Id
                                 where user.Email == username
                                 select role.Rolle).ToArray();
                return userRoles;
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
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