<template>
  <div class="container mt-5">
    <div class="row justify-content-center">
      <div class="col-md-6">
        <div class="card">
          <div class="card-body">
            <h1 class="card-title text-center mb-4">登入</h1>
            <form @submit.prevent="login">
              <div class="mb-3">
                <label for="username" class="form-label"
                  >使用者名稱或信箱</label
                >
                <input
                  v-model="username"
                  type="text"
                  id="username"
                  class="form-control"
                  placeholder="使用者名稱或信箱"
                  required
                />
              </div>
              <div class="mb-3">
                <label for="password" class="form-label">密碼</label>
                <input
                  v-model="password"
                  type="password"
                  id="password"
                  class="form-control"
                  placeholder="輸入密碼"
                  required
                />
              </div>
              <button type="submit" class="btn btn-primary w-100 mb-2">
                登錄
              </button>
            </form>
            <p v-if="error" class="text-danger mt-3 text-center">{{ error }}</p>
            <!-- Button trigger modal -->
            <div class="d-flex">
              <button
                type="button"
                class="btn btn-link w-100"
                data-bs-toggle="modal"
                data-bs-target="#forgetPasswordModal"
              >
                忘記密碼
              </button>
              <button
                type="button"
                class="btn btn-link w-100"
                data-bs-toggle="modal"
                data-bs-target="#resendEmailModal"
              >
                重新發送驗證郵件
              </button>
            </div>

            <InputMailModal
              title="重設密碼"
              modalName="forgetPasswordModal"
              @submit-form="sendResetPasswordMail"
            />

            <InputMailModal
              title="重新發送驗證郵件"
              modalName="resendEmailModal"
              @submit-form="resendEmail"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import api from "@/services/api";

import InputMailModal from "@/components/InputMailModal.vue";

export default {
  components: {
    InputMailModal,
  },
  data() {
    return {
      username: "",
      password: "",
      error: "",
    };
  },
  computed: {
    ...mapGetters("user", ["isAuthenticated"]),
  },
  methods: {
    ...mapActions("user", ["loginUser"]),
    async login() {
      const params = {
        username: this.username,
        password: this.password,
      };
      try {
        const res = await this.loginUser(params);
        if (res.code == 200) {
          this.username = "";
          this.password = "";
          this.$router.push("/user-profile");
        } else {
          this.error = res.message;
        }
      } catch (error) {
        this.error = error || "登入失敗";
      }
    },
    // 重設密碼
    async sendResetPasswordMail(email) {
      const params = { email };
      try {
        const res = await api.sendResetPasswordMail(params);
        alert(res.message);
      } catch (error) {
        console.error(error);
      }
    },
    async resendEmail(email) {
      const params = { email };
      try {
        const res = await api.resendEmail(params);
        alert(res.message);
      } catch (error) {
        console.error(error);
      }
    },
  },
};
</script>

<style scoped>
.container {
  max-width: 800px;
}

.card {
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}
</style>
