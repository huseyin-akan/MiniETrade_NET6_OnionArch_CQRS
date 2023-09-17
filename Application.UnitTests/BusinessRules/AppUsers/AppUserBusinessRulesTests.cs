using MiniETrade.Application.BusinessRules.AppUsers;
using MiniETrade.Application.Common.Abstractions.Localization;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.BusinessRules.AppUsers;

public class AppUserBusinessRulesTests
{
    private readonly AppUserBusinessRules _appUserBusinessRules;
    private readonly Mock<ILanguageService> _languageService;

    public AppUserBusinessRulesTests()
    {
        _languageService = new();
        _appUserBusinessRules = new(It.IsAny<ILanguageService>() );
    }

    [Theory]
    [InlineData("SomePascalCasePassword", "somepascalcasepassword", false)]
    [InlineData("", "          ", false)]
    [InlineData("AgoodPassword ", "AgoodPassword", false)]
    [InlineData("ThistimeI'mveryserious!!", "ThistimeI'mveryserious!!", true)]
    public void PasswordShouldMatchWhenCreatingUser(string password, string passwordRepeat, bool expectedResult)
    {
        
        //Act --> We execute the test scenerios here
        var testResult = _appUserBusinessRules.CheckIfPasswordMatches(password, passwordRepeat);

        //Assert --> We check our test results here.
        testResult.Should().Be(expectedResult);
    }
}
