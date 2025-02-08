using SchoolManage.BL.Facades;
using SchoolManage.BL.Models;
using SchoolManage.Common.Enums;
using SchoolManage.Common.Tests;
using SchoolManage.Common.Tests.Seeds;
using SchoolManage.DAL.Entities;
using Xunit;
using Xunit.Abstractions;
using ActivitySeeds = SchoolManage.DAL.Seeds.ActivitySeeds;

namespace SchoolManage.BL.Tests;

public class EvaluationFacadeTests : FacadeTestsBase
{
    private readonly IEvaluationFacade _facadeSUT;

    public EvaluationFacadeTests(ITestOutputHelper output) : base(output)
    {
        _facadeSUT = new EvaluationFacade(UnitOfWorkFactory, EvaluationModelMapper);
    }

    [Fact]
    public async Task Create_WithNewEvaluation_EqualsCreated()
    {
        var model = new EvaluationDetailModel()
        {
            Points = "100",
            Description = "Excellent",
            Activities = new ActivityEntity() 
            {
                Id = Guid.NewGuid(),
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(1),
                Room = Room.Classroom,
                Type = ActivityType.Exam,
                SubjectsId = SubjectSeeds.math.Id,
                Subjects = new SubjectEntity()
                {
                    Id = Guid.Empty,
                    Name = "Math",
                    Acronym = "MTH",
                }
            }, 
            Students = new StudentEntity()
            {
                Id = Guid.NewGuid(),
                Name = "Adolf",
                Surname = "Patrick",
                ImageUrl = "https://example.com/adolfik.jpg"
            }, 
        };

        var returnedModel = await _facadeSUT.SaveAsync(model);
        model.Id = returnedModel.Id;
        DeepAssert.Equal(model, returnedModel);
    }

    [Fact]
    public async Task GetById_FromSeeded_EqualsSeeded()
    {
        var detailModel = EvaluationModelMapper.MapToDetailModel(EvaluationSeeds.one);

        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);

        DeepAssert.Equal(detailModel, returnedModel);
    }

    [Fact]
    public async Task Update_FromSeeded_DoesNotThrow()
    {
        var detailModel = EvaluationModelMapper.MapToDetailModel(EvaluationSeeds.one);
        detailModel.Points = "95";

        await _facadeSUT.SaveAsync(detailModel);
    }

    [Fact]
    public async Task Update_Points_FromSeeded_Updated()
    {
        var detailModel = EvaluationModelMapper.MapToDetailModel(EvaluationSeeds.two);
        detailModel.Points = "90";

        await _facadeSUT.SaveAsync(detailModel);

        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        Assert.Equal("90", returnedModel.Points);
    }

    [Fact]
    public async Task GetFirstWtih10Points_Single_SeededEvaluation()
    {
        var evaluations = await _facadeSUT.GetAsync();
        var evaluation = evaluations.First(i => i.Points == "10");

        DeepAssert.Equal(EvaluationModelMapper.MapToListModel(EvaluationSeeds.one), evaluation);
    }
    
    [Fact]
    public async Task DeleteById_FromSeeded_DoesNotThrow()
    {
        //Arrange & Act & Assert
        await _facadeSUT.DeleteAsync(EvaluationSeeds.one.Id);
    }
    
}