import { Component, Input, OnDestroy, OnInit } from '@angular/core';

import { AuthService } from '../services/auth.service';
import { Comment } from '../models/Comment';
import { CommentsService } from '../services/comments.service';
import { Subscription } from 'rxjs';
import { UserStatus } from  '../models/UserStatus';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'feedback-panel',
  templateUrl: './feedback.component.html',
  styleUrls: ['./feedback.component.css']
})
export class FeedbackComponent implements OnInit, OnDestroy {
    @Input() userId: number;
    public currentUserId: number;
    private userSubscription: Subscription;
    public userComments: Comment[];
    public usersStatusMap: any;
    public isPersonal: boolean = false;
    public errorMessage: string = '';
    public loadingComments: boolean = false;
    public loadingStatuses: boolean = false;
    public mySentComments: number = 0;
    public newComment: Comment;
    public isSavingComment: boolean= false;
    public isLikingComment: boolean = false;

  constructor(private authSrv: AuthService,
      private commService: CommentsService,
      public usersService: UsersService) {

   }

   ngOnInit() {
     this.getUsersStatusMap();
    this.userSubscription = this.authSrv.authCurrentUser$
    .subscribe(user => {
      if (user) {
        this.currentUserId = user.Id;
        this.isPersonal = (user.Id === this.userId);
      }
      this.getComments();
    });
   }

   getComments() {
     this.loadingComments = true;
     this.commService.getComments(this.userId, this.isPersonal)
     .subscribe(
       response => {
         if (response && response.data) {
           for (let comm of response.data){
            if (comm.fromUserId === this.currentUserId) {
              this.mySentComments++;
            }
            if (!this.isPersonal) {
              this.userComments = [];
              for (let comm of response.data){
                if (comm.commentType === 1 || (comm.commentType === 0 && comm.fromUserId === this.currentUserId)) {
                  this.userComments.push(comm);
                }
              }
            }else {
              this.userComments = response.data;
            }
          }
         }
         this.loadingComments = false;
       },
       error => {
         this.errorMessage = error;
         this.loadingComments = false;
       }
     );
   }

   addComment() {
    this.newComment = new Comment();
    this.newComment.content = '';
    this.newComment.fromUserId = this.currentUserId;
    this.newComment.likes = [];
    this.newComment.toUserId = this.userId;
   }

   cancelCommentEdit() {
     this.newComment = null;
   }

   getUsersStatusMap() {
     this.loadingStatuses = true;
     this.usersService.getUsersStatusMap()
     .subscribe(
       response => {
         if (response && response.data) {
           this.usersStatusMap = response.data;
         }
         this.loadingStatuses = false;
       },
       error => {
         this.errorMessage = error;
         this.loadingStatuses = false;
       }
     );
   }

   getUserStatus(comment: Comment) {
     return '/assets/images/' + this.usersStatusMap[comment.fromUserId].christmasStatus + '.jpg';
   }

   ngOnDestroy() {
       this.userSubscription.unsubscribe();
   }

   isLoading() {
     return this.loadingComments || this.loadingStatuses;
   }

   likeComment(comment: Comment) {
     this.isLikingComment = true;
     this.commService.likeComment(this.currentUserId, comment.id)
     .subscribe(
       response => {
         this.isLikingComment = false;
         comment.likes.push(this.currentUserId);
        },
       error => {
         this.errorMessage = error;
         this.isLikingComment = false;
        }
     );
   }

   disableLike(comment: Comment) {
    if (this.isLikingComment || comment.likes.indexOf(this.currentUserId) !== -1) {
      return true;
    }
    if (this.currentUserId === comment.fromUserId) {
      return true;
    }

    return false;
   }


   canAddComment() {
      if (this.isPersonal || this.newComment) {
        return false;
      }

      return true;
   }

   saveComment() {
     this.isSavingComment = true;
     this.commService.saveItem(this.newComment)
     .subscribe(
       response => {
        if (response) {
          this.newComment.id = response.id;
          this.newComment.fromUserStatus = response.userStatus;
          this.newComment.commentDate = response.date;
          if (!this.userComments) {
            this.userComments = [];
          }
          this.userComments.unshift(this.newComment);
          this.newComment = null;
          this.mySentComments++;
          this.isSavingComment = false;
        }
       },
       error => {
         this.errorMessage = error;
         this.isSavingComment = false;
       }
     );
   }
}
