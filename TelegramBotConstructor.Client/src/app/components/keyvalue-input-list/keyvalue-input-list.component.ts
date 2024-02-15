import { Component } from '@angular/core';
import {NgForOf} from "@angular/common";

@Component({
  selector: 'app-keyvalue-input-list',
  standalone: true,
  imports: [
    NgForOf
  ],
  templateUrl: './keyvalue-input-list.component.html',
  styleUrl: './keyvalue-input-list.component.scss'
})
export class KeyvalueInputListComponent {
  public keyvalueList: Array<{key: string, value: string}> = [{key: '', value: ''}];

  addKeyValue() {
    this.keyvalueList.push({key: '', value: ''});
  }

  deleteKeyValue(index: number) {
    this.keyvalueList.splice(index, 1);
  }
}
