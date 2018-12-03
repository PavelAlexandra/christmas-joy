import { UserStatus } from './UserStatus';

export class Comment {
    id: string;
    fromUserId: number;
    content: string;
    toUserId: number;
    commentDate: Date;
    likes: number[]; // array of user ids
    fromUserStatus: UserStatus;
    isAnonymous = false;
    isPrivate = false;
}
