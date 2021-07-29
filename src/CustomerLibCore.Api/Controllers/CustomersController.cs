//using System.Collections.Generic;
//using AutoMapper;
//using CustomerLibCore.Api.Dtos;
//using CustomerLibCore.Api.Dtos.Validators;
//using CustomerLibCore.Business.Entities;
//using CustomerLibCore.Business.Exceptions;
//using CustomerLibCore.Business.Validators;
//using CustomerLibCore.ServiceLayer.Services;
//using Microsoft.AspNetCore.Mvc;

//namespace CustomerLibCore.Api.Controllers
//{
//[Route("api/customers")]
//[ApiController]
//public class CustomersController : ControllerBase
//{
//	private readonly CustomerDtoValidator _customerDtoValidator = new();
//	private readonly CustomerBasicDetailsDtoValidator _customerBasicDetailsDtoValidator = new();

//	private readonly ICustomerService _customerService;
//	private readonly IMapper _mapper;

//	public CustomersController(ICustomerService customerService, IMapper mapper)
//	{
//		_customerService = customerService;
//		_mapper = mapper;
//	}

//	// GET: api/customers?includeAddresses=true&includeNotes=false
//	[HttpGet]
//	public ActionResult<IEnumerable<CustomerDto>> GetPage(
//		[FromQuery] int page, [FromQuery] int pageSize,
//		[FromQuery] bool includeAddresses, [FromQuery] bool includeNotes)
//	{
//		//var customers = _customerService.GetPage(page, pageSize includeAddresses, includeNotes);

//		//var customersDto = _mapper.Map<IEnumerable<CustomerDto>>(customers);

//		//return Ok(customersDto);
//		return Ok();
//	}

//	// GET api/customers/5?includeAddresses=true&includeNotes=false
//	[HttpGet("{customerId:int}")]
//	public ActionResult<CustomerDto> Get([FromRoute] int customerId,
//		[FromQuery] bool includeAddresses, [FromQuery] bool includeNotes)
//	{
//		CheckRouteArgument.ValidId(customerId, nameof(customerId));

//		var customer = _customerService.Get(customerId, includeAddresses, includeNotes);

//		var customerDto = _mapper.Map<CustomerDto>(customer);

//		return Ok(customerDto);
//	}

//	// POST api/customers/5
//	[HttpPost]
//	public IActionResult Save([FromBody] CustomerDto customerDto)
//	{
//		_customerDtoValidator.ValidateFull(customerDto).WithInvalidBodyException();

//		var customer = _mapper.Map<Customer>(customerDto);

//		try
//		{
//			_customerService.Save(customer);
//		}
//		catch (EmailTakenException)
//		{
//			throw ConflictWithExistingException.EmailTaken(nameof(CustomerDto.Email),
//				customerDto.Email);
//		}

//		return Ok();
//	}

//	// PUT api/customers/5
//	[HttpPut("{customerId:int}")]
//	public IActionResult Update([FromRoute] int customerId,
//		[FromBody] CustomerBasicDetailsDto customerBasicDetailsDto)
//	{
//		CheckRouteArgument.ValidId(customerId, nameof(customerId));

//		_customerBasicDetailsDtoValidator.Validate(customerBasicDetailsDto)
//			.WithInvalidBodyException();

//		var customer = _mapper.Map<Customer>(customerBasicDetailsDto);

//		customer.CustomerId = customerId;

//		try
//		{
//			_customerService.Update(customer);
//		}
//		catch (EmailTakenException)
//		{
//			throw ConflictWithExistingException.EmailTaken(
//				nameof(CustomerBasicDetailsDto.Email), customerBasicDetailsDto.Email);
//		}

//		return Ok();
//	}

//	// DELETE api/customers/5
//	[HttpDelete("{customerId:int}")]
//	public IActionResult Delete([FromRoute] int customerId)
//	{
//		CheckRouteArgument.ValidId(customerId, nameof(customerId));

//		_customerService.Delete(customerId);

//		return Ok();
//	}
//}
//}
