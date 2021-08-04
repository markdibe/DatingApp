import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';
import { Observable, ReplaySubject } from 'rxjs';
import { unary } from '@angular/compiler/src/output/output_ast';
import { environment } from 'src/environments/environment';
import { Photo } from '../_models/photo';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }
  //creating a buffer object as replaySubject 
  currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  login(model: any) {
    return this.http.post<User>(this.baseUrl + "account/login", model).pipe(
      map((user: User) => {
        if (user) {
         this.setCurrentUser(user);
        }
      })
    )
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + "account/register", model)
      .pipe(
        map((user: User) => {
          if (user) {
            this.setCurrentUser(user);
          }
          return user;
        }))
  }

  setCurrentUser(user: User) {
    localStorage.setItem("user", JSON.stringify(user));       
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem("user");
    this.currentUserSource.next(undefined);
  }

  addFile(data:FormData){
    let headers = new HttpHeaders();
    headers.append('content-type','multipart/form-data');
    headers.append('accept','application/json');
    const httpOptions = {
      headers:headers
    };
    return this.http.post<Photo>(this.baseUrl+'users/addphoto',data,httpOptions);
  }

}
