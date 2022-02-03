import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'neo4j-call-in-progress',
  templateUrl: './call-in-progress.component.html',
  styleUrls: ['./call-in-progress.component.scss'],
})
export class CallInProgressComponent implements OnInit {

  @Output() callEnd = new EventEmitter<void>();

  time: string = "calling phone...";

  private min: any = 0;
  private sec: any = 0;
  private stoptime = false;

  constructor() {
    setTimeout(()=>{
      this.timerCycle();
    },5000)
    
  }

  ngOnInit(): void {}

  private timerCycle(): void {
    if (this.stoptime == false) {
      this.sec = parseInt(this.sec);
      this.min = parseInt(this.min);

      this.sec = this.sec + 1;

      if (this.sec == 60) {
        this.min = this.min + 1;
        this.sec = 0;
      }

      if (this.sec < 10 || this.sec == 0) {
        this.sec = '0' + this.sec;
      }
      if (this.min < 10 || this.min == 0) {
        this.min = '0' + this.min;
      }

      this.time = this.min + ':' + this.sec;
      setTimeout(()=>{this.timerCycle()}, 1000);
    }
  }
}
