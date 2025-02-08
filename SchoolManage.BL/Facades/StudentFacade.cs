using SchoolManage.BL.Mappers;
using SchoolManage.BL.Models;
using SchoolManage.DAL.Entities;
using SchoolManage.DAL.Mappers;
using SchoolManage.DAL.Repositories;
using SchoolManage.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SchoolManage.BL.Facades
{
    public class StudentFacade : FacadeBase<StudentEntity, StudentListModel, StudentDetailModel, StudentEntityMapper>, IStudentFacade
    {
        public StudentFacade(IUnitOfWorkFactory unitOfWorkFactory, StudentModelMapper studentModelMapper)
            : base(unitOfWorkFactory, studentModelMapper)
        {
        }

        public async Task CreateStudentAsync(StudentDetailModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var entity = ModelMapper.MapToEntity(model);

            await using var uow = UnitOfWorkFactory.Create();
            var repository = uow.GetRepository<StudentEntity, StudentEntityMapper>();

            if (await repository.ExistsAsync(entity))
            {
                throw new InvalidOperationException($"Student with ID {entity.Id} already exists.");
            }

            repository.Insert(entity);
            await uow.CommitAsync();
        }

        public async Task UpdateStudentAsync(StudentDetailModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            await using var uow = UnitOfWorkFactory.Create();
            var studentRepository = uow.GetRepository<StudentEntity, StudentEntityMapper>();
            var subjectRepository = uow.GetRepository<SubjectEntity, SubjectEntityMapper>();

            var existingStudent = await studentRepository.Get().Include(s => s.Subjects).SingleOrDefaultAsync(s => s.Id == model.Id);
            if (existingStudent == null)
            {
                throw new KeyNotFoundException($"Student with ID {model.Id} not found.");
            }

            // Update student properties
            existingStudent.Name = model.Name;
            existingStudent.Surname = model.Surname;
            existingStudent.ImageUrl = model.ImageUrl;

            // Update subjects only if they are different
            var newSubjectIds = model.Subjects.Select(s => s.Id).ToList();
            var existingSubjectIds = existingStudent.Subjects.Select(s => s.Id).ToList();

            if (!newSubjectIds.SequenceEqual(existingSubjectIds))
            {
                var newSubjects = await subjectRepository.Get()
                    .Where(s => newSubjectIds.Contains(s.Id))
                    .ToListAsync();

                existingStudent.Subjects = newSubjects;
            }

            await studentRepository.UpdateAsync(existingStudent);
            await uow.CommitAsync();
        }




        public async Task DeleteStudentAsync(Guid studentId)
        {
            await using var uow = UnitOfWorkFactory.Create();
            var repository = uow.GetRepository<StudentEntity, StudentEntityMapper>();

            var student = await repository.Get().SingleOrDefaultAsync(s => s.Id == studentId);
            if (student == null)
            {
                throw new KeyNotFoundException($"Student with ID {studentId} not found.");
            }

            await repository.DeleteAsync(studentId);
            await uow.CommitAsync();
        }

        public async Task<IEnumerable<SubjectListModel>> GetStudentSubjectsAsync(Guid studentId)
        {
            await using var uow = UnitOfWorkFactory.Create();
            var studentRepository = uow.GetRepository<StudentEntity, StudentEntityMapper>();

            var studentWithSubjects = await studentRepository
                .Get()
                .Include(s => s.Subjects)
                .SingleOrDefaultAsync(s => s.Id == studentId);

            if (studentWithSubjects == null)
            {
                throw new KeyNotFoundException($"Student with ID {studentId} not found.");
            }

            var subjectModelMapper = new SubjectModelMapper();

            return studentWithSubjects.Subjects.Select(subjectModelMapper.MapToListModel).ToList();
        }

        public async Task<IEnumerable<EvaluationListModel>> GetStudentEvaluationsAsync(Guid studentId)
        {
            await using var uow = UnitOfWorkFactory.Create();
            var studentRepository = uow.GetRepository<StudentEntity, StudentEntityMapper>();
            var studentWithEvaluations = await studentRepository
                .Get()
                .Include(s => s.Evaluations)
                .SingleOrDefaultAsync(s => s.Id == studentId);

            if (studentWithEvaluations == null)
            {
                throw new KeyNotFoundException($"Student with ID {studentId} not found.");
            }

            var evaluationModelMapper = new EvaluationModelMapper();
            return studentWithEvaluations.Evaluations.Select(evaluationModelMapper.MapToListModel).ToList();
        }

        public async Task<IEnumerable<StudentListModel>> GetAllStudentsAsync()
        {
            await using var uow = UnitOfWorkFactory.Create();
            var studentRepository = uow.GetRepository<StudentEntity, StudentEntityMapper>();

            var students = await studentRepository
                .Get()
                .ToListAsync();

            var studentModelMapper = new StudentModelMapper();

            return students.Select(studentModelMapper.MapToListModel).ToList();
        }

        public async Task<IEnumerable<StudentListModel>> SearchStudentsAsync(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                throw new ArgumentException("Search query cannot be null or whitespace.", nameof(searchQuery));
            }

            await using var uow = UnitOfWorkFactory.Create();
            var studentRepository = uow.GetRepository<StudentEntity, StudentEntityMapper>();

            var students = await studentRepository
                .Get()
                .ToListAsync();

            students = students
                .Where(s => s.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                            s.Surname.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var studentModelMapper = new StudentModelMapper();

            return students.Select(student => studentModelMapper.MapToListModel(student)).ToList();
        }
        public async Task<IEnumerable<StudentListModel>> GetAllStudentsWithSubjectAsync(SubjectEntity Subject)
        {
            await using var uow = UnitOfWorkFactory.Create();
            var studentRepository = uow.GetRepository<StudentEntity, StudentEntityMapper>();

            var students = await studentRepository
                .Get()
                .Where(s => s.Subjects.Contains(Subject))
                .ToListAsync();

            var studentModelMapper = new StudentModelMapper();

            return students.Select(studentModelMapper.MapToListModel).ToList();
        }
    }
}
