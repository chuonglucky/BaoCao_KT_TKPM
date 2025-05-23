using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASC.business.interfaces;
using ASC.DataAccess.Interfaces;
using ASC.Model.Models;

namespace ASC.business
{

    public class MasterDataOperations : IMasterDataOperations
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<MasterDataKey> _masterDataKeyRepo;
        private readonly IRepository<MasterDataValue> _masterDataValueRepo;

        public MasterDataOperations(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _masterDataKeyRepo = _unitOfWork.Repository<MasterDataKey>();
            _masterDataValueRepo = _unitOfWork.Repository<MasterDataValue>();
        }

        // === MasterDataKey Operations ===
        public async Task<MasterDataKey> AddMasterDataKeyAsync(MasterDataKey masterDataKey)
        {
            if (masterDataKey == null) throw new ArgumentNullException(nameof(masterDataKey));
            if (string.IsNullOrWhiteSpace(masterDataKey.Name))
                throw new ArgumentException("Tên MasterDataKey không được để trống.", nameof(masterDataKey.Name));

            var existingKeys = await _masterDataKeyRepo.FindAllByQuery(k => k.Name.ToLower() == masterDataKey.Name.ToLower() && !k.IsDeleted);
            if (existingKeys.Any())
            {
                throw new InvalidOperationException($"MasterDataKey với tên '{masterDataKey.Name}' đã tồn tại và chưa bị xóa.");
            }

            await _masterDataKeyRepo.AddAsync(masterDataKey);
            await _unitOfWork.SaveChangesAsync(); // Thêm dòng này
            return masterDataKey;
        }

        public async Task<MasterDataKey> GetMasterDataKeyByIdAsync(Guid id, bool includeSoftDeleted = false)
        {
            var key = await _masterDataKeyRepo.FindAsync(id);
            if (key == null) return null; // Không tìm thấy key
            if (!includeSoftDeleted && key.IsDeleted)
            {
                return null; // Coi như không tìm thấy nếu đã soft-delete và không yêu cầu lấy
            }
            return key;
        }

