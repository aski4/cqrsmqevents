var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component } from '@angular/core';
let AppComponent = class AppComponent {
    constructor(http) {
        this.http = http;
        this.name = '';
        this.baseurl = '/api/archivedocument';
        this.formData = new FormData();
    }
    uploadFiles(file) {
        console.log('file', file);
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
            .subscribe((resp) => {
            this.text = resp.text;
        });
    }
    RequestCommit(id) {
        this.http.patch(this.baseurl + '/' + id, JSON.stringify(this.text))
            .subscribe(() => {
        });
    }
};
AppComponent = __decorate([
    Component({
        selector: 'app',
        templateUrl: './app.component.html',
        styleUrls: ['./app.component.css']
    })
], AppComponent);
export { AppComponent };
//# sourceMappingURL=app.component.js.map