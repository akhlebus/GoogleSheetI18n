export const i18nService = {
  getSettings: function() {
    return window.fetch('/i18n/settings');
  },
  reloadLocalStore: function(token) {
    return window.fetch('/i18n/reload-local-store',
      {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: {}
      });
  }
}
