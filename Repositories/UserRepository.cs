#nullable disable

using System.Data;
using Dapper;
using RazServer.Entities;

namespace RazServer.Repositories
{
    public interface IUserRepository
    {
        Task<UserAccount> Create(UserAccount item, IDbConnection tx = null);
    }

    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IConfiguration config) : base(config)
        {

        }
        public async Task<UserAccount> Create(UserAccount item, IDbConnection tx = null)
        {
            var query = @"
                    INSERT INTO user_account 
                    (first_name, middle_name, last_name, mobile_number, email, password_hash, dob, 
                    country_of_birth, gender, residential_address, created_at, updated_at, is_active) 
                    VALUES 
                    (@FirstName, @MiddleName, @LastName, @MobileNumber, @Email, @PasswordHash, @Dob, 
                    @CountryOfBirth, @Gender, @ResidentialAddress, now(), @UpdatedAt, @IsActive)
                    RETURNING *";

            using var con = NewConnection;

            return await con.QuerySingleOrDefaultAsync<UserAccount>(query, item);

        }
    }
}
