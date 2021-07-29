﻿using CustomerLibCore.Api.Dtos.Notes;
using FluentValidation;

namespace CustomerLibCore.Api.Dtos.Validators.Notes
{
	/// <summary>
	/// The fluent validator for <see cref="NoteRequest"/> objects.
	/// </summary>
	public class NoteRequestValidator : AbstractValidator<NoteRequest>
	{
		public NoteRequestValidator()
		{
			Include(new NoteDetailsValidator());
		}
	}
}
