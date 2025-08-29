export interface BoardEntity {
    id: string,
    title: string,
    inviteCode?: string,
    isOwner?: boolean,
    ownerId?: string,
}