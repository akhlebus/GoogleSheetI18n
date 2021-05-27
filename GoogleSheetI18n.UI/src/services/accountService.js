import { ENVIRONMENT } from '../environments/environment';

export const accountService = {
  login: function() {
    return window.fetch(`${ENVIRONMENT.api}/account/login`,
      {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          username: 'john',
          password: 'john123'
        })
      });
  },
  logout: function(token) {
    return window.fetch(`${ENVIRONMENT.api}/account/logout`,
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
