<template>
  <div class="container mt-5">
    <div class="row justify-content-center">
      <div class="col-md-6 border p-5 rounded">
        <h2 class="text-center mb-4">註冊</h2>
        <form @submit.prevent="register">
          <!-- Email -->
          <div class="mb-3">
            <label for="email" class="form-label">Email</label>
            <input
              type="email"
              id="email"
              v-model="email"
              class="form-control"
              :class="{ 'is-invalid': emailError }"
              placeholder="請輸入Email"
              required
            />
            <div class="invalid-feedback" v-if="emailError">{{ emailError }}</div>
          </div>

          <!-- Username -->
          <div class="mb-3">
            <label for="username" class="form-label">帳號</label>
            <input
              type="text"
              id="username"
              v-model="username"
              class="form-control"
              :class="{ 'is-invalid': usernameError }"
              placeholder="請輸入帳號"
              required
            />
            <div class="invalid-feedback" v-if="usernameError">{{ usernameError }}</div>
          </div>

          <!-- Password -->
          <div class="mb-3">
            <label for="password" class="form-label">密碼</label>
            <input
              type="password"
              id="password"
              v-model="password"
              class="form-control"
              :class="{ 'is-invalid': passwordError }"
              placeholder="請輸入密碼"
              required
            />
            <div class="invalid-feedback" v-if="passwordError">{{ passwordError }}</div>
          </div>

          <!-- Confirm Password -->
          <div class="mb-3">
            <label for="confirmPassword" class="form-label">確認密碼</label>
            <input
              type="password"
              id="confirmPassword"
              v-model="confirmPassword"
              class="form-control"
              :class="{ 'is-invalid': confirmPasswordError }"
              placeholder="請輸入確認密碼"
              required
            />
            <div class="invalid-feedback" v-if="confirmPasswordError">{{ confirmPasswordError }}</div>
          </div>

          <!-- Submit Button -->
          <button type="submit" class="btn btn-primary w-100">註冊</button>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import api from '@/services/api';

export default {
  data() {
    return {
      email: '',
      username: '',
      password: '',
      confirmPassword: '',
      emailError: '',
      usernameError: '',
      passwordError: '',
      confirmPasswordError: '',
    };
  },
  methods: {
    validateForm() {
      let isValid = true;
      this.emailError = '';
      this.usernameError = '';
      this.passwordError = '';
      this.confirmPasswordError = '';

      // Email validation
      const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
      if (!emailPattern.test(this.email)) {
        this.emailError = '請輸入有效的 Email';
        isValid = false;
      }

      // Username validation
      if (this.username.length < 3) {
        this.usernameError = '帳號必須至少 3 個字符';
        isValid = false;
      }

      // Password validation
      if (this.password.length < 6) {
        this.passwordError = '密碼必須至少 6 個字符';
        isValid = false;
      }

      // Confirm Password validation
      if (this.password !== this.confirmPassword) {
        this.confirmPasswordError = '密碼與確認密碼不一致';
        isValid = false;
      }

      return isValid;
    },
    async register() {
      if (this.validateForm()) {
        const params = {
          email: this.email,
          username: this.username,
          password: this.password,
        };
        await api.register(params)
          .then((res) => {
            if (res.code == 200){
              this.$router.push({ name: 'Login' });
            }
            alert(res.message);
          })
          .catch((error) => {
            console.error(error);
          });
      }
    },
  },
};
</script>
