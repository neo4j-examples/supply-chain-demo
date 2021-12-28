import { MarkerColor } from './../../enums/marker-color.enum';
import { DOCUMENT } from '@angular/common';
import {
  EventEmitter,
  Inject,
  Injectable,
  Renderer2,
  RendererFactory2,
} from '@angular/core';
import { Connection } from '../../models/connection.model';
import { LocationMap } from '../../models/location.model';
import { Router } from '@angular/router';
declare const Earth: any;

@Injectable({
  providedIn: 'root',
})
export class Map3dService {
  renderer: Renderer2;
  earth: any;
  earthClicked = new EventEmitter<void>();
  alreadyInit: any;

  constructor(
    private readonly router: Router,
    private readonly rendererFactory: RendererFactory2,
    @Inject(DOCUMENT) private document: any
  ) {
    this.renderer = this.rendererFactory.createRenderer(null, null);
    const script = this.renderer.createElement('script');
    this.renderer.appendChild(this.document.body, script);
    script.src = '../assets/js/miniature.earth.core.js';
  }

  initMap3D(): void {
    this.earth = new Earth('myearth', {
      location: { lat: 40.41434807989499, lng: -3.6996870586777537 },
      zoom: 0.02,
      mapLandColor: '#002929',
      mapSeaColor: 'rgba(0,0,0,0.8)',
      mapBorderColor: 'rgba(255,255,255,1)',
      mapBorderWidth: 0.05,
      light: 'none',
      transparent: true,
      zoomable: true,

      autoRotate: false,
      autoRotateSpeed: 1.2,
      autoRotateDelay: 100,
      autoRotateStart: 2000,
    });

    this.renderer.listen(this.earth, 'click', () => {
      this.earth.goTo(
        { lat: 37.424458541586716, lng: -120.37403641400874 },

        {
          zoom: 3,
          duration: 5000,
          easing: 'out-quad',
          complete: () => {
            if (!this.alreadyInit) {
              this.earthClicked.emit();
            }
            this.alreadyInit = true;
          },
        }
      );
    });
  }

  addMarkers(locations: LocationMap[]): void {
    for (const location of locations) {
      const image = this.earth.addImage({
        location: { lat: location.position.lat, lng: location.position.lng },
        image: location.hasProblem ? MarkerColor.RED : location.color,
        scale: 0.5,
        offset: location.hasProblem ? 0.05 : 0,
        hotspot: true,
      });
      image.addEventListener('click', (event: any) => {
        this.earth.goTo(
          {
            lat: event.target.options.location.lat,
            lng: event.target.options.location.lng,
          },

          {
            zoom: 10,
            duration: 2000,
            easing: 'out-quad',
            complete: () => {
              this.alreadyInit = false;
              this.router.navigate(['/2d/'+ location.id]);
            },
          }
        );
      });

      if (location.hasProblem) {
        image.animate('scale', 0.25, {
          loop: true,
          oscillate: true,
          duration: 2000,
          easing: 'in-out-quad',
        });
        image.animate('opacity', 0.25, {
          loop: true,
          oscillate: true,
          duration: 2000,
          easing: 'in-out-quad',
        });
      }
    }
  }

  showLines(connections: Connection[]): void {
    const line: any = {
      color: '#006767',
      opacity: 0.35,
      offset: 0,
      width: 0.15,
      alwaysBehind: false,
      offsetFlow: 0.5,
      transparent: false,
      clip: 0,
    };
    for (let connection of connections) {
      line.locations = [
        { lat: connection.startPoint.lat, lng: connection.startPoint.lng },
        { lat: connection.endPoint.lat, lng: connection.endPoint.lng },
      ];
      const t = this.earth.addLine(line);
      t.animate('clip', 1, { loop: false, oscillate: false, duration: 2000 });
    }
  }

  flyTo(lat: number, lng: number): void {
    this.earth.goTo(
      {
        lat,
        lng
      },

      {
        zoom: 10,
        duration: 2000,
        easing: 'out-quad',
       
      }
    );
  }
}
