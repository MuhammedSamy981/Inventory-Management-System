import client from "./client";

export const authApi = {
  login: (email, password) => client.post("/Users/login", { email, password }),

  register: (payload) => client.post("/Users/register", payload),

  verifyEmail: (email, token) => client.post("/Users/verification", { email, token }),

  currentUser: () => client.get("/Users/currentUserInfo"),

  forgotPassword: (email) => client.post("/Users/forgot-password", { email }),

  resetPassword: (email, token, newPassword) =>
    client.post("/Users/reset-password", { email, token, newPassword }),
};
