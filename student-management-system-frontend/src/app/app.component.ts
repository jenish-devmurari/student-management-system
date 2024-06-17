import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  constructor(private toastr: ToastrService) { }

  ngOnInit(): void {
    this.toastr.success("welcome ")
  }

  showAlert() {
    Swal.fire({
      title: 'Success!',
      text: 'SweetAlert2 is working',
      icon: 'success',
      confirmButtonText: 'Cool'
    });
  }

}
