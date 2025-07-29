using AutoMapper;
using Moq;
using Shouldly;
using Tomou.Application.Services.Auth;
using Tomou.Application.UseCases.Dependent.Update;
using Tomou.Communication.Responses.Dependent.Update;
using Tomou.Domain.Repositories.Dependent;
using Tomou.Domain.Repositories.UnitOfWork;
using Tomou.Domain.Repositories.User;
using Tomou.TestUtils.Dependent.Update.Request;

namespace Tomou.UnitTests.UseCases.Dependent.Update;
public class UpdateDependentUseCaseTest
{
    [Fact]
    public async Task ShouldUpdateDependentSuccessfullWhenUserIsCaregiver()
    {
        var mapperMock = new Mock<IMapper>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userReadonlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        var dependentWriteOnlyRepositoryMock = new Mock<IDependentUpdateOnlyRepository>();
        var userContextMock = new Mock<IUserContext>();

        var userId = Guid.NewGuid();
        var dependentId = Guid.NewGuid();

        userContextMock.Setup(d => d.GetUserId()).Returns(userId);
        userReadonlyRepositoryMock.Setup(r => r.GetUserById(userId)).ReturnsAsync(new Tomou.Domain.Entities.User
        {
            IsCaregiver = true,
            Id = userId,
        });

        var request = RequestUpdateDependentJsonBuilder.Build();


        mapperMock
          .Setup(m => m.Map<ResponseUpdatedDependentJson>(It.IsAny<Tomou.Domain.Entities.Dependent>()))
          .Returns<Tomou.Domain.Entities.Dependent>(ent => new ResponseUpdatedDependentJson
          {
              Name = ent.Name,
              Message = ""
          });

        dependentWriteOnlyRepositoryMock.Setup(r => r.GetById(dependentId))
            .ReturnsAsync(new Tomou.Domain.Entities.Dependent
            {
                Id = dependentId,
                Name = request.Name,
                CaregiverId = userId
            });

        dependentWriteOnlyRepositoryMock
            .Setup(r => r.Update(It.IsAny<Tomou.Domain.Entities.Dependent>()))
            .Verifiable();

        unitOfWorkMock
        .Setup(u => u.Commit())
         .Returns(Task.CompletedTask)
        .Verifiable();

       var useCase = new UpdateDependentUseCase(
            dependentWriteOnlyRepositoryMock.Object,
            mapperMock.Object,
            unitOfWorkMock.Object,
            userReadonlyRepositoryMock.Object,
            userContextMock.Object);

        var response = await useCase.Execute(request, dependentId);

        response.ShouldNotBeNull();
        response.Name.ShouldBe(request.Name);
        response.Message.ShouldBe("Dependente atualizado(a) com sucesso");

        dependentWriteOnlyRepositoryMock.Verify(r => r.Update(It.IsAny<Tomou.Domain.Entities.Dependent>()), Times.Once);

        unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
    }
}
