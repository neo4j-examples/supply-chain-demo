import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Map2dComponent } from './map2d/map2d.component';
import { Map3dComponent } from './map3d/map3d.component';

const routes: Routes = [
  { path: '2d', component: Map2dComponent },
  {
    path: '2d/:id',
    component: Map2dComponent,
  },

  { path: '', component: Map3dComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MapsdRoutingModule {}
