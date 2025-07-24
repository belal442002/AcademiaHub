using AcademiaHub.Data;
using AcademiaHub.Interfaces;
using AcademiaHub.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace AcademiaHub.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AcademiaHubDbContext _dbContext;
        private IDbContextTransaction? _transaction;

        // Repositories
        public IStudentRepository StudentRepository { get; private set; }
        public ITeacherRepository TeacherRepository {  get; private set; }
        public ISubjectRepository SubjectRepository { get; private set; }
        public IClassroomRepository ClassroomRepository { get; private set; }
        public IStudent_ClassroomRepository StudentClassroomRepository {  get; private set; }
        public ITeacherClassroomRepository TeacherClassroomRepository { get; private set; }
        public IQuestionBankRepository QuestionBankRepository { get; private set; }
        public IQuestionTypeRepository QuestionTypeRepository { get; private set; }
        public IDifficultyRepository DifficultyRepository { get; private set; }
        public IFormTypeRepository FormTypeRepository { get; private set; }
        public IQuestionsFormRepository QuestionsFormRepository { get; private set; }
        public IFormDetailsRepository FormDetailsRepository { get; private set; }
        public IFormQuestionsRepository FormQuestionsRepository { get; private set; }
        public IFormStudentAnswerRepository FormStudentAnswerRepository { get; private set; }
        public IStudent_QuestionsFormRepository Student_QuestionsFormRepository { get; private set; }

        public UnitOfWork(AcademiaHubDbContext dbContext)
        {
            _dbContext = dbContext;
            _transaction = null;

            // Repositories
            StudentRepository = new StudentRepository(dbContext);
            TeacherRepository = new TeacherRepository(dbContext);
            SubjectRepository = new SubjectRepository(dbContext);
            ClassroomRepository = new ClassroomRepository(dbContext);
            StudentClassroomRepository = new Student_ClassroomRepository(dbContext);
            TeacherClassroomRepository = new TeacherClassroomRepository(dbContext);
            QuestionBankRepository = new QuestionBankRepository(dbContext);
            QuestionTypeRepository = new QuestionTypeRepository(dbContext);
            DifficultyRepository = new DifficultyRepository(dbContext);
            FormTypeRepository = new FormTypeRepository(dbContext);
            QuestionsFormRepository = new QuestionsFormRepository(dbContext);
            FormDetailsRepository = new FormDetailsRepository(dbContext);
            FormQuestionsRepository = new FormQuestionsRepository(dbContext);
            FormStudentAnswerRepository = new FormStudentAnswersRepository(dbContext);
            Student_QuestionsFormRepository = new Student_QuestionsFormRepository(dbContext);
        }

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();


        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction is null)
            {
                throw new InvalidOperationException("No transaction to commit.");
            }
            await _transaction.CommitAsync();
            _transaction = null;
        }

        public async Task RollBackAsync()
        {
            if (_transaction is null)
            {
                throw new InvalidOperationException("No transaction to roll back.");
            }
            await _transaction.RollbackAsync();
            _transaction = null;
        }

        public async Task<bool> CompleteAsync()
        {
            try
            {
                await SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
                return true;
            }
            catch (Exception)
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                }
                return false;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _dbContext.Dispose();
        }
    }
}
