 using SchoolManage.Common.Tests;
 using SchoolManage.Common.Tests.Seeds;
 using SchoolManage.DAL.Entities;
 using Microsoft.EntityFrameworkCore;
 using Xunit;
using SchoolManage.Common.Enums;
using System.Diagnostics;

namespace SchoolManage.DAL.Tests;
 public class DbContextSubjectTests : DbContextTestsBase
 {
    [Fact]
    public async Task AddNew_Subject_Persisted()
    {
        //Arrange
        var entity = SubjectSeeds.EmptySubject with
        {
            Id = Guid.Parse("2d4fa150-ad80-4d46-a511-4c666166ec5e"),
            Name = "Physical education",
            Acronym = "PE",
        };

        //Act
        SchoolManageDbContextSUT.Subjects.Add(entity);
        await SchoolManageDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Subjects.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task UpdateById_Subject_ChangedAcronym() //kdyz pred nazev dam AA, projde to v pohode
    {
        //Arrange
        var baseEntity = SubjectSeeds.SubjectEntityUpdate;
        var entity = baseEntity with
        {
            Acronym = "MYTH"
        };

        //Act
        SchoolManageDbContextSUT.Subjects.Update(entity);
        await SchoolManageDbContextSUT.SaveChangesAsync();
        
        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Subjects.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task GetById_Subject_SubjectRetrieved()
    {
        //Act
        var retrievedEntity = await SchoolManageDbContextSUT.Subjects.SingleAsync(i => i.Id == SubjectSeeds.english.Id);
        
        //Assert
        DeepAssert.Equal(SubjectSeeds.english, retrievedEntity);
    }

    [Fact]
    public async Task DeleteById_Subject_SubjectDeleted()
    {
        //Arrange
        var baseEnitity = SubjectSeeds.english;

        //Act
        SchoolManageDbContextSUT.Remove(
            SchoolManageDbContextSUT.Subjects.Single(i=> i.Id == baseEnitity.Id));
        await SchoolManageDbContextSUT.SaveChangesAsync();
        
        //Assert
        Assert.False(await SchoolManageDbContextSUT.Subjects.AnyAsync(i=> i.Id == baseEnitity.Id));
    }
    
    
}
