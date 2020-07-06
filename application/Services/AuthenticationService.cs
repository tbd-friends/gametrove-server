using System;
using System.Linq;
using GameTrove.Application.Infrastructure;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using GameTrove.Storage.Models;

namespace GameTrove.Application.Services
{
    public class AuthenticationService
    {
        private readonly GameTrackerContext _context;

        public AuthenticationService(GameTrackerContext context)
        {
            _context = context;
        }

        public UserViewModel Get(string email)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == email);

            return new UserViewModel
            {
                UserId = user.Id,
                TenantId = user.TenantId
            };
        }

        public bool Verify(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var user = _context.Users.SingleOrDefault(u => u.Email == email);

                if (user == null)
                {
                    user = new User
                    {
                        Email = email,
                        TenantId = Guid.NewGuid()
                    };

                    _context.Add(user);

                    _context.SaveChanges();
                }

                return true;
            }

            return false;
        }
    }
}