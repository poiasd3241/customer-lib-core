using System.Collections.Generic;
using AutoMapper;
using CustomerLibCore.Api.DTOs;
using CustomerLibCore.Api.DTOs.Validators;
using CustomerLibCore.Business.Entities;
using CustomerLibCore.ServiceLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerLibCore.Api.Controllers
{
	[Route("api/customers/{customerId:int}/addresses")]
	[ApiController]
	public class AddressesController : ControllerBase
	{
		private readonly AddressDtoValidator _addressDtoValidator = new();

		private readonly IAddressService _addressService;
		private readonly IMapper _mapper;

		public AddressesController(IAddressService addressService, IMapper mapper)
		{
			_addressService = addressService;
			_mapper = mapper;
		}

		// GET: api/customers/5/addresses
		[HttpGet]
		public ActionResult<IEnumerable<AddressDto>> FindAllForCustomer([FromRoute] int customerId)
		{
			CheckRouteArgument.ValidId(customerId, nameof(customerId));

			var addresses = _addressService.FindAllForCustomer(customerId);

			var addressesDto = _mapper.Map<IEnumerable<AddressDto>>(addresses);

			return Ok(addressesDto);
		}

		// GET api/customers/5/addresses/7
		[HttpGet("{addressId:int}")]
		public ActionResult<AddressDto> GetForCustomer(
			[FromRoute] int customerId, [FromRoute] int addressId)
		{
			CheckRouteArgument.ValidId(customerId, nameof(customerId));
			CheckRouteArgument.ValidId(addressId, nameof(addressId));

			var address = _addressService.GetForCustomer(addressId, customerId);

			var addressDto = _mapper.Map<AddressDto>(address);

			return Ok(addressDto);
		}

		// POST api/customers/5/addresses
		[HttpPost]
		public IActionResult Save(int customerId, [FromBody] AddressDto addressDto)
		{
			CheckRouteArgument.ValidId(customerId, nameof(customerId));

			_addressDtoValidator.Validate(addressDto).WithInvalidBodyException();

			var address = _mapper.Map<Address>(addressDto);

			address.CustomerId = customerId;

			_addressService.Save(address);

			return Ok();
		}

		// PUT api/customers/5/addresses/7
		[HttpPut("{addressId:int}")]
		public IActionResult Update([FromRoute] int customerId, [FromRoute] int addressId,
			[FromBody] AddressDto addressDto)
		{
			CheckRouteArgument.ValidId(customerId, nameof(customerId));
			CheckRouteArgument.ValidId(addressId, nameof(addressId));

			_addressDtoValidator.Validate(addressDto).WithInvalidBodyException();

			var address = _mapper.Map<Address>(addressDto);

			address.AddressId = addressId;
			address.CustomerId = customerId;

			_addressService.UpdateForCustomer(address);

			return Ok();
		}

		// DELETE api/customers/5/addresses/7
		[HttpDelete("{addressId:int}")]
		public IActionResult Delete([FromRoute] int customerId, [FromRoute] int addressId)
		{
			CheckRouteArgument.ValidId(customerId, nameof(customerId));
			CheckRouteArgument.ValidId(addressId, nameof(addressId));

			_addressService.DeleteForCustomer(addressId, customerId);

			return Ok();
		}
	}
}
