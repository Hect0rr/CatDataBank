import { Component, OnInit } from '@angular/core';
import { CatService } from '../gateway/CdbService/cat.service';

@Component({
  selector: 'app-cat',
  templateUrl: './cat.component.html',
  styleUrls: ['./cat.component.css']
})
export class CatComponent implements OnInit {

  constructor(private catService: CatService) { }

  ngOnInit() {

    this.catService.getCats().subscribe(data => console.log(data));

  }

}
