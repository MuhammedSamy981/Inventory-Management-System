import { createContext, useCallback, useContext, useEffect, useState } from "react";
import { authApi } from "../api/authApi";
import { clearTokens, extractErrorMessage, setTokens } from "../api/client";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [initializing, setInitializing] = useState(true);

  const loadCurrentUser = useCallback(async () => {
    const token = localStorage.getItem("accessToken");
    if (!token) {
      setInitializing(false);
      return;
    }
    try {
      const { data } = await authApi.currentUser();
      setUser(data);
    } catch {
      clearTokens();
      setUser(null);
    } finally {
      setInitializing(false);
    }
  }, []);

  useEffect(() => {
    loadCurrentUser();
    const handleLogout = () => setUser(null);
    window.addEventListener("auth:logout", handleLogout);
    return () => window.removeEventListener("auth:logout", handleLogout);
  }, [loadCurrentUser]);

  const login = async (email, password) => {
    const { data } = await authApi.login(email, password);

    // Dev-mode / unverified accounts: API returns { email, token } instead of tokens
    if (data?.token && !data?.accessToken) {
      return { needsVerification: true, email: data.email, token: data.token };
    }

    if (data?.AccessToken || data?.accessToken) {
      setTokens({
        accessToken: data.AccessToken || data.accessToken,
        refreshToken: data.RefreshToken || data.refreshToken,
      });
      await loadCurrentUser();
      return { success: true };
    }

    return { success: true, message: data?.Message || data?.message };
  };

  const register = async (payload) => {
    const { data } = await authApi.register(payload);
    return data;
  };

  const verifyEmail = async (email, token) => {
    const { data } = await authApi.verifyEmail(email, token);
    return data;
  };

  const logout = () => {
    clearTokens();
    setUser(null);
  };

  return (
    <AuthContext.Provider
      value={{ user, initializing, login, register, verifyEmail, logout, extractErrorMessage }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  return useContext(AuthContext);
}
