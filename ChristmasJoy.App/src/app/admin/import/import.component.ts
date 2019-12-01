import { Component } from "@angular/core";
import { UsersService } from "../../services/users.service";

@Component({
    selector: 'import-dashboard',
    templateUrl: './import.component.html',
    styleUrls: ['./import.component.css']
  })
  export class ImportComponent {
    password: string;
    errorMessage: string;
    alertMessage: string;
    fileToUpload: File = null;

    constructor(private usersService: UsersService)
    {
    this.errorMessage = "";
    }

    handleFileInput(files: FileList) {
        this.fileToUpload = files.item(0);
    }

    uploadUsers() {
        this.usersService.uploadFile(this.fileToUpload, this.password).subscribe(result => {
            this.alertMessage ="uploaded users count = " + result.userCount;
          // do something, if upload success
          }, error => {
            console.log(error);
          });
      }
  }