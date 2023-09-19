using MiniETrade.Application.Features.AppUsers.Commands.CreateUser;
using MiniETrade.Domain.Messages;
using MiniETrade.Infrastructure.Helpers.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.Features.AppUsers.Commands;

public class CreateUserValidationTests
{
    [Fact]
    public void ShouldValidateEmailFieldCorrectly()
    {
        // Arrange
        var validationHelper = new FluentValidationTestHelper<CreateAppUserValidator, CreateUserCommand>();
        CreateUserCommand model = new();

        //Act & Arrange
        validationHelper.TestEmailFieldForSuccessCases(a => a.Email, model);
    }

    [Fact]
    public void ShouldNotValidateEmailFieldCorrectly()
    {
        // Arrange
        var validationHelper = new FluentValidationTestHelper<CreateAppUserValidator, CreateUserCommand>();
        CreateUserCommand model = new();

        //Act & Arrange
        validationHelper.TestEmailFieldForErrorCases(a => a.Email, model, Messages.InvalidEmailAddress);
    }
}