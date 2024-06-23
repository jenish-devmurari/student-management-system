using Repository.Interfaces;
using Repository.Modals;
using Service.DTOs;
using Service.Interfaces;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Service.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IEmailService _emailService;

        public TeacherService(ITeacherRepository teacherRepository, IStudentRepository studentRepository, IAttendanceRepository attendanceRepository, IEmailService emailService, IGradeRepository gradeRepository, IUserRepository userRepository)
        {
            _teacherRepository = teacherRepository;
            _studentRepository = studentRepository;
            _attendanceRepository = attendanceRepository;
            _emailService = emailService;
            _gradeRepository = gradeRepository;
            _userRepository = userRepository;
        }


        #region Get All Students list of teacher
        public async Task<ResponseDTO> GetAllStudentOfTeacherClass(int userId)
        {
            try
            {
                Teachers teacher = await _teacherRepository.GetTeacherDetailsByIdAsync(userId);

                if (teacher == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "Teacher Not Found"
                    };
                }

                List<Students> students = await _studentRepository.GetAllStudentByTeacherClass((int)teacher.ClassId);

                var studentDetailDTOs = students.Select(student => new StudentDetailDTO
                {
                    Id = student.Users.UserId,
                    StudentId = student.StudentId,
                    Name = student.Users.Name,
                    Email = student.Users.Email,
                    ClassId = student.ClassId,
                    RollNumber = student.RollNumber,
                    DateOfBirth = student.Users.DateOfBirth,
                    DateOfEnrollment = student.Users.DateOfEnrollment,
                    IsActive = student.Users.IsActive,
                }).ToList();

                return new ResponseDTO
                {
                    Status = 200,
                    Data = studentDetailDTOs,
                    Message = "Get all student data of teacher class."
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
        #endregion

        #region MarkAttendance
        public async Task<ResponseDTO> MarkAttendance(StudentListAttendanceDTO attendanceDTOs, int userId)
        {
            try
            {
                Teachers teacher = await _teacherRepository.GetTeacherDetailsByIdAsync(userId);

                // if teacher not found 
                if (teacher == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "Teacher Not Found"
                    };
                }


                // check attendence is already done or not 
                if (await _attendanceRepository.IsAttendenceDone(DateTime.Now, teacher.TeacherId))
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "Attendance is already Done."
                    };
                };

                var attendances = attendanceDTOs.Attendances.Select(a => new Attendance
                {
                    StudentId = a.StudentId,
                    TeacherId = teacher.TeacherId,
                    SubjectId = teacher.SubjectId,
                    ClassId = teacher.ClassId,
                    Date = DateTime.Now,
                    IsPresent = a.IsPresent,
                    CreatedBy = teacher.Users.UserId,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = teacher.Users.UserId,
                    ModifiedOn = DateTime.Now,
                }).ToList();

                await _attendanceRepository.AddAttendancesAsync(attendances);
                List<string> teacherList = await _teacherRepository.GetTeacherEmailsByClassAsync((int)teacher.ClassId);

                foreach (var a in attendanceDTOs.Attendances)
                {
                    if (a.IsPresent == false)
                    {
                        var student = await _studentRepository.GetStudentDetailsByStudentIdAsync(a.StudentId);

                        if (student != null)
                        {
                            // send email who is absent 
                            await _emailService.SendAttendanceNotificationEmailAsync(
                                student.Users.Email,
                                student.Users.Name,
                                a.IsPresent,
                                teacherList
                            );
                        }
                    }
                }

                return new ResponseDTO
                {
                    Status = 200,
                    Message = "Attendance marked successfully."
                };


            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
        #endregion


        #region Attendance History Of Teacher Class 
        public async Task<ResponseDTO> AttendancHistory(int userId)
        {
            try
            {
                Teachers teacher = await _teacherRepository.GetTeacherDetailsByIdAsync(userId);

                if (teacher == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "Teacher Not Found"
                    };
                }

                List<Attendance> attendanceDetails = await _attendanceRepository.GetAttendanceDetailsOfStudent((int)teacher.TeacherId);

                List<StudentAttendanceHistoryDTO> studentAttendanceDTOs = attendanceDetails.Select(attendance => new StudentAttendanceHistoryDTO
                {
                    Id = attendance.id,
                    Name = attendance.Students.Users.Name,
                    Date = attendance.Date,
                    SubjectName = attendance.Subjects.SubjectName,
                    IsPresent = attendance.IsPresent
                }).ToList();

                return new ResponseDTO
                {
                    Status = 200,
                    Data = studentAttendanceDTOs,
                    Message = "Get all student attendance data of teacher class."
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
        #endregion

        #region edit attendance history of student
        public async Task<ResponseDTO> editAttendancHistory(int id, int userId, StudentAttendanceHistoryDTO editattendance)
        {
            try
            {
                Attendance attendance = await _attendanceRepository.GetAttendanceAsync(id);

                if (attendance == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "Attendance Not Found"
                    };
                }

                attendance.IsPresent = editattendance.IsPresent;
                attendance.ModifiedBy = userId;
                attendance.ModifiedOn = DateTime.Now;

                await _attendanceRepository.UpdateAttendanceAsync(attendance);

                return new ResponseDTO
                {
                    Status = 200,
                    Data = attendance.id,
                    Message = "successfully edit attendance of student"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
        #endregion

        #region AddMarks of student by teacher 
        public async Task<ResponseDTO> AddMarks(StudentMarksDTO marksDetails, int userId)
        {
            try
            {

                // Validate email format before checking if it exists
                if (string.IsNullOrEmpty(marksDetails.Email?.Trim()) ||
                    !Regex.IsMatch(marksDetails.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "Invalid email format."
                    };
                }

                Users user = await _userRepository.GetUserByEmailAsync(marksDetails.Email);

                if (user == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "User not found."
                    };
                }

                var teacher = await _teacherRepository.GetTeacherDetailsByIdAsync(userId);

                if (teacher == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "Teacher not found."
                    };
                }

                if (user.Student == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "Student not found."
                    };
                }

                if (marksDetails.Marks > marksDetails.TotalMarks)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "Marks is not greter than Total marks"
                    };
                }

                Grades grades = new Grades
                {
                    StudentId = user.Student.StudentId,
                    TeacherId = teacher.TeacherId,
                    Marks = marksDetails.Marks,
                    TotalMarks = marksDetails.TotalMarks,
                    Date = marksDetails.Date,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.Now,
                };

                await _gradeRepository.AddMarks(grades);

                // email to student about their grade 
                await _emailService.SendGradeNotificationEmailAsync(user.Email, user.Name, teacher.SubjectId, marksDetails.Marks, false);

                return new ResponseDTO
                {
                    Status = 200,
                    Message = "Marks added successfully."
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
        #endregion

        #region Get All Student Grades Detail
        public async Task<ResponseDTO> GetAllStudentGrades(int userId)
        {
            try
            {
                Teachers teacher = await _teacherRepository.GetTeacherDetailsByIdAsync(userId);

                if (teacher == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "Teacher not found."
                    };
                }

                List<Grades> studentMarksDTOs = await _gradeRepository.GetAllStudentGradesOfTeacherSubject(teacher.TeacherId);

                List<StudentMarksDTO> studentMarks = studentMarksDTOs.Select(grade => new StudentMarksDTO
                {
                    userId = grade.Students.Users.UserId,
                    GradeId = grade.id,
                    Email = grade.Students.Users.Email,
                    SubjectId = grade.Teachers.SubjectId,
                    Marks = grade.Marks,
                    TotalMarks = grade.TotalMarks,
                    Date = grade.Date
                }).ToList();

                return new ResponseDTO
                {
                    Status = 200,
                    Data = studentMarks,
                    Message = "Student marks retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }


        }


        #endregion

        #region Update Grades of Student
        public async Task<ResponseDTO> UpdateStudentGrades(StudentMarksDTO updateMarks, int userId)
        {
            try
            {

                Grades gradesDetails = await _gradeRepository.GetGradeDetailsByID(updateMarks.GradeId);

                if (gradesDetails == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "Grades are not found."
                    };
                }

                gradesDetails.Marks = updateMarks.Marks;
                gradesDetails.TotalMarks = updateMarks.TotalMarks;
                gradesDetails.ModifiedBy = userId;
                gradesDetails.ModifiedOn = DateTime.Now;

                await _gradeRepository.UpdateGrades(gradesDetails);




                return new ResponseDTO
                {
                    Status = 200,
                    Data = gradesDetails.id,
                    Message = "Grades are update successfully."
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
        #endregion


        #region get student grade details of teacher subject
        public async Task<ResponseDTO> GetStudentGradeDetailsByStudentID(int studentUserID, int userID)
        {
            try
            {

                // retrive student grade for particular teacher
                List<Grades> gradesDetails = await _gradeRepository.GetGradeDetailsByUserID(studentUserID, userID);

                if (gradesDetails == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "Grades are not found."
                    };
                }

                List<StudentMarksDTO> studentMarks = gradesDetails.Select(grade => new StudentMarksDTO
                {
                    userId = studentUserID,
                    GradeId = grade.id,
                    Marks = grade.Marks,
                    TotalMarks = grade.TotalMarks,
                    Date = grade.Date
                }).ToList();

                return new ResponseDTO
                {
                    Status = 200,
                    Data = studentMarks,
                    Message = "Grades are retrive successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
        #endregion


        #region particular month's date attendance for teacher
        public async Task<ResponseDTO> GetAttendanceDetailsByDate(DateTime date, int userId)
        {
            try
            {
                Teachers teacher = await _teacherRepository.GetTeacherDetailsByIdAsync(userId);

                if (teacher == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "Teacher Not Found"
                    };
                }

                List<Attendance> attendanceDetails = await _attendanceRepository.GetAttendanceDetailsByDate(date, teacher.TeacherId);

                if (attendanceDetails == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "No attendance found for this date"
                    };
                }

                List<StudentAttendanceHistoryDTO> studentAttendanceDTOs = attendanceDetails.Select(attendance => new StudentAttendanceHistoryDTO
                {
                    Id = attendance.id,
                    Name = attendance.Students.Users.Name,
                    Date = attendance.Date,
                    SubjectName = attendance.Subjects.SubjectName,
                    IsPresent = attendance.IsPresent
                }).ToList();

                return new ResponseDTO
                {
                    Status = 200,
                    Data = studentAttendanceDTOs,
                    Message = "Get all student attendance data of teacher class of specific date."
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        #endregion


        #region get student email list based on search 
        public async Task<ResponseDTO> GetStudentEmailList(string searchEmail, int userId)
        {
            try
            {
                Teachers teacher = await _teacherRepository.GetTeacherDetailsByIdAsync(userId);

                if (teacher == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "Teacher Not Found"
                    };
                }

                List<string> emailList= await _userRepository.GetStudentEmails(searchEmail,(int)teacher.ClassId);


                if (emailList == null)
                {
                    return new ResponseDTO
                    {
                        Status = 404,
                        Message = "No student found"
                    };
                }

                return new ResponseDTO
                {
                    Status = 200,
                    Data = emailList,
                    Message = "Get all student emails based on search."
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
            #endregion
        }
    }
}
