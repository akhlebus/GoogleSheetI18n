export const accountService = {
  login: function() {
    return window.fetch('/account/login',
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
    return window.fetch('/account/logout',
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
