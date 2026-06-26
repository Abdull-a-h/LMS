using AutoMapper;

namespace LMS.Application.Common.Mappings;

public class BorrowMappingProfile : Profile
{
    public BorrowMappingProfile()
    {
        // TODO: BorrowRecord -> BorrowRecordDto (flatten BookTitle, compute IsOverdue)
    }
}
