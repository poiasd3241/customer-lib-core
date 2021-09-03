using System;
using System.Collections.Generic;
using System.Linq;
using CustomerLibCore.Domain.FluentValidation;
using CustomerLibCore.Domain.Models;
using CustomerLibCore.Domain.Models.Validators;
using Xunit;

namespace CustomerLibCore.TestHelpers.ModelsAssert
{
	public class AssertDomainModels
	{
		private static readonly AddressValidator _addressValidator = new();
		private static readonly NoteValidator _noteValidator = new();
		private static readonly CustomerValidator _customerValidator = new();

		#region Address

		public static void Meaningful(Address obj)
		{
			Assert.NotEqual(default, obj.Line);
			Assert.NotEqual(default, obj.Line2);
			Assert.NotEqual(default, obj.Type);
			Assert.NotEqual(default, obj.City);
			Assert.NotEqual(default, obj.PostalCode);
			Assert.NotEqual(default, obj.State);
			Assert.NotEqual(default, obj.Country);

			// string
			AssertX.Unique(new[]
			{
				obj.Line,
				obj.Line2,
				obj.City,
				obj.PostalCode,
				obj.State,
				obj.Country
			});
		}

		public static void MeaningfulValid(Address obj)
		{
			Meaningful(obj);

			_addressValidator.Validate(obj).WithInternalValidationException();
		}

		public static void MeaningfulValidWithIds(Address obj, IEnumerable<int> noClash = null)
		{
			MeaningfulValid(obj);

			Assert.NotEqual(default, obj.AddressId);
			Assert.NotEqual(default, obj.CustomerId);

			AssertX.Unique(Merge(new[]{
				obj.AddressId,
				obj.CustomerId
			}, noClash));
		}

		#endregion

		#region Note

		public static void Meaningful(Note obj)
		{
			Assert.NotEqual(default, obj.Content);
		}

		public static void MeaningfulValid(Note obj)
		{
			Meaningful(obj);

			_noteValidator.Validate(obj).WithInternalValidationException();
		}

		public static void MeaningfulValidWithIds(Note obj, IEnumerable<int> noClash = null)
		{
			MeaningfulValid(obj);

			Assert.NotEqual(default, obj.NoteId);
			Assert.NotEqual(default, obj.CustomerId);

			AssertX.Unique(Merge(new[]{
				obj.NoteId,
				obj.CustomerId
			}, noClash));
		}

		#endregion

		#region Customer

		public static void Meaningful(Customer obj)
		{
			Assert.NotEqual(default, obj.FirstName);
			Assert.NotEqual(default, obj.LastName);
			Assert.NotEqual(default, obj.PhoneNumber);
			Assert.NotEqual(default, obj.Email);
			Assert.NotEqual(default, obj.TotalPurchasesAmount);
			Assert.NotEqual(default, obj.Addresses);
			Assert.NotEqual(default, obj.Notes);

			// string
			AssertX.Unique(new[]
			{
				obj.FirstName,
				obj.LastName,
				obj.PhoneNumber,
				obj.Email,
			});

			foreach (var address in obj.Addresses)
			{
				Meaningful(address);
			}

			foreach (var note in obj.Notes)
			{
				Meaningful(note);
			}
		}

		public static void MeaningfulValid(Customer obj)
		{
			Meaningful(obj);

			_customerValidator.Validate(obj).WithInternalValidationException();
		}

		public static void MeaningfulValidWithIds(Customer obj, IEnumerable<int> noClash = null)
		{
			MeaningfulValid(obj);

			Assert.NotEqual(default, obj.CustomerId);

			var unique = Merge(new[] { obj.CustomerId }, noClash);

			foreach (var address in obj.Addresses)
			{
				MeaningfulValidWithIds(address, unique);

				Assert.Equal(obj.CustomerId, address.CustomerId);
			}

			foreach (var note in obj.Notes)
			{
				MeaningfulValidWithIds(note, unique);

				Assert.Equal(obj.CustomerId, note.CustomerId);
			}
		}

		#endregion

		#region PagedResult<Customer>

		public static void Meaningful(PagedResult<Customer> obj)
		{
			Assert.NotEqual(default, obj.Page);
			Assert.NotEqual(default, obj.PageSize);
			Assert.NotEqual(default, obj.LastPage);

			Assert.NotEqual(default, obj.Items);

			// int
			AssertX.Unique(new[]
			{
				obj.Page,
				obj.PageSize,
				obj.LastPage
			});

			foreach (var item in obj.Items)
			{
				Meaningful(item);
			}
		}

		public static void MeaningfulValid(PagedResult<Customer> obj)
		{
			Meaningful(obj);

			foreach (var customer in obj.Items)
			{
				_customerValidator.Validate(customer).WithInternalValidationException();
			}
		}

		public static void MeaningfulValidWithIds(PagedResult<Customer> obj)
		{
			MeaningfulValid(obj);

			var unique = new[]
			{
				obj.Page,
				obj.PageSize,
				obj.LastPage
			};

			foreach (var customer in obj.Items)
			{
				MeaningfulValidWithIds(customer, unique);
			}
		}

		#endregion

		#region Private Methods

		private static IEnumerable<T> Merge<T>(IEnumerable<T> main, IEnumerable<T> extra = null)
		{
			if (extra is null)
			{
				return main;
			}

			var merged = new List<T>(main.Count() + extra.Count());
			merged.AddRange(main);
			merged.AddRange(extra);

			return merged;
		}

		#endregion
	}
}
