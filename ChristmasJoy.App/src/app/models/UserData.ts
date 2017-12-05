import { User } from "./User";
import { WishListItem } from "./WishListItem";
import { Comment } from "./Comment";
import { UserStatus } from "./UserStatus";

export class UserData{
    userInfo: User;
    wishList: WishListItem[];
    status: UserStatus;
}