import { createRouter, createWebHistory } from "vue-router";
import store from "../store"; // 確保導入 store

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      redirect: "/login", // 進入應用後重定向到 Login 頁面
    },
    {
      path: "/login",
      name: "Login",
      props: (route) => ({
        isAuthenticated: store.getters["user/isAuthenticated"],
      }),
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import("../views/Login.vue"),
    },
    {
      path: "/register",
      name: "Register",
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import("../views/Register.vue"),
    },
    {
      path: "/reset",
      name: "Reset",
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import("../views/Reset.vue"),
    },
    {
      path: "/product",
      name: "Product",
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import("../views/Product.vue"),
      meta: { requiresAuth: true }, // 需要登錄才可訪問
    },
    {
      path: "/cart",
      name: "Cart",
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import("../views/Cart.vue"),
      meta: { requiresAuth: true }, // 需要登錄才可訪問
    },
    {
      path: "/user-profile",
      name: "UserProfile",
      component: () => import("../views/UserProfile.vue"),
      meta: { requiresAuth: true }, // 需要登錄才可訪問
    },
  ],
});

// 路由守衛，檢查是否需要登錄
router.beforeEach(async (to, from, next) => {
  if (to.matched.some((record) => record.meta.requiresAuth)) {
    const isAuthenticated = store.getters["user/isAuthenticated"];
    if (!isAuthenticated) {
      await store.dispatch("user/checkAuthStatus"); // 確保檢查狀態後再做處理
      const updatedAuthStatus = store.getters["user/isAuthenticated"];
      if (!updatedAuthStatus) {
        next({ name: "Login" }); // 未登入或授權已過期，重定向到登入頁
      } else {
        next(); // 已登入，繼續導航
      }
    } else {
      next(); // 已登入，繼續導航
    }
  } else {
    next(); // 不需要驗證的路由，繼續導航
  }
});

export default router;
