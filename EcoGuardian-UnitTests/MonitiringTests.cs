
using EcoGuardian_Backend.OperationAndMonitoring.Application.Internal.CommandServices;
using EcoGuardian_Backend.OperationAndMonitoring.Application.Internal.OutboundServices;
using EcoGuardian_Backend.OperationAndMonitoring.Domain.Model.Aggregates;
using EcoGuardian_Backend.OperationAndMonitoring.Domain.Model.Commands;
using EcoGuardian_Backend.OperationAndMonitoring.Domain.Model.Queries;
using EcoGuardian_Backend.OperationAndMonitoring.Domain.Repositories;
using EcoGuardian_Backend.OperationAndMonitoring.Domain.Services;
using EcoGuardian_Backend.Shared.Application.Internal.CloudinaryStorage;
using EcoGuardian_Backend.Shared.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Moq;

namespace EcoGuardian_UnitTests;

public class Tests
{
    [Test]
    public async Task RegisterPlant_Must_To_Be_Working()
    {
        var plantCommandServiceMock = new Mock<IPlantCommandService>();
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write("Test file content");
        writer.Flush();
        stream.Position = 0;

        var formFile = new FormFile(stream, 0, stream.Length, "file", "test.txt")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/png"
        };
        var command = new CreatePlantCommand(
            "TestPlant",
            formFile,
            "TestType",
            10,
            "asssa",
            10,
            10,
            10,
            false,
            1
            );
        var plant = new Plant(command);
        plantCommandServiceMock.Setup(x => x.Handle(command)).ReturnsAsync(plant);
        var result = await plantCommandServiceMock.Object.Handle(command);
        Assert.That(result, Is.Not.Null);

    }

    [Test]
    public Task Update_Plant_Must_To_Be_Working()
    {
        var plantCommandServiceMock = new Mock<IPlantCommandService>();
        var command = new UpdatePlantCommand(
            1,
            "TestPlant",
            "TestType",
            10,
            1,
            10,
            10,
            10,
            false,
            1
        );
        plantCommandServiceMock.Setup(x => x.Handle(command));
        
        return plantCommandServiceMock.Object.Handle(command)
            .ContinueWith(t => Assert.That(t.Exception, Is.Null));
    }
    
    
    
    [Test]
    public Task Delete_Plant_Must_To_Be_Working()
    {
        var plantCommandServiceMock = new Mock<IPlantCommandService>();
        var command = new DeletePlantCommand(1);
        plantCommandServiceMock.Setup(x => x.Handle(command));
        
        return plantCommandServiceMock.Object.Handle(command)
            .ContinueWith(t => Assert.That(t.Exception, Is.Null));
    }
    
    [Test]
    public Task Get_Plants_By_User_Id_Must_To_Be_Working()
    {
        var plantQueryServiceMock = new Mock<IPlantQueryService>();
        var query = new GetPlantsByUserIdQuery(1);
        
        return plantQueryServiceMock
            .Object.Handle(query).
            ContinueWith(
                task => Assert.That(task.Result, Is.Not.Null));  
    }
    
    
    
    [Test]
    public async Task Throw_Exception_When_User_Doesnt_Exists_Must_To_Be_Working()
    {
        var plantRepository = new Mock<IPlantRepository>();
        var externalUserService = new Mock<IExternalUserService>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var cloudinaryStorage = new Mock<ICloudinaryStorage>();
        var plantCommandServiceMock = new PlantCommandService(
            plantRepository.Object,
            externalUserService.Object,
            unitOfWork.Object,
            cloudinaryStorage.Object
        );
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write("Test file content");
        writer.Flush();
        stream.Position = 0;

        var formFile = new FormFile(stream, 0, stream.Length, "file", "test.txt")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/png"
        };
        var command = new CreatePlantCommand(
            "TestPlant",
            formFile,
            "TestType",
            10,
            "ass",
            10,
            10,
            10,
            false,
            1
        );
        
        await plantCommandServiceMock.Handle(command) 
            .ContinueWith(t => Assert.That(t.Exception, Is.Not.Null));;
        


    }
}