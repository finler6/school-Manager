using SchoolManage.Common.Tests;
using SchoolManage.Common.Tests.Seeds;
using SchoolManage.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using SchoolManage.Common.Enums;
using System.Diagnostics;

namespace SchoolManage.DAL.Tests;
 public class DBContextEvaluationBase : DbContextTestsBase
 {
     [Fact]
     public async Task AddNew_Evaluation_Persisted()
     {
         //Arrange
         var entity = EvaluationSeeds.EmptyEvaluation with
         {
             Id = Guid.NewGuid(),
             Points = "30",
             StudentsId = StudentSeeds.student1.Id,
             ActivitiesId = ActivitySeeds.one.Id
         };
         
         //Act
         SchoolManageDbContextSUT.Evaluations.Add(entity);
         await SchoolManageDbContextSUT.SaveChangesAsync();
         
         //Assert
         await using var dbx = await DbContextFactory.CreateDbContextAsync();
         var actualEntity = await dbx.Evaluations.SingleAsync(i => i.Id == entity.Id);
         DeepAssert.Equal(entity, actualEntity);
     }
     
     [Fact]
     public async Task Update_EvaluationPoints_Persisted()
     {
         //Arrange
         var baseEntity = EvaluationSeeds.IngredientAmountEntityUpdate;
         var entity = baseEntity with
         {
             Points = "29"
         };
        
         //Act
         SchoolManageDbContextSUT.Evaluations.Update(entity);
         await SchoolManageDbContextSUT.SaveChangesAsync();
        
         //Assert
         await using var dbx = await DbContextFactory.CreateDbContextAsync();
         var actualEntity = await dbx.Evaluations.SingleAsync(i => i.Id == entity.Id);
         DeepAssert.Equal(entity, actualEntity);
     }
         
     [Fact]
     public async Task GetById_Evaluation_EvaluationRetrieved()
     {
         //Act
         var retrievedEntity = await SchoolManageDbContextSUT.Evaluations.SingleAsync(i => i.Id == EvaluationSeeds.one.Id);
        
         //Assert
         DeepAssert.Equal(EvaluationSeeds.one, retrievedEntity);
     }
     
     [Fact]
     public async Task DeleteById_Subject_SubjectDeleted()
     {
         //Arrange
         var baseEnitity = EvaluationSeeds.one;

         //Act
         SchoolManageDbContextSUT.Remove(
             SchoolManageDbContextSUT.Evaluations.Single(i=> i.Id == baseEnitity.Id));
         await SchoolManageDbContextSUT.SaveChangesAsync();
        
         //Assert
         Assert.False(await SchoolManageDbContextSUT.Evaluations.AnyAsync(i=> i.Id == baseEnitity.Id));
     }
         
 }
