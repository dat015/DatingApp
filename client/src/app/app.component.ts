import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NgFor } from '@angular/common';
import { NavComponent } from './nav/nav.component'; // Đường dẫn chính xác đến NavComponent

import { AccountService } from './_services/account.service';
import { HomeComponent } from './home/home.component';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'], // Lưu ý tên `styleUrls` phải có 's'
  imports: [RouterOutlet, NgFor, NavComponent, HomeComponent], // Thêm NavComponent ở đây
})
export class AppComponent implements OnInit {
  private accountService = inject(AccountService);
  title = 'DatingApp';
  user: any;

  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser(){
    const  userString = localStorage.getItem('user');
    if(!userString) return;

    const user = JSON.parse(userString);
    this.accountService.currentUser.set(user);
  }

 
}
