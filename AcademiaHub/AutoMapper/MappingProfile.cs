using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto.Classroom;
using AcademiaHub.Models.Dto.Difficulty;
using AcademiaHub.Models.Dto.Form_Questions;
using AcademiaHub.Models.Dto.FormDetails;
using AcademiaHub.Models.Dto.FormStudentAnswers;
using AcademiaHub.Models.Dto.FormType;
using AcademiaHub.Models.Dto.QBAnswers;
using AcademiaHub.Models.Dto.QuestionBank;
using AcademiaHub.Models.Dto.QuestionsForm;
using AcademiaHub.Models.Dto.QuestionType;
using AcademiaHub.Models.Dto.Student;
using AcademiaHub.Models.Dto.Student_Classroom;
using AcademiaHub.Models.Dto.Student_QuestionsForm;
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

            // QBAnswers
            // QBAnswers => QBAnswersGetRequest
            CreateMap<QBAnswers, QBAnswersGetRequest>();
            // QBAnswersAddRequest => QBAnswers
            CreateMap<QBAnswersAddRequest, QBAnswers>();

            // QuestionBank
            // QuestionBank => QuestionBankGetRequest
            CreateMap<QuestionBank, QuestionsGetRequest>().
                ForMember(dest => dest.ClassroomName, opt => opt.MapFrom(q => q.Classroom!.Name)).
                ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(q => q.Difficulty!.DifficultyLevel)).
                ForMember(dest => dest.QuestionType, opt => opt.MapFrom(q => q.QuestionType!.Type));
            // QuestionsAddRequest => QuestionBank
            CreateMap<QuestionsAddRequest, QuestionBank>();

            // QuestionType
            // QuestionType => QuestionTypeGetRequest
            CreateMap<QuestionType, QuestionTypeGetRequest>();

            // Difficulty
            // Difficulty => DifficultyGetRequest
            CreateMap<Difficulty, DifficultyGetRequest>();

            // FormType
            // FormType => FormTypeGetRequest
            CreateMap<FormType, FormTypeGetRequest>();

            // QuestionsForm & FormDetails
            CreateMap<QuestionsFormAddRequest, FormDetails>().
                ForMember(dest => dest.Title, opt => opt.MapFrom(f => f.FormDetailsAddRequest.Title)).
                ForMember(dest => dest.StartDate, opt => opt.MapFrom(f => f.FormDetailsAddRequest.StartDate)).
                ForMember(dest => dest.EndDate, opt => opt.MapFrom(f => f.FormDetailsAddRequest.EndDate)).
                ForMember(dest => dest.Description, opt => opt.MapFrom(f => f.FormDetailsAddRequest.Description));
            // QuestionsFormAddRequest => QuestionsForm
            CreateMap<QuestionsFormAddRequest, QuestionsForm>();
            // QuestionsForm => QuestionsFormGetRequest
            CreateMap<QuestionsForm, QuestionsFormGetRequest>().
                ForMember(dest => dest.ClassroomName, opt => opt.MapFrom(f => f.Classroom!.Name)).
                ForMember(dest => dest.FormType, opt => opt.MapFrom(f => f.FormType!.Type)).
                ForMember(dest => dest.Duration,
                         opt => opt.MapFrom(f => (f.FormDetails!.EndDate - f.FormDetails.StartDate).TotalMinutes))
                .ForPath(dest => dest.FormDetailsGetRequest.Id,
                 opt => opt.MapFrom(f => f.FormDetails!.Id))
                .ForPath(dest => dest.FormDetailsGetRequest.Title,
                 opt => opt.MapFrom(f => f.FormDetails!.Title))
                .ForPath(dest => dest.FormDetailsGetRequest.StartDate,
                 opt => opt.MapFrom(f => f.FormDetails!.StartDate))
                .ForPath(dest => dest.FormDetailsGetRequest.EndDate,
                 opt => opt.MapFrom(f => f.FormDetails!.EndDate))
                .ForPath(dest => dest.FormDetailsGetRequest.Description,
                 opt => opt.MapFrom(f => f.FormDetails!.Description));


            // FormStudentAnswers
            // FormStudentAnswersUploadRequest => FormStudentAnswer
            //CreateMap<FormStudentAnswerUploadRequest, FormStudentAnswers>();
            // FormStudentAnswers => FormQuestionsGetRequest
            CreateMap<FormStudentAnswers, FormQuestionsGetRequest>().
                ForMember(dest => dest.QuestionTypeId, opt => opt.MapFrom(f => f.Question!.QuestionTypeId)).
                ForMember(dest => dest.QuestionText, opt => opt.MapFrom(f => f.Question!.QuestionText)).
                ForMember(dest => dest.QuestionType, opt => opt.MapFrom(f => f.Question!.QuestionType!.Type))
                .AfterMap((src, dest) =>
                {
                    List<QBAnswers> answers = src.Question!.Answers!.ToList();
                    foreach (var answer in answers)
                    {
                        dest.Choices.Add(answer.Text ?? string.Empty);
                    }
                });
            

            // FormDetails => FormDetailsGetRequest
            CreateMap<FormDetails, FormDetailsGetRequest>();
            

            // Form_Questions
            // FormQuestionsAddRequest => Form_Questions 
            CreateMap<FormQuestionsAddRequest, Form_Questions>();
            // FormQuestions => FormQuestionsGetRequest
            CreateMap<Form_Questions, FormQuestionsGetRequest>().
                ForMember(dest => dest.QuestionTypeId, opt => opt.MapFrom(f => f.Question!.QuestionTypeId)).
                ForMember(dest => dest.QuestionText, opt => opt.MapFrom(f => f.Question!.QuestionText)).
                ForMember(dest => dest.QuestionType, opt => opt.MapFrom(f => f.Question!.QuestionType!.Type)).
                AfterMap((src, dest) =>
                {
                    List<QBAnswers> answers = src.Question!.Answers!.ToList();
                    foreach (var answer in answers)
                    {
                        dest.Choices.Add(answer.Text ?? string.Empty);
                    }
                });

            // Student_QuestionsForm
            // Student_QuestionsForm => Student_QuestionsFormGetRequest
            CreateMap<Student_QuestionsForm, Student_QuestionsFormGetRequest>().
                ForMember(dest => dest.StudentName, opt => opt.MapFrom(e => e.Studnet!.Name)).
                ForMember(dest => dest.FormTitle, opt => opt.MapFrom(e => e.QuestionsForm!.FormDetails!.Title)).
                ForMember(dest => dest.FormType, opt => opt.MapFrom(e => e.QuestionsForm!.FormType!.Type));

            CreateMap<Student_QuestionsFormAddRequest, Student_QuestionsForm>();
        }
    }
}
