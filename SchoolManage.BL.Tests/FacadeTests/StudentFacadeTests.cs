using Microsoft.EntityFrameworkCore;
using SchoolManage.BL.Facades;
using SchoolManage.BL.Models;
using SchoolManage.Common.Enums;
using SchoolManage.Common.Tests;
using SchoolManage.Common.Tests.Seeds;
using Xunit;
using Xunit.Abstractions;
using System.Collections.ObjectModel;

namespace SchoolManage.BL.Tests;

public class StudentFacadeTests : FacadeTestsBase
{
    private readonly IStudentFacade _facadeSUT;

    public StudentFacadeTests(ITestOutputHelper output) : base(output)
    {
        _facadeSUT = new StudentFacade(UnitOfWorkFactory, StudentModelMapper);
    }

    [Fact]
    public async Task Create_WithNewStudent_EqualsCreated()
    {
        //Arrange
        var model = new StudentDetailModel()
        {
            Name = "John",
            Surname = "Doe",
            ImageUrl = "https://example.com/johndoe.jpg",
        };
        //Act
        var returnedModel = await _facadeSUT.SaveAsync(model);
        //Assert
        model.Id = returnedModel.Id;
        DeepAssert.Equal(model, returnedModel);
    }
    [Fact]
    public async Task Create_WithNonExistingEvaluation_Throws()
    {
        //Arrange
        var model = new StudentDetailModel()
        {
            Name = "Bruce",
            Surname = "Wane",
            Subjects = new ObservableCollection<SubjectListModel>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Physical Education",
                    Acronym = "PE",
                }
            },
            Evaluations = new ObservableCollection<EvaluationListModel>()
            {
                new()
                {
                    Id = Guid.Empty,
                    Points = null,
                    Activities = ActivitySeeds.EmptyActivity,
                    Students = StudentSeeds.EmptyStudentSeed
                }
            }
        };
        //Assert & Act
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _facadeSUT.SaveAsync(model));
    }
    [Fact]
    public async Task Create_WithNonExistingSubject_Throws()
    {
        //Arrange
        var model = new StudentDetailModel()
        {
            Name = "Klack",
            Surname = "Kent",
            Subjects = new ObservableCollection<SubjectListModel>()
            {
                new()
                {
                    Id = Guid.Empty,
                    Name = null,
                    Acronym = null,
                }
            },
            Evaluations = new ObservableCollection<EvaluationListModel>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Points = "81",
                    Activities = ActivitySeeds.EmptyActivity,
                    Students = StudentSeeds.EmptyStudentSeed
                }
            }
        };
        //Assert & Act
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _facadeSUT.SaveAsync(model));
    }
    [Fact]
    public async Task Create_ExistingStudent_Throws()
    {
        //Arrange
        var model = new StudentDetailModel()
        {
            Name = "Tonny",
            Surname = "Stark",
            Subjects = new ObservableCollection<SubjectListModel>()
                {
                    new()
                    {
                        Id = SubjectSeeds.math.Id,
                        Name = SubjectSeeds.math.Name,
                        Acronym = SubjectSeeds.math.Acronym,
                    }
                },
            Evaluations = new ObservableCollection<EvaluationListModel>()
                {
                    new()
                    {
                        Id = EvaluationSeeds.one.Id,
                        Points = EvaluationSeeds.one.Points,
                        Activities = EvaluationSeeds.one.Activities,
                        Students = EvaluationSeeds.one.Students
                    }
                }
        };
        //Act + Assert
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _facadeSUT.SaveAsync(model));
    }

    [Fact]
    public async Task Update_FromSeeded_DoesNotThrow()
    {
        //Arrange
        var detailModel = StudentModelMapper.MapToDetailModel(StudentSeeds.student2);
        detailModel.Name = "Tonny";
        //Act & Assert
        await _facadeSUT.SaveAsync(detailModel with { Subjects = default!, Evaluations = default! });
    }

    [Fact]
    public async Task Update_Name_FromSeeded_Updated()
    {
        //Arrange
        var detailModel = StudentModelMapper.MapToDetailModel(StudentSeeds.student2);
        detailModel.Name = "Updated Tonny";

        //Act
        await _facadeSUT.SaveAsync(detailModel with { Subjects = default!, Evaluations = default! });

        //Assert
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal("Updated Tonny", returnedModel.Name);
    }
    [Fact]
    public async Task Update_RemoveEvaluation_FromSeeded_NotUpdated()
    {
        //Arrange
        var detailModel = StudentModelMapper.MapToDetailModel(StudentSeeds.student1);
        detailModel.Evaluations.Clear();

        //Act
        await _facadeSUT.SaveAsync(detailModel);

        //Assert
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(StudentModelMapper.MapToDetailModel(StudentSeeds.student1), returnedModel);
    }
    [Fact]
    public async Task Update_RemoveSubject_FromSeeded_NotUpdated()
    {
        //Arrange
        var detailModel = StudentModelMapper.MapToDetailModel(StudentSeeds.student1);
        detailModel.Subjects.Clear();

        //Act
        await _facadeSUT.SaveAsync(detailModel);

        //Assert
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(StudentModelMapper.MapToDetailModel(StudentSeeds.student1), returnedModel);
    }
    [Fact]
    public async Task GetById_FromSeeded_EqualsSeeded()
    {
        //Arrange
        var detailModel = StudentModelMapper.MapToDetailModel(StudentSeeds.student1);

        //Act
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);

        //Assert
        DeepAssert.Equal(detailModel, returnedModel);
    }

    [Fact]
    public async Task GetById_NonExistent()
    {
        //Arrange & Act
        var ingredient = await _facadeSUT.GetAsync(StudentSeeds.EmptyStudentSeed.Id);
        //Assert
        Assert.Null(ingredient);
    }

    [Fact]
    public async Task DeleteById_FromSeeded_Deleted()
    {
        //Arrange & Act & Assert
        await _facadeSUT.DeleteAsync(StudentSeeds.student1.Id);
    }
}
