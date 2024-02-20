import {Component, Input} from '@angular/core';
import {NgForOf, NgIf} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {KeyValueModel} from "../../models/keyValue.model";

@Component({
  selector: 'app-bot-replies-list',
  standalone: true,
    imports: [
        NgForOf,
        FormsModule,
        NgIf
    ],
  templateUrl: './bot-replies-list.component.html',
  styleUrl: './bot-replies-list.component.scss'
})
export class BotRepliesListComponent {

  @Input() public keyvalueList: Array<KeyValueModel>;

  deleteKeyValue(index: number) {
    this.keyvalueList.splice(index, 1);
  }

  addNestedKeyValue(index: number) {
    this.keyvalueList[index].nested.push({key: '', value: '', button: false, nested: []});
  }
}
