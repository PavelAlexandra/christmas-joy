import { UserStatus } from './UserStatus';

export class Comment{
    id: string;
    fromUserId: number;
    content: string;
    commentType: number; //0-Negative, 1-Positive
    toUserId: number;
    commentDate: Date;
    likes: number[]; //array of user ids
    fromUserStatus: UserStatus;
    isAnonymous: boolean = false;
    
    commentTypeTxt(){
        if(this.commentType == 1)
            return "positive";
        else
            return "negative";
    }
}
