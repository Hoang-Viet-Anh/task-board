import { ColumnEntity } from "@app/features/selected-board/models/column.model";
import { UserEntity } from "@app/features/selected-board/models/user.model";

export interface BoardEntity {
    id: string,
    title: string,
    inviteCode?: string,
    isOwner?: boolean,
    ownerId?: string,
    columns?: ColumnEntity[],
    members?: UserEntity[]
}