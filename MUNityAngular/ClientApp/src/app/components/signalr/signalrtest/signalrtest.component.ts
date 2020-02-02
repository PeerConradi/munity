import { Component, OnInit } from '@angular/core';
import { SignalrtestService } from '../../../services/signalrtest.service';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-signalrtest',
  templateUrl: './signalrtest.component.html',
  styleUrls: ['./signalrtest.component.css']
})
export class SignalrtestComponent implements OnInit {

  constructor(public signalRService: SignalrtestService, private httpClient: HttpClient) { }

  ngOnInit() {
    this.signalRService.startConnection();
    this.signalRService.addMessageDataListener();
    
  }

  sendMessage(): void {
    this.httpClient.get('https://localhost:44395/api/SignalR/PushMessage').subscribe(
      data => { },
      error => { }
    );
  }

  subscribe(): void {
    this.signalRService.subscribeToHub();
  }

  messageGroup(): void {
    this.httpClient.get('https://localhost:44395/api/SignalR/PushGroup').subscribe(
      data => { },
      error => { }
    );
  }

}
