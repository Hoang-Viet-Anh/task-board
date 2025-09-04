import { Log } from "./log.model";
import { UserEntity } from "./user.model";

export interface TaskEntity {
    id?: string,
    columnId?: string,
    title?: string,
    description?: string,
    dueDate?: Date,
    priority?: string,
    assignedUsers?: UserEntity[],
    taskActivityLogs?: Log[]
}