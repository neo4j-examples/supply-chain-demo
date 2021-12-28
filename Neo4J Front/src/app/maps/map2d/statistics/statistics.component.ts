import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import Chart from 'chart.js/auto';
import { DistributionCenter } from '../../models/distribution-center.model';

@Component({
  selector: 'neo4j-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss'],
})
export class StatisticsComponent implements OnInit {
  @ViewChild('planning') planning: ElementRef;
  @ViewChild('sales') sales: ElementRef;
  @ViewChild('sourcing') sourcing: ElementRef;
  @ViewChild('production') production: ElementRef;
  @Input() details: DistributionCenter;
  wharehouseCapacity = [...Array(23).keys()];
  capacity: number;
  constructor() {}

  ngOnInit(): void {
    this.setWharehouse();
    setInterval(() => {
      this.details.warehouseAndDistribution += 1;
      this.setWharehouse();
    }, 5000);
  }

  ngAfterViewInit(): void {
    const planningCtx = this.planning.nativeElement.getContext('2d');
    const planningChart = this.generatePieCharts(
      planningCtx,
      this.details.planning || 0,
      '#fe685a'
    );
    const salesCtx = this.sales.nativeElement.getContext('2d');
    const salesChart = this.generatePieCharts(
      salesCtx,
      this.details.sales || 0,
      '#55f9e2'
    );
    const sourcingCtx = this.sourcing.nativeElement.getContext('2d');
    const sourcingChart = this.generatePieCharts(
      sourcingCtx,
      this.details.sourcing || 0,
      '#55f9e2'
    );

    this.startRandom(planningChart, 25, 1, 'planning');
    this.startRandom(salesChart, 100, 26, 'sales');
    this.startRandom(sourcingChart, 100, 26, 'sourcing');
    this.generateLineChart();
  }

  private generateLineChart(): void {
    const ctx = this.production.nativeElement.getContext('2d');
    new Chart(ctx, {
      type: 'line',
      data: {
        labels: this.details.production.map(
          ({ year }: { year: number; production: number }) => year
        ),

        datasets: [
          {
            data: this.details.production.map(
              ({ production }: { year: number; production: number }) =>
                production
            ),
            borderColor: '#55f9e255',
          },
        ],
      },
      options: {
        responsive: false,
        scales: {
          x: {
            title: {
              color: 'white',
            },
            grid: {
              color: '#55f9e233',
            },
          },
          y: {
            grid: {
              color: '#55f9e233',
            },
          },
        },
        plugins: {
          legend: { display: false },
        },
        events: [],
      },
    });
  }

  private generatePieCharts(ctx: any, value: number, color: string): Chart {
    return new Chart(ctx, {
      type: 'pie',
      data: {
        labels: [],
        datasets: [
          {
            data: [value, 100 - value],
            backgroundColor: [color, '#ffffff00'],
            borderWidth: 0,
          },
        ],
      },
      options: {
        events: [],
      },
    });
  }

  private setWharehouse(): void {
    this.capacity =
      (this.details.warehouseAndDistribution * 23) /
      this.details.warehouseAndDistributionTotal;
  }

  private startRandom(
    chart: Chart,
    max: number,
    min: number,
    model: string
  ): void {
    const rand = Math.floor(Math.random() * (20 - 10 + 1) + 10);

    (this.details as any)[model] = this.updateChart(chart, max, min);

    setTimeout(() => this.startRandom(chart, max, min, model), rand * 1000);
  }

  private updateChart(chart: Chart, max: number, min: number): number {
    const oldValue = chart.data.datasets[0].data[0] as number;
    let newValue = oldValue + Math.floor(Math.random() * (3 - -1) + -1);
    newValue = newValue < 100 ? newValue : 100;
    chart.data.datasets[0].data = [newValue, 100 - newValue];
    if (newValue < 50) {
      chart.data.datasets[0].backgroundColor = ['#fe685a', '#ffffff00'];
    } else {
      chart.data.datasets[0].backgroundColor = ['#55f9e2', '#ffffff00'];
    }
    chart.update();
    return newValue;
  }
}
