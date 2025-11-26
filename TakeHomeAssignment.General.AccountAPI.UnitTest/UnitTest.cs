using TakeHomeAssignment.General.AccountAPI.Model.General;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System.Dynamic;
using Xunit.Abstractions;
using TakeHomeAssignment.General.AccountAPI.Utils;
using TakeHomeAssignment.General.AccountAPI.Model;
using TakeHomeAssignment.General.AccountAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using static TakeHomeAssignment.General.AccountAPI.Model.General.Responses;
using System.Security.Claims;
using TakeHomeAssignment.General.AccountAPI.Controllers;
using TakeHomeAssignment.General.AccountAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace TakeHomeAssignment.General.AccountAPI.UnitTest
{
    public class UnitTest
    {
        public class AccountTest : BaseControllerTest
        {
            private Mock<IOptions<ApplicationSetting>> applicationSetting = new Mock<IOptions<ApplicationSetting>>();

            public AccountTest(ITestOutputHelper output) : base(output)
            {
                var appSetting = new ApplicationSetting { JWTValidHours = "24", JWTValidScale = "Hours" };
                applicationSetting.Setup(x => x.Value).Returns(appSetting);

                ResetStores();
            }

            private void ResetStores()
            {
                InMemoryUserStore.Users.Clear();
                SessionStore.UserActivity.Clear();
            }

            #region Register
            [Fact]
            public void Register_ShouldFail_WhenPasswordNotMatch()
            {
                var repo = new AuthRepository(applicationSetting.Object);
                var controller = new AuthController(repo);
                var result = controller.Register(new RegisterUserParam
                                                    {
                                                        Email = "a@a.com",
                                                        Password = "123",
                                                        ConfirmPassword = "456"
                                                    });

                dynamic Status = ((RegisterResponse)result.Value).Status;
                dynamic Message = ((RegisterResponse)result.Value).Message;
                Assert.False(Status);
                Assert.Equal(Messages.PasswordConfirmationNotMatch, Message);
            }

            [Fact]
            public void Register_ShouldFail_WhenEmailAlreadyExists()
            {
                InMemoryUserStore.Users["a@a.com"] = new User();

                var repo = new AuthRepository(applicationSetting.Object);
                var controller = new AuthController(repo);
                var result = controller.Register(new RegisterUserParam
                {
                    Email = "a@a.com",
                    Password = "123",
                    ConfirmPassword = "123"
                });

                dynamic Status = ((RegisterResponse)result.Value).Status;
                dynamic Message = ((RegisterResponse)result.Value).Message;

                Assert.False(Status);
                Assert.Equal(Messages.EmailAlreadyRegistered, Message);
            }

            [Fact]
            public void Register_ShouldSuccess()
            {
                var repo = new AuthRepository(applicationSetting.Object);
                var controller = new AuthController(repo);
                var result = controller.Register(new RegisterUserParam
                {
                    Email = "a@a.com",
                    Password = "123",
                    ConfirmPassword = "123"
                });

                dynamic Status = ((RegisterResponse)result.Value).Status;
                dynamic Message = ((RegisterResponse)result.Value).Message;

                Assert.True(Status);
                Assert.Equal(Messages.UserRegisterSuccess, Message);
                Assert.True(InMemoryUserStore.Users.ContainsKey("a@a.com"));
            }
            #endregion

            #region Login
            [Fact]
            public void Login_ShouldFail_WhenEmailNotFound()
            {
                var repo = new AuthRepository(applicationSetting.Object);
                var controller = new AuthController(repo);

                var result = controller.Login(new LoginUserParam
                {
                    Email = "x@x.com",
                    Password = "123"
                });

                dynamic Status = ((BasicResponse)result.Value).Status;
                dynamic Message = ((BasicResponse)result.Value).Message;

                Assert.False(Status);
                Assert.Equal(Messages.PasswordConfirmationNotMatch, Message);
            }

            [Fact]
            public void Login_ShouldFail_WhenPasswordIncorrect()
            {
                InMemoryUserStore.Users["a@a.com"] = new User
                {
                    Email = "a@a.com",
                    PasswordHash = PasswordHasher.HashPassword("CORRECT")
                };

                var repo = new AuthRepository(applicationSetting.Object);
                var controller = new AuthController(repo);

                var result = controller.Login(new LoginUserParam
                {
                    Email = "a@a.com",
                    Password = "WRONG"
                });

                dynamic Status = ((BasicResponse)result.Value).Status;
                dynamic Message = ((BasicResponse)result.Value).Message;

                Assert.False(Status);
                Assert.Equal(Messages.PasswordConfirmationNotMatch, Message);
            }

            [Fact]
            public void Login_ShouldSuccess()
            {
                InMemoryUserStore.Users["a@a.com"] = new User
                {
                    Email = "a@a.com",
                    PasswordHash = PasswordHasher.HashPassword("123")
                };

                var repo = new AuthRepository(applicationSetting.Object);
                var controller = new AuthController(repo);

                var result = controller.Login(new LoginUserParam
                {
                    Email = "a@a.com",
                    Password = "123"
                });

                dynamic Status = ((BasicResponse)result.Value).Status;
                dynamic Message = ((BasicResponse)result.Value).Message;

                Assert.True(Status);
                Assert.Equal(Messages.LoginSuccess, Message);

                Assert.True(SessionStore.UserActivity.ContainsKey("a@a.com"));
            }

            #endregion

            #region Me
            [Fact]
            public void Me_ShouldReturnHelloMessage()
            {
                var context = new DefaultHttpContext();

                context.User = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, "john@doe.com")
                    }, "mockAuth")
                );

                var controller = new AuthController(null);
                controller.ControllerContext = new ControllerContext()
                {
                    HttpContext = context
                };

                var result = controller.Me() as JsonResult;
                dynamic data = result.Value;

                Assert.True(data.Status);
                Assert.Contains("john@doe.com", data.Message);
            }
            #endregion
        }
    }
}