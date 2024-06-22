export interface IUser {
    id?: number;
    studentId?: number;
    name: string;
    email: string;
    isActive: boolean;
    dateOfBirth: string;
    dateOfEnrollment: string;
    classId?: number;
    rollNumber?: number;
    role: number
    subjectId?: number;
}