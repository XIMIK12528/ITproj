using Domain.Models.Enums;
using DbModels.Enums;

namespace DbModels.Converters.Enums
{
    public static class AccountRoleDbConvertor
    {
        public static AccountRoleDb ToDb(this AccountRole role)
        {
            return role switch
            {
                AccountRole.guest => AccountRoleDb.guest,
                AccountRole.student => AccountRoleDb.student,
                AccountRole.professor => AccountRoleDb.professor,
                AccountRole.admin => AccountRoleDb.admin,
                _ => throw new ArgumentOutOfRangeException(nameof(role), role, null),
            };
        }

        public static AccountRole ToDomain(this AccountRoleDb role)
        {
            return role switch
            {
                AccountRoleDb.guest => AccountRole.guest,
                AccountRoleDb.student => AccountRole.student,
                AccountRoleDb.professor => AccountRole.professor,
                AccountRoleDb.admin => AccountRole.admin,
                _ => throw new ArgumentOutOfRangeException(nameof(role), role, null),
            };
        }

    }
}
