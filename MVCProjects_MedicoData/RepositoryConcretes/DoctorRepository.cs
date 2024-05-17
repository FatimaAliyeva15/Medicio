using MVCProjects_MedicoCore.Models;
using MVCProjects_MedicoCore.RepositoryAbstracts;
using MVCProjects_MedicoData.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProjects_MedicoData.RepositoryConcretes
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
