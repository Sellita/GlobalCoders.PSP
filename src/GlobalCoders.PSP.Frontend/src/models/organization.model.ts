import { WorkingSchedule } from './WorkingSchedule.model';

export interface Organization {
    displayName: string;
    legalName: string;
    address: string;
    email: string;
    mainPhoneNumber: string;
    secondaryPhoneNumber: string;
    workingSchedule: WorkingSchedule[];
    id: string;
}