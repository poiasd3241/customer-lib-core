using System;
using AutoMapper;
using CustomerLibCore.Business.Entities;
using CustomerLibCore.Business.Enums;

namespace CustomerLibCore.Api.DTOs
{
	public class AutoMapperApiProfile : Profile
	{
		public AutoMapperApiProfile()
		{
			CreateMap<Note, NoteDto>().ReverseMap();

			CreateMap<Address, AddressDto>().ReverseMap()
				.ForMember(dest => dest.Type, options => options.MapFrom((src, dest) =>
				{
					if (Enum.IsDefined(typeof(AddressType), src.Type) == false)
					{
						return AddressType.Shipping;
					}

					return Enum.Parse(typeof(AddressType), src.Type);
				}));

			CreateMap<Customer, CustomerDto>().ReverseMap();
			CreateMap<Customer, CustomerBasicDetailsDto>().ReverseMap();
		}
	}
}
