import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { APP_CONFIG } from 'src/app/main/configs/environment.config';
import { MissionService } from 'src/app/main/services/mission.service';
import { HeaderComponent } from '../header/header.component';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { FormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgFor, NgIf, NgStyle } from '@angular/common';
import { FilterPipe } from 'src/app/main/pipes/filter.pipe';
import { Subscription } from 'rxjs';
declare var window:any;
@Component({
  selector: 'app-missiontheme',
  standalone: true,
  imports: [SidebarComponent, HeaderComponent, FormsModule, RouterModule, NgxPaginationModule, NgStyle, NgIf, NgFor, FilterPipe],
  templateUrl: './missiontheme.component.html',
  styleUrls: ['./missiontheme.component.css'],
})
export class MissionthemeComponent implements OnInit, OnDestroy {
  missionThemeList: any[] = [];
  page: number = 1;
  itemsPerPages: number = 10;
  searchText: any;
  themeId: any;
  deleteThemeModal:any;
  private unsubscribe: Subscription[] = [];
  
  constructor(
    private _service: MissionService,
    private _router: Router,
    private _toast: NgToastService
  ) {}

  ngOnInit(): void {
    this.getMissionThemeList();
    this.deleteThemeModal = new window.bootstrap.Modal(
      document.getElementById('removemissionThemeModal')
    );
  }
  getMissionThemeList() {
    const missionThemeSubscribe = this._service.missionThemeList().subscribe(
      (data: any) => {
        if (data.result == 1) {
          this.missionThemeList = data.data;
        } else {
          this._toast.error({ summary: data.message, duration: APP_CONFIG.toastDuration });
        }
      },
      (err) => this._toast.error({ summary: err.message, duration: APP_CONFIG.toastDuration })
    );
    this.unsubscribe.push(missionThemeSubscribe);
  }
  openRemoveMissionThemeModal(id:any){
    this.themeId = id;
    this.deleteThemeModal.show();
  }
  closeRemoveMissionThemeModal(){
    this.deleteThemeModal.hide();
  }
  // deleteMissionTheme() {
  //   const deleteMissionThemeSubscribe = this._service.deleteMissionTheme(this.themeId).subscribe(
  //     (data: any) => {
  //       if (data.result == 1) {
  //         this._toast.success({detail: 'SUCCESS',summary: data.data,duration: APP_CONFIG.toastDuration});
  //         this.closeRemoveMissionThemeModal();
  //         setTimeout(() => {
  //           this._router.navigate(['admin/missionTheme']);
  //         }, 1000);
  //       } else {
  //         this._toast.error({ summary: data.message, duration: APP_CONFIG.toastDuration });
  //       }
  //     },
  //     (err) => this._toast.error({ summary: err.message, duration: APP_CONFIG.toastDuration })
  //   );
  //   this.unsubscribe.push(deleteMissionThemeSubscribe);
  // }
  deleteMissionTheme() {
  console.log('Deleting Theme ID:', this.themeId); // ✅ Check if ID is set

  this._service.deleteMissionTheme(this.themeId).subscribe({
    next: (res) => {
      console.log('Delete Success:', res);
      this._toast.success({
        detail: 'Success',
        summary: 'Theme deleted successfully',
        duration: 3000,
      });
      this.closeRemoveMissionThemeModal();
      this.getMissionThemeList(); // ✅ Refresh list
    },
    error: (err) => {
      console.error('Delete Error:', err); // ✅ See this in browser console
      this._toast.error({
        detail: 'Error',
        summary: 'Failed to delete theme',
        duration: 3000,
      });
    },
  });
}

  ngOnDestroy() {
    this.unsubscribe.forEach((sb) => sb.unsubscribe());
  }
}
