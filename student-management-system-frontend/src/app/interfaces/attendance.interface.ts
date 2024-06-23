export interface IAttendance {
    classId: number;
    date: string;
    id: number;
    isPresent: boolean;
    name: string;
    subjectName: string;
}

export interface IAttendanceRecord {
    studentId: number;
    isPresent: boolean;
}

export interface IAttendanceData {
    date: string;
    attendances: IAttendanceRecord[];
}