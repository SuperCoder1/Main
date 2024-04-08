export interface Report {
 id: string;
 thumbnailUrl: string;
 name: string;
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Report } from './report.interface';

@Injectable({
 providedIn: 'root'
})
export class ReportService {
 private reportsUrl = 'path/to/your/report.json';

 constructor(private http: HttpClient) {}

 getReports(): Observable<Report[]> {
  return this.http.get<Report[]>(this.reportsUrl);
 }
}

import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Report } from '../report.interface';
import { ReportService } from '../report.service';

@Component({
 selector: 'app-report-list',
 templateUrl: './report-list.component.html',
 styleUrls: ['./report-list.component.css']
})
export class ReportListComponent implements OnInit {
 reports: Report[] = [];

 constructor(private reportService: ReportService, private router: Router) {}

 ngOnInit(): void {
  this.getReports();
 }

 getReports(): void {
  this.reportService.getReports().subscribe(reports => {
   this.reports = reports;
  });
 }

 goToReport(report: Report): void {
  this.router.navigate(['/reports', report.id]);
 }
}

<div class="report-card-container" fxLayout="row wrap" fxLayout.xs="column" fxLayoutAlign="start stretch" fxLayoutGap="16px">
 <div class="report-card" *ngFor="let report of reports" (click)="goToReport(report)">
  <div class="report-header" fxLayout="row" fxLayoutAlign="space-between center">
   <div class="report-name">{{ report.name }}</div>
  </div>
  <div class="report-thumbnail">
   <img [src]="report.thumbnailUrl" alt="Report Thumbnail">
  </div>
 </div>
</div>

.report-card-container {
 width: 100%;
}

.report-card {
 border: 1px solid #ddd;
 border-radius: 4px;
 overflow: hidden;
 cursor: pointer;
 transition: box-shadow 0.3s ease;
}

.report-card:hover {
 box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.report-header {
 padding: 12px;
 background-color: #f5f5f5;
}

.report-name {
 font-size: 16px;
 font-weight: bold;
}

.report-thumbnail {
 padding: 12px;
}

.report-thumbnail img {
 width: 100%;
 height: auto;
}
