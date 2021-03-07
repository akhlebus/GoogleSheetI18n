export const i18nService = {
  getSettings: function() {
    return window.fetch('/i18n/settings');
  },
  clearCache: function(channelId, resourceId, token) {
    return window.fetch('/i18n/empty-cache',
      {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          'X-Goog-Channel-ID': channelId,
          'X-Goog-Resource-ID': resourceId,
          'Authorization': `Bearer ${token}`
        },
        body: {}
      });
  }
}
