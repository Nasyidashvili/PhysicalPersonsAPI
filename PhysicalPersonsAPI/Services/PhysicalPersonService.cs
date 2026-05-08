using PhysicalPersonsAPI.DTOS;
using PhysicalPersonsAPI.Interfaces;
using PhysicalPersonsAPI.Models;
using PhysicalPersonsAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using PhysicalPersonsAPI.Repositories.Interfaces;

namespace PhysicalPersonsAPI.Services
{
    public class PhysicalPersonService : IPhysicalPersonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public PhysicalPersonService(AppDbContext context,IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        public async Task<IEnumerable<PhysicalPersonResponseDto>> GetAllAsync()
        {
            var physicalPersons = await _unitOfWork.Persons.GetAllWithCityAsync();
            return physicalPersons
                .Select(p => new PhysicalPersonResponseDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    PersonalNumber = p.PersonalNumber,
                    Gender = p.Gender,
                    BirthDate = p.BirthDate,
                    CityName = p.City.CityName,
                    PhoneNumbers = p.PhoneNumbers?.Select(r => new PhoneNumberResponseDto
                    {
                        Number = r.Number,
                        PhoneType = r.NumberType
                    }).ToList() ?? new()
                });
        }

        public async Task<PhysicalPersonResponseDto?> GetByIdAsync(int id)
        {
            var physicalPerson = await _unitOfWork.Persons.GetByIdDetailsAsync(id);

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
                CityName = physicalPerson.City.CityName,
                PhoneNumbers = physicalPerson.PhoneNumbers?.Select(p => new PhoneNumberResponseDto
                {
                    Number = p.Number,
                    PhoneType = p.NumberType
                }).ToList() ?? new()
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

            await _unitOfWork.Persons.AddAsync(physicalPerson);
            await _unitOfWork.SaveAsync();

            foreach (var phoneDto in createPersonDto.PhoneNumbers)
                _context.PhoneNumbers.Add(new PhoneNumber
                {
                    Number = phoneDto.Number,
                    NumberType = phoneDto.PhoneType,
                    PhysicalPersonId = physicalPerson.Id
                });

            await _unitOfWork.SaveAsync();

            return await GetByIdAsync(physicalPerson.Id);
        }

        public async Task<PhysicalPersonResponseDto?> UpdateAsync(int id, UpdatePhysicalPersonDto updatePersonDto)
        {
            var physicalPersons = await _unitOfWork.Persons.GetByIdAsync(id);

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

            var existingPhones = _context.PhoneNumbers.Where(p => p.PhysicalPersonId == id);
            _context.PhoneNumbers.RemoveRange(existingPhones);

            foreach(var phone in updatePersonDto.PhoneNumbers)
            {
                _context.PhoneNumbers.Add(new PhoneNumber
                {
                    Number = phone.Number,
                    NumberType = phone.PhoneType,
                    PhysicalPersonId = id
                });
            }

            await _unitOfWork.Persons.UpdateAsync(physicalPersons);
            await _unitOfWork.SaveAsync();

            return await GetByIdAsync(physicalPersons.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var physicalPersons = await _unitOfWork.Persons.GetByIdAsync(id);

            if ( physicalPersons == null)
            {
                return false;
            }

            await _unitOfWork.Persons.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

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

        public async Task<RelatedPersonResponseDto?> AddRelatedPerson(int personId, int relatedId, RelationType type)
        {
            var person = await _context.PhysicalPersons.FindAsync(personId);
            var relatedPerson = await _context.PhysicalPersons.FindAsync(relatedId);
            if (person == null || relatedPerson == null) {
                throw new Exception("Person not found");
            }
            var relation = new RelatedPerson
            {
                PhysicalPersonId = personId,
                RelativePersonId = relatedId,
                Related = type
            };

            _context.RelatedPersons.Add(relation);
            await _context.SaveChangesAsync();

            return new RelatedPersonResponseDto
            {
                Id = relation.Id,
                FirstName = relatedPerson.FirstName,
                LastName = relatedPerson.LastName,
                BirthDate = relatedPerson.BirthDate,
                Type = type
            };
        }
        public async Task<bool> RemoveRelatedPerson(int personId, int relatedId)
        {
            var relation = await _context.RelatedPersons.FirstOrDefaultAsync(r => r.PhysicalPersonId == personId && r.RelativePersonId == relatedId);
            if (relation == null)
            {
                return false;
            }
            
            _context.RelatedPersons.Remove(relation);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<RelatedPersonResponseDto>> GetRelatedPerson(int personId)
        {
            var relations = await _context.RelatedPersons
                .Include(p => p.RelativePerson)
                .Where(p => p.PhysicalPersonId == personId)
                .Select(p => new RelatedPersonResponseDto
                {
                    Id = p.Id,
                    FirstName = p.RelativePerson.FirstName,
                    LastName = p.RelativePerson.LastName,
                    BirthDate = p.RelativePerson.BirthDate,
                    Type = p.Related
                })
                .ToListAsync();
            return relations;
        }
    }
}