using PhysicalPersonsAPI.DTOS;

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
    }
}
