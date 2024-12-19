import { Timetable } from "./timetable";

export interface User {
    id: string;
    name: string;
    email: string;
    phoneNumber: string;
    role: string;
    isActive: boolean;
    organizationId: string;
    workingSchedule: Timetable[];
}
