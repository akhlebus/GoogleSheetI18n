import { spreadsheetTypes } from '../enums/SpreadsheetType';
import { ENVIRONMENT } from '../environments/environment';

export const i18nService = {
    getSettings: function () {
        return window.fetch(`${ENVIRONMENT.api}/i18n/settings`);
    },
    reloadLocalStore: function (token) {
        return window.fetch(`${ENVIRONMENT.api}/i18n/reload-local-store`,
            {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: {}
            });
    },
    validateSpreadsheet: function (type) {
        return type === spreadsheetTypes.LOCAL ? window.fetch(`${ENVIRONMENT.api}/i18n/validate-local-spreadsheet`) : window.fetch(`${ENVIRONMENT.api}/i18n/validate-spreadsheet`);
    }
}
