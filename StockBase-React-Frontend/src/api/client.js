import axios from "axios";

const BASE_URL = import.meta.env.VITE_API_URL || "https://localhost:7261/api";

const client = axios.create({
  baseURL: BASE_URL,
  headers: { "Content-Type": "application/json" },
});

function getTokens() {
  return {
    accessToken: localStorage.getItem("accessToken"),
    refreshToken: localStorage.getItem("refreshToken"),
  };
}

function setTokens({ accessToken, refreshToken }) {
  if (accessToken) localStorage.setItem("accessToken", accessToken);
  if (refreshToken) localStorage.setItem("refreshToken", refreshToken);
}

function clearTokens() {
  localStorage.removeItem("accessToken");
  localStorage.removeItem("refreshToken");
}

client.interceptors.request.use((config) => {
  const { accessToken } = getTokens();
  if (accessToken) {
    config.headers.Authorization = `Bearer ${accessToken}`;
  }
  return config;
});

let isRefreshing = false;
let pendingQueue = [];

function resolvePendingQueue(error, token) {
  pendingQueue.forEach(({ resolve, reject }) => {
    if (error) reject(error);
    else resolve(token);
  });
  pendingQueue = [];
}

client.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;
    const status = error.response?.status;

    const isAuthEndpoint =
      originalRequest?.url?.includes("/Users/login") ||
      originalRequest?.url?.includes("/Users/register") ||
      originalRequest?.url?.includes("/Users/refresh-token");

    if (status === 401 && !originalRequest._retry && !isAuthEndpoint) {
      const { refreshToken, accessToken } = getTokens();

      if (!refreshToken || !accessToken) {
        clearTokens();
        window.dispatchEvent(new CustomEvent("auth:logout"));
        return Promise.reject(error);
      }

      if (isRefreshing) {
        return new Promise((resolve, reject) => {
          pendingQueue.push({ resolve, reject });
        }).then((token) => {
          originalRequest.headers.Authorization = `Bearer ${token}`;
          return client(originalRequest);
        });
      }

      originalRequest._retry = true;
      isRefreshing = true;

      try {
        const { data } = await axios.post(`${BASE_URL}/Users/refresh-token`, {
          accessToken,
          refreshToken,
        });
        setTokens({ accessToken: data.accessToken, refreshToken: data.refreshToken });
        resolvePendingQueue(null, data.accessToken);
        originalRequest.headers.Authorization = `Bearer ${data.accessToken}`;
        return client(originalRequest);
      } catch (refreshError) {
        resolvePendingQueue(refreshError, null);
        clearTokens();
        window.dispatchEvent(new CustomEvent("auth:logout"));
        return Promise.reject(refreshError);
      } finally {
        isRefreshing = false;
      }
    }

    return Promise.reject(error);
  }
);

export function extractErrorMessage(error) {
  const data = error?.response?.data;
  if (!data) return error?.message || "Something went wrong. Please try again.";
  if (typeof data === "string") return data;
  if (data.Message) return data.Message;
  if (data.message) return data.message;
  if (Array.isArray(data.Errors)) {
    return data.Errors.map((e) => e.ErrorMessage || e.errorMessage || e.description).join(" ");
  }
  if (Array.isArray(data.errors)) {
    return data.errors.map((e) => e.description || e.ErrorMessage || e).join(" ");
  }
  if (data.errors && typeof data.errors === "object") {
    return Object.values(data.errors).flat().join(" ");
  }
  if (Array.isArray(data)) {
    return data.map((e) => e.description || e.ErrorMessage || JSON.stringify(e)).join(" ");
  }
  return "Something went wrong. Please try again.";
}

export { getTokens, setTokens, clearTokens };
export default client;
