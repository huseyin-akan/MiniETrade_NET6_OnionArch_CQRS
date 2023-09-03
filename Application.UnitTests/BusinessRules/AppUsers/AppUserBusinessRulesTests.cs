using MiniETrade.Application.BusinessRules.AppUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.BusinessRules.AppUsers;

public class AppUserBusinessRulesTests
{
    [Theory]
    [InlineData("SomePascalCasePassword", "somepascalcasepassword", false)]
    [InlineData("", "          ", false)]
    [InlineData("AgoodPassword ", "AgoodPassword", false)]
    [InlineData("ThistimeI'mveryserious!!", "ThistimeI'mveryserious!!", true)]
    public void PasswordShouldMatchWhenCreatingUser(string password, string passwordRepeat, bool expectedResult)
    {
        //Arrange --> We get all variables we need for testing.
        //No-arrange was necessary here.

        //Act --> We execute the test scenerios here
        var testResult = AppUserBusinessRules.CheckIfPasswordMatches(password, passwordRepeat);

        //Assert --> We check our test results here.
        testResult.Should().Be(expectedResult);
    }
}
