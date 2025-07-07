import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent {
  user = {
    name: '',
    email: ''
  };

  submitted = false;

  submitForm() {
    this.submitted = true;
    console.log('Form Submitted:', this.user);
  }
}
