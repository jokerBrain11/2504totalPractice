import axios from "axios";

const apiUrl = import.meta.env.VITE_API_URL;

const apiClient = axios.create({
  baseURL: apiUrl+"/api", // 替換為你的 API 基址
  withCredentials: true, // 這允許跨域請求
});

export default {
  // 註冊用戶
  register(params) {
    return new Promise((resolve, reject) => {
      apiClient
        .post("/user/register", params)
        .then((res) => resolve(res.data))
        .catch((err) => reject(err));
    });
  },
  // 重新發送驗證碼
  resendMail(params) {
    return new Promise((resolve, reject) => {
      apiClient
        .post("/user/resend-email", params)
        .then((res) => resolve(res.data))
        .catch((err) => reject(err));
    });
  },
   // 登入
   loginHandler(params) {
    console.log(params);
    return new Promise((resolve, reject) => {
      apiClient
        .post("/user/login", params)
        .then((res) => resolve(res.data))
        .catch((err) => reject(err));
    });
  },
  // 檢查登入狀態
  checkLogin() {
    return new Promise((resolve, reject) => {
      apiClient
        .get("/user/check-login")
        .then((res) => resolve(res.data))
        .catch((err) => reject(err));
    });
  },
  // 登出
  logoutHandler() {
    return new Promise((resolve, reject) => {
      apiClient
        .get("/user/logout")
        .then((res) => resolve(res.data))
        .catch((err) => reject(err));
    });
  },
  // 使用者資訊
  userProfile() {
    return new Promise((resolve, reject) => {
      apiClient
        .get("/user/user-profile")
        .then((res) => resolve(res.data))
        .catch((err) => reject(err));
    });
  },
  // 更新使用者資訊
  updateUserProfile(params) {
    return new Promise((resolve, reject) => {
      apiClient
        .post("/user/update-userProfile", params)
        .then((res) => resolve(res.data))
        .catch((err) => reject(err));
    });
  },
  // 重新發送驗證碼
  sendResetPasswordMail(params) {
    return new Promise((resolve, reject) => {
      apiClient
        .post("/user/send-reset-password-mail", params)
        .then((res) => resolve(res.data))
        .catch((err) => reject(err));
    });
  },
  // 重設密碼
  resetPassword(params) {
    return new Promise((resolve, reject) => {
      apiClient
        .post("/user/reset-password", params)
        .then((res) => resolve(res.data))
        .catch((err) => reject(err));
    });
  },

  getProducts() {
    return apiClient.get("/products");
  },
};
