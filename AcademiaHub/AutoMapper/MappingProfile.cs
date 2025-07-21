using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto.Classroom;
using AcademiaHub.Models.Dto.Student;
using AcademiaHub.Models.Dto.Student_Classroom;
using AcademiaHub.Models.Dto.Subject;
using AcademiaHub.Models.Dto.Teacher;
using AcademiaHub.Models.Dto.Teacher_Classroom;
using AutoMapper;

namespace AcademiaHub.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Student
            // Student => StudentGetRequest
            CreateMap<Student, StudentGetRequest>().
                ForMember(dest => dest.AccountId, opt => opt.MapFrom(student => student.Account!.Id)).
                ForMember(dest => dest.Email, opt => opt.MapFrom(student => student.Account!.Email)).
                ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(student => student.Account!.PhoneNumber));

            // Teacher
            // Teacher => TeacherGetRequest
            CreateMap<Teacher, TeacherGetRequest>().
                ForMember(dest => dest.AccountId, opt => opt.MapFrom(teacher => teacher.Account!.Id)).
                ForMember(dest => dest.Email, opt => opt.MapFrom(teacher => teacher.Account!.Email)).
                ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(teacher => teacher.Account!.PhoneNumber));

            // Subject
            // SubjectAddRequest => Subject
            CreateMap<SubjectAddRequest, Subject>();
            // Subject => SubjectGetRequest
            CreateMap<Subject, SubjectGetRequest>();

            // Classroom
            // Classroom => ClassroomGetRequest
            CreateMap<Classroom, ClassroomGetRequest>().
                ForMember(dest => dest.SubjectName, opt => opt.MapFrom(classroom => classroom.Subject!.Name));
            // ClassroomAddRequest => Classroom
            CreateMap<ClassroomAddRequest, Classroom>();

            // Student_Classroom
            // Student_Classroom => Student_ClassroomGetRequest
            CreateMap<Student_Classroom, Student_ClassroomGetRequest>().
                ForMember(dest => dest.StudentName, opt => opt.MapFrom(sc => sc.Student!.Name)).
                ForMember(dest => dest.ClassroomName, opt => opt.MapFrom(sc => sc.Classroom!.Name));
            // Student_ClassroomAddRequest => Student_Classroom
            CreateMap<Student_ClassroomAddRequest, Student_Classroom>();

            // Teacher_Classroom
            // Teacher_Classroom => Teacher_ClassroomGetRequest
            CreateMap<Teacher_Classroom, Teacher_ClassroomGetRequest>().
                ForMember(dest => dest.TeacherName, opt => opt.MapFrom(tc => tc.Teacher!.Name)).
                ForMember(dest => dest.ClassroomName, opt => opt.MapFrom(tc => tc.Classroom!.Name));
            // Teacher_ClassroomAddRequest => Teacher_Classroom
            CreateMap<Teacher_ClassroomAddRequest, Teacher_Classroom>();
        }
    }
}
