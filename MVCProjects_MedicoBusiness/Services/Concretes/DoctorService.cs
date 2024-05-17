using MVCProjects_MedicoBusiness.Exceptions.Doctor;
using MVCProjects_MedicoBusiness.Services.Abstracts;
using MVCProjects_MedicoCore.Models;
using MVCProjects_MedicoCore.RepositoryAbstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProjects_MedicoBusiness.Services.Concretes
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public void AddDoctor(Doctor doctor)
        {
            if (doctor == null) throw new NullReferenceException("Doctor not null");

            if (!doctor.ImgFile.ContentType.Contains("image/"))
                throw new FileContentTypeException("ImgFile", "File content type error");

            if (doctor.ImgFile.Length > 2097152)
                throw new FileSizeException("ImgFile", "File size error");

            string fileName = doctor.ImgFile.FileName;
            string path = @"C:\Users\II novbe\Desktop\Praktika-Codlar\praktika 05.04.24\MVCProjects_Medico\wwwroot\upload\doctor\" + fileName;
            using(FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                doctor.ImgFile.CopyTo(fileStream);
            }
            doctor.ImgUrl = doctor.ImgFile.FileName;

            _doctorRepository.Add(doctor);
            _doctorRepository.Commit();
        }

        public void DeleteDoctor(int id)
        {
            var existDoctor = _doctorRepository.Get(x => x.Id == id);
            if (existDoctor == null)
                throw new EntityNotFoundException("", "Entity not found");

            string path = @"C:\Users\II novbe\Desktop\Praktika-Codlar\praktika 05.04.24\MVCProjects_Medico\wwwroot\upload\doctor\" + existDoctor.ImgUrl;
            if (!File.Exists(path))
                throw new Exceptions.Doctor.FileNotFoundException("ImgFile", "File not found");

            File.Delete(path);

            _doctorRepository.Delete(existDoctor);
            _doctorRepository.Commit();
        }

        public List<Doctor> GetAllDoctors(Func<Doctor, bool>? func = null)
        {
            return _doctorRepository.GetAll(func);
        }

        public Doctor GetDoctor(Func<Doctor, bool>? func = null)
        {
            return _doctorRepository.Get(func);
        }

        public void UpdateDoctor(int id, Doctor doctor)
        {
            var existDoctor = _doctorRepository.Get(x => x.Id == id);
            if (existDoctor == null)
                throw new EntityNotFoundException("", "Entity not found");

            if(doctor.ImgFile != null)
            {
                if (!doctor.ImgFile.ContentType.Contains("image/"))
                    throw new FileContentTypeException("ImgFile", "File content type error");

                if (doctor.ImgFile.Length > 2097152)
                    throw new FileSizeException("ImgFile", "File size error");

                string fileName = doctor.ImgFile.FileName;
                string path = @"C:\Users\II novbe\Desktop\Praktika-Codlar\praktika 05.04.24\MVCProjects_Medico\wwwroot\upload\doctor\" + fileName;
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    doctor.ImgFile.CopyTo(fileStream);
                }
                doctor.ImgUrl = fileName;

                existDoctor.ImgUrl = doctor.ImgUrl;
            }

            existDoctor.FullName = doctor.FullName;
            existDoctor.Position = doctor.Position;

            _doctorRepository.Commit();
        }
    }
}
