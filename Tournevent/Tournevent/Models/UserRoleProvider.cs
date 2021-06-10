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
            using (DataContext db = new DataContext())
            {
                foreach(string username in usernames)
                {
                    foreach(string roleName in roleNames)
                    {
                        Benutzer benutzer = (from user in db.Benutzer
                                      where user.Email == username
                                      select user).FirstOrDefault();
                        benutzer.Rolle = roleName;
                        db.Benutzer.Add(benutzer);
                        db.SaveChanges();
                    }
                }
                
            }
        }
        public  void AddUserToRole(string username, string roleName)
        {
            using (DataContext db = new DataContext())
            {
                Benutzer benutzer = (from user in db.Benutzer
                              where user.Email == username
                                                select user).FirstOrDefault();
                benutzer.Rolle = roleName;
                db.Benutzer.Add(benutzer);
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
            using (DataContext db = new DataContext())
            {
                var userRoles = (from user in db.Benutzer
                                 where user.Email == username
                                 select user.Rolle).ToArray();
                return userRoles;
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (DataContext db = new DataContext())
            {
                Benutzer benutzer = (from b in db.Benutzer
                              where b.Email == username select b).SingleOrDefault();
                return (benutzer.Rolle == roleName);
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
            using (DataContext db = new DataContext())
            {
                Benutzer benutzer = (from b in db.Benutzer
                              where b.Email == username
                              select b).SingleOrDefault();
                benutzer.Rolle = roleName;
                db.Benutzer.Attach(benutzer);
                ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.ChangeObjectState(benutzer, System.Data.Entity.EntityState.Modified);
                db.SaveChanges();
            }
        }
    }
}