export interface IGradebook {
    classId: number;
    date: string;
    gradeId: number;
    marks: number;
    name: string;
    subjectName: string;
    totalMarks: number;
    userId?: number;
    email?:string;
}