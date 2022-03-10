using Core;
using Microsoft.EntityFrameworkCore;

namespace Syst
{

    public class AdminRepository : IAdminRepository
    {
        ISystematicContext _context;
        public AdminRepository(ISystematicContext context)
        {
            _context = context;
        }

        public async Task<(Status, int id)> Create(AdminDTO adminDTO) {

            foreach (Admin a in _context.admins) {
                if (a.Name == a.Name) return (Status.Conflict, a.Id);
            }
                var entity = new Admin
                {
                    Name = adminDTO.Name!,
                    Email = adminDTO.Email!,
                };

                _context.admins.Add(entity);

                await _context.SaveChangesAsync();

                return (Status.Created, entity.Id);
        }

        public async Task<(Status, int)> ReadIdFromName(string name) {
            int id = await _context.admins.Where(a => a.Name == name).Select(a => a.Id).FirstOrDefaultAsync();
            return (id == 0 ? Status.NotFound : Status.Found, id);
        }

        public async Task<(Status, AdminDTO)> Read(int id)
        {
            var a = await _context.admins.Where(a => a.Id == id).Select(a => new AdminDTO(){
                Name = a.Name!,
                Id = a.Id,
                Email = a.Email!,
            }).FirstOrDefaultAsync();

            if (a == default(AdminDTO)) return (Status.NotFound, a);
            else return (Status.Found, a);
        }
        public async Task<Status> Update(int id, AdminDTO adminDTO)
        {
            var a = await _context.admins.Where(a => a.Id == id).FirstOrDefaultAsync();

            if (a == default(Admin)) return Status.NotFound;

            a.Name = adminDTO.Name;
            a.Email = adminDTO.Email!;

            await _context.SaveChangesAsync();

            return Status.Updated;
        }

        public async Task<Status> Delete(int id){

            var a = await _context.admins.Where(a => a.Id == id).FirstOrDefaultAsync();
            
            if (a == default(Admin)) return Status.NotFound;

            _context.admins.Remove(a);

            await _context.SaveChangesAsync();
            return Status.Deleted;
        }
    }
}