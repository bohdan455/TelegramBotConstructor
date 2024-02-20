import {Component} from '@angular/core';
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
        let settings: TelegramBotSettingsModel = {
            messageAnswers: this.keyvalueList.map(item => this.parseKeyValueModelToTelegramAnswerPairModel(item))
        }

        this.botCreatorService.createBot(settings).subscribe({
            next: (data) => {
                let blob = new Blob([data], {type: 'application/zip'});
                let url = window.URL.createObjectURL(blob);
                window.open(url);
                this.botIsGenerating = false;
            },
            error: () => {
                this.botIsGenerating = false;
            }
        });
    }

    private parseKeyValueModelToTelegramAnswerPairModel(keyValueModel: KeyValueModel): TelegramAnswerPairModel {
        if (keyValueModel.nested.length === 0) {
            return {
                message: keyValueModel.key,
                answer: keyValueModel.value,
                button: keyValueModel.button,
                nested: []
            }
        } else {
            return {
                message: keyValueModel.key,
                answer: keyValueModel.value,
                button: keyValueModel.button,
                nested: keyValueModel.nested.map(item => this.parseKeyValueModelToTelegramAnswerPairModel(item))
            }
        }
    }
}
