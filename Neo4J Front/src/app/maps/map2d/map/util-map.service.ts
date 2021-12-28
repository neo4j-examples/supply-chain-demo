import { Injectable } from "@angular/core";
import { ReplaySubject } from "rxjs";
import { distinctUntilChanged } from "rxjs/operators";

@Injectable({providedIn:'root'})
export class UtilMapService {
     zoomLevelSubject = new ReplaySubject<number>(1);
     zoomLevel = this.zoomLevelSubject.asObservable().pipe(distinctUntilChanged());
}