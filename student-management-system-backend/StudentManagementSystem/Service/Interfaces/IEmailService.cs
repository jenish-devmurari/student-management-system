using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailToStudentAsync(StudentRegisterDTO student);

        Task SendEmailTeacherAsync(string toEmail);

        Task SendGradeNotificationEmailAsync(string toEmail, string studentName, int subjectId, int grade, bool isedit);

        Task SendAttendanceNotificationEmailAsync(string studentEmail, string studentName, bool isAbsent, List<string> ccEmails = null);
    }
}
