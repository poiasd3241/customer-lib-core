using System.Collections.Generic;
using AutoMapper;
using CustomerLibCore.Api.Dtos;
using CustomerLibCore.Api.Dtos.Addresses.Request;
using CustomerLibCore.Api.Dtos.Addresses.Response;
using CustomerLibCore.Api.Dtos.Validators;
using CustomerLibCore.Api.Dtos.Validators.Addresses.Request;
using CustomerLibCore.Api.Dtos.Validators.Addresses.Response;
using CustomerLibCore.Api.FluentValidation;
using CustomerLibCore.Domain.FluentValidation;
using CustomerLibCore.Domain.Models;
using CustomerLibCore.Domain.Validators;
using CustomerLibCore.ServiceLayer.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CustomerLibCore.Api.Controllers
{
	[Route("api/customers/{customerId:int}/addresses")]
	[ApiController]
	public class AddressesController : ControllerBase
	{
		private readonly AddressRequestValidator _requestValidator = new();
		private readonly AddressResponseValidator _responseValidator = new();
		private readonly AddressListResponseValidator _listResponseValidator = new();

		private readonly IAddressService _addressService;
		private readonly IMapper _mapper;

		public AddressesController(IAddressService addressService, IMapper mapper)
		{
			_addressService = addressService;
			_mapper = mapper;
		}

		// GET: api/customers/5/addresses
		[HttpGet]
		public ActionResult<AddressListResponse> FindAllForCustomer([FromRoute] int customerId)
		{
			CheckRouteArgument.Id(customerId, nameof(customerId));

			var addresses = _addressService.FindAllForCustomer(customerId);

			var response = _mapper.Map<AddressListResponse>(addresses);
			_listResponseValidator.Validate(response).WithInternalValidationException();

			return Ok(response);
		}

		// GET api/customers/5/addresses/7
		[HttpGet("{addressId:int}")]
		public ActionResult<AddressResponse> GetForCustomer(
			[FromRoute] int customerId, [FromRoute] int addressId)
		{
			CheckRouteArgument.Id(customerId, nameof(customerId));
			CheckRouteArgument.Id(addressId, nameof(addressId));

			var address = _addressService.GetForCustomer(addressId, customerId);

			var response = _mapper.Map<AddressResponse>(address);
			_responseValidator.Validate(response).WithInternalValidationException();

			return Ok(response);
		}

		// POST api/customers/5/addresses
		[HttpPost]
		public IActionResult Save(int customerId, [FromBody] AddressRequest request)
		{
			CheckRouteArgument.Id(customerId, nameof(customerId));

			_requestValidator.Validate(request).WithInvalidBodyException();

			var address = _mapper.Map<Address>(request);
			address.CustomerId = customerId;

			_addressService.Save(address);

			return Ok();
		}

		// PUT api/customers/5/addresses/7
		[HttpPut("{addressId:int}")]
		public IActionResult Update([FromRoute] int customerId, [FromRoute] int addressId,
			[FromBody] AddressRequest request)
		{
			CheckRouteArgument.Id(customerId, nameof(customerId));
			CheckRouteArgument.Id(addressId, nameof(addressId));

			_requestValidator.Validate(request).WithInvalidBodyException();

			var address = _mapper.Map<Address>(request);
			address.CustomerId = customerId;
			address.AddressId = addressId;

			_addressService.UpdateForCustomer(address);

			return Ok();
		}

		// DELETE api/customers/5/addresses/7
		[HttpDelete("{addressId:int}")]
		public IActionResult Delete([FromRoute] int customerId, [FromRoute] int addressId)
		{
			CheckRouteArgument.Id(customerId, nameof(customerId));
			CheckRouteArgument.Id(addressId, nameof(addressId));

			_addressService.DeleteForCustomer(addressId, customerId);

			return Ok();
		}
	}
}
