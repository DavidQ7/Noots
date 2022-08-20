﻿using DatabaseContext.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatabaseContext.Seed
{
    public static class DatabaseInitializer
    {
        public static void SeedDatabase(ModelBuilder builder)
        {
            var rolesEntitesHolder = new RolesEntitiesHolder();

            AddEntities(builder, rolesEntitesHolder.GetRoles());

            var usersEntitiesHolder = new UsersEntitiesHolder();
            var users = usersEntitiesHolder.GetUsers();

            PasswordHasher<User> ph = new PasswordHasher<User>();

            foreach (var user in users)
            {
                //random GUID generated by hands
                user.SecurityStamp = "9819F4B5-F389-4603-BF0B-1E3C88379627";
                var newPassword = "1qaz2wsx";

                if (user.PasswordHash == null ||
                    ph.VerifyHashedPassword(user, user.PasswordHash, newPassword)
                    == PasswordVerificationResult.Failed)
                {
                    user.PasswordHash = ph.HashPassword(user, newPassword);
                }
            }

            AddEntities(builder, users);

            var userRolesEntitiesHolder = new UserRolesEntitiesHolder();

            AddEntities(builder, userRolesEntitiesHolder.GetUserRoles());
        }

        private static void AddEntities<T>(ModelBuilder builder, List<T> entities) where T : class
        {
            builder.Entity<T>().HasData(entities);
        }
    }
}