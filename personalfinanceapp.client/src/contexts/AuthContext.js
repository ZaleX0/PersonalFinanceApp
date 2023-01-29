import { createContext, useEffect, useState } from "react";
import { useLocalStorage } from "../hooks/useLocalStorage";
import AuthService from "../services/AuthService";

export const AuthContext = createContext({})

export function AuthProvider( { children } ) {
  const authService = new AuthService()
  const [isLogin, setIsLogin] = useState(false)
  const [user, setUser] = useLocalStorage("user", {})

  useEffect(() => {
    setIsLogin(JSON.stringify(user) !== "{}");
  }, [])

  async function login(username, password) {
    const response = await authService.login(username, password);
    if (response.status === 200) {
      const user = await response.json();
      setUser(user);
      setIsLogin(true);
    }
    return response;
  }

  function logout() {
    localStorage.removeItem("user");
    setUser({});
    setIsLogin(false);
  }

  return (
    <AuthContext.Provider value={{
      user,
      isLogin,
      login,
      logout
    }}>
      {children}
    </AuthContext.Provider>
  )
}