using PhysicalPersonsAPI.DTOS;
using PhysicalPersonsAPI.Interfaces;
using PhysicalPersonsAPI.Models;
using PhysicalPersonsAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace PhysicalPersonsAPI.Services
{
    public class PhysicalPersonService : IPhysicalPersonService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public PhysicalPersonService(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IEnumerable<PhysicalPersonResponseDto>> GetAllAsync()
        {
            var physicalPersons = await _context.PhysicalPersons
                .Include(p => p.City)
                .Select(p => new PhysicalPersonResponseDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    PersonalNumber = p.PersonalNumber,
                    Gender = p.Gender,
                    BirthDate = p.BirthDate,
                    CityName = p.City.CityName
                })
                .ToListAsync();

            return physicalPersons;
        }

        public async Task<PhysicalPersonResponseDto?> GetByIdAsync(int id)
        {
            var physicalPerson = await _context.PhysicalPersons
                .Include(p => p.City)
                .FirstOrDefaultAsync();

            if (physicalPerson == null)
            {
                return null;
            }

            var responseDto = new PhysicalPersonResponseDto
            {
                Id = physicalPerson.Id,
                FirstName = physicalPerson.FirstName,
                LastName = physicalPerson.LastName,
                PersonalNumber = physicalPerson.PersonalNumber,
                Gender = physicalPerson.Gender,
                BirthDate = physicalPerson.BirthDate,
                CityName = physicalPerson.City.CityName
            };
            return responseDto;
        }

        public async Task<PhysicalPersonResponseDto?> CreateAsync(CreatePhysicalPersonDto createPersonDto)
        {
            var physicalPerson = new PhysicalPerson
            {
                FirstName = createPersonDto.FirstName,
                LastName = createPersonDto.LastName,
                PersonalNumber = createPersonDto.PersonalNumber,
                Gender = createPersonDto.Gender,
                BirthDate = createPersonDto.BirthDate,
                CityId = createPersonDto.CityId
            };

            _context.PhysicalPersons.Add(physicalPerson);
            await _context.SaveChangesAsync();
            
            return await GetByIdAsync(physicalPerson.Id);
        }

        public async Task<PhysicalPersonResponseDto?> UpdateAsync(int id, UpdatePhysicalPersonDto updatePersonDto)
        {
            var physicalPersons = await _context.PhysicalPersons.FindAsync(id);
            
            if (physicalPersons == null)
            {
                return null;
            }

            physicalPersons.FirstName = updatePersonDto.FirstName;
            physicalPersons.LastName = updatePersonDto.LastName;
            physicalPersons.PersonalNumber = updatePersonDto.PersonalNumber;
            physicalPersons.Gender = updatePersonDto.Gender;
            physicalPersons.BirthDate = updatePersonDto.BirthDate;
            physicalPersons.CityId = updatePersonDto.CityId;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(physicalPersons.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var physicalPersons = await _context.PhysicalPersons.FindAsync(id);

            if ( physicalPersons == null)
            {
                return false;
            }

            _context.PhysicalPersons.Remove(physicalPersons);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<ResultDto<PhysicalPersonResponseDto>> SearchAsync(SearchDto searchDto)
        {
            IQueryable<PhysicalPerson> query = _context.PhysicalPersons.Include(p => p.City);
            if (!string.IsNullOrEmpty(searchDto.FirstName))
            {
                query = query.Where(p => p.FirstName.ToLower().Contains(searchDto.FirstName.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchDto.LastName))
            {
                query = query.Where(p => p.LastName.ToLower().Contains(searchDto.LastName.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchDto.PersonalNumber)) 
            {
                query = query.Where(p => p.PersonalNumber.Contains(searchDto.PersonalNumber));
            }
            if (searchDto.Gender != null)
            {
                query = query.Where(p => p.Gender == searchDto.Gender);
            }
            if (searchDto.CityId != null)
            {
                query = query.Where(p => p.CityId == searchDto.CityId);
            }

            int totalCount = await query.CountAsync();

            query = query
                .Skip((searchDto.PageNumber - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize);

            var items = await query.ToListAsync();

            return new ResultDto<PhysicalPersonResponseDto>
            {
                Items = items.Select(p => new PhysicalPersonResponseDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    PersonalNumber = p.PersonalNumber,
                    Gender = p.Gender,
                    BirthDate = p.BirthDate,
                    CityName = p.City.CityName
                }),
                TotalCount = totalCount,
                PageNumber = searchDto.PageNumber,
                PageSize = searchDto.PageSize
            };
        }
        public async Task<string> UploadImageAsync(int id, IFormFile image)
        {
            var person = _context.PhysicalPersons.Find(id);
            if(person == null)
            {
                throw new Exception("Person not found");
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(_environment.WebRootPath, "images", fileName);

            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            person.ImagePath = $"/images/{fileName}";

            await _context.SaveChangesAsync();

            return person.ImagePath;
        }
    }
}