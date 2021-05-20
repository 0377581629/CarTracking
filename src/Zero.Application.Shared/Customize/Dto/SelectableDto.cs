using Abp.Application.Services.Dto;

namespace Zero.Dto
{
    public class SelectableDto<T>
    {
        public T ObjectDto { get; set; }
        public bool Selected { get; set; }
    }
}