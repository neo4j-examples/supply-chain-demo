import {
  Component,
  ElementRef,
  Input,
  OnInit,
  QueryList,
  Renderer2,
  ViewChild,
  ViewChildren,
} from '@angular/core';
import { Incident } from '../../models/incident.model';

@Component({
  selector: 'neo4j-truck',
  templateUrl: './truck.component.svg',
  styleUrls: ['./truck.component.scss'],
})
export class TruckComponent implements OnInit {
  tagSensor: any;
  tagSensorLine: any;
  tagSensorText: any;
  @Input() incident: Incident;
  @ViewChildren('electricSystem') electricSystem: QueryList<ElementRef>;
  @ViewChildren('breakSystem') breakSystem: QueryList<ElementRef>;
  @ViewChildren('engineSystem') engineSystem: QueryList<ElementRef>;
  @ViewChildren('batterySystem') batterySystem: QueryList<ElementRef>;
  @ViewChildren('pressureSystem') pressureSystem: QueryList<ElementRef>;
  @ViewChildren('coolingSystem') coolingSystem: QueryList<ElementRef>;
  sensors: any;

  constructor(private readonly renderer: Renderer2) {}

  ngOnInit(): void {
    this.tagSensor = this.renderer.createElement('div');
    this.tagSensorLine = this.renderer.createElement('img');
    this.tagSensorText = this.renderer.createElement('span');
    this.renderer.appendChild(this.tagSensor, this.tagSensorLine);
    this.renderer.appendChild(this.tagSensor, this.tagSensorText);

    this.renderer.setStyle(this.tagSensor, 'position', 'absolute');
    this.renderer.setStyle(
      this.tagSensor,
      'transform',
      'translate(-100%, -100%'
    );
    this.renderer.addClass(this.tagSensorLine, 'tag-sensor-image');
    this.renderer.addClass(this.tagSensorText, 'tag-sensor-text');
    this.renderer.setStyle(this.tagSensor, 'z-index', '800');
    this.renderer.setStyle(this.tagSensor, 'height', '85px');
    this.renderer.setStyle(this.tagSensorLine, 'height', '100%');

    this.renderer.setAttribute(
      this.tagSensorLine,
      'src',
      'assets/images/tag-line.svg'
    );

    this.renderer.setProperty(this.tagSensorText, 'innerText', 'hola');
  }

  ngAfterViewInit(): void {
    this.sensors = {
      'Pressure System': this.pressureSystem,
      'Battery System': this.batterySystem,
      'Cooling System': this.coolingSystem,
      'Electric System': this.electricSystem,
      'Engine System': this.engineSystem,
      'Break System': this.breakSystem,
    };

    this.incident.sensorsState;
    for (const sensor of this.incident.sensorsState) {
      if (sensor.failure) {
        this.sensors[sensor.name]
          .toArray()
          .forEach((sensorElement: ElementRef) => {
            this.renderer.addClass(sensorElement.nativeElement, 'problem');
          });
      }
    }
  }

  onMouseEnter(e: any): void {
    const sensorName = e.target.getAttribute('sensor');
    this.renderer.setStyle(
      this.tagSensor,
      'top',
      `${
        e.target.getBoundingClientRect().y +
        e.target.getBoundingClientRect().height / 2
      }px`
    );
    this.renderer.setStyle(
      this.tagSensor,
      'left',
      `${
        e.target.getBoundingClientRect().x +
        e.target.getBoundingClientRect().width / 2
      }px`
    );
    this.renderer.setProperty(
      this.tagSensorText,
      'innerText',
      sensorName || 'unknow'
    );
    this.renderer.appendChild(document.body, this.tagSensor);
  }

  onMouseLeave(e: any): void {
    try {
      this.renderer.removeChild(document.body, this.tagSensor);
    } catch {}
  }
}

/*
[
  {
      "name": "Pressure System",
      "failure": false,
      "value": 59.531597574927
  },
  {
      "name": "Battery System",
      "failure": false,
      "value": 59.531597574927
  },
  {
      "name": "Electric System",
      "failure": false,
      "value": 59.531597574927
  },
  {
      "name": "Cooling System",
      "failure": true,
      "value": 59.531597574927
  },
  {
      "name": "Engine System",
      "failure": false,
      "value": 59.531597574927
  }
]
*/