        public async Task<MasterDataKey> GetMasterDataKeyByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            // Trả về key active và không bị soft-delete
            var keys = await _masterDataKeyRepo.FindAllByQuery(k => k.Name == name && !k.IsDeleted && k.IsActive);
            return keys.FirstOrDefault(); // Giả định tên là duy nhất trong số các key active, không bị xóa
        }

        public async Task<IEnumerable<MasterDataKey>> GetMasterDataKeysAsync(bool includeInactive, bool includeSoftDeleted = false)
        {
            return await _masterDataKeyRepo.FindAllByQuery(k =>
                (includeSoftDeleted || !k.IsDeleted) &&
                (includeInactive || k.IsActive)
            );
        }

        public async Task UpdateMasterDataKeyAsync(MasterDataKey masterDataKey)
        {
            if (masterDataKey == null) throw new ArgumentNullException(nameof(masterDataKey));

            var existingKey = await _masterDataKeyRepo.FindAsync(masterDataKey.Id);
            if (existingKey == null || existingKey.IsDeleted)
            {
                throw new KeyNotFoundException($"Không tìm thấy MasterDataKey với Id '{masterDataKey.Id}' hoặc đã bị xóa.");
            }

            if (!string.IsNullOrWhiteSpace(masterDataKey.Name) && existingKey.Name != masterDataKey.Name)
            {
                var duplicateCheck = await _masterDataKeyRepo.FindAllByQuery(k =>
                    k.Name.ToLower() == masterDataKey.Name.ToLower() &&
                    k.Id != masterDataKey.Id &&
                    !k.IsDeleted);
                if (duplicateCheck.Any())
                {
                    throw new InvalidOperationException($"Một MasterDataKey khác với tên '{masterDataKey.Name}' đã tồn tại.");
                }
                existingKey.Name = masterDataKey.Name;
            }

            existingKey.IsActive = masterDataKey.IsActive;
            _masterDataKeyRepo.Update(existingKey);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteMasterDataKeyAsync(Guid id)
        {
            var masterDataKey = await _masterDataKeyRepo.FindAsync(id);
            if (masterDataKey == null)
            {
                return; // Hoặc throw KeyNotFoundException tùy theo yêu cầu
            }
            if (masterDataKey.IsDeleted) return;

            // Soft delete các MasterDataValue liên quan
            var valuesToDelete = await _masterDataValueRepo.FindAllByQuery(v => v.MasterDataKeyId == id && !v.IsDeleted);
            foreach (var value in valuesToDelete)
            {
                _masterDataValueRepo.Delete(value);
            }

            _masterDataKeyRepo.Delete(masterDataKey);
            await _unitOfWork.SaveChangesAsync();
        }


        // === MasterDataValue Operations ===
        public async Task<MasterDataValue> AddMasterDataValueAsync(MasterDataValue masterDataValue)
        {
            if (masterDataValue == null) throw new ArgumentNullException(nameof(masterDataValue));
            if (masterDataValue.MasterDataKeyId == Guid.Empty)
                throw new ArgumentException("MasterDataKeyId phải được cung cấp.", nameof(masterDataValue.MasterDataKeyId));
            if (string.IsNullOrWhiteSpace(masterDataValue.Name))
                throw new ArgumentException("Tên MasterDataValue không được để trống.", nameof(masterDataValue.Name));

            var parentKey = await GetMasterDataKeyByIdAsync(masterDataValue.MasterDataKeyId); // Mặc định không lấy key đã soft-delete
            if (parentKey == null) // Nếu GetMasterDataKeyByIdAsync trả về null (do bị xóa hoặc không tồn tại)
            {
                throw new InvalidOperationException($"Không tìm thấy MasterDataKey cha với Id '{masterDataValue.MasterDataKeyId}' hoặc đã bị xóa.");
            }
            if (!parentKey.IsActive)
            {
                throw new InvalidOperationException($"MasterDataKey cha '{parentKey.Name}' không hoạt động. Không thể thêm value.");
            }

            var existingValues = await _masterDataValueRepo.FindAllByQuery(v =>
                v.MasterDataKeyId == masterDataValue.MasterDataKeyId &&
                v.Name.ToLower() == masterDataValue.Name.ToLower() &&
                !v.IsDeleted);
            if (existingValues.Any())
            {
                throw new InvalidOperationException($"MasterDataValue với tên '{masterDataValue.Name}' đã tồn tại cho MasterDataKey này và chưa bị xóa.");
            }

            await _masterDataValueRepo.AddAsync(masterDataValue);
            await _unitOfWork.SaveChangesAsync(); // Thêm dòng này
            return masterDataValue;
        }

        public async Task<MasterDataValue> GetMasterDataValueByIdAsync(Guid id, bool includeSoftDeleted = false)
        {
            var value = await _masterDataValueRepo.FindAsync(id);
            if (value == null) return null;
            if (!includeSoftDeleted && value.IsDeleted)
            {
                return null;
            }
            return value;
        }

        public async Task<IEnumerable<MasterDataValue>> GetMasterDataValuesAsync(Guid masterDataKeyId, bool includeInactive, bool includeSoftDeleted = false)
        {
            // Kiểm tra xem MasterDataKey cha có tồn tại và không bị xóa không (trừ khi explicitly yêu cầu includeSoftDeleted)
            // Điều này quan trọng để tránh trả về value của một key đã bị xóa (trừ khi có yêu cầu đặc biệt)
            var parentKey = await GetMasterDataKeyByIdAsync(masterDataKeyId, includeSoftDeleted);
            if (parentKey == null) // Nếu key cha không tồn tại (hoặc bị xóa và không được yêu cầu)
            {
                // Nếu key cha bị xóa và không yêu cầu includeSoftDeleted, thì không nên trả về value nào của nó
                if (!includeSoftDeleted) return Enumerable.Empty<MasterDataValue>();
                // Nếu key cha bị xóa và YÊU CẦU includeSoftDeleted, vẫn có thể lấy các value (tùy theo logic của includeSoftDeleted cho value)
            }
            // Nếu key cha tồn tại (hoặc bị xóa nhưng được yêu cầu includeSoftDeleted)
            // Và nếu key cha không active, nhưng yêu cầu includeInactive, vẫn tiếp tục lấy values
            if (parentKey != null && !parentKey.IsActive && !includeInactive)
            {
                // Nếu key cha không active và không yêu cầu includeInactive, không trả về value nào
                return Enumerable.Empty<MasterDataValue>();
            }


            return await _masterDataValueRepo.FindAllByQuery(v =>
                v.MasterDataKeyId == masterDataKeyId &&
                (includeSoftDeleted || !v.IsDeleted) &&
                (includeInactive || v.IsActive)
            );
        }

        public async Task<IEnumerable<MasterDataValue>> GetMasterDataValuesAsync(string masterDataKeyName, bool includeInactive, bool includeSoftDeleted = false)
        {
            // GetMasterDataKeyByNameAsync trả về key active, không bị soft-delete.
            // Nếu bạn muốn lấy value của key inactive hoặc soft-deleted thông qua tên, bạn cần một phương thức GetMasterDataKeyByNameAsync khác
            // hỗ trợ các tham số includeInactive/includeSoftDeleted, hoặc điều chỉnh logic ở đây.
            // Hiện tại, nó sẽ chỉ lấy value cho key active, không bị xóa khớp với tên.
            var parentKey = await GetMasterDataKeyByNameAsync(masterDataKeyName);
            if (parentKey == null) // Sẽ là null nếu key không active, bị xóa, hoặc không tồn tại
            {
                // Nếu masterDataKeyName không trỏ đến một key active, không bị xóa,
                // và không có cờ includeInactive/includeSoftDeleted ở mức key name search,
                // thì việc trả về empty là hợp lý.
                // Nếu muốn tìm value của key inactive/soft-deleted bằng tên, logic này cần mở rộng.
                return Enumerable.Empty<MasterDataValue>();
            }

            // Nếu parentKey.IsActive là false ở đây (dù GetMasterDataKeyByNameAsync thường trả về active),
            // và !includeInactive, thì nên trả về empty.
            // Tuy nhiên, GetMasterDataKeyByNameAsync đã lọc IsActive=true, nên kiểm tra này có thể không cần thiết
            // trừ khi GetMasterDataKeyByNameAsync thay đổi.
            // if (!parentKey.IsActive && !includeInactive) return Enumerable.Empty<MasterDataValue>();


            return await GetMasterDataValuesAsync(parentKey.Id, includeInactive, includeSoftDeleted);
        }

        public async Task UpdateMasterDataValueAsync(MasterDataValue masterDataValue)
        {
            if (masterDataValue == null) throw new ArgumentNullException(nameof(masterDataValue));

            var existingValue = await _masterDataValueRepo.FindAsync(masterDataValue.Id);
            if (existingValue == null || existingValue.IsDeleted)
            {
                throw new KeyNotFoundException($"Không tìm thấy MasterDataValue với Id '{masterDataValue.Id}' hoặc đã bị xóa.");
            }

            // Kiểm tra Key cha có active không trước khi cho update value của nó
            // (trừ khi value đó đang được set là Inactive)
            if (masterDataValue.IsActive) // Chỉ kiểm tra nếu value đang được cập nhật thành active
            {
                var parentKey = await GetMasterDataKeyByIdAsync(existingValue.MasterDataKeyId); // Lấy key cha (không bao gồm soft-deleted)
                if (parentKey == null || !parentKey.IsActive)
                {
                    throw new InvalidOperationException($"Không thể cập nhật MasterDataValue thành active vì MasterDataKey cha ('{parentKey?.Name ?? existingValue.MasterDataKeyId.ToString()}') không tồn tại hoặc không hoạt động.");
                }
            }


            if (masterDataValue.MasterDataKeyId != Guid.Empty && existingValue.MasterDataKeyId != masterDataValue.MasterDataKeyId)
            {
                throw new InvalidOperationException("Không thể thay đổi MasterDataKeyId của một MasterDataValue đã tồn tại. Hãy xóa và tạo lại dưới key mới nếu cần.");
            }

            if (!string.IsNullOrWhiteSpace(masterDataValue.Name) && existingValue.Name != masterDataValue.Name)
            {
                var duplicateCheck = await _masterDataValueRepo.FindAllByQuery(v =>
                    v.MasterDataKeyId == existingValue.MasterDataKeyId &&
                    v.Name.ToLower() == masterDataValue.Name.ToLower() &&
                    v.Id != masterDataValue.Id &&
                    !v.IsDeleted);
                if (duplicateCheck.Any())
                {
                    throw new InvalidOperationException($"Một MasterDataValue khác với tên '{masterDataValue.Name}' đã tồn tại cho MasterDataKey này.");
                }
                existingValue.Name = masterDataValue.Name;
            }

            existingValue.IsActive = masterDataValue.IsActive;
            _masterDataValueRepo.Update(existingValue);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteMasterDataValueAsync(Guid id)
        {
            var masterDataValue = await _masterDataValueRepo.FindAsync(id);
            if (masterDataValue == null)
            {
                return;
            }
            if (masterDataValue.IsDeleted) return;

            _masterDataValueRepo.Delete(masterDataValue);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
