export default class AuthService {

  async register(username, password) {
    return await fetch("/api/auth/register", {
      method: "POST",
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        username: username,
        password: password
      })
    });
  }

  async login(username, password) {
    return await fetch("/api/auth/login", {
      method: "POST",
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        username: username,
        password: password
      })
    });
  }
}
