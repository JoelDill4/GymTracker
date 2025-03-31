import { HttpClientModule, HttpClient, HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule, Injectable } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { APP_BASE_HREF } from '@angular/common';
import { Observable } from 'rxjs';
import { AppRoutingModule } from './app-routing.module';
import { ExercisesModule } from './features/exercises/exercises.module';
import { AppComponent } from './app.component';

@Injectable()
export class ApiInterceptor implements HttpInterceptor {
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const baseUrl = 'https://localhost:7175';
    const apiReq = request.clone({
      url: `${baseUrl}${request.url}`
    });
    return next.handle(apiReq);
  }
}

@NgModule({
  declarations: [],
  imports: [
    BrowserModule, 
    HttpClientModule,
    AppRoutingModule,
    ExercisesModule,
    AppComponent
  ],
  providers: [
    { provide: APP_BASE_HREF, useValue: '/' },
    { provide: HTTP_INTERCEPTORS, useClass: ApiInterceptor, multi: true }
  ],
  bootstrap:[AppComponent]
})
export class AppModule { }

