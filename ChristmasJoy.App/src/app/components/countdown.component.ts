import { Component, 
         OnInit, 
         OnDestroy, 
         Input, 
         ElementRef,
         ChangeDetectorRef,
         ChangeDetectionStrategy } from '@angular/core';
import { Observable, Subscription } from 'rxjs'; 
@Component({
    selector: 'count-down',
    template: `
    <span *ngIf="message"><b>{{message.daysLeft}}</b>d <b>{{message.hoursLeft}}</b>h <b>{{message.minLeft}}</b>m <b>{{message.secLeft}}</b>s </span>
    `
  })
export class CountdownComponent implements OnInit, OnDestroy{
    @Input() dateString: string;
    private date: Date;
    private diff: number;
    private $counter: Observable<number>;
    private subscription: Subscription;
    public message: any;
    
    constructor() {
    }
    dhms(t) {
        var days, hours, minutes, seconds;
        days = Math.floor(t / 86400);
        t -= days * 86400;
        hours = Math.floor(t / 3600) % 24;
        t -= hours * 3600;
        minutes = Math.floor(t / 60) % 60;
        t -= minutes * 60;
        seconds = t % 60;

        return {
            daysLeft: days,
            hoursLeft: hours,
            minLeft : minutes,
            secLeft: seconds
        }
    }

    ngOnInit() {
        this.date = new Date(this.dateString);
        this.$counter = Observable.interval(1000).map((x) => {
            this.diff = Math.floor((this.date.getTime() - new Date().getTime()) / 1000);
            return x;
        });

        this.subscription = this.$counter.subscribe((x) =>
        { 
            this.message = this.dhms(this.diff);
        });
    }

    ngOnDestroy(): void {
        this.subscription.unsubscribe();
    }
}