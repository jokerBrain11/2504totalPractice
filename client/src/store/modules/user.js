import api from "@/services/api";

const state = {
  isAuthenticated: false,
  user: null
};

const mutations = {
  SET_AUTH_STATUS(state, status) {
    state.isAuthenticated = status;
  },
  LOGIN(state, user) {
    state.isAuthenticated = true;
    state.user = user;
  },
  LOGOUT(state) {
    state.isAuthenticated = false;
    state.user = null;
  }
};

const actions = {
  async loginUser({ commit }, userData) {
    try {
      const res = await api.loginHandler(userData);
      if (res.code == 200) {
        commit("LOGIN", res.data);
        commit("SET_AUTH_STATUS", true);
      }
      return res;
    } catch (error) {
      console.error("Login failed:", error);
    }
  },
  async logoutUser({ commit }) {
    try {
      await api.logoutHandler();
      commit("LOGOUT");
    } catch (error) {
      console.error("Logout failed:", error);
    }
  },
  async checkAuthStatus({ commit }) {
    try {
      await api.checkLogin(); // 向後端請求檢查登入狀態
      commit("SET_AUTH_STATUS", true); // 設置已受驗證狀態
    } catch (error) {
      if (error.response && error.response.status === 401) {
        commit('SET_AUTH_STATUS', false); // 設置未受驗證狀態
        commit('LOGOUT'); // 清除用戶狀態
      }
    }
  }
};

const getters = {
  isAuthenticated(state) {
    return state.isAuthenticated;
  }
};

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters,
};



// const state = {
//   user: JSON.parse(localStorage.getItem("user")) || null, // 初始化時檢查 localStorage
//   roles: JSON.parse(localStorage.getItem("roles")) || null,
//   isAuthenticated: !!localStorage.getItem("user"),
// };

// const mutations = {
//   SET_USER(state, user) {
//     state.user = user.username;
//     state.roles = user.roles;
//     state.isAuthenticated = !!user;
//   },
//   LOGOUT(state) {
//     state.user = null;
//     state.roles = null;
//     state.isAuthenticated = false;
//   },
// };

// const actions = {
//   // 假設這是在 Vuex 的 action 中
//   async loginUser({ commit }, userData) {
//     try {
//       const res = await api.loginHandler(userData);
//       if (res.code === 200) {
//         commit("SET_USER", res.data);
//         localStorage.setItem("user", JSON.stringify(res.data)); // 將用戶資料存儲在 localStorage
//       }
//       console.log("Login success:", res);
//       return res; // 返回結果給調用這個 action 的地方
//     } catch (error) {
//       console.error("Login failed:", error);
//       throw error; // 如果需要，可以在這裡處理錯誤
//     }
//   },
//   async logoutUser({ commit }) {
//     try {
//       const res = await api.logoutHandler();
//       commit("LOGOUT");
//       localStorage.removeItem("user");
//       console.log("Logout success:", res);
//       return res;
//     } catch (error) {
//       console.error("Logout failed:", error);
//       throw error;
//     }
//   }
// };

// const getters = {
//   user: (state) => state.user,
//   roles: (state) => state.roles,
//   isLogin: (state) => state.isAuthenticated,
// };

