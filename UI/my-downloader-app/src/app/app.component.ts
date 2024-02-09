import { HttpClient, HttpClientModule, HttpErrorResponse, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { catchError, concatMap, map, throwError, Observable } from 'rxjs';
import { saveAs } from 'file-saver'
import { CommonHelper } from '../Helpers/CommonHelper';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HttpClientModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})

export class AppComponent implements OnInit {
  title = 'my-downloader-app';

  constructor(private httpClient: HttpClient) { }
  ngOnInit(): void {
    
    //this.exportTSR();
    //this.exportTSROptimally().subscribe();
    this.exportTSROptimally1();
  }

  // New TSR Report
  public exportTSROptimally1() {
    var start = Date.now();
    console.log('--->>> Started file download');

    this.downloadFile().subscribe((data: Blob) => {
      const blob = new Blob([data], { type: 'text/csv' });

      // Create a link element, set its href, and trigger a click to initiate the download
      const link = document.createElement('a');
      link.href = window.URL.createObjectURL(blob);
      link.download = 'downloaded_file.csv';

      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      
      var end = Date.now();
      console.log('--->>> Completed file download. Total time taken (secs): ' + (end - start)/1000);
    })
  }

  public downloadFile(): Observable<Blob> {
    const apiUrl = `https://localhost:7072/api/v1/File/Download_TSR_V3`;
    //const apiUrl = `https://b9b8-103-166-187-181.ngrok-free.app/api/v1/File/DownloadTSROptimally`;
    return this.httpClient.get(apiUrl, { responseType: 'blob' });
  }



/*
  public exportTSROptimally(): Observable<any> {
    let fileName = "optimized-tsr.csv"
    const apiUrl = `https://localhost:7072/api/v1/File/DownloadTSROptimally`

    var start = Date.now();
    console.log('--->>> Started exportTSROptimally');

    return this.httpClient.get(apiUrl, { responseType: 'blob' })
    .pipe(map((resp) => {
          saveAs(resp, fileName);
          var end = Date.now();
          console.log('--->>> Completed exportTSROptimally. Total time taken (secs): ' + (end - start)/1000);
        }),
          catchError(this.handleError)
        );
  }

  private handleError = (error: HttpErrorResponse): any => {
    const newError: any = new Error(`${error}`)
    newError.details = error;
    console.log(newError);
    throw newError;
  }

 // Current TSR Report

  public exportTSR() {

    console.log('--->>> Started exportTSR');

    this.downloadTSRFile("optimal-tsr.csv")
    .subscribe((blob: HttpResponse<Blob>) => {
      const blobBody = blob.body ?? new Blob();
      let content = blob.headers.get('content-disposition') ?? "";
      const fileName = CommonHelper.getFileNameFromHeader(content);
    
      const ele = document.createElement('a');
      ele.href = URL.createObjectURL(blobBody);
      ele.download = fileName;
      document.body.appendChild(ele);
      ele.click();
      document.body.removeChild(ele);

      console.log('--->>> Completed exportTSR');
    })
  }

  public downloadTSRFile(fileName: string): Observable<any> {
    const apiUrl = `https://localhost:7072/api/v1/File/DownloadTSR`

    return this.httpClient.get(apiUrl, { observe: 'response', responseType: 'blob' })
        .pipe(map((response: any) => response));
  }
  */
}