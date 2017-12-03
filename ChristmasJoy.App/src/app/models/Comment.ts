export class Comment{
    id: string;
    fromUserId: number;
    content: string;
    commentType: number; //0-Negative, 1-Positive
    toUserId: number;
    commentDate: Date;
    Likes: number[]; //array of user ids
}
