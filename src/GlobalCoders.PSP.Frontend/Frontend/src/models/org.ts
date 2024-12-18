import { Timetable } from "./timetable";

export interface Org {
    id: string;
    displayName: string;
    legalName: string;
    address: string;
    email: string;
    mainPhoneNumber: string;
    secondaryPhoneNumber: string;
    workingSchedule: Timetable[];
}
