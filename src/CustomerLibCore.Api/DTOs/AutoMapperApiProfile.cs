using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Castle.Core.Resource;
using CustomerLibCore.Api.Controllers;
using CustomerLibCore.Api.Dtos.Addresses;
using CustomerLibCore.Api.Dtos.Customers;
using CustomerLibCore.Api.Dtos.Notes;
using CustomerLibCore.Business.Entities;
using CustomerLibCore.Business.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CustomerLibCore.Api.Dtos
{
	public class AutoMapperApiProfile : Profile
	{
		public AutoMapperApiProfile()
		{
			#region Address

			// Address -> AddressResponse
			CreateMap<Address, AddressResponse>()
			.ForSourceMember(src => src.CustomerId, o => o.DoNotValidate())
			.ForSourceMember(src => src.AddressId, o => o.DoNotValidate())
			.ForMember(dest => dest.Self, o => o.MapFrom((src, dest) =>
			{
				return LinkHelper.Address(src.CustomerId, src.AddressId);
			}));

			// IEnumerable<Address> -> AddressListResponse
			CreateMap<IEnumerable<Address>, AddressListResponse>()
			.ForMember(dest => dest.Self, o => o.Ignore())
			.ForMember(dest => dest.Items, o => o.MapFrom((src, dest, destMember, context) =>
			{
				return context.Mapper.Map<IEnumerable<AddressResponse>>(src);
			}));

			// AddressRequest -> Address
			CreateMap<AddressRequest, Address>()
			.ForMember(dest => dest.CustomerId, o => o.Ignore())
			.ForMember(dest => dest.AddressId, o => o.Ignore())
			.ForMember(dest => dest.Type, o => o.MapFrom((src, dest) =>
			{
				if (Enum.IsDefined(typeof(AddressType), src.Type) == false)
				{
					throw new Exception(
						$"the {nameof(src.Type)} must be " +
						$"a name of a defined {nameof(AddressType)} enum");
				}

				return Enum.Parse(typeof(AddressType), src.Type);
			}));

			#endregion

			#region Note

			// Note -> NoteResponse
			CreateMap<Note, NoteResponse>()
			.ForSourceMember(src => src.CustomerId, o => o.DoNotValidate())
			.ForSourceMember(src => src.NoteId, o => o.DoNotValidate())
			.ForMember(dest => dest.Self, o => o.MapFrom((src, dest) =>
			{
				return LinkHelper.Note(src.CustomerId, src.NoteId);
			}));

			// IEnumerable<Note> -> NoteListResponse
			CreateMap<IEnumerable<Note>, NoteListResponse>()
			.ForMember(dest => dest.Self, o => o.Ignore())
			.ForMember(dest => dest.Items, o => o.MapFrom((src, dest, destMember, context) =>
			{
				return context.Mapper.Map<IEnumerable<NoteResponse>>(src);
			}));

			//NoteRequest -> Note
			CreateMap<NoteRequest, Note>()
			.ForMember(dest => dest.CustomerId, o => o.Ignore())
			.ForMember(dest => dest.NoteId, o => o.Ignore());

			#endregion

			#region Customer

			// Customer -> CustomerResponse
			CreateMap<Customer, CustomerResponse>()
			.ForSourceMember(src => src.CustomerId, o => o.DoNotValidate())
			.ForMember(dest => dest.Self, o => o.MapFrom((src, dest) =>
			{
				return LinkHelper.Customer(src.CustomerId);
			}))
			.ForMember(dest => dest.Addresses, o => o.MapFrom((src, dest, destMember, context) =>
			{
				PreventNull(src.Addresses, nameof(src.Addresses));

				destMember = context.Mapper.Map<AddressListResponse>(src.Addresses);
				destMember.Self = LinkHelper.Addresses(src.CustomerId);

				return destMember;
			}))
			.ForMember(dest => dest.Notes, o => o.MapFrom((src, dest, destMember, context) =>
			{
				PreventNull(src.Notes, nameof(src.Notes));

				destMember = context.Mapper.Map<NoteListResponse>(src.Notes);
				destMember.Self = LinkHelper.Notes(src.CustomerId);

				return destMember;
			}));

			// PagedResult<Customer> -> CustomerPagedResponse
			CreateMap<PagedResult<Customer>, CustomerPagedResponse>()
			.ForMember(dest => dest.Self, o => o.MapFrom((src, dest) =>
			{
				return LinkHelper.CustomersPage(src.Page, src.PageSize);
			}))
			.ForMember(dest => dest.Previous, o => o.MapFrom((src, dest) =>
			{
				return src.Page > 1
					? LinkHelper.CustomersPage(src.Page - 1, src.PageSize)
					: null;
			}))
			.ForMember(dest => dest.Next, o => o.MapFrom((src, dest) =>
			{
				return src.Page < src.LastPage
					? LinkHelper.CustomersPage(src.Page + 1, src.PageSize)
					: null;
			}))
			.ForMember(dest => dest.Items, o => o.MapFrom((src, dest, destMember, context) =>
			{
				return context.Mapper.Map<IEnumerable<CustomerResponse>>(src.Items);
			}));

			// CustomerCreateRequest -> Customer
			CreateMap<CustomerCreateRequest, Customer>()
			.ForMember(dest => dest.CustomerId, o => o.Ignore())
			.ForMember(dest => dest.TotalPurchasesAmount, o => o.MapFrom((src, dest) =>
				ConvertToNullableDecimal(
					src.TotalPurchasesAmount, nameof(src.TotalPurchasesAmount))
			));

			// CustomerUpdateRequest -> Customer
			CreateMap<CustomerUpdateRequest, Customer>()
			.ForMember(dest => dest.CustomerId, o => o.Ignore())
			.ForMember(dest => dest.Addresses, o => o.Ignore())
			.ForMember(dest => dest.Notes, o => o.Ignore())
			.ForMember(dest => dest.TotalPurchasesAmount, o => o.MapFrom((src, dest) =>
				ConvertToNullableDecimal(
					src.TotalPurchasesAmount, nameof(src.TotalPurchasesAmount))
			));

			#endregion
		}

		private static void PreventNull(object obj, string propertyName)
		{
			if (obj is null)
			{
				throw new Exception($"the {propertyName} cannot be null");
			}
		}

		private static decimal? ConvertToNullableDecimal(string input, string propertyName)
		{
			if (input is null)
			{
				return null;
			}

			if (input.Length != input.Trim().Length ||
				decimal.TryParse(
					input, out decimal decimalValue) == false)
			{
				throw new Exception(
					$"the {propertyName} cannot contain whitespace and must be " +
					$"either null or a decimal point number");
			}

			return decimalValue;
		}
	}
}
