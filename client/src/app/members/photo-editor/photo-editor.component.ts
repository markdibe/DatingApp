import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Photo } from 'src/app/_models/photo';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {

  @Input() member: Member;
  @ViewChild('uploadForm') uploadForm: NgForm;
  selectedFile: File;

  baseUrl = environment.apiUrl;
  user: User;


  constructor(private accountService: AccountService,
    private toastr: ToastrService,
    private memberService: MembersService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {

  }
  onFileChange(event: any) {

    const maxSize = 5 * 1024 * 1024;
    const fileList: FileList = event.target.files;
    console.log(fileList);
    if (fileList.length > 0) {
      if (fileList[0].size < maxSize) {
        this.selectedFile = fileList[0];
      }
      else {
        this.toastr.warning("make sure that your file is less than " + maxSize + " MB");
      }
    }
    console.log(this.selectedFile);
  }
  uploadFile() {

    if (this.selectedFile.size > 0) {
      const formData = new FormData();
      formData.append('file', this.selectedFile, this.selectedFile.name)
      this.accountService.addFile(formData).subscribe((photo: Photo) => {
        this.member.photos.push(photo);
      });
    }
  }

  setMainPhoto(photo: Photo) {
    // alert(photo.id);
    this.memberService.setMainPhoto(photo.id).subscribe(() => {
      this.user.photoUrl = photo.url;
      this.accountService.setCurrentUser(this.user);
      this.member.photoUrl = photo.url;
      this.member.photos.forEach((p) => {
        if (p.isMain) { p.isMain = false; }
        if (p.id === photo.id) { p.isMain = true; }
      })
    })
  }

  deletePhoto(photo: Photo) {
    if (photo.isMain) {
      this.toastr.warning("can not delete main photo!");
      return;
    }
    this.memberService.deletePhoto(photo.id).subscribe(() => {
      this.member.photos = this.member.photos.filter(x => x.id != photo.id);
    });
  }


}
