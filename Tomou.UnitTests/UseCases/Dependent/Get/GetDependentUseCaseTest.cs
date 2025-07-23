namespace Tomou.UnitTests.UseCases.Dependent.Get;

using AutoMapper;
using Moq;
using Shouldly;
using Tomou.Application.Services.Auth;
using Tomou.Application.UseCases.Dependent.GetAll;
using Tomou.Communication.Responses.Dependent.Get;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.User;

public class GetDependentUseCaseTest
{
    [Fact]
    public async Task ShouldReturnAllDependentsSuccessfullWhenUserIsCaregiver()
    {
        var mapperMock = new Mock<IMapper>();
        var dependentReadOnlyRepositoryMock = new Mock<IDependentReadOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();
        var userReadOnlyRepositoryMock = new Mock<IUserReadOnlyRepository>();

        var caregiverId = Guid.NewGuid();
        userContextMock.Setup(d => d.GetUserId()).Returns(caregiverId);
        userReadOnlyRepositoryMock
            .Setup(r => r.GetUserById(caregiverId))
            .ReturnsAsync(new Tomou.Domain.Entities.User
            {
                IsCaregiver = true,
                Id = caregiverId,
                Dependents = new List<Tomou.Domain.Entities.Dependent>()
            });
       
        var dependents = new List<Tomou.Domain.Entities.Dependent>()
        {
           new() {Id = Guid.NewGuid(),
                Name = "Leandro Almeida",
                CaregiverId = caregiverId
            },
            new() {
                Id = Guid.NewGuid(),
                Name = "Andrey Girotto",
                CaregiverId = caregiverId
            }
        };

        mapperMock
        .Setup(m => m.Map<List<ResponseDependentShortJson>>(It.IsAny<List<Tomou.Domain.Entities.Dependent>>()))
        .Returns(dependents
       .Select(d => new ResponseDependentShortJson
       {
           Id = d.Id,
           Name = d.Name
       }).ToList());


        dependentReadOnlyRepositoryMock
            .Setup(d => d.GetDependents(caregiverId, null, true))
            .ReturnsAsync(dependents);

        var useCase = new GetDependentsUseCase(
            dependentReadOnlyRepositoryMock.Object,
            mapperMock.Object,
            userContextMock.Object,
            userReadOnlyRepositoryMock.Object
        );

        var response = await useCase.Execute();

        response.Dependents.Count.ShouldBe(2);
        response.Dependents.ShouldContain(d => d.Name == "Leandro Almeida");
        response.Dependents.ShouldContain(d => d.Name == "Andrey Girotto");
    }
}
