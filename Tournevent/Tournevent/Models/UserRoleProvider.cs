using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
            using (DBContext db = new DBContext())
            {
                foreach(string username in usernames)
                {
                    foreach(string roleName in roleNames)
                    {
                        int userId = (from user in db.Benutzer
                                      where user.Email == username
                                      select user.Id).FirstOrDefault();
                        int rolesId = (from role in db.Rollen
                                       where role.Rolle == roleName
                                      select role.Id).FirstOrDefault();
                        BenutzerRollen userRolesMapping = new BenutzerRollen();
                        userRolesMapping.BenutzerId = userId;
                        userRolesMapping.RollenId = rolesId;
                        db.BenutzerRollen.Add(userRolesMapping);
                        db.SaveChanges();
                    }
                }
                
            }
        }
        public  void AddUserToRole(string username, string roleName)
        {
            using (DBContext db = new DBContext())
            {
                int userId = (from user in db.Benutzer
                              where user.Email == username
                                                select user.Id).FirstOrDefault();
                int rolesId = (from role in db.Rollen
                               where role.Rolle == roleName
                                                select role.Id).FirstOrDefault();
                BenutzerRollen userRolesMapping = new BenutzerRollen();
                userRolesMapping.BenutzerId = userId;
                userRolesMapping.RollenId = rolesId;
                db.BenutzerRollen.Add(userRolesMapping);
                db.SaveChanges();
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
            using (DBContext db = new DBContext())
            {
                var userRoles = (from user in db.Benutzer
                                 join roleMapping in db.BenutzerRollen
                                 on user.Id equals roleMapping.BenutzerId
                                 join role in db.Rollen
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
            using (DBContext db = new DBContext())
            {
                var userId = (from b in db.Benutzer
                              where b.Email == username
                              select b.Id).SingleOrDefault();
                var roleId = (from b in db.Rollen
                              where b.Rolle == roleName
                              select b.Id).SingleOrDefault();
                var userInRole = (from r in db.Rollen
                                  join b in db.BenutzerRollen on r.Id equals b.RollenId
                                  where b.BenutzerId == userId && b.RollenId == roleId
                                  select b).SingleOrDefault();
                return (userInRole != null);
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public void ChangeUserRole(string username, string roleName)
        {
            using (DBContext db = new DBContext())
            {
                var userId = (from b in db.Benutzer
                              where b.Email == username
                              select b.Id).SingleOrDefault();
                var id = (from b in db.BenutzerRollen
                          where b.BenutzerId == userId
                          select b.Id).SingleOrDefault();
                var roleId = (from b in db.Rollen
                              where b.Rolle == roleName
                              select b.Id).SingleOrDefault();
                BenutzerRollen userRolesMapping = new BenutzerRollen();
                userRolesMapping.Id = id;
                userRolesMapping.BenutzerId = userId;
                userRolesMapping.RollenId = roleId;
                db.BenutzerRollen.Attach(userRolesMapping);
                ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.ChangeObjectState(userRolesMapping, System.Data.Entity.EntityState.Modified);
                db.SaveChanges();
            }
        }
    }
}