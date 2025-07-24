
using AcademiaHub.Helpers;
using AcademiaHub.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AcademiaHub.Data
{
    public class AcademiaHubDbContext : IdentityDbContext
    {
        public AcademiaHubDbContext(DbContextOptions<AcademiaHubDbContext> options) : base(options)
        {
            
        }

        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Form_Questions> Form_Questions { get; set; }
        public DbSet<FormDetails> FormDetails { get; set; }
        public DbSet<Models.Domain.FormType> FormTypes { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialType> MaterialTypes { get; set; }
        public DbSet<QBAnswers> QBAnswers { get; set; }
        public DbSet<QuestionBank> questionBank { get; set; }
        public DbSet<QuestionsForm> QuestionsForms { get; set; }
        public DbSet<Models.Domain.QuestionType> QuestionTypes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Student_Classroom> Student_Classrooms { get; set; }
        public DbSet<Student_QuestionsForm> Student_QuestionsForms { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Teacher_Classroom> Teacher_Classrooms { get; set; }
        public DbSet<FormStudentAnswers> FormStudentAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Form_Questions
            builder.Entity<Form_Questions>(entity =>
            {
                entity.HasKey(f => new { f.FormId, f.QuestionId });

                entity.HasOne(f => f.Question)
                .WithMany(q => q.Form_Questions)
                .HasForeignKey(f => f.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(f => f.Form)
                .WithMany(f => f.Form_Questions)
                .HasForeignKey(f => f.FormId)
                .OnDelete(DeleteBehavior.NoAction);
            });

            // Student_Classroom
            builder.Entity<Student_Classroom>(entity =>
            {
                entity.HasKey(sc => new { sc.StudentId, sc.ClassroomId });

                entity.HasOne(sc => sc.Student).
                WithMany(s => s.Student_Classrooms).
                HasForeignKey(sc => sc.StudentId);

                entity.HasOne(sc => sc.Classroom).
                WithMany(c => c.Student_Classrooms).
                HasForeignKey(sc => sc.ClassroomId);
            });

            // Student_QuestionsForm
            builder.Entity<Student_QuestionsForm>(entity =>
            {
                entity.HasKey(sq => new { sq.StudentId, sq.QuestionsFormId });

                entity.HasOne(sq => sq.Studnet).
                WithMany(s => s.Student_QuestionsForms).
                HasForeignKey(sq => sq.StudentId);

                entity.HasOne(sq => sq.QuestionsForm).
                WithMany(f => f.Student_QuestionsForms).
                HasForeignKey(sq => sq.QuestionsFormId);
            });

            // FormStudentAnswers
            builder.Entity<FormStudentAnswers>(entity =>
            {
                entity.HasKey(e => new {e.StudentId, e.FormId, e.QuestionId });

                entity.HasOne(fsa => fsa.Student).
                WithMany(s => s.FormStudentAnswers).
                HasForeignKey(fsa => fsa.StudentId);

                entity.HasOne(fsa => fsa.Question).
                WithMany(q => q.FormStudentAnswers).
                HasForeignKey(fsa => fsa.QuestionId);

                entity.HasOne(fsa => fsa.QuestionsForm).
                WithMany(f => f.FormStudentAnswers).
                HasForeignKey(fsa => fsa.FormId);
            });

            // Seeding Roles
            var studentRoleId = "26c2b87c-bd59-48aa-87b6-34b414f8d12e";
            var teacherRoleId = "f637afd6-a47d-44fc-84bc-fdbca6ed2e4d";
            var adminRoleId = "b985b240-2dce-4365-bcf5-c4c792b9076b";
            List<IdentityRole> roles = new List<IdentityRole>()
            {
                new IdentityRole
                {
                    Id = studentRoleId,
                    ConcurrencyStamp = studentRoleId,
                    Name = Helpers.Roles.Student.ToString(),
                    NormalizedName = Helpers.Roles.Student.ToString().ToUpper()
                },
                new IdentityRole
                {
                    Id = teacherRoleId,
                    ConcurrencyStamp = teacherRoleId,
                    Name = Helpers.Roles.Teacher.ToString(),
                    NormalizedName = Helpers.Roles.Teacher.ToString().ToUpper()
                },
                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = Helpers.Roles.Admin.ToString(),
                    NormalizedName = Helpers.Roles.Admin.ToString().ToUpper()
                },
            };

            builder.Entity<IdentityRole>().HasData(roles);

            // Seeding Difficulty
            List<Difficulty> difficulties = new List<Difficulty>
            {
                new Difficulty
                {
                    Id = (int) DifficultyLevel.Hard,
                    DifficultyLevel = DifficultyLevel.Hard.ToString()
                },

                new Difficulty
                {
                    Id = (int) DifficultyLevel.Medium,
                    DifficultyLevel = DifficultyLevel.Medium.ToString()
                },

                new Difficulty
                {
                    Id = (int) DifficultyLevel.Easy,
                    DifficultyLevel = DifficultyLevel.Easy.ToString()
                }
            };

            builder.Entity<Difficulty>(entity =>
            {
                entity.Property(d => d.Id).ValueGeneratedNever();
                entity.HasData(difficulties);
            });

            // Seeding FormTypes
            List<Models.Domain.FormType> formTypes = new List<Models.Domain.FormType>
            {
                new Models.Domain.FormType
                {
                    Id = (int) Helpers.FormType.Assignment,
                    Type = Helpers.FormType.Assignment.ToString()
                },

                new Models.Domain.FormType
                {
                    Id = (int) Helpers.FormType.Quiz,
                    Type = Helpers.FormType.Quiz.ToString()
                }
            };

            builder.Entity<Models.Domain.FormType>(entity =>
            {
                entity.Property(ft => ft.Id).ValueGeneratedNever();
                entity.HasData(formTypes);
            });

            // Seeding QuestionTypes
            List<Models.Domain.QuestionType> questionTypes = new List<Models.Domain.QuestionType>
            {
                new Models.Domain.QuestionType
                {
                    Id = (int) Helpers.QuestionType.MultipleChoice,
                    Type = Helpers.QuestionType.MultipleChoice.ToString()
                },

                new Models.Domain.QuestionType
                {
                    Id = (int) Helpers.QuestionType.Essay,
                    Type = Helpers.QuestionType.Essay.ToString()
                },

                new Models.Domain.QuestionType
                {
                    Id = (int) Helpers.QuestionType.TrueOrFalse,
                    Type = Helpers.QuestionType.TrueOrFalse.ToString()
                },

            };

            builder.Entity<Models.Domain.QuestionType>(entity =>
            {
                entity.Property(qt => qt.Id).ValueGeneratedNever();
                entity.HasData(questionTypes);
            });

            
        }
    }
}
