using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Models;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UsersDTO>> GetAll();
        Task<UsersDTO> GetById(int id);
//        Task<UsersDTO> Create(CreationUserDTO creationUserDTO);
//        Task UpdateAsync(int Id, CreationUserDTO creationUserDTO);
        Task DeleteAsync(int Id);
        Task<UsersDTO> RecoverPassword(string Password);
    }
}
