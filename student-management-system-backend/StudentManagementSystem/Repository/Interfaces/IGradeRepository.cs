using Repository.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IGradeRepository
    {
        Task AddMarks(Grades grades);

        Task<List<Grades>> GetAllStudentGradesOfTeacherSubject(int teacherId);

        Task<Grades> GetGradeDetailsByID(int id);
        Task UpdateGrades(Grades grades);

        Task<List<Grades>> GetGradeDetailsByUserID(int studentUserId, int subjectId);

        Task<List<Grades>> StudentGradesBySubject(int studentId, int subjectId);


    }
}
