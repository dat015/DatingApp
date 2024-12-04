import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'] // Sửa từ "styleUrl" thành "styleUrls"
})
export class RegisterComponent {
  private accountService  = inject(AccountService);

  // Sử dụng @Output() để gửi dữ liệu hoặc sự kiện về component cha
  @Output() cancelRegister = new EventEmitter<boolean>();

  model: any = {};

  register() {
    this.accountService.register(this.model).subscribe({
        next : response =>{
          console.log(response);
          this.cancel();
        },
        error : error => console.log(error)
    });
  }

  cancel() {
    this.cancelRegister.emit(false); // Phát sự kiện hủy về component cha
  }
}
