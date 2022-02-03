import { SearcherService } from './../../services/searcher.service';
import { FormBuilder } from '@angular/forms';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'neo4j-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit {
  form = this.fb.group({
    searcher: [],
  });

  isOpen: boolean = false;

  constructor(
    private fb: FormBuilder,
    private searcherService: SearcherService
  ) {}

  ngOnInit(): void {}

  onSubmit(): void {
    this.searcherService
      .search(this.form.get('searcher')?.value)
      .subscribe((res) => {
        this.searcherService.searched.emit(res);
      });
    this.isOpen = false;
  }

  openSearcher(): void {
    this.isOpen = true;
  }

  closeSearcher(): void {
    this.isOpen = false;
  }
}
