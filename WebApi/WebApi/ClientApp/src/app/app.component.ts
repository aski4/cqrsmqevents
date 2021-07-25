import { Component } from '@angular/core';
import { HttpClient, HttpHeaders  } from '@angular/common/http';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})

export class AppComponent { 
    name = '';
    constructor(private http: HttpClient) { }
    baseurl = '/api/archivedocument';
    text: any;

    public formData = new FormData();
    
    uploadFiles(file) {
        console.log('file', file)
        var lab = document.getElementById("lab");
        lab.innerHTML = file[0]['name'];
        this.formData.append("file", file[0], file[0]['name']);
    }

    RequestUpload() {
        this.http.post(this.baseurl, this.formData)
            .subscribe(() => {
            });
    }

    RequestGet(id) {
        this.http.get(this.baseurl + '/base/' + id)
            .subscribe((resp: any) => {
                this.text = resp.text;
            });

    }

    RequestCommit(id) {
        const header = new HttpHeaders().set('Content-Type', 'application/json');

        this.http.patch(this.baseurl + '/' + id, JSON.stringify(this.text), { headers: header })
            .subscribe(() =>
            {
            });
    }
}

