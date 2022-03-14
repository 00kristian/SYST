
using System.Collections.Generic;


namespace Core {
public interface IAdminRepository
    {
        Task<(Status, int id)> Create(AdminDTO adminDTO);
        Task<(Status, AdminDTO)> Read(int id);
        Task<(Status, string?)> ReadNameFromId(int id);
        Task<Status> Update(int id, AdminDTO adminDTO);
        Task<Status> Delete(int id);

    }
}