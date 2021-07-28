import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';
import { ReplaySubject } from 'rxjs';
import { unary } from '@angular/compiler/src/output/output_ast';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = "https://localhost:44336/api/"
  constructor(private http: HttpClient) { }
  //creating a buffer object as replaySubject 
  currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  login(model: any) {
    return this.http.post<User>(this.baseUrl + "account/login", model).pipe(
      map((user: User) => {
        if (user) {
          this.currentUserSource.next(user);
          localStorage.setItem("user", JSON.stringify(user));
        }
      })
    )
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + "account/register", model)
      .pipe(
        map((user: User) => {
          if (user) {
            localStorage.setItem("user", JSON.stringify(user));
            this.currentUserSource.next(user);
          }
          return user;
        }))
  }

  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem("user");
    this.currentUserSource.next(undefined);
  }

}
