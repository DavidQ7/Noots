import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Label } from './models/label';
import { Labels } from './models/labels';

@Injectable()
export class ApiServiceLabels {

  constructor(private httpClient: HttpClient) { }

  getAll() {
    return this.httpClient.get<Labels>(environment.writeAPI + '/api/label');
  }

  new() {
    return this.httpClient.post<number>(environment.writeAPI + '/api/label', {});
  }

  setDeleted(id: number) {
    return this.httpClient.delete(environment.writeAPI + `/api/label/${id}`);
  }

  delete(id: number) {
    return this.httpClient.delete(environment.writeAPI + `/api/label/perm/${id}`);
  }

  update(label: Label) {
    return this.httpClient.put(environment.writeAPI + `/api/label`, label);
  }

  restore(id: number) {
    return this.httpClient.get(environment.writeAPI + `/api/label/restore/${id}`);
  }
}