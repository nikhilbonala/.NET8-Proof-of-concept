using Microsoft.EntityFrameworkCore;
using Test1LearnNewVersion.Data;
using Test1LearnNewVersion.Models.Entities;

namespace Test1LearnNewVersion.Repositories
{
    public class SQLUserRepository : IUserRepository
    {
        public readonly LearnDbContext dbContext;
        public SQLUserRepository(LearnDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<user> CreateAsync(user addUser)
        {
            await dbContext.Users.AddAsync(addUser);
            await dbContext.SaveChangesAsync();

            return addUser;
        }

        public async Task<user?> DeleteAsync(int id)
        {
            var existUser = await dbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);
            
            if (existUser==null)
            {
                return null;
            }

            dbContext.Users.Remove(existUser);
            await dbContext.SaveChangesAsync();

            return existUser;
        }

        public async Task<List<user>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null,
            bool isAscending = true, int pageNumber = 1, int pageSize = 10)
        {

            //var Users = await dbContext.Users.ToListAsync();
            //return Users;
            var Users =  dbContext.Users.AsQueryable();

            //filtering
            if(string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery)==false)
            {
                if(filterOn.Equals("email", StringComparison.OrdinalIgnoreCase))
                {
                    Users = Users.Where(x => x.Email.Contains(filterQuery));
                }
            }

            //sorting
            if(string.IsNullOrWhiteSpace(sortBy)==false)
            {
                if(sortBy.Equals("email", StringComparison.OrdinalIgnoreCase))
                {
                    Users = isAscending ? Users.OrderBy(x => x.Email) : Users.OrderByDescending(x => x.Email);
                }
                else if (sortBy.Equals("passwordhash", StringComparison.OrdinalIgnoreCase))
                {
                    Users = isAscending ? Users.OrderBy(x => x.PasswordHash) : Users.OrderByDescending(x => x.PasswordHash);

                }
            }

            //pagination
            var skipResults = (pageNumber - 1) * pageSize;


            return await Users.Skip(skipResults).Take(pageSize).ToListAsync();



        }

        public async Task<user?> GetByIdAsync(int id)
        {
            return await dbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<user?> UpdateAsync(int id, user userUpdate)
        {
            var existUser = await dbContext.Users.FirstOrDefaultAsync(x =>x.UserId == id);

            if (existUser == null)
            {
                return null;
            }
            
            existUser.Email = userUpdate.Email;
            existUser.PasswordHash = userUpdate.PasswordHash;

            await dbContext.SaveChangesAsync();
            return existUser;

        }
    }
}
