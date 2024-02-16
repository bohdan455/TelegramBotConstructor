import { Component } from '@angular/core';
import {NgForOf, NgIf} from "@angular/common";
import {TelegramBotSettingsModel} from "../../models/telegramBotSettingsModel";
import {TelegramAnswerPairModel} from "../../models/telegramAnswerPairModel";
import {BotCreatorService} from "../../services/botCreator.service";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-keyvalue-input-list',
  standalone: true,
  imports: [
    NgForOf,
    FormsModule,
    NgIf
  ],
  templateUrl: './keyvalue-input-list.component.html',
  styleUrl: './keyvalue-input-list.component.scss'
})
export class KeyvalueInputListComponent {
  public keyvalueList: Array<{key: string, value: string}> = [{key: '', value: ''}];
  public botIsGenerating: boolean = false;

  constructor(public botCreatorService: BotCreatorService) {
  }

  addKeyValue() {
    this.keyvalueList.push({key: '', value: ''});
  }

  deleteKeyValue(index: number) {
    this.keyvalueList.splice(index, 1);
  }

  getFile(event: SubmitEvent) {
    event.preventDefault();
    this.botIsGenerating = true;
    let settings : TelegramBotSettingsModel;
    settings = {
      messageAnswers: this.keyvalueList.map((item) : TelegramAnswerPairModel => {
        return {
          message: item.key,
          answer: item.value
        }
      })
    };

    this.botCreatorService.createBot(settings).subscribe({
      next: (data) => {
      let blob = new Blob([data], { type: 'application/zip' });
      let url= window.URL.createObjectURL(blob);
      window.open(url);
      this.botIsGenerating = false;
      },
      error: () => {
      this.botIsGenerating = false;
    }});
  }
}
