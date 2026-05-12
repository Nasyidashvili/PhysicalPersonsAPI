using Microsoft.EntityFrameworkCore;
using PhysicalPersonsAPI.Data;
using PhysicalPersonsAPI.DTOS;
using PhysicalPersonsAPI.Interfaces;
using PhysicalPersonsAPI.Models;
using PhysicalPersonsAPI.Repositories.Implementations;
using PhysicalPersonsAPI.Repositories.Interfaces;
using System.Runtime.InteropServices;

namespace PhysicalPersonsAPI.Services
{
    public class PhysicalPersonService : IPhysicalPersonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;
        public PhysicalPersonService(IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
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
               await _unitOfWork.PhoneNumbers.AddAsync(new PhoneNumber
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

            var existingPhones = await _unitOfWork.PhoneNumbers.GetIdAsync(physicalPersons.Id);
            _unitOfWork.PhoneNumbers.RemoveRange(existingPhones);

            foreach (var phone in updatePersonDto.PhoneNumbers)
            {
                await _unitOfWork.PhoneNumbers.AddAsync(new PhoneNumber
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
           var (items, totalCount) = await _unitOfWork.Persons.SearchAsync(searchDto);

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
                    CityName = p.City.CityName,
                    PhoneNumbers = p.PhoneNumbers?.Select(r => new PhoneNumberResponseDto
                    {
                        Number = r.Number,
                        PhoneType = r.NumberType
                    }).ToList() ?? new()
                }),
                TotalCount = totalCount,
                PageNumber = searchDto.PageNumber,
                PageSize = searchDto.PageSize
            };
        }
        public async Task<string> UploadImageAsync(int id, IFormFile image)
        {
            var person = await _unitOfWork.Persons.FindIdAsync(id);
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

            await _unitOfWork.SaveAsync();

            return person.ImagePath;
        }

        public async Task<RelatedPersonResponseDto?> AddRelatedPerson(int personId, int relatedId, RelationType type)
        {
            var person = await _unitOfWork.Persons.FindIdAsync(personId);
            var relatedPerson = await _unitOfWork.Persons.FindIdAsync(relatedId);
            if (person == null || relatedPerson == null) {
                throw new Exception("Person not found");
            }
            var relation = new RelatedPerson
            {
                PhysicalPersonId = personId,
                RelativePersonId = relatedId,
                Related = type
            };

            await _unitOfWork.RelatedPersons.AddAsync(relation);
            await _unitOfWork.SaveAsync();

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
            var relation = await _unitOfWork.RelatedPersons.GetBothIdAsync(personId, relatedId);
            if (relation == null)
            {
                return false;
            }

            _unitOfWork.RelatedPersons.Remove(relation);
            await _unitOfWork.SaveAsync();
            return true;
        }
        public async Task<IEnumerable<RelatedPersonResponseDto>> GetRelatedPerson(int personId)
        {
            var relations = await _unitOfWork.RelatedPersons.GetByPersonIdAsync(personId);

            return relations.Select(p => new RelatedPersonResponseDto
            {
                Id = p.Id,
                FirstName = p.RelativePerson.FirstName,
                LastName = p.RelativePerson.LastName,
                BirthDate = p.RelativePerson.BirthDate,
                Type = p.Related
            })
            ;
        }

        public async Task<ReportDto> GenerateReportAsync()
        {
            var totalPersons = await _unitOfWork.Persons.GetAllAsync();
            var totalCount = totalPersons.Count();
            var byGender = await _unitOfWork.Persons.CountByGennderAsync();
            return new ReportDto
            {
                TotalPersons = totalCount,
                ByGender = byGender,
                RelatedPersons = await _unitOfWork.RelatedPersons.CountAsync()
            };
        }
    }
}