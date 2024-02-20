import { Component } from '@angular/core';
import {NgForOf, NgIf} from "@angular/common";
import {TelegramBotSettingsModel} from "../../models/telegramBotSettings.model";
import {TelegramAnswerPairModel} from "../../models/telegramAnswerPair.model";
import {BotCreatorService} from "../../services/botCreator.service";
import {FormsModule} from "@angular/forms";
import {BotRepliesListComponent} from "../bot-replies-list/bot-replies-list.component";
import {KeyValueModel} from "../../models/keyValue.model";

@Component({
  selector: 'app-bot-configuration',
  standalone: true,
  imports: [
    NgForOf,
    FormsModule,
    NgIf,
    BotRepliesListComponent
  ],
  templateUrl: './bot-configuration.component.html',
  styleUrl: './bot-configuration.component.scss'
})
export class BotConfigurationComponent {
  public keyvalueList: Array<KeyValueModel> = [{key: '', value: '', button: false, nested: []}];
  public botIsGenerating: boolean = false;

  constructor(public botCreatorService: BotCreatorService) {
  }

  addKeyValue() {
    this.keyvalueList.push({key: '', value: '', button: false, nested: []});
  }

  getFile(event: SubmitEvent) {
    event.preventDefault();
    this.botIsGenerating = true;
    let settings : TelegramBotSettingsModel;
    settings = {
      messageAnswers: this.keyvalueList.map((item) : TelegramAnswerPairModel => {
        return {
          message: item.key,
          answer: item.value,
          button: item.button
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
