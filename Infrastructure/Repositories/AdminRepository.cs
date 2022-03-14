using Core;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{

    public class AdminRepository : IAdminRepository
    {
        ISystematicContext _context;
        public AdminRepository(ISystematicContext context)
        {
            _context = context;
        }

        //Creates an admin
        public async Task<(Status, int id)> Create(AdminDTO adminDTO) {

            foreach (Admin a in _context.Admins) {
                if (a.Name == a.Name) return (Status.Conflict, a.Id);
            }
                var entity = new Admin
                {
                    Name = adminDTO.Name!,
                    Email = adminDTO.Email!,
                };

                _context.Admins.Add(entity);

                await _context.SaveChangesAsync();

                return (Status.Created, entity.Id);
        }

        //Return a name given the admin id 
        public async Task<(Status, string?)> ReadNameFromId(int id) {
            string? name = await _context.Admins.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefaultAsync();
            return (name == null ? Status.NotFound : Status.Found, name);
        }

        //Return an admin given the admin id
        public async Task<(Status, AdminDTO)> Read(int id)
        {
            var a = await _context.Admins.Where(a => a.Id == id).Select(a => new AdminDTO(){
                Name = a.Name!,
                Id = a.Id,
                Email = a.Email!,
            }).FirstOrDefaultAsync();

            if (a == default(AdminDTO)) return (Status.NotFound, a);
            else return (Status.Found, a);
        }

        //Updates an admins name and email values
        public async Task<Status> Update(int id, AdminDTO adminDTO)
        {
            var a = await _context.Admins.Where(a => a.Id == id).FirstOrDefaultAsync();

            if (a == default(Admin)) return Status.NotFound;

            a.Name = adminDTO.Name;
            a.Email = adminDTO.Email!;

            await _context.SaveChangesAsync();

            return Status.Updated;
        }

        //Deletes an admin given the admin id
        public async Task<Status> Delete(int id){

            var a = await _context.Admins.Where(a => a.Id == id).FirstOrDefaultAsync();
            
            if (a == default(Admin)) return Status.NotFound;

            _context.Admins.Remove(a);

            await _context.SaveChangesAsync();
            return Status.Deleted;
        }
    }
}