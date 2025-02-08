using SchoolManage.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace SchoolManage.DAL.Seeds;

public static class StudentSeeds
{
    public static readonly StudentEntity student1 = new()
    {
        Id = Guid.NewGuid(),
        Name = "John",
        Surname = "Doe",
        ImageUrl = @"https://www.pngitem.com/pimgs/m/40-406527_cartoon-glass-of-water-png-glass-of-water.png",
    };

    public static readonly StudentEntity student2 = new()
    {
        Id = Guid.NewGuid(),
        Name = "Jane",
        Surname = "Doe",
        ImageUrl =
            @"https://www.eatthis.com/wp-content/uploads/sites/4/2020/02/lemons.jpg?quality=82&strip=1&resize=640%2C360"
    };

    public static void Seed(ModelBuilder modelBuilder) =>  modelBuilder.Entity<StudentEntity>().HasData(
        student1,
        student2
    );
}