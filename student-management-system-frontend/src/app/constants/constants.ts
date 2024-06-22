export const passwordRegex: string = '^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,12}$'
export const emailRegex: string = "^[a-z]{1}[a-z0-9.]+@[a-z0-9]+\.[a-z]{2,6}$"
export const AdminApi = "https://localhost:7080/api/Admin";
export const TeacherApi = "https://localhost:7080/api/Teacher";
export const StudentApi = "https://localhost:7080/api/Student";
export const UserApi = "https://localhost:7080/api/User";