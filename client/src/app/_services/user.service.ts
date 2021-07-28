import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = "https://localhost:44336/api/"
  constructor(private http:HttpClient) 
  { }
  getUsers(){
      return this.http.get(this.baseUrl+"users/get");
  }
}
