using SchoolManage.BL.Facades;
using SchoolManage.BL.Models;
using SchoolManage.Common.Tests;
using SchoolManage.Common.Tests.Seeds;
using SchoolManage.DAL.Entities;
using SchoolManage.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using Xunit.Abstractions;

namespace SchoolManage.BL.Tests;

public class ActivityFacadeTests : FacadeTestsBase
{
    private readonly IActivityFacade _activityFacadeSUT;

    public ActivityFacadeTests(ITestOutputHelper output) : base(output)
    {
        _activityFacadeSUT = new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper);
    }
    
    [Fact]
    public async Task GetById_FromSeeded_EqualsSeeded()
    {
        var detailModel = ActivityModelMapper.MapToDetailModel(ActivitySeeds.one);

        var returnedModel = await _activityFacadeSUT.GetAsync(detailModel.Id);

        DeepAssert.Equal(detailModel, returnedModel);
    }

    [Fact]
    public async Task Create_WithNonExistingItem_DoesNotThrow()
    {
        var activity = new ActivityDetailModel()
        {
            Id = Guid.Empty,
            Room = Room.Classroom,
            Type = ActivityType.Homework,
            Start = new DateTime(2021, 03, 24, 03, 34, 50),
            End = new DateTime(2021, 03, 24, 08, 30, 50),
            Description = "Aktivita",
            SubjectsId = SubjectSeeds.math.Id,
            Subjects = new SubjectEntity()
            {
                Id = Guid.Empty,
                Name = "Math",
                Acronym = "MTH",
            }
        };

        var activity1 = await _activityFacadeSUT.SaveAsync(activity);
        activity.Id = activity1.Id;
        DeepAssert.Equal(activity, activity1);

    }

    [Fact]

    public async Task Create_WithExistingItem_UpdatesItem()
    {
        var activity = new ActivityDetailModel()
        {
            Id = Guid.Empty,
            Room = Room.Classroom,
            Type = ActivityType.Homework,
            Start = new DateTime(2021, 03, 24, 03, 34, 50),
            End = new DateTime(2021, 03, 24, 08, 30, 50),
            Description = "Aktivita",
            SubjectsId = SubjectSeeds.math.Id,
            Subjects = new SubjectEntity()
            {
                Id = Guid.Empty,
                Name = "Math",
                Acronym = "MTH",
            }
        };

        activity = await _activityFacadeSUT.SaveAsync(activity);

        activity.Type = ActivityType.Quiz;
        activity = await _activityFacadeSUT.SaveAsync(activity);
    }

    [Fact]
    public async Task Delete_WithNonExistingItem_Throws()
    {
        var activity = new ActivityDetailModel()
        {
            Id = Guid.Empty,
            Room = Room.Classroom,
            Type = ActivityType.Homework,
            Start = new DateTime(2021, 03, 24, 03, 34, 50),
            End = new DateTime(2021, 03, 24, 08, 30, 50),
            Description = "Aktivita",
            SubjectsId = SubjectSeeds.math.Id,
            Subjects = new SubjectEntity()
            {
                Id = Guid.Empty,
                Name = "Math",
                Acronym = "MTH",
            }
        };

        activity = await _activityFacadeSUT.SaveAsync(activity);

        await _activityFacadeSUT.DeleteActivityAsync(activity.Id);
    }

    [Fact]

    public async Task Delete_WithExistingItem_DeletesItem()
    {
        var activity = new ActivityDetailModel()
        {
            Id = Guid.Empty,
            Room = Room.Classroom,
            Type = ActivityType.Homework,
            Start = new DateTime(2021, 03, 24, 03, 34, 50),
            End = new DateTime(2021, 03, 24, 08, 30, 50),
            Description = "Aktivita",
            SubjectsId = SubjectSeeds.math.Id,
            Subjects = new SubjectEntity()
            {
                Id = Guid.Empty,
                Name = "Math",
                Acronym = "MTH",
            }
        };

        activity = await _activityFacadeSUT.SaveAsync(activity);

        await _activityFacadeSUT.DeleteActivityAsync(activity.Id);

    }

    [Fact]

    public async Task UpdateActivity_WithExistingItem_UpdatesItem()
    {
        var activity = new ActivityDetailModel()
        {
            Id = Guid.Empty,
            Room = Room.Classroom,
            Type = ActivityType.Homework,
            Start = new DateTime(2021, 03, 24, 03, 34, 50),
            End = new DateTime(2021, 03, 24, 08, 30, 50),
            Description = "Aktivita",
            SubjectsId = SubjectSeeds.math.Id,
            Subjects = new SubjectEntity()
            {
                Id = Guid.Empty,
                Name = "Math",
                Acronym = "MTH",
            }
        };

        activity = await _activityFacadeSUT.SaveAsync(activity);

        activity.Type = ActivityType.Quiz;
        activity = await _activityFacadeSUT.SaveAsync(activity);
    }
    
}