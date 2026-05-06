using PhysicalPersonsAPI.DTOS;
using PhysicalPersonsAPI.Models;

namespace PhysicalPersonsAPI.Interfaces
{
    public interface IPhysicalPersonService
    {
        public Task<IEnumerable<PhysicalPersonResponseDto>> GetAllAsync();
        public Task<PhysicalPersonResponseDto?> GetByIdAsync(int id);
        public Task<PhysicalPersonResponseDto?> CreateAsync(CreatePhysicalPersonDto createPersonDto);
        public Task<PhysicalPersonResponseDto?> UpdateAsync(int id, UpdatePhysicalPersonDto updatePersonDto);
        public Task<bool> DeleteAsync(int id);
        public Task<ResultDto<PhysicalPersonResponseDto>> SearchAsync(SearchDto searchDto);
        public Task<string> UploadImageAsync(int id, IFormFile image);
        public Task<RelatedPersonResponseDto?> AddRelatedPerson(int personId, int relatedId, RelationType type);
        public Task<bool> RemoveRelatedPerson(int personId, int relatedId);
        public Task<IEnumerable<RelatedPersonResponseDto>> GetRelatedPerson(int personId);
    }
}
