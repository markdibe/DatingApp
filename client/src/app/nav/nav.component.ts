import { ChangeDetectionStrategy, Component, OnInit, ViewEncapsulation } from '@angular/core';
import { async } from '@angular/core/testing';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
  encapsulation:ViewEncapsulation.Emulated
  
})
export class NavComponent implements OnInit {

  model: any = {};

  constructor(public accountService: AccountService
    ,private router:Router
    ,private toastr:ToastrService) {

  }

  ngOnInit(): void {
    
  }

  login() {
    this.accountService.login(this.model).subscribe((response)=>{
      this.router.navigateByUrl('/members');
    },
    (error)=>{
      this.toastr.error(error.error);
    });
  }
  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

}