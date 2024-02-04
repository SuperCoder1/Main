
//Angular  Service to call fx app

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class MyService {
  constructor(private http: HttpClient) { }

  getPowerBITokenAndLink() {
    return this.http.get('<Your-Azure-Function-URL>');
  }
}

