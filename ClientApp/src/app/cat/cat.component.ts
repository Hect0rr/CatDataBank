import { Component, OnInit } from '@angular/core';
import { CatService } from '../gateway/CdbService/cat.service';
import { Cat } from '../gateway/CdbService/model/cat';

@Component({
  selector: 'app-cat',
  templateUrl: './cat.component.html',
  styleUrls: ['./cat.component.css']
})
export class CatComponent implements OnInit {
  cats: Cat[]
  constructor(private catService: CatService) { }

  ngOnInit() {

    this.catService.getCats().subscribe((data: Cat[]) => this.cats = data);

  }

}
