export interface TelegramAnswerPairModel{
  message: string;
  answer: string;
  button: boolean;
  nested: TelegramAnswerPairModel[];
}
