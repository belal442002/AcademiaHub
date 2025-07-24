using AcademiaHub.Interfaces;

namespace AcademiaHub.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // Repositories
        IStudentRepository StudentRepository { get; }
        ITeacherRepository TeacherRepository { get; }
        ISubjectRepository SubjectRepository { get; }
        IStudent_ClassroomRepository StudentClassroomRepository { get; }
        IClassroomRepository ClassroomRepository { get; }
        ITeacherClassroomRepository TeacherClassroomRepository { get; }
        IQuestionBankRepository QuestionBankRepository { get; }
        IQuestionTypeRepository QuestionTypeRepository { get; }
        IDifficultyRepository DifficultyRepository { get; }
        IFormTypeRepository FormTypeRepository { get; }
        IQuestionsFormRepository QuestionsFormRepository { get; }
        IFormDetailsRepository FormDetailsRepository { get; }
        IFormQuestionsRepository FormQuestionsRepository { get; }
        IFormStudentAnswerRepository FormStudentAnswerRepository { get; }
        IStudent_QuestionsFormRepository Student_QuestionsFormRepository { get; }

        // Methods
        Task BeginTransactionAsync();
        Task<int> SaveChangesAsync();
        Task CommitAsync();
        Task RollBackAsync();
        Task<bool> CompleteAsync();
    }
}
