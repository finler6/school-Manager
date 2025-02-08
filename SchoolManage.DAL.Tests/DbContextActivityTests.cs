using SchoolManage.Common.Tests;
using SchoolManage.Common.Tests.Seeds;
using SchoolManage.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using SchoolManage.Common.Enums;
using Xunit;
// using Xunit.Abstractions;

namespace SchoolManage.DAL.Tests;

public class DbContextActivityTests : DbContextTestsBase
{
    [Fact]
    public async Task AddNew_Activity_Persisted()
    {
        var entity = ActivitySeeds.EmptyActivity with
        {
            Id = Guid.NewGuid(),
            Start = DateTime.Now,
            End = DateTime.Now.AddHours(2),
            Room = Room.Classroom,
            Type = ActivityType.Homework,
            SubjectsId = SubjectSeeds.math.Id
        };
        
        //Act
        SchoolManageDbContextSUT.Activities.Add(entity);
        await SchoolManageDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Activities.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }
    
    [Fact]
    public async Task Update_ActivityType_Persisted()
    {
        //Arrange
        var baseEntity = ActivitySeeds.ActivityEntityUpdate;
        var entity = baseEntity with
        {
            Type = ActivityType.Quiz
        };
        
        //Act
        SchoolManageDbContextSUT.Activities.Update(entity);
        await SchoolManageDbContextSUT.SaveChangesAsync();
        
        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Activities.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }
    
    [Fact]
    public async Task GetById_Activity_ActivityRetrieved()
    {
        //Act
        var retrievedEntity = await SchoolManageDbContextSUT.Activities.SingleAsync(i => i.Id == ActivitySeeds.one.Id);
        
        //Assert
        DeepAssert.Equal(ActivitySeeds.one with { Subjects = null! },retrievedEntity);
    }
    
    [Fact]
    public async Task DeleteById_Activity_ActivityDeleted()
    {
        //Arrange
        var baseEntity = ActivitySeeds.ActivityEntityDelete;

        //Act
        SchoolManageDbContextSUT.Remove(
            SchoolManageDbContextSUT.Activities.Single(i => i.Id == baseEntity.Id));
        await SchoolManageDbContextSUT.SaveChangesAsync();
        
        //Assert
        Assert.False(await SchoolManageDbContextSUT.Activities.AnyAsync(i => i.Id == baseEntity.Id));
    }
}