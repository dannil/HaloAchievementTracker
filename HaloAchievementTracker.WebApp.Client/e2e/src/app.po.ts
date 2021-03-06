import { browser, by, element, promise } from 'protractor';

export class AppPage {
  navigateTo(): promise.Promise<string> {
    return browser.get('/');
  }

  getMainHeading(): promise.Promise<string> {
    return element(by.css('app-root h1')).getText();
  }
}
