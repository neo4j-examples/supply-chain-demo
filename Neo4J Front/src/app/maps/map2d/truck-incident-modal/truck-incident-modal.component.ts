import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import Chart from 'chart.js/auto';

import { Incident } from '../../models/incident.model';
import { Truck } from '../../models/truck.model';
import { EChartsOption } from 'echarts'
import * as echarts from 'echarts';
@Component({
  selector: 'neo4j-truck-incident-modal',
  templateUrl: './truck-incident-modal.component.html',
  styleUrls: ['./truck-incident-modal.component.scss'],
})
export class TruckIncidentModalComponent implements OnInit, AfterViewInit {
  @Input() truckInfo: Truck;
  @Input() incident: Incident;
  @Output() closeModal = new EventEmitter<void>();
  @Output() callDriver = new EventEmitter<void>();
  @ViewChild('mockChartFirst') mockChartFirst: ElementRef;
  @ViewChild('mockChartSecond') mockChartSecond: ElementRef;
  @ViewChild('chart') chart: any;
  @ViewChild('chartTwo') chartTwo: any;

  option = {
    series: [
      {
        type: 'gauge',
        startAngle: 180,
        endAngle: 0,
        min: 0,
        max: 160,
        splitNumber: 8,
        itemStyle: {
          color: '#55f9e2',
          shadowColor: 'rgba(0,138,255,0.45)',
          shadowBlur: 10,
          shadowOffsetX: 2,
          shadowOffsetY: 2
        },
        progress: {
          show: true,
          roundCap: true,
          width: 7
        },
        pointer: {
          icon: 'path://M2090.36389,615.30999 L2090.36389,615.30999 C2091.48372,615.30999 2092.40383,616.194028 2092.44859,617.312956 L2096.90698,728.755929 C2097.05155,732.369577 2094.2393,735.416212 2090.62566,735.56078 C2090.53845,735.564269 2090.45117,735.566014 2090.36389,735.566014 L2090.36389,735.566014 C2086.74736,735.566014 2083.81557,732.63423 2083.81557,729.017692 C2083.81557,728.930412 2083.81732,728.84314 2083.82081,728.755929 L2088.2792,617.312956 C2088.32396,616.194028 2089.24407,615.30999 2090.36389,615.30999 Z',
          length: '70%',
          width: 6,
          offsetCenter: [0, '5%']
        },
        axisLine: {
          roundCap: true,
          lineStyle: {
            width: 7,
            color: [[1, '#58D9F944']]
          }
        },
        axisTick: {
          length: 4,
          distance: 2,
          splitNumber: 2,
          lineStyle: {
            width: 1,
            color: '#55f9e2'
          }
        },
        splitLine: {
          length: 8,
          distance: 2,
          lineStyle: {
            width: 1,
            color: '#55f9e2'
          }
        },
        axisLabel: {
          distance: 10,
          color: '#999',
          fontSize: 9
        },
        title: {
          show: false
        },
        detail: {
          width: '60%',
          lineHeight: 40,
          height: 40,
          borderRadius: 8,
          offsetCenter: [0, '35%'],
          valueAnimation: true,
          formatter: function (value: any) {
            return '{value|' + value.toFixed(0) + '}{unit|mph}';
          },
          rich: {
            value: {
              fontSize: 16,
              fontWeight: 'bolder',
              color: '#777'
            },
            unit: {
              fontSize: 10,
              color: '#999',
              padding: [0, 0, -10, 5]
            }
          }
        },
        data: [
          {
            value: 100
          }
        ]
      }
    ]
  };
  optionMPG = {
    series: [
      {
        type: 'gauge',
        startAngle: 180,
        endAngle: 0,
        min: 1,
        max: 120,
        splitNumber: 8,
        itemStyle: {
          color: '#55f9e2',
          shadowColor: 'rgba(0,138,255,0.45)',
          shadowBlur: 10,
          shadowOffsetX: 2,
          shadowOffsetY: 2
        },
        progress: {
          show: true,
          roundCap: true,
          width: 7
        },
        pointer: {
          icon: 'path://M2090.36389,615.30999 L2090.36389,615.30999 C2091.48372,615.30999 2092.40383,616.194028 2092.44859,617.312956 L2096.90698,728.755929 C2097.05155,732.369577 2094.2393,735.416212 2090.62566,735.56078 C2090.53845,735.564269 2090.45117,735.566014 2090.36389,735.566014 L2090.36389,735.566014 C2086.74736,735.566014 2083.81557,732.63423 2083.81557,729.017692 C2083.81557,728.930412 2083.81732,728.84314 2083.82081,728.755929 L2088.2792,617.312956 C2088.32396,616.194028 2089.24407,615.30999 2090.36389,615.30999 Z',
          length: '70%',
          width: 6,
          offsetCenter: [0, '5%']
        },
        axisLine: {
          roundCap: true,
          lineStyle: {
            width: 7,
            color: [[1, '#58D9F944']]
          }
        },
        axisTick: {
          length: 4,
          distance: 2,
          splitNumber: 2,
          lineStyle: {
            width: 1,
            color: '#55f9e2'
          }
        },
        splitLine: {
          length: 8,
          distance: 2,
          lineStyle: {
            width: 1,
            color: '#55f9e2'
          }
        },
        axisLabel: {
          distance: 10,
          color: '#999',
          fontSize: 9
        },
        title: {
          show: false
        },
        detail: {
          width: '60%',
          lineHeight: 40,
          height: 40,
          borderRadius: 8,
          offsetCenter: [0, '35%'],
          valueAnimation: true,
          formatter: function (value: any) {
            return '{value|' + value.toFixed(0) + '}{unit|mpg}';
          },
          rich: {
            value: {
              fontSize: 16,
              fontWeight: 'bolder',
              color: '#777'
            },
            unit: {
              fontSize: 10,
              color: '#999',
              padding: [0, 0, -10, 5]
            }
          }
        },
        data: [
          {
            value: 35
          }
        ]
      }
    ]
  };
  mph: void;
  mpg: void;
  constructor() {}

  ngOnInit(): void {}

  ngAfterViewInit(): void {
   echarts.init(this.chart.nativeElement).setOption(this.option)
   echarts.init(this.chartTwo.nativeElement).setOption(this.optionMPG)
 
    this.startRandom(this.chartTwo.nativeElement, 60, 30, 'optionMPG');
    this.startRandom(this.chart.nativeElement, 75, 110, 'option');

  }



  closeClicked(): void {
    this.closeModal.emit();
  }


  private updateChart(chart: any, max:number, min: number, model: string) {
    const rand = Math.floor(Math.random() * (max - min + 1) + min);
    (this as any)[model].series[0].data = [{ value: rand }];
    echarts.getInstanceByDom(chart)?.setOption((this as any)[model]);
  }

  private startRandom(
    chart: any,
    max: number,
    min: number,
    model: string
  ): void {
    const rand = Math.floor(Math.random() * (3 - 2 + 1) + 2);

    this.updateChart(chart, max, min, model);

    setTimeout(() => this.startRandom(chart, max, min, model), rand * 1000);
  }
}
