using Microsoft.AspNetCore.Mvc;
using Test1LearnNewVersion.Models.DTO;
using Test1LearnNewVersion.Models.Entities;

namespace Test1LearnNewVersion.Repositories
{
    public interface IUserRepository
    {

        //beacuse of design consistency and safety in coding practices tolist return empty list, not the null value for list, so we dont use nullable reffrenece
        Task<List<user>> GetAllAsync(string? filterOn=null, string? filterQuery=null, string? sortBy=null, bool isAscending=true,
            int pageNumber=1, int pageSize=10);
        
        // here if we didnt find id, user can be nullable
        Task<user?> GetByIdAsync(int id);

        Task<user> CreateAsync(user addUser);

        Task<user?> UpdateAsync(int id,user userUpdate);

        Task<user?> DeleteAsync(int id);
    }
}
