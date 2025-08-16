#nullable disable

using System.Data;
using Dapper;
using Nconnect.Entities;
using RazServer.Entities;

namespace RazServer.Repositories
{
    public interface IUserRepository
    {
        Task<UserAccount> Create(UserAccount item, TxConnection tx = null);
        Task<UserDocument> CreateDocument(UserDocument item, TxConnection tx = null);
        Task<UserBankAccount> CreateBankAccount(UserBankAccount item, TxConnection tx = null);
    }

    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IConfiguration config) : base(config)
        {

        }
        public async Task<UserAccount> Create(UserAccount item, TxConnection tx = null)
        {
            var query = @"
                    INSERT INTO user_account 
                    (first_name, middle_name, last_name, country_code, mobile_number, email, password_hash, dob, 
                    country_of_birth, gender, residential_address, created_at, updated_at, is_active) 
                    VALUES 
                    (@FirstName, @MiddleName, @LastName, @CountryCode, @MobileNumber, @Email, @PasswordHash, @Dob, 
                    @CountryOfBirth, @Gender, @ResidentialAddress, now(), @UpdatedAt, @IsActive)
                    RETURNING *";

            using var con = NewConnection;
            return await (tx?.con ?? con).QuerySingleOrDefaultAsync<UserAccount>(query, item, tx?.transaction);
        }

        public async Task<UserDocument> CreateDocument(UserDocument item, TxConnection tx = null)
        {
            var query = @"
                INSERT INTO user_document
                (user_id, document_media_id, created_at, updated_at)
                VALUES
                (@UserId, @DocumentMediaId, now(), @UpdatedAt)
                RETURNING *";

            using var con = NewConnection;
            return await (tx?.con ?? con).QuerySingleOrDefaultAsync<UserDocument>(query, item, tx?.transaction); ;
        }

        public async Task<UserBankAccount> CreateBankAccount(UserBankAccount item, TxConnection tx = null)
        {
            var query = @"
                INSERT INTO user_bank_account
                (user_id, bank_name, account_number, ifsc_code, bank_media_id, created_at, updated_at)
                VALUES
                (@UserId, @BankName, @AccountNumber, @IfscCode, @BankMediaId, now(), @UpdatedAt)
                RETURNING *";

            using var con = NewConnection;
            return await (tx?.con ?? con).QuerySingleOrDefaultAsync<UserBankAccount>(query, item, tx?.transaction); ;
        }
    }
}
