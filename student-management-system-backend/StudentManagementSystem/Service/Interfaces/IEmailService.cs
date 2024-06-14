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
        Task SendEmailToStudentAsync(StudentRegisterDTO student, List<string> ccEmails = null);

        Task SendEmailTeacherAsync(string toEmail);
    }
}
