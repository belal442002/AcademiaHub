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
        // Methods
        Task BeginTransactionAsync();
        Task<int> SaveChangesAsync();
        Task CommitAsync();
        Task RollBackAsync();
        Task<bool> CompleteAsync();
    }
}
