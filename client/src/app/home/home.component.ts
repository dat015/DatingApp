import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from '../register/register.component';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  registerMode = false;
  http = inject(HttpClient);
  user: any;

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle(){
    this.registerMode = ! this.registerMode
  }

  cancelRegisterMode(event : boolean){
    this.registerMode = event;
  }

  getUsers() {
    this.http.get('https://localhost:5001/api/user/GetUsers').subscribe({
      next: (response: any) => this.user = response,
      error: (error: any) => console.log(error),
      complete: () => console.log('Request has completed')
    });
  }
}
