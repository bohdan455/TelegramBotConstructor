import {Component, Input} from '@angular/core';
import {NgForOf} from "@angular/common";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-bot-replies-list',
  standalone: true,
  imports: [
    NgForOf,
    FormsModule
  ],
  templateUrl: './bot-replies-list.component.html',
  styleUrl: './bot-replies-list.component.scss'
})
export class BotRepliesListComponent {

  @Input() public keyvalueList: Array<{key: string, value: string}>;

  deleteKeyValue(index: number) {
    this.keyvalueList.splice(index, 1);
  }
}
