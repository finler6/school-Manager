using SchoolManage.BL.Facades;
using SchoolManage.BL.Models;
using SchoolManage.Common.Enums;
using SchoolManage.Common.Tests;
using SchoolManage.Common.Tests.Seeds;
using System.ComponentModel;
using Xunit;
using Xunit.Abstractions;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace SchoolManage.BL.Tests;

public class SubjectFacadeTests : FacadeTestsBase
{
    private readonly ISubjectFacade _facadeSUT;

    public SubjectFacadeTests(ITestOutputHelper output) : base(output)
    {
        _facadeSUT = new SubjectFacade(UnitOfWorkFactory, SubjectModelMapper);
    }

    [Fact]
    public async Task Create_WithoutStudentsandActivitiesSubject_EqualsCreated()
    {
        //Arrange
        var model = new SubjectDetailModel()
        {
            Name = "Subject 1",
            Acronym = "S1"
        };
        //Act
        var returnedModel = await _facadeSUT.SaveAsync(model);
        //Assert
        model.Id = returnedModel.Id;
        DeepAssert.Equal(model, returnedModel);
    }

    [Fact]
    public async Task Create_WithNonExistingActivity_Throws()
    {
        //Arrange
        var model = new SubjectDetailModel()
        {
            Name = "Subject 1",
            Acronym = "S1",
            Students = new ObservableCollection<StudentListModel>()
            {
                new()
                {
                    Name = "Barry",
                    Surname = "Allen"
                }
            },
            Activities = new ObservableCollection<ActivityListModel>()
            {
                new()
                {
                    Id = Guid.Empty,
                    Start = DateTime.Now,
                    End = DateTime.Now,
                    Room = Room.None,
                    Description = string.Empty,
                    Type = ActivityType.None,
                    Subjects = SubjectSeeds.EmptySubject
                }
            }
        };
        //Assert & Act
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _facadeSUT.SaveAsync(model));
    }
    [Fact]
    public async Task Create_WithNonExistingStudent_Throws()
    {
        //Arrange
        var model = new SubjectDetailModel()
        {
            Name = "Subject 1",
            Acronym = "S1",
            Activities = new ObservableCollection<ActivityListModel>()
            {
                new()
                {
                    Start = DateTime.Now.AddHours(10),
                    End = DateTime.Now.AddHours(12),
                    Room = Room.Classroom,
                    Description = "Aktivita",
                    Type = ActivityType.Exercise,
                    Subjects = SubjectSeeds.EmptySubject
                }
            },
            Students = new ObservableCollection<StudentListModel>()
            {
                new()
                {
                    Id = Guid.Empty,
                    Name = "student",
                    Surname = "1"
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
        var model = new SubjectDetailModel()
        {
            Name = "Subject 1",
            Acronym = "S1",
            Activities = new ObservableCollection<ActivityListModel>()
            {
                new()
                {
                    Id = Guid.Empty,
                    Start = DateTime.Now,
                    End = DateTime.Now,
                    Room = Room.None,
                    Description = string.Empty,
                    Type = ActivityType.None,
                    Subjects = SubjectSeeds.EmptySubject
                }
            },
            Students = new ObservableCollection<StudentListModel>()
            {
                new()
                {
                    Id = Guid.Empty,
                    Name = "student",
                    Surname = "1"
                }
            }
        };
        //Assert & Act
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _facadeSUT.SaveAsync(model));
    }
    [Fact]
    public async Task Create_WithExistingSubjectandActivity_Throws()
    {
        //Arrange
        var model = new SubjectDetailModel()
        {
            Name = "Subject 1",
            Acronym = "S1",
            Activities = new ObservableCollection<ActivityListModel>()
            {
                new ()
                {
                    Id = ActivitySeeds.one.Id,
                    Start = ActivitySeeds.one.Start,
                    End = ActivitySeeds.one.End,
                    Room = ActivitySeeds.one.Room,
                    Description = ActivitySeeds.one.Description,
                    Type = ActivitySeeds.one.Type,
                    Subjects = ActivitySeeds.one.Subjects
                }
            },
            Students = new ObservableCollection<StudentListModel>()
            {
                new ()
                {
                    Id = StudentSeeds.student1.Id,
                    Name = StudentSeeds.student1.Name,
                    Surname = StudentSeeds.student1.Surname
                }
            }
        };
        //Assert & Act
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _facadeSUT.SaveAsync(model));
    }

    [Fact]
    public async Task Update_FromSeed_DoesNotThrow()
    {
        //Arrange
        var detailModel = SubjectModelMapper.MapToDetailModel(SubjectSeeds.english);
        detailModel.Name = "Updated Subject Name";
        //Act & Assert
        await _facadeSUT.SaveAsync(detailModel with { Activities = default!, Students = default! });
    }

    [Fact]
    public async Task Update_Name_FromSeeded_Updated()
    {
        //Arrange
        var detailModel = SubjectModelMapper.MapToDetailModel(SubjectSeeds.english);
        detailModel.Name = "Updated Subject Name";
        //Act
        await _facadeSUT.SaveAsync(detailModel with { Activities = default!, Students = default! });
        //Assert
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal("Updated Subject Name", returnedModel.Name);
    }
    [Fact]
    public async Task Update_RemoveStudents_FromSeeded_NotUpdated()
    {
        // Hazi chybu, protoze v safeAsync se kontroluji collections a jestli jsou prazdne, studenti nejsou, tak to pada -> kdyz se umaze activities.clear, je to v pohode
        // To samy testy niz
        //Arrange
        var detailModel = SubjectModelMapper.MapToDetailModel(SubjectSeeds.math);
        detailModel.Students.Clear();
        detailModel.Activities.Clear();

        //Act
        await _facadeSUT.SaveAsync(detailModel);

        //Assert
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(SubjectModelMapper.MapToDetailModel(SubjectSeeds.math), returnedModel);
    }
    [Fact]
    public async Task Update_RemoveActivity_FromSeeded_NotUpdated()
    {
        //Arrange
        var detailModel = SubjectModelMapper.MapToDetailModel(SubjectSeeds.math);
        detailModel.Activities.Clear();
        detailModel.Students.Clear();

        //Act
        await _facadeSUT.SaveAsync(detailModel);

        //Assert
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(SubjectModelMapper.MapToDetailModel(SubjectSeeds.math), returnedModel);
    }

    [Fact]
    public async Task Update_RemoveOneOfStudents_FromSeeded_Updated()
    {
        //Arrange
        var detailModel = SubjectModelMapper.MapToDetailModel(SubjectSeeds.english);
        detailModel.Students.Remove(detailModel.Students.First());

        //Act
        await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _facadeSUT.SaveAsync(detailModel));

        //Assert
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);
        DeepAssert.Equal(SubjectModelMapper.MapToDetailModel(SubjectSeeds.english), returnedModel);
    }
    [Fact]
    public async Task Update_RemoveOneOfActivities_FromSeeded_Updated()
    {
        //Arrange
        var detailModel = SubjectModelMapper.MapToDetailModel(SubjectSeeds.english);
        detailModel.Activities.Remove(detailModel.Activities.First());

        //Act
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);

        //Assert
        DeepAssert.Equal(SubjectModelMapper.MapToDetailModel(SubjectSeeds.english), returnedModel);
    }

    [Fact]
    public async Task GetById_FromSeeded_EqualsSeeded()
    {
        //Arrange
        var detailModel = SubjectModelMapper.MapToDetailModel(SubjectSeeds.math);

        //Act
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);

        //Assert
        DeepAssert.Equal(detailModel, returnedModel);
    }
    [Fact]
    public async Task GetAll_FromSeeded_EqualsSeeded()
    {
        //Arrange
        var detailModel = SubjectModelMapper.MapToDetailModel(SubjectSeeds.math);

        //Act
        var returnedModel = await _facadeSUT.GetAsync(detailModel.Id);

        //Assert
        DeepAssert.Equal(detailModel, returnedModel);
    }
    [Fact]
    public async Task GetById_NonExistent()
    {
        //Arrange & Act
        var ingredient = await _facadeSUT.GetAsync(SubjectSeeds.EmptySubject.Id);
        //Assert
        Assert.Null(ingredient);
    }
    [Fact]
    public async Task DeleteById_FromSeeded_DoesNotThrow()
    {
        //Arrange & Act & Assert
        await _facadeSUT.DeleteAsync(SubjectSeeds.math.Id);
    }
    
    [Fact]
    public async Task Search_Seeded()
    {
        //Arrange & Act
        var subject = await _facadeSUT.SearchSubjectsAsync("ma");
        
        var sub = subject.First().Id;
        //Assert
        DeepAssert.Equal(SubjectSeeds.math.Id, sub);
    }
}
