import { TaskEntity } from "./task.model";

export interface ColumnEntity {
    id?: string,
    title?: string,
    boardId?: string,
    tasks?: TaskEntity[],
    order?: string
}