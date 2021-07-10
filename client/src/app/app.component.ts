import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'client';

  //create a constructor and inject http client service just like dot net core
  constructor(private http: HttpClient) {

   }

  // remove type safety
  users:any;

  ngOnInit() {
    this.getUsers();
  }

  getUsers() {
    this.http.get("https://localhost:44336/api/users/get")
      //subscribe
      .subscribe(
        //case success
        (response) => {
          this.users = response;
        },
        //case error
        (error) => { console.log(error) }
        //finally completed
        , () => {
          console.log("completed")
        });
  }


}

export interface AppUser {
  id: string;
  userName: string;
  normalizedUserName: string;
  email: string;
  normalizedEmail: string;
  emailConfirmed: boolean;
  passwordHash: string;
  securityStamp: string;
  concurrencyStamp: string;
  phoneNumber: string;
  phoneNumberConfirmed: boolean;
  twoFactorEnabled: boolean;
  lockoutEnd: Date;
  lockoutEnabled: boolean;
  accessFailedCount: number;
  firstName: string;
  lastName: string;
  fatherName: string;
  motherName: string;
}
