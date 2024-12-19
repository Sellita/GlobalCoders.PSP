import { Timetable } from "./timetable";

export interface User {
    employeeId: string;
    name: string;
    email: string;
    phoneNumber: string;
    role: string;
    isActive: boolean;
    organizationId: string;
    workingSchedule: Timetable[];
}
