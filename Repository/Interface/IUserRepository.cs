using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Closetly.Models;

namespace Closetly.Repository
{
    public interface IUserRepository
    {
        public TbUser? GetById(Guid id);
        public void UpdateUser(Guid id, string newUserName, string newPhone, string newEmail);
    }
}