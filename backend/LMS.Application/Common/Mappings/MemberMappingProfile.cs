using AutoMapper;

namespace LMS.Application.Common.Mappings;

public class MemberMappingProfile : Profile
{
    public MemberMappingProfile()
    {
        // TODO: Member -> MemberSummaryDto
        // TODO: Member -> MemberDetailDto (with nested BorrowRecordDto[] + ActiveBorrowCount)
    }
}
