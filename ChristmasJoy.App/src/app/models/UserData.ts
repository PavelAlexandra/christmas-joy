import { User } from "./User";
import { WishListItem } from "./WishListItem";
import { Comment } from "./Comment";

export class UserData{
    userInfo: User;
    wishList: WishListItem[];
    receivedComments: Comment[];
}