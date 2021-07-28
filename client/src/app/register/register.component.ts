import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

import { HomeComponent } from '../home/home.component';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  model: any = {};
  //receiving from parent
  // @Input() usersFromHomeComponent: any;
  @Output() cancelRegister = new EventEmitter();

  constructor(private accountService: AccountService
    , private toastr:ToastrService) { }

  ngOnInit(): void {
  }

  register() {
    this.accountService.register(this.model).subscribe((response) => {
      console.log(response);
      this.cancel();
    }, (error) => {
      console.error(error.error);
      this.toastr.error(error.error);
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }


  /**
   * to send data from child to parent you need 4 things : 
   * 1- output parameter = new RegsiterEvent()
   * 2- a function that emits that event
   * 3- on the parent create a function that receive that event 
   * 4- add this parameter inside the html calling component with $event 
   * example : 
   * in register.component.ts => @Output() cancelRegister = new EventEmitter();  cancel() {this.cancelRegister.emit(false);}
   * in home.component.ts=> cancelRegisterMode(event:boolean){this.mode = event; }
    * 
   */
}
