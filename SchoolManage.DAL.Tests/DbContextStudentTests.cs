using SchoolManage.Common.Tests;
using SchoolManage.Common.Tests.Seeds;
using SchoolManage.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
// using Xunit.Abstractions;

namespace SchoolManage.DAL.Tests;
public class DbContextStudentTests : DbContextTestsBase
{
    [Fact]
    public async Task AddNew_Student_Persisted()
    {
        //Arrange
        var entity = StudentSeeds.EmptyStudentSeed with
        {
            Id = Guid.NewGuid(),
            Name = "James",
            Surname = "Duke",
            ImageUrl = @"https://www.eatthis.com/wp-content/uploads/sites/4/2020/02/lemons.jpg?quality=82&strip=1&resize=640%2C360",
        };

        //Act
        SchoolManageDbContextSUT.Students.Add(entity);
        await SchoolManageDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Students.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task Update_StudentName_Persisted()
    {
        //Arrange
        var baseEntity = StudentSeeds.StudentEntityUpdate;
        var entity = baseEntity with
        {
            Name = "Pepan"
        };
        
        //Act
        SchoolManageDbContextSUT.Students.Update(entity);
        await SchoolManageDbContextSUT.SaveChangesAsync();
        
        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Students.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task GetById_Student_StudentRetrieved()
    {
        //Act
        var retrievedEntity = await SchoolManageDbContextSUT.Students.SingleAsync(i => i.Id == StudentSeeds.student1.Id);
        
        //Assert
        DeepAssert.Equal(StudentSeeds.student1,retrievedEntity);
    }

    [Fact]
    public async Task DeleteById_Student_StudentDeleted()
    {
        //Arrange
        var baseEntity = StudentSeeds.StudentEntityDelete;

        //Act
        SchoolManageDbContextSUT.Remove(
            SchoolManageDbContextSUT.Students.Single(i => i.Id == baseEntity.Id));
        await SchoolManageDbContextSUT.SaveChangesAsync();
        
        //Assert
        Assert.False(await SchoolManageDbContextSUT.Students.AnyAsync(i => i.Id == baseEntity.Id));
    }
    
}
