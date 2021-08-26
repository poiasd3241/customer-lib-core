using AutoMapper;
using CustomerLibCore.Api.Dtos.Customers.Request;
using CustomerLibCore.Api.Dtos.Customers.Response;
using CustomerLibCore.Api.Dtos.Validators.Customers.Request;
using CustomerLibCore.Api.Dtos.Validators.Customers.Response;
using CustomerLibCore.Api.FluentValidation;
using CustomerLibCore.Domain.Exceptions;
using CustomerLibCore.Domain.FluentValidation;
using CustomerLibCore.Domain.Models;
using CustomerLibCore.ServiceLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerLibCore.Api.Controllers
{
	[Route("api/customers")]
	[ApiController]
	public class CustomersController : ControllerBase
	{
		private readonly CustomerCreateRequestValidator _createRequestValidator = new();
		private readonly CustomerUpdateRequestValidator _updateRequestValidator = new();
		private readonly CustomerPagedResponseValidator _pagedResponseValidator = new();
		private readonly CustomerResponseValidator _responseValidator = new();

		private readonly ICustomerService _customerService;
		private readonly IMapper _mapper;

		public CustomersController(ICustomerService customerService, IMapper mapper)
		{
			_customerService = customerService;
			_mapper = mapper;
		}

		// GET: api/customers?addresses=1&notes=0
		[HttpGet]
		public ActionResult<CustomerPagedResponse> GetPage(
			[FromQuery] int page, [FromQuery] int pageSize,
			[FromQuery] int addresses, [FromQuery] int notes)
		{
			var includeAddresses = CheckRouteArgument.Flag(addresses, nameof(addresses));
			var includeNotes = CheckRouteArgument.Flag(notes, nameof(notes));

			var customers = _customerService.GetPage(
				page, pageSize, includeAddresses, includeNotes);

			var response = _mapper.Map<CustomerPagedResponse>(customers);
			_pagedResponseValidator.Validate(response).WithInternalValidationException();

			return Ok(response);
		}

		// GET api/customers/5?addresses=1&notes=0
		[HttpGet("{customerId:int}")]
		public ActionResult<CustomerResponse> Get([FromRoute] int customerId,
			[FromQuery] int addresses, [FromQuery] int notes)
		{
			var includeAddresses = CheckRouteArgument.Flag(addresses, nameof(addresses));
			var includeNotes = CheckRouteArgument.Flag(notes, nameof(notes));

			var customer = _customerService.Get(customerId, includeAddresses, includeNotes);

			var response = _mapper.Map<CustomerResponse>(customer);
			_responseValidator.Validate(response).WithInternalValidationException();

			return Ok(response);
		}

		// POST api/customers/5
		[HttpPost]
		public IActionResult Save([FromBody] CustomerCreateRequest request)
		{
			_createRequestValidator.Validate(request).WithInvalidBodyException();

			var customer = _mapper.Map<Customer>(request);

			try
			{
				_customerService.Save(customer);
			}
			catch (EmailTakenException)
			{
				throw ConflictWithExistingException.EmailTaken(nameof(CustomerCreateRequest.Email),
					request.Email);
			}

			return Ok();
		}

		// PUT api/customers/5
		[HttpPut("{customerId:int}")]
		public IActionResult Update([FromRoute] int customerId,
			[FromBody] CustomerUpdateRequest request)
		{
			CheckRouteArgument.Id(customerId, nameof(customerId));

			_updateRequestValidator.Validate(request).WithInvalidBodyException();

			var customer = _mapper.Map<Customer>(request);
			customer.CustomerId = customerId;

			try
			{
				_customerService.Update(customer);
			}
			catch (EmailTakenException)
			{
				throw ConflictWithExistingException.EmailTaken(nameof(CustomerUpdateRequest.Email),
					request.Email);
			}

			return Ok();
		}

		// DELETE api/customers/5
		[HttpDelete("{customerId:int}")]
		public IActionResult Delete([FromRoute] int customerId)
		{
			CheckRouteArgument.Id(customerId, nameof(customerId));

			_customerService.Delete(customerId);

			return Ok();
		}
	}
}
