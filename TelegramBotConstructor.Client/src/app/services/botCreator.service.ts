import {HttpClient} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {EndpointsConfiguration} from "../../configurations/endpoints.configuration";
import {TelegramBotSettingsModel} from "../models/telegramBotSettingsModel";

@Injectable({
  providedIn: 'root'
})
export class BotCreatorService{
  constructor(public http:HttpClient ){

  }

  createBot(settings: TelegramBotSettingsModel) : Observable<Blob> {
    return this.http.post(EndpointsConfiguration.baseUrl + EndpointsConfiguration.createBot, settings, {responseType: 'blob'});
  }
}
