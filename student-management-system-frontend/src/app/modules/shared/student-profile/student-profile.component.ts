import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IStudent } from 'src/app/interfaces/student.interface';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-student-profile',
  templateUrl: './student-profile.component.html',
  styleUrls: ['./student-profile.component.scss']
})
export class StudentProfileComponent implements OnInit {
  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }


}
