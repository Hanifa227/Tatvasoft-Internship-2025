import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MissionComponent } from './mission/mission.component';
import { UserComponent } from './user/user.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,MissionComponent,UserComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'platform';
}
