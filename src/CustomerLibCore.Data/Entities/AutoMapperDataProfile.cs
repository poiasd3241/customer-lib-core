using AutoMapper;
using CustomerLibCore.Domain.Models;

namespace CustomerLibCore.Data.Entities
{
	public class AutoMapperDataProfile : Profile
	{
		public AutoMapperDataProfile()
		{
			// Address <-> AddressEntity
			CreateMap<Address, AddressEntity>()
				.ReverseMap();

			// Note <-> NoteEntity
			CreateMap<Note, NoteEntity>()
				.ReverseMap();

			// Customer <-> CustomerEntity
			CreateMap<Customer, CustomerEntity>()
				.ReverseMap();
		}
	}
}
