﻿using FluentValidation;
using MiniETrade.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.AppUsers.Commands.CreateUser
{
    public class CreateAppUserValidator : AbstractValidator<AppUser>
    {
        public CreateAppUserValidator()
        {
            //RuleFor(p => p.Name)
            //    .NotEmpty()
            //    .NotNull()
            //        .WithMessage("Lütfen ürün adını boş geçmeyiniz")
            //    .MaximumLength(150)
            //    .MinimumLength(5)
            //        .WithMessage("Lütfen ürün adını 5 ile 150 karakter arasında giriniz.");

            //RuleFor(p => p.Stock)
            //    .Must(s => s >= 0)
            //    .WithMessage("Stock bilgisi 0'dan küçük olamaz!");
        }
    }
}
