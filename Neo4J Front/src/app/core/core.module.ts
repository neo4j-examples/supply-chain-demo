import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FooterComponent } from './components/footer/footer.component';
import { HeaderComponent } from './components/header/header.component';
import { AppComponent } from './views/app.component';

@NgModule({
  declarations: [AppComponent, FooterComponent, HeaderComponent],
  exports: [AppComponent, FooterComponent, HeaderComponent],
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
})
export class CoreModule {}
