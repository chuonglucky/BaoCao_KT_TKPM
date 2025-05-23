using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASC.Model.Models;

namespace ASC.business.interfaces
{
    public interface IMasterDataOperations
    {
        // === MasterDataKey Operations ===
        Task<MasterDataKey> AddMasterDataKeyAsync(MasterDataKey masterDataKey);
        Task<MasterDataKey> GetMasterDataKeyByIdAsync(Guid id, bool includeSoftDeleted = false);
        Task<MasterDataKey> GetMasterDataKeyByNameAsync(string name); // Trả về key active, không bị soft-delete
        Task<IEnumerable<MasterDataKey>> GetMasterDataKeysAsync(bool includeInactive, bool includeSoftDeleted = false);
        Task UpdateMasterDataKeyAsync(MasterDataKey masterDataKey);
        Task DeleteMasterDataKeyAsync(Guid id); // Soft delete

        // === MasterDataValue Operations ===
        Task<MasterDataValue> AddMasterDataValueAsync(MasterDataValue masterDataValue);
        Task<MasterDataValue> GetMasterDataValueByIdAsync(Guid id, bool includeSoftDeleted = false);
        Task<IEnumerable<MasterDataValue>> GetMasterDataValuesAsync(Guid masterDataKeyId, bool includeInactive, bool includeSoftDeleted = false);
        Task<IEnumerable<MasterDataValue>> GetMasterDataValuesAsync(string masterDataKeyName, bool includeInactive, bool includeSoftDeleted = false);
        Task UpdateMasterDataValueAsync(MasterDataValue masterDataValue);
        Task DeleteMasterDataValueAsync(Guid id); // Soft delete
    }
}
